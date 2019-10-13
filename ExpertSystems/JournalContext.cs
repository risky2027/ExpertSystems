using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpertSystems
{
    public class JournalContext : DbContext
    {
        public JournalContext() : base("JournalDbConnection") { }
        public DbSet<Journal> Journals { get; set; }
    }
}
