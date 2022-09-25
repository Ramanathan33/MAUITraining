using JobCartAPI.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobCartAPI.DataServices
{
   
    public class JobService : IJobCardService
    {
        SQLiteAsyncConnection _dbConnection;  
        public string StatusMessage;
        int result = 0;
        public JobService()
        {
            
        }

        private async Task Init()
        {
            if(_dbConnection == null)
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "JobCards.db3");
                _dbConnection = new SQLiteAsyncConnection(dbPath);
                await _dbConnection.CreateTableAsync<JobCardModel>();
            }
        }

        public async Task<List<JobCardModel>> GetJobList()
        {
            try
            {
                await Init();
                return await _dbConnection.Table<JobCardModel>().ToListAsync();
            }
            catch (Exception)
            {
                StatusMessage = "Failed to retrieve data.";
            }

            return new List<JobCardModel>();
        }

        public async Task<JobCardModel> GetJob(int id)
        {
            try
            {
                await Init();
                return await _dbConnection.Table<JobCardModel>().FirstOrDefaultAsync(q => q.Id == id);
            }
            catch (Exception)
            {
                StatusMessage = "Failed to retrieve data.";
            }

            return null;
        }

        public async Task<int> DeleteJob(JobCardModel jobCardModel)
        {
            try
            {
                await Init();
                return await  _dbConnection.Table<JobCardModel>().DeleteAsync(q => q.Id == jobCardModel.Id);
            }
            catch (Exception)
            {
                StatusMessage = "Failed to delete data.";
            }

            return 0;
        }

        public async Task<int> AddJob(JobCardModel jobCart)
        {
            try
            {
                await Init();

                if (jobCart == null)
                    throw new Exception("Invalid Car Record");

                result = await _dbConnection.InsertAsync(jobCart);
                StatusMessage = result == 0 ? "Insert Failed" : "Insert Successful";
            }
            catch (Exception ex)
            {
                StatusMessage = "Failed to Insert data.";
            }

            return result;
        }

        public async Task<int> UpdateJob(JobCardModel jobCart)
        {
            try
            {
                await Init();

                if (jobCart == null)
                    throw new Exception("Invalid Car Record");

                result = await _dbConnection.UpdateAsync(jobCart);
                StatusMessage = result == 0 ? "Update Failed" : "Update Successful";
            }
            catch (Exception ex)
            {
                StatusMessage = "Failed to Update data.";
            }

            return result;
        }
    }
}
