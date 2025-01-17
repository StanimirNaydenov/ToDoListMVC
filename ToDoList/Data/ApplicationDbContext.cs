using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ToDoItem> ToDoItems { get; set; }
    

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

                // Seed данни за първоначален потребител
                var hasher = new PasswordHasher<User>();
                var user = new User
                {
                    UserId=1,
                    Username = "admin",
                    Email = "admin@example.com",
                    PasswordHash = "ADMIN@EXAMPLE.COM",
                   
                };
                user.PasswordHash = hasher.HashPassword(user, "Admin@123");

            modelBuilder.Entity<User>().HasData(user);

            // Configure User to Category relationship
            modelBuilder.Entity<Category>()
                .HasOne(c => c.User)
                .WithMany(u => u.Categories)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete allowed

            // Configure Category to ToDoItem relationship
            modelBuilder.Entity<ToDoItem>()
                .HasOne(t => t.Category)
                .WithMany(c => c.ToDoItems)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete allowed

        }

      


    }
}
