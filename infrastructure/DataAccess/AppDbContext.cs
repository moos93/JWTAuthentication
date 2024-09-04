using Domain.Entities;
using Microsoft.EntityFrameworkCore;
 

namespace Infrastructure.DataAccess
{
    internal class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) 
        {
            
        }
        public DbSet<ApplicationUser> Users { get; set; }
    }
}
