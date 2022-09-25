using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobCartAPI.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JobCartAPI.DataServices;
using JobCartAPI.Views;

namespace JobCartAPI.ViewModels
{
    public partial class JobListViewModel : ObservableObject
    {
        const string editButtonText = "Update Job";
        const string createButtonText = "Add Job";
        public static List<JobCardModel> JobCartsListForSearch { get; private set; } = new List<JobCardModel>();
        public ObservableCollection<JobCardModel> JobCarts { get; set; } = new ObservableCollection<JobCardModel>();
        private readonly IJobCardService _jobCardService;

        [ObservableProperty]
        bool isRefreshing;

        [ObservableProperty]
        string title;
        [ObservableProperty]
        int id;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotLoading))]
        bool isLoading;

        public bool IsNotLoading => !isLoading;

        public JobListViewModel(IJobCardService jobCardService)
        {
            Title = "Job List";
            _jobCardService = jobCardService;
        }

        [RelayCommand]
        async Task GetJobList()
        {
            if (IsLoading) return;
            try
            {
                IsLoading = true;
                JobCarts.Clear();
                var jobList = await _jobCardService.GetJobList();
                if (jobList?.Count > 0)
                {
                    jobList = jobList.OrderBy(f => f.CustomerName).ToList();
                    foreach (var student in jobList)
                    {
                        jobList.Add(student);
                    }
                    JobCartsListForSearch.Clear();
                    JobCartsListForSearch.AddRange(jobList);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get job list: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Failed to retrive list of jobs.", "Ok");
            }
            finally
            {
                IsLoading = false;
                IsRefreshing = false;
            }
        }


        [RelayCommand]
        public async void AddUpdateStudent()
        {
            await AppShell.Current.GoToAsync(nameof(AddEditJob));
        }

        [RelayCommand]
        public async void DisplayAction(JobCardModel jobCartModel)
        {
            var response = await AppShell.Current.DisplayActionSheet("Select Option", "OK", null, "Edit", "Delete");
            if (response == "Edit")
            {
                var navParam = new Dictionary<string, object>();
                navParam.Add("StudentDetail", jobCartModel);
                await AppShell.Current.GoToAsync(nameof(AddEditJob), navParam);
            }
            else if (response == "Delete")
            {
                if (jobCartModel.Id == 0)
                {
                    await Shell.Current.DisplayAlert("Invalid Job", "Please try again", "Ok");
                    return;
                }

                var delResponse = await _jobCardService.DeleteJob(jobCartModel);
                if (delResponse > 0)
                {
                    await Shell.Current.DisplayAlert("Deletion Successful", "Record Job Successfully", "Ok");
                    await GetJobList();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Deletion Failed", "Please select valid job to delete.", "Ok");
                }
            }
        }


        //[RelayCommand]
        //Task ClearForm()
        //{
        //    AddEditButtonText = createButtonText;
        //    JobCardModel.Id = 0;
        //    JobCardModel.ModelNo = string.Empty;
        //    JobCardModel.TypeOfService = 0;
        //    JobCardModel.CustomerName = string.Empty;
        //    JobCardModel.MobileNo = string.Empty;
        //    JobCardModel.Complaints = string.Empty;
        //    JobCardModel.DateOfService = string.Empty;
        //    JobCardModel.DateOfDelivery = string.Empty;
        //    JobCardModel.CreatedDate = string.Empty;
        //    JobCardModel.Status = 0;
        //    JobCardModel.ReceiverName = string.Empty;
        //    JobCardModel.Comments = string.Empty;
        //    return Task.CompletedTask;
        //}
        
    }
}
