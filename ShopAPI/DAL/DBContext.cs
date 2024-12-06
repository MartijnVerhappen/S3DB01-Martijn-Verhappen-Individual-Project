using Logic.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class DBContext : DbContext
    {
        public DbSet<Product> Product { get; set; }
        public DbSet<Winkelmand> Winkelmand { get; set; }
        public DbSet<Klant> Klant { get; set; }
        public DbSet<WinkelmandProduct> WinkelmandProducts { get; set; }

        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WinkelmandProduct>()
                .HasKey(wp => new { wp.WinkelmandsId, wp.ProductId });

            modelBuilder.Entity<WinkelmandProduct>()
                .HasOne(wp => wp.Winkelmand)
                .WithMany(w => w.WinkelmandProducts)
                .HasForeignKey(wp => wp.WinkelmandsId);

            modelBuilder.Entity<WinkelmandProduct>()
                .HasOne(wp => wp.Product)
                .WithMany(p => p.WinkelmandProducts)
                .HasForeignKey(wp => wp.ProductId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
