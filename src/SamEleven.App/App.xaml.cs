namespace SamEleven.App;

public partial class App : Application
{
    private readonly ServiceProvider _provider;

    public IServiceProvider Services => _provider;

    public App()
    {
        InitializeComponent();

        IConfiguration configuration = BuildConfiguration();

        ServiceCollection services = new();

        services.AddLogging(builder =>
        {
            builder.AddDebug();
        });

        services.AddOptions<SteamCdnOptions>()
            .Configure(o => configuration.Bind(SteamCdnOptions.SectionName, o));
        services
          .AddRefitClient<ISteamStoreApi>()
          .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://store.steampowered.com/api/appdetails?appids"));
        services.AddSingleton<SteamDesktopApiService>();
        services.AddSingleton<ISteamCdnService, SteamCdnService>();
        services.AddSingleton<ISteamService, SteamService>();
        services.AddHttpClient<SteamCommunityWebApiService>(options =>
        {
            configuration.Bind(SteamCommunityWebApiService.OptionsSectionName, options);
        });

        services.AddSingleton(_ => new WeakReferenceMessenger());

        services.AddSingleton<GamePickerViewModel>();
        services.AddSingleton<MainWindowViewModel>();

        _provider = services.BuildServiceProvider(new ServiceProviderOptions
        {
#if DEBUG
            ValidateOnBuild = true
#endif
        });
    }

    private static IConfiguration BuildConfiguration()
    {
        using Stream appSettingsJson = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{typeof(App).Namespace}.appsettings.json")!;
        Debug.Assert(appSettingsJson != null);

        return new ConfigurationBuilder()
            .AddJsonStream(appSettingsJson)
            .Build();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        //using SteamClient steamClient = SteamClient.CreateFromRegistry();

        //    SteamApiService steamApiService = new();
        //    steamApiService.GetAllInstalledApps();
        //SteamClient.Init(4000);
        //var dsa = SteamClient.SteamId;// Your SteamId
        //var d = SteamClient.Name;// Your Name
        //foreach (var a in SteamUserStats.Achievements)
        //{

        //    SteamUserStats.
        //    Debug.WriteLine($"{a.Name} ({a.State})");
        //}
        //SteamApi.Load("C:\\Program Files (x86)\\Steam");

        GamePickerViewModel gamePickerViewModel = Services.GetRequiredService<GamePickerViewModel>();
        _ = gamePickerViewModel.InitializeAsync();

        MainWindowViewModel mainWindowViewModel = Services.GetRequiredService<MainWindowViewModel>();
        mainWindowViewModel.Initialize();

        MainWindow window = new(mainWindowViewModel, new Microsoft.UI.Xaml.Controls.Frame(), gamePickerViewModel);
        window.Closed += WindowClosed;
        window.Activate();
    }

    private void WindowClosed(object sender, WindowEventArgs args)
    {
        // Blocks the app thread to dispose of everything correctly.
        // Dispose methods can also contain work to do before destroying the service.
        _provider.DisposeAsync().AsTask().GetAwaiter().GetResult();
    }
}
