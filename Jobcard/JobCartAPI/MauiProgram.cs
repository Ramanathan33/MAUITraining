using JobCartAPI.DataServices;
using JobCartAPI.ViewModels;
using JobCartAPI.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace JobCartAPI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();       
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "JobCards.db3");
    
        builder
            .UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

       // builder.Services.AddSingleton(s => ActivatorUtilities.CreateInstance <JobService> (s, dbPath));
        // Services
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
