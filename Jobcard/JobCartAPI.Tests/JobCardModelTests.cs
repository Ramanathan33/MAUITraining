using JobCartAPI.Entities;

namespace JobCartAPI.Tests
{
    public class JobCardModelTests
    {
        [Fact]
        public void Copy_DuplicatesFieldsWithoutSharingTheInstance()
        {
            var original = new JobCardModel
            {
                Id = 7,
                CustomerName = "Ada",
                ModelNo = "MX-10",
                TypeOfService = 1,
                MobileNo = "5551234567",
                Complaints = "No power",
                Location = "Front desk",
                DateOfService = "2024-05-01",
                DateOfDelivery = "2024-05-03",
                Status = 2,
                ReceiverName = "Grace",
                Comments = "Call first",
                Signature = "Ada",
                CreatedDate = "2024-05-01 09:00",
                CreatedBy = "app",
            };

            var copy = original.Copy();
            original.CustomerName = "Changed";

            Assert.NotSame(original, copy);
            Assert.Equal(7, copy.Id);
            Assert.Equal("Ada", copy.CustomerName);
            Assert.Equal("MX-10", copy.ModelNo);
            Assert.Equal(1, copy.TypeOfService);
            Assert.Equal("5551234567", copy.MobileNo);
            Assert.Equal("No power", copy.Complaints);
            Assert.Equal("Front desk", copy.Location);
            Assert.Equal("2024-05-01", copy.DateOfService);
            Assert.Equal("2024-05-03", copy.DateOfDelivery);
            Assert.Equal(2, copy.Status);
            Assert.Equal("Grace", copy.ReceiverName);
            Assert.Equal("Call first", copy.Comments);
            Assert.Equal("Ada", copy.Signature);
            Assert.Equal("2024-05-01 09:00", copy.CreatedDate);
            Assert.Equal("app", copy.CreatedBy);
        }

        [Fact]
        public void Copy_ReplacesNullTextWithEmptyStrings()
        {
            var copy = new JobCardModel { CustomerName = null, ModelNo = null }.Copy();

            Assert.Equal(string.Empty, copy.CustomerName);
            Assert.Equal(string.Empty, copy.ModelNo);
        }

        [Fact]
        public void DisplayText_UsesLookupNamesAndSkipsEmptyParts()
        {
            var job = new JobCardModel
            {
                CustomerName = "Ada",
                ModelNo = "MX-10",
                TypeOfService = 1,
                MobileNo = "5551234567",
                Status = 0,
                DateOfService = "2024-05-01",
                Complaints = "No power\r\nwhen plugged in",
            };

            Assert.Equal("Repair", job.ServiceTypeName);
            Assert.Equal("Received", job.StatusName);
            Assert.Equal("MX-10 · Repair · 5551234567", job.SummaryLine);
            Assert.Equal("Service 2024-05-01", job.ScheduleText);
            Assert.Equal("No power when plugged in", job.ComplaintPreview);
            Assert.True(job.HasComplaint);
        }
    }
}
