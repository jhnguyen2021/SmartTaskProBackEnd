using Microsoft.EntityFrameworkCore;
using SmartTaskPro.Models;




namespace SmartTaskPro.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }


        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b); // <- IMPORTANT for Identity!
                                     // ... your entity config, table names, indexes, delete behaviors ...
        }
    }
}
