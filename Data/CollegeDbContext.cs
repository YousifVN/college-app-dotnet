using CollegeApp.Data.Config;
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
        // table 1
        modelBuilder.ApplyConfiguration(new StudentConfig());
        
        // table 2....
        
        // table 3....
    }
}