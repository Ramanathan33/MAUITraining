

namespace APIServices.Data.Entities
{
    public class JobCart : BaseEntity
    {
        public string CustomerName { get; set; }
        public string ModelNo { get; set; }
      
        public int TypeOfService { get; set; }
        public string MobileNo { get; set; }
        public string Complaints { get; set; }
        public string? Location { get; set; }
        public string DateOfService { get; set; }
        public string DateOfDelivery { get; set; }    
        public int Status { get; set; }
        public string ReceiverName { get; set; }
        public string Comments { get; set; }
        public string Signature { get; set; }
    }
}