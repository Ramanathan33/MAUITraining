using Microsoft.Extensions.DependencyInjection;

namespace JobCartAPI;

public partial class App : Application
{
    private readonly IServiceProvider _services;

    public App(IServiceProvider services)
    {
        InitializeComponent();
        _services = services;
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        // Create pages after resources load so StaticResource lookups in XAML succeed.
        var shell = _services.GetRequiredService<AppShell>();
        shell.ShowJobList(_services.GetRequiredService<JobListPage>());
        return new Window(shell);
    }
}
