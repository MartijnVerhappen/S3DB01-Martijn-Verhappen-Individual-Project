using DAL.Entities;
using Logic.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class DBContext : DbContext
    {
        public DbSet<ProductEntity> Product { get; set; }
        public DbSet<WinkelmandEntity> Winkelmand { get; set; }
        public DbSet<KlantEntity> Klant { get; set; }
        public DbSet<WinkelmandProductEntity> WinkelmandProductEntitie { get; set; }

        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
