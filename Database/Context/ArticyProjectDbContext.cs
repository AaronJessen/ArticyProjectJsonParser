using Microsoft.EntityFrameworkCore;
using ArticyProjectJsonParser.Database.Models;

namespace ArticyProjectJsonParser.Database.Context
{
    public class ArticyProjectDbContext : DbContext
    {
        public new DbSet<ModelDbo> Model { get; set; }
        public DbSet<PinDbo> Pin { get; set; }
        public DbSet<ConnectionDbo> Connection { get; set; }
    }
}