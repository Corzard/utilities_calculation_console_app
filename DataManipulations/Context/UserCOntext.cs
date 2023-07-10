using Microsoft.EntityFrameworkCore;
using System.IO;
using Models;

namespace DataManipulations.Context
{
    internal class UserContext : DbContext
    {
        static readonly string RootStorage = $"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent}\\";
        string dataSource = null!;
        public UserContext(string DataSource)
        {
            dataSource = DataSource;
            Database.EnsureCreated();
        }
        public DbSet<UserConsumptionData> UserConsumptionData => Set<UserConsumptionData>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={RootStorage + dataSource}");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserConsumptionData>().HasKey(u => new
            {
                u.ID,
                u.Period
            });
        }
    }
}
