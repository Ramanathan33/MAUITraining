
using JobCartAPI.ViewModels;

namespace JobCartAPI;

public partial class JobListPage : ContentPage
{
    private JobListViewModel _viewMode;
    public JobListPage(JobListViewModel jobListViewModel)
	{
		InitializeComponent();
        _viewMode = jobListViewModel;
        this.BindingContext = jobListViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewMode.GetJobListCommand.Execute(null);
    }
}