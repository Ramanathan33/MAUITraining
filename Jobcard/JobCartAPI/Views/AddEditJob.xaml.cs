using JobCartAPI.ViewModels;

namespace JobCartAPI.Views;

public partial class AddEditJob : ContentPage
{
	public AddEditJob(JobViewModel jobViewModel)
	{
		InitializeComponent();
		BindingContext = jobViewModel;

    }
}