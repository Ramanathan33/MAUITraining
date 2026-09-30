namespace JobCartAPI;

public partial class App : Application
{
    public App(IServiceProvider services)
    {
        InitializeComponent();

        // Create pages after resources load so StaticResource lookups in XAML succeed.
        var shell = services.GetRequiredService<AppShell>();
        shell.ShowJobList(services.GetRequiredService<JobListPage>());
        MainPage = shell;
    }
}
