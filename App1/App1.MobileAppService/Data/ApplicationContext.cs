using App1.Models;

using Microsoft.EntityFrameworkCore;

namespace App1.MobileAppService.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
    }
}
