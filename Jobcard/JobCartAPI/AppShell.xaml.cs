namespace JobCartAPI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
    }

    public void ShowJobList(JobListPage jobListPage)
    {
        Items.Clear();
        Items.Add(new ShellContent
        {
            Title = "Job Cards",
            Content = jobListPage,
            Route = "JobListPage"
        });
    }
}
