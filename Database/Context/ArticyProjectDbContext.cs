using Microsoft.EntityFrameworkCore;
using ArticyProjectJsonParser.Database.Models;

namespace ArticyProjectJsonParser.Database.Context
{
    public class ArticyProjectDbContext : DbContext
    {
        public string DatabasePath { get; set; }

        public new DbSet<ModelDbo> Model { get; set; }
        public DbSet<PinDbo> Pin { get; set; }
        public DbSet<ConnectionDbo> Connection { get; set; }
    
        public ArticyProjectDbContext(string databasePath)
        {
            DatabasePath = databasePath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DatabasePath}");
        }
    }
}