using JobCartAPI.Helpers;
using SQLite;

namespace JobCartAPI.Entities
{
    [Table("JobCard")]
    public class JobCardModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string ModelNo { get; set; } = string.Empty;
        public int TypeOfService { get; set; }
        public string MobileNo { get; set; } = string.Empty;
        public string Complaints { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string DateOfService { get; set; } = string.Empty;
        public string DateOfDelivery { get; set; } = string.Empty;
        public int Status { get; set; }
        public string ReceiverName { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public string Signature { get; set; } = string.Empty;
        public string CreatedDate { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;

        [Ignore]
        public string ServiceTypeName => JobLookups.GetServiceTypeName(TypeOfService);

        [Ignore]
        public string StatusName => JobLookups.GetStatusName(Status);

        [Ignore]
        public string SummaryLine
        {
            get
            {
                var parts = new List<string>();
                if (!string.IsNullOrWhiteSpace(ModelNo))
                    parts.Add(ModelNo.Trim());

                var serviceType = ServiceTypeName;
                if (!string.IsNullOrWhiteSpace(serviceType) && serviceType != "Not set")
                    parts.Add(serviceType);

                if (!string.IsNullOrWhiteSpace(MobileNo))
                    parts.Add(MobileNo.Trim());

                return string.Join(" · ", parts);
            }
        }

        [Ignore]
        public bool HasSummary => !string.IsNullOrWhiteSpace(SummaryLine);

        [Ignore]
        public string ScheduleText
        {
            get
            {
                var hasService = !string.IsNullOrWhiteSpace(DateOfService);
                var hasDelivery = !string.IsNullOrWhiteSpace(DateOfDelivery);
                if (hasService && hasDelivery)
                    return $"Service {DateOfService.Trim()}  ·  Delivery {DateOfDelivery.Trim()}";
                if (hasService)
                    return $"Service {DateOfService.Trim()}";
                if (hasDelivery)
                    return $"Delivery {DateOfDelivery.Trim()}";
                return string.Empty;
            }
        }

        [Ignore]
        public bool HasSchedule => !string.IsNullOrWhiteSpace(ScheduleText);

        [Ignore]
        public string ComplaintPreview
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Complaints))
                    return string.Empty;

                var parts = Complaints.Split(new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                return string.Join(" ", parts);
            }
        }

        [Ignore]
        public bool HasComplaint => !string.IsNullOrWhiteSpace(ComplaintPreview);

        public JobCardModel Copy()
        {
            return new JobCardModel
            {
                Id = Id,
                CustomerName = CustomerName ?? string.Empty,
                ModelNo = ModelNo ?? string.Empty,
                TypeOfService = TypeOfService,
                MobileNo = MobileNo ?? string.Empty,
                Complaints = Complaints ?? string.Empty,
                Location = Location ?? string.Empty,
                DateOfService = DateOfService ?? string.Empty,
                DateOfDelivery = DateOfDelivery ?? string.Empty,
                Status = Status,
                ReceiverName = ReceiverName ?? string.Empty,
                Comments = Comments ?? string.Empty,
                Signature = Signature ?? string.Empty,
                CreatedDate = CreatedDate ?? string.Empty,
                CreatedBy = CreatedBy ?? string.Empty,
            };
        }
    }
}
