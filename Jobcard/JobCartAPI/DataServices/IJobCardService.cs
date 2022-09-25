using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobCartAPI.Entities;

namespace JobCartAPI.DataServices
{
    public interface IJobCardService
    {
        Task<List<JobCardModel>> GetJobList();
        Task<JobCardModel> GetJob(int id);
        Task<int> AddJob(JobCardModel jobCardModel);
        Task<int> DeleteJob(JobCardModel jobCardModel);
        Task<int> UpdateJob(JobCardModel jobCardModel);
    }
}
