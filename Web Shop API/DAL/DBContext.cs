using Microsoft.EntityFrameworkCore;
using Web_Shop_API.Models;

namespace DAL
{
    public class DBContext : DbContext
    {
        public DbSet<ProductModel> Product { get; set; }

        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
