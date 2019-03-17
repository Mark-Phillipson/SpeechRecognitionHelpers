using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KillApplications.Models
{
    class LocalDbContext: DbContext
    {
        public LocalDbContext()
            : base("LocalDbContext")
        {
        }
        public DbSet<Application>  Applications { get; set; }
    }
}
