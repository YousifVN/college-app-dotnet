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
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Role> Roles { get; set; }
    
    public DbSet<RolePrivilege> RolePrivileges { get; set; }
    
    public DbSet<UserRoleMapping> UserRoleMappings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // table 1
        modelBuilder.ApplyConfiguration(new StudentConfig());
        
        // table 2....
        modelBuilder.ApplyConfiguration(new DepartmentConfig());
        
        // table 3....
        modelBuilder.ApplyConfiguration(new UserConfig());
        
        modelBuilder.ApplyConfiguration(new RoleConfig());
        
        modelBuilder.ApplyConfiguration(new RolePrivilegeConfig());
        
        modelBuilder.ApplyConfiguration(new UserRoleMappingConfig());
    }
}