using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostgreSqler.Models
{
    public class FeatBitContext : DbContext
    {
        //public DbSet<Event> Events { get; set; }
        public DbSet<Tests> TestsTable { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(@"Host=34.23.104.152:5432;Username=postgres;Password=azerty@123;Database=postgres");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tests>().ToTable("tests");
            //modelBuilder.Entity<Tests>().HasNoKey();
        }

        public FeatBitContext(DbContextOptions<FeatBitContext> options) : base(options)
        {
        }
    }
}
