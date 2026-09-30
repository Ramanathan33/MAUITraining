using JobCartAPI.DataServices;
using JobCartAPI.ViewModels;
using JobCartAPI.Views;

namespace JobCartAPI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		SQLitePCL.Batteries_V2.Init();

		var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        builder.Services.AddSingleton<IJobCardService, JobService>();

        //Views Registration
        builder.Services.AddSingleton<JobListPage>();
        builder.Services.AddTransient<AddEditJob>();

        //View Models 
        builder.Services.AddSingleton<JobListViewModel>();
        builder.Services.AddTransient<JobViewModel>();

        return builder.Build();
	}
}
