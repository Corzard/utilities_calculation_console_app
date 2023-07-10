using Microsoft.EntityFrameworkCore;
using System.IO;
using Models;

namespace DataManipulations.Context
{

    internal class TariffContext : DbContext
    {
        static readonly string RootStorage = $"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent}\\";
        string dataSource = null!;
        public TariffContext(string DataSource)
        {
            dataSource = DataSource;
            Database.EnsureCreated();
        }
        public DbSet<TariffPlans> TariffPlans => Set<TariffPlans>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={RootStorage + dataSource}");
        }
    }
}
