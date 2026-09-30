using JobCartAPI.Entities;
using JobCartAPI.ViewModels;

namespace JobCartAPI.Views;

public partial class AddEditJob : ContentPage
{
    public AddEditJob(JobViewModel jobViewModel)
    {
        InitializeComponent();
        BindingContext = jobViewModel;
    }

    public void LoadJob(JobCardModel job)
    {
        if (BindingContext is JobViewModel viewModel)
            viewModel.LoadJob(job);
    }
}
