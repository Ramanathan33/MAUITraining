using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JobCartAPI.DataServices;
using JobCartAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobCartAPI.ViewModels
{
    [QueryProperty(nameof(JobViewModel), "JobViewModel")]
    public partial class JobViewModel : ObservableObject
    {
        [ObservableProperty]
        private JobCardModel job = new JobCardModel();

        private readonly IJobCardService _jobCardService;
        public JobViewModel(IJobCardService jobCardService)
        {
            _jobCardService = jobCardService;
        }

        [RelayCommand]
        public async void AddUpdateJob()
        {
            int response = -1;
            if (Job.Id > 0)
            {
                response = await _jobCardService.UpdateJob(Job);
            }
            else
            {
                response = await _jobCardService.AddJob(new Entities.JobCardModel
                {
                    CustomerName = Job.CustomerName,
                    TypeOfService = Job.TypeOfService,
                    ModelNo = Job.ModelNo,
                });
            }

            if (response > 0)
            {
                await Shell.Current.DisplayAlert("Job Info Saved", "Record Saved", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Heads Up!", "Something went wrong while adding record", "OK");
            }
        }
    }
}
