using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ClassLib;

namespace TransAmBank.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
;
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Entry> Entries { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Quantity> Quantities { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Unit> Units { get; set; }


    }
}
