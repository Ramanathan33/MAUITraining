using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JobCartAPI.DataServices;
using JobCartAPI.Entities;
using JobCartAPI.Helpers;
using JobCartAPI.Validation;

namespace JobCartAPI.ViewModels
{
    public partial class JobViewModel : ObservableObject
    {
        private readonly IJobCardService _jobCardService;

        [ObservableProperty]
        private JobCardModel job = new JobCardModel();

        [ObservableProperty]
        private LookupItem selectedServiceType;

        [ObservableProperty]
        private LookupItem selectedStatus;

        [ObservableProperty]
        private DateTime serviceDate = DateTime.Today;

        [ObservableProperty]
        private DateTime deliveryDate = DateTime.Today;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotSaving))]
        private bool isSaving;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasValidationMessage))]
        private string validationMessage = string.Empty;

        public IReadOnlyList<LookupItem> ServiceTypes { get; } = JobLookups.ServiceTypes;
        public IReadOnlyList<LookupItem> Statuses { get; } = JobLookups.Statuses;
        public bool IsNotSaving => !IsSaving;
        public bool HasValidationMessage => !string.IsNullOrEmpty(ValidationMessage);
        public string PageTitle => Job != null && Job.Id > 0 ? "Edit Job" : "Add Job";
        public string SaveButtonText => Job != null && Job.Id > 0 ? "Update Job" : "Save Job";

        public JobViewModel(IJobCardService jobCardService)
        {
            _jobCardService = jobCardService;
            SyncFromJob(Job);
        }

        partial void OnJobChanged(JobCardModel value) => SyncFromJob(value);

        public void LoadJob(JobCardModel job)
        {
            if (job == null)
                return;

            Job = job.Copy();
        }

        [RelayCommand]
        private async Task AddUpdateJob()
        {
            if (IsSaving)
                return;

            try
            {
                IsSaving = true;
                ValidationMessage = string.Empty;
                ApplyFormToJob();

                var error = JobCardRules.Validate(
                    Job.CustomerName,
                    Job.ModelNo,
                    Job.MobileNo,
                    SelectedServiceType?.Id,
                    ServiceDate,
                    DeliveryDate);

                if (error != null)
                {
                    ValidationMessage = error;
                    return;
                }

                var isUpdate = Job.Id > 0;
                int response;
                if (isUpdate)
                {
                    response = await _jobCardService.UpdateJob(Job);
                }
                else
                {
                    Job.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                    response = await _jobCardService.AddJob(Job);
                }

                if (response > 0)
                {
                    await Shell.Current.DisplayAlertAsync(isUpdate ? "Job updated" : "Job saved", "The job card was saved.", "OK");
                    await Shell.Current.Navigation.PopAsync();
                    return;
                }

                ValidationMessage = "Couldn't save this job card. Please try again.";
            }
            catch (Exception)
            {
                ValidationMessage = "Couldn't save this job card. Please try again.";
            }
            finally
            {
                IsSaving = false;
            }
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        private void ApplyFormToJob()
        {
            Job.CustomerName = Job.CustomerName?.Trim() ?? string.Empty;
            Job.ModelNo = Job.ModelNo?.Trim() ?? string.Empty;
            Job.MobileNo = Job.MobileNo?.Trim() ?? string.Empty;
            Job.Complaints = Job.Complaints?.Trim() ?? string.Empty;
            Job.Location = Job.Location?.Trim() ?? string.Empty;
            Job.ReceiverName = Job.ReceiverName?.Trim() ?? string.Empty;
            Job.Comments = Job.Comments?.Trim() ?? string.Empty;
            Job.Signature = Job.Signature?.Trim() ?? string.Empty;
            Job.TypeOfService = SelectedServiceType?.Id ?? 0;
            Job.Status = SelectedStatus?.Id ?? JobLookups.Statuses[0].Id;
            Job.DateOfService = ServiceDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            Job.DateOfDelivery = DeliveryDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private void SyncFromJob(JobCardModel job)
        {
            var source = job ?? new JobCardModel();
            SelectedServiceType = JobLookups.FindServiceType(source.TypeOfService);
            SelectedStatus = JobLookups.FindStatus(source.Status) ?? JobLookups.Statuses[0];
            ServiceDate = JobCardRules.ParseDateOrToday(source.DateOfService);
            DeliveryDate = JobCardRules.ParseDateOrToday(source.DateOfDelivery);
            OnPropertyChanged(nameof(PageTitle));
            OnPropertyChanged(nameof(SaveButtonText));
        }
    }
}
