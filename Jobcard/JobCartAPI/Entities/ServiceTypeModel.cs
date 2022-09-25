using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace JobCartAPI.Entities
{
    [Table("ServiceTypes")]
    public class ServiceTypeModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string ServiceType { get; set; }

    }
}
