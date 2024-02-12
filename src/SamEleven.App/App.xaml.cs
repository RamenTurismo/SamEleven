using Emyfreya.Steam.Desktop;
using Emyfreya.Steam.Desktop.Abstractions;

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

        services.AddSteamApi()
            .AddDefaultLogger();

        services.AddSingleton<ISteamClientManager, SteamClientManager>();
        services.AddSingleton<ISteamService, SteamService>();

        services.AddSingleton(_ => new WeakReferenceMessenger());

        Frame frame = new();

        services.AddSingleton(frame)
            .AddSingleton<MainWindowViewModel>()
            .AddSingleton<MainWindow>();

        services.AddNavigationService(frame)
            .AddView<GamePickerPage, GamePickerPageViewModel>()
            .AddView<AchievementPage, AchievementPageViewModel>();

        services.AddDispatcherQueueService();

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

        MainWindowViewModel mainWindowViewModel = Services.GetRequiredService<MainWindowViewModel>();
        mainWindowViewModel.Initialize();

        MainWindow window = Services.GetRequiredService<MainWindow>();
        ConfigureWindow(window);

        _ = Services.GetRequiredService<INavigationService>().NavigateAsync<GamePickerPageViewModel>();
        window.Activate();

        Log.Started(_logger, startAppWatch.ElapsedMilliseconds);
    }

    private void ConfigureWindow(Window window)
    {
        window.SetWindowSize(1300, 720);
        window.Title = $"S.A.M. Eleven | v{Assembly.GetExecutingAssembly().GetName().Version}";
        window.Closed += WindowClosed;
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
