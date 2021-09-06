using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreditVillageBackend.Models
{
    public class ActivityFeed
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset DateTime { get; set; }
    }
}
