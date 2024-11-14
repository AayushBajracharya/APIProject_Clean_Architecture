using Microsoft.EntityFrameworkCore;
using APIProject.Models;

namespace APIProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }

        public DbSet<User> Users { get; set; }
    }

}
