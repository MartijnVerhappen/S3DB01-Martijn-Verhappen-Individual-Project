using Microsoft.EntityFrameworkCore;
using Core.Models;

namespace DAL
{
    public class DBContext : DbContext
    {
        public virtual DbSet<ProductModel> Product { get; set; }

        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
