using System.Runtime.InteropServices;
using SamEleven.App.Steam.SteamworksSdk;

namespace SamEleven.App;

public partial class App : Application
{
    public static partial class Log
    {
        [LoggerMessage(LogLevel.Information, "Starting application")]
        public static partial void Starting(ILogger logger);
        [LoggerMessage(LogLevel.Information, "Started application in {Elapsed} ms")]
        public static partial void Started(ILogger logger, long elapsed);
        [LoggerMessage(LogLevel.Information, "Stopping application")]
        public static partial void Stopping(ILogger logger);
        [LoggerMessage(LogLevel.Information, "Stopped application in {Elapsed} ms")]
        public static partial void Stopped(ILogger logger, long elapsed);
    }

    private readonly ServiceProvider _provider;
    private readonly ILogger<App> _logger;

    public IServiceProvider Services => _provider;

    public App()
    {
        InitializeComponent();

        IConfiguration configuration = BuildConfiguration();

        ServiceCollection services = new();

        services.AddLogging(builder =>
        {
            builder.AddConfiguration(configuration.GetSection("Logging"));
            builder.AddDebug();
        });

        services.AddSingleton(configuration);

        services.AddOptions<SteamCdnOptions>()
            .Configure(o => configuration.Bind(SteamCdnOptions.SectionName, o));
        services.AddSingleton<ISteamCdnService, SteamCdnService>();

        services.AddSteamStoreApi();
        services.AddSteamCommunityApi();

        services.AddSingleton<SteamworksSdkApi>();
        services.AddSingleton<ISteamService, SteamService>();

        services.AddSingleton(_ => new WeakReferenceMessenger());

        services.AddSingleton<GamePickerViewModel>();
        services.AddSingleton<MainWindowViewModel>();

        _provider = services.BuildServiceProvider(new ServiceProviderOptions
        {
#if DEBUG
            ValidateOnBuild = true
#endif
        });

        _logger = _provider.GetRequiredService<ILogger<App>>();
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
        Log.Starting(_logger);
        Stopwatch startAppWatch = Stopwatch.StartNew();

        SteamApiClient client = new();
        uint[] appIds = client.GetAppList();

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

        Log.Started(_logger, startAppWatch.ElapsedMilliseconds);
    }

    private void WindowClosed(object sender, WindowEventArgs args)
    {
        Log.Stopping(_logger);
        Stopwatch stopAppWatch = Stopwatch.StartNew();

        // Blocks the app thread to dispose of everything correctly.
        // Dispose methods can also contain work to do before destroying the service.
        _provider.DisposeAsync().AsTask().GetAwaiter().GetResult();

        Log.Stopped(_logger, stopAppWatch.ElapsedMilliseconds);
    }
}
