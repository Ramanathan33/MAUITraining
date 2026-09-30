using JobCartAPI.Entities;
using SQLite;

namespace JobCartAPI.DataServices
{
    public class JobService : IJobCardService
    {
        private readonly string _dbPath;
        private SQLiteAsyncConnection _dbConnection;
        public string StatusMessage { get; private set; } = string.Empty;

        public JobService()
            : this(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "JobCards.db3"))
        {
        }

        public JobService(string dbPath)
        {
            if (string.IsNullOrWhiteSpace(dbPath))
                throw new ArgumentException("A database path is required.", nameof(dbPath));

            _dbPath = dbPath;
        }

        private async Task Init()
        {
            if (_dbConnection != null)
                return;

            _dbConnection = new SQLiteAsyncConnection(_dbPath);
            await _dbConnection.CreateTableAsync<JobCardModel>();
        }

        public async Task<List<JobCardModel>> GetJobList()
        {
            await Init();
            return await _dbConnection.Table<JobCardModel>().ToListAsync();
        }

        public async Task<JobCardModel> GetJob(int id)
        {
            await Init();
            return await _dbConnection.Table<JobCardModel>().FirstOrDefaultAsync(job => job.Id == id);
        }

        public async Task<int> DeleteJob(JobCardModel jobCardModel)
        {
            try
            {
                await Init();
                if (jobCardModel == null || jobCardModel.Id <= 0)
                {
                    StatusMessage = "Invalid job record.";
                    return 0;
                }

                var deleted = await _dbConnection.Table<JobCardModel>().DeleteAsync(job => job.Id == jobCardModel.Id);
                StatusMessage = deleted == 0 ? "Delete failed." : "Delete successful.";
                return deleted;
            }
            catch (Exception)
            {
                StatusMessage = "Failed to delete data.";
                return 0;
            }
        }

        public async Task<int> AddJob(JobCardModel jobCart)
        {
            try
            {
                await Init();
                if (jobCart == null)
                {
                    StatusMessage = "Invalid job record.";
                    return 0;
                }

                var inserted = await _dbConnection.InsertAsync(jobCart);
                StatusMessage = inserted == 0 ? "Insert failed." : "Insert successful.";
                return inserted;
            }
            catch (Exception)
            {
                StatusMessage = "Failed to insert data.";
                return 0;
            }
        }

        public async Task<int> UpdateJob(JobCardModel jobCart)
        {
            try
            {
                await Init();
                if (jobCart == null || jobCart.Id <= 0)
                {
                    StatusMessage = "Invalid job record.";
                    return 0;
                }

                var updated = await _dbConnection.UpdateAsync(jobCart);
                StatusMessage = updated == 0 ? "Update failed." : "Update successful.";
                return updated;
            }
            catch (Exception)
            {
                StatusMessage = "Failed to update data.";
                return 0;
            }
        }
    }
}
