using CollegeApp.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data;

public class CollegeDbContext : DbContext
{
    public CollegeDbContext(DbContextOptions<CollegeDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Student> Students { get; set; }
    
    public DbSet<Department> Departments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // table 1
        modelBuilder.ApplyConfiguration(new StudentConfig());
        
        // table 2....
        modelBuilder.ApplyConfiguration(new DepartmentConfig());
        
        // table 3....
    }
}