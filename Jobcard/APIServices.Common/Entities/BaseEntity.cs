
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices.Data.Entities
{
    public class BaseEntity
    {
        [Key]      
        public int Id { get; set; }
        public string? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
    }
}
