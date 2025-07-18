namespace CollegeApp.Data;

public class RolePrivilege
{
    public int Id { get; set; }
    
    public string RolePrivilegeName { get; set; }
    
    public string Description { get; set; }
    
    public int RoleId { get; set; }
    
    public Role Role { get; set; }

    public bool IsActive { get; set; }
    
    public bool IsDeleted { get; set; }

    public DateTime CreationDate { get; set; }
    
    public DateTime ModificationDate { get; set; }
}