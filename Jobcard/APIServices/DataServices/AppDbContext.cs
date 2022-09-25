using APIServices.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices.Data.DataServices
{
    public   class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbOptions) : base(dbOptions)
        {
                
        }

        public DbSet<JobCart>? JobCarts => Set<JobCart>();
        public DbSet<Users>? Users => Set<Users>();
        public DbSet<ServiceTypes>? ServiceTypes => Set<ServiceTypes>();
    }
}
