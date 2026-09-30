using System.Linq;
using JobCartAPI.Entities;
using JobCartAPI.Services;
using JobCartAPI.Views;

namespace JobCartAPI.SearchHandlers
{
    public class JobCardSearchHandler : SearchHandler
    {
        public IList<JobCardModel> JobCards { get; set; }

        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            if (JobCards == null || string.IsNullOrWhiteSpace(newValue))
            {
                ItemsSource = null;
                return;
            }

            ItemsSource = JobCards.Where(job => Matches(job, newValue.Trim())).ToList();
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);
            if (item is not JobCardModel job)
                return;

            var page = AppServices.GetRequired<AddEditJob>();
            page.LoadJob(job);
            await Shell.Current.Navigation.PushAsync(page);
        }

        private static bool Matches(JobCardModel job, string query)
        {
            if (job == null)
                return false;

            return Contains(job.CustomerName, query)
                || Contains(job.ModelNo, query)
                || Contains(job.MobileNo, query);
        }

        private static bool Contains(string value, string query) =>
            !string.IsNullOrWhiteSpace(value)
            && value.Contains(query, StringComparison.OrdinalIgnoreCase);
    }
}
