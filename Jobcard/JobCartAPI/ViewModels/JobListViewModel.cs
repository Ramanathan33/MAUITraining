using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JobCartAPI.DataServices;
using JobCartAPI.Entities;
using JobCartAPI.Helpers;
using JobCartAPI.Services;
using JobCartAPI.Validation;
using JobCartAPI.Views;

namespace JobCartAPI.ViewModels
{
    public partial class JobListViewModel : ObservableObject
    {
        public static List<JobCardModel> JobCartsListForSearch { get; } = new List<JobCardModel>();

        private readonly IJobCardService _jobCardService;
        private readonly List<JobCardModel> _allJobs = new List<JobCardModel>();

        public ObservableCollection<JobCardModel> JobCarts { get; } = new ObservableCollection<JobCardModel>();
        public IReadOnlyList<LookupItem> StatusFilters { get; } = JobLookups.StatusFilters;

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ShowLoading))]
        private bool isRefreshing;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ShowEmpty))]
        [NotifyPropertyChangedFor(nameof(ShowLoading))]
        private bool isLoading;

        [ObservableProperty]
        private LookupItem selectedStatusFilter = JobLookups.StatusFilters[0];

        public bool ShowEmpty => !IsLoading && JobCarts.Count == 0;
        public bool ShowLoading => IsLoading && !IsRefreshing;

        public string EmptyMessage =>
            _allJobs.Count > 0
                ? "No job cards match this status."
                : "No job cards yet. Tap Add Job to record a service visit.";

        public string JobCountText
        {
            get
            {
                var visible = JobCarts.Count;
                var total = _allJobs.Count;
                if (SelectedStatusFilter != null && SelectedStatusFilter.Id >= 0 && visible != total)
                    return $"{visible} of {total} jobs";

                return visible == 1 ? "1 job" : $"{visible} jobs";
            }
        }

        public JobListViewModel(IJobCardService jobCardService)
        {
            _jobCardService = jobCardService;
            Title = "Job Cards";
        }

        partial void OnSelectedStatusFilterChanged(LookupItem value) => PublishLists();

        [RelayCommand]
        private async Task GetJobList()
        {
            if (IsLoading)
            {
                IsRefreshing = false;
                return;
            }

            try
            {
                IsLoading = true;
                var jobList = await _jobCardService.GetJobList();
                _allJobs.Clear();
                if (jobList != null)
                    _allJobs.AddRange(jobList);

                PublishLists();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get job list: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Failed to retrieve the job list.", "OK");
            }
            finally
            {
                IsLoading = false;
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        private async Task AddJob() => await OpenEditor(null);

        [RelayCommand]
        private async Task EditJob(JobCardModel job)
        {
            if (job == null || job.Id == 0)
            {
                await Shell.Current.DisplayAlert("Invalid job", "Please try again.", "OK");
                return;
            }

            await OpenEditor(job);
        }

        [RelayCommand]
        private async Task DeleteJob(JobCardModel job)
        {
            if (job == null || job.Id == 0)
            {
                await Shell.Current.DisplayAlert("Invalid job", "Please try again.", "OK");
                return;
            }

            var name = string.IsNullOrWhiteSpace(job.CustomerName) ? "this customer" : job.CustomerName.Trim();
            var confirm = await Shell.Current.DisplayAlert("Delete job", $"Delete the job card for {name}?", "Delete", "Cancel");
            if (!confirm)
                return;

            var deleted = await _jobCardService.DeleteJob(job);
            if (deleted > 0)
            {
                await GetJobList();
                return;
            }

            await Shell.Current.DisplayAlert("Delete failed", "The job card could not be deleted.", "OK");
        }

        private async Task OpenEditor(JobCardModel job)
        {
            var page = AppServices.GetRequired<AddEditJob>();
            if (job != null)
                page.LoadJob(job);

            await Shell.Current.Navigation.PushAsync(page);
        }

        private void PublishLists()
        {
            if (!MainThread.IsMainThread)
            {
                MainThread.BeginInvokeOnMainThread(PublishLists);
                return;
            }

            var statusFilterId = SelectedStatusFilter?.Id ?? JobLookups.AllStatusesId;
            var visible = JobCardRules.PrepareVisibleJobs(_allJobs, statusFilterId, job => job.Status, job => job.CustomerName);
            var searchable = JobCardRules.PrepareVisibleJobs(_allJobs, JobLookups.AllStatusesId, job => job.Status, job => job.CustomerName);

            JobCarts.Clear();
            foreach (var job in visible)
                JobCarts.Add(job);

            JobCartsListForSearch.Clear();
            JobCartsListForSearch.AddRange(searchable);

            OnPropertyChanged(nameof(ShowEmpty));
            OnPropertyChanged(nameof(EmptyMessage));
            OnPropertyChanged(nameof(JobCountText));
        }
    }
}
