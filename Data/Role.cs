namespace CollegeApp.Data;

public class Role
{
    public int Id { get; set; }
    
    public string RoleName { get; set; }
    
    public string Description { get; set; }

    public bool IsActive { get; set; }
    
    public bool IsDeleted { get; set; }

    public DateTime CreationDate { get; set; }
    
    public DateTime ModificationDate { get; set; }
    
    public virtual ICollection<RolePrivilege> RolePrivileges { get; set; }
    
    public virtual ICollection<UserRoleMapping> UserRolesMappings { get; set; }

}