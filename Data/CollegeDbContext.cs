using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data;

public class CollegeDbContext : DbContext
{
    public CollegeDbContext(DbContextOptions<CollegeDbContext> options) : base(options)
    {
        
    }
    
    DbSet<Student> Students { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().HasData(new List<Student>()
        {
            new() { Id = 1, Name = "yousif", Email = "yousif@gmail.com", Address = "Baghdad", DOB = new DateTime(2022, 8, 3)},
            new() { Id = 2, Name = "ali", Email = "ali@gmail.com", Address = "Erbil", DOB = new DateTime(2005, 3, 19) },
            new() { Id = 3, Name = "hassan", Email = "hassan@gmail.com", Address = "Basra", DOB = new DateTime(2014, 9, 27) },
            new() { Id = 4, Name = "ali", Email = "ali2@gmail.com", Address = "Samawah", DOB = new DateTime(2003, 8, 18) }
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(n => n.Name).IsRequired().HasMaxLength(250);
            entity.Property(n => n.Address).IsRequired(false).HasMaxLength(500);
            entity.Property(n => n.Name).IsRequired().HasMaxLength(250);
        });
    }
}