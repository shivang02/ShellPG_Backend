using Microsoft.EntityFrameworkCore;
using ShellPG_Backend.Data.Models;

namespace ShellPG_Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
}
