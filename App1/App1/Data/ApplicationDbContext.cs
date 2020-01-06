using App1.Models;

using Microsoft.EntityFrameworkCore;

namespace App1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

#nullable disable
        public DbSet<Item> Items { get; set; }
#nullable enable
    }
}
