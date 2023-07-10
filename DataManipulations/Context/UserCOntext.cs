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
        //public DbSet<TariffPlans> TariffPlans => Set<TariffPlans>();
        //public DbSet<UserRawData> UserData => Set<UserRawData>();
        public DbSet<UserCunsumptionData> UserCunsumptionData => Set<UserCunsumptionData>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={RootStorage + dataSource}");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCunsumptionData>().HasKey(u => new
            {
                u.ID,
                u.Period
            });
        }
    }
}
