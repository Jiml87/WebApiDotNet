using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConfigurationItem>().HasNoKey();
        }

        public DbSet<ConfigurationItem> ConfigurationItems { get; set; }

        public async Task EnsureDatabaseCreatedAsync()
        {
            var sql = @"
                CREATE TABLE IF NOT EXISTS ConfigurationItems (
                    Key NCHAR(55) NOT NULL UNIQUE,
                    Value VARCHAR(255) NOT NULL
                );
            ";

            await Database.ExecuteSqlRawAsync(sql);
        }
    }
}
