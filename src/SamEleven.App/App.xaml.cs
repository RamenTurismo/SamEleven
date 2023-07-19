using System;
using System.Diagnostics;
using System.Reflection;
using CommunityToolkit.Mvvm.Messaging;
using Gibbed.Steamworks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using SamEleven.App.Abstractions;
using SamEleven.App.Picker;
using SamEleven.App.Steam;
using SamEleven.App.Steam.Client;
using SamEleven.Steamworks;

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
        services.AddSingleton<ISteamCdnService, SteamCdnService>();
        services.AddSingleton<ISteamService, SteamService>();
        services.AddSingleton<ISteamClient, GibbedSteamClientWrapper>();

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
        using System.IO.Stream appSettingsJson = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{typeof(App).Namespace}.appsettings.json")!;
        Debug.Assert(appSettingsJson != null);

        return new ConfigurationBuilder()
            .AddJsonStream(appSettingsJson)
            .Build();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        // TODO catch, error out for user + rethrow
        SteamInstallationInfo steamInstallationInfo = SteamInstallationInfo.FromRegistry();

        ISteamService steamService = Services.GetRequiredService<ISteamService>();
        steamService.Initialize(steamInstallationInfo);

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
        gamePickerViewModel.Initialize();

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
