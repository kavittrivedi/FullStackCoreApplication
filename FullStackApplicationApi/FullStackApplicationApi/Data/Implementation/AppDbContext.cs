using FullStackApplicationApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FullStackApplicationApi.Data.Implementation
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
    }
}
