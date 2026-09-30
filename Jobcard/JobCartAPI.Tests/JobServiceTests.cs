using JobCartAPI.DataServices;
using JobCartAPI.Entities;

namespace JobCartAPI.Tests
{
    public class JobServiceTests : IDisposable
    {
        private readonly string _dbPath;
        private readonly JobService _service;

        public JobServiceTests()
        {
            SQLitePCL.Batteries_V2.Init();
            _dbPath = Path.Combine(Path.GetTempPath(), $"jobcards-{Guid.NewGuid():N}.db3");
            _service = new JobService(_dbPath);
        }

        [Fact]
        public async Task AddUpdateAndDelete_RoundTripsEveryField()
        {
            var job = new JobCardModel
            {
                CustomerName = "Ada",
                ModelNo = "MX-10",
                TypeOfService = 3,
                MobileNo = "5551234567",
                Complaints = "No power",
                Location = "Front desk",
                DateOfService = "2024-05-01",
                DateOfDelivery = "2024-05-03",
                Status = 1,
                ReceiverName = "Grace",
                Comments = "Call first",
                Signature = "Ada",
                CreatedDate = "2024-05-01 09:00",
                CreatedBy = "app",
            };

            var inserted = await _service.AddJob(job);
            var addedAgain = await _service.AddJob(new JobCardModel
            {
                CustomerName = "Grace",
                ModelNo = "ZX-2",
                TypeOfService = 1,
            });

            Assert.True(inserted > 0);
            Assert.True(addedAgain > 0);
            Assert.True(job.Id > 0);

            var listed = await _service.GetJobList();
            Assert.Equal(2, listed.Count);

            job.CustomerName = "Ada Lovelace";
            job.Complaints = "Replaced the board";
            var updated = await _service.UpdateJob(job);
            var stored = await _service.GetJob(job.Id);

            Assert.True(updated > 0);
            Assert.Equal("Ada Lovelace", stored.CustomerName);
            Assert.Equal("Replaced the board", stored.Complaints);
            Assert.Equal(3, stored.TypeOfService);
            Assert.Equal("5551234567", stored.MobileNo);
            Assert.Equal(1, stored.Status);

            var deleted = await _service.DeleteJob(job);
            var remaining = await _service.GetJobList();

            Assert.True(deleted > 0);
            Assert.DoesNotContain(remaining, item => item.Id == job.Id);
        }

        [Fact]
        public async Task WriteMethods_ReturnZeroForInvalidRecords()
        {
            Assert.Equal(0, await _service.AddJob(null));
            Assert.Equal(0, await _service.UpdateJob(null));
            Assert.Equal(0, await _service.UpdateJob(new JobCardModel()));
            Assert.Equal(0, await _service.DeleteJob(null));
            Assert.Empty(await _service.GetJobList());
        }

        public void Dispose()
        {
            foreach (var suffix in new[] { string.Empty, "-journal", "-wal", "-shm" })
            {
                var candidate = _dbPath + suffix;
                if (File.Exists(candidate))
                    File.Delete(candidate);
            }
        }
    }
}
