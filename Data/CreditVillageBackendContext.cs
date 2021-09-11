using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CreditVillageBackend.Models;

namespace CreditVillageBackend.Data
{
    public class CreditVillageBackendContext : DbContext
    {
        public CreditVillageBackendContext (DbContextOptions<CreditVillageBackendContext> options)
            : base(options)
        {
        }

        public DbSet<CreditVillageBackend.Models.Subscription> Subscription { get; set; }
    }
}
