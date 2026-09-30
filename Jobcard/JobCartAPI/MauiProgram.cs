using JobCartAPI.DataServices;
using JobCartAPI.ViewModels;
using JobCartAPI.Views;

namespace JobCartAPI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<IJobCardService, JobService>();
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<JobListPage>();
        builder.Services.AddTransient<AddEditJob>();
        builder.Services.AddSingleton<JobListViewModel>();
        builder.Services.AddTransient<JobViewModel>();

        return builder.Build();
    }
}
