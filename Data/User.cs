namespace CollegeApp.Data;

public class User
{
    public int Id { get; set; }
    
    public string Username { get; set; }
    
    public string Password { get; set; }

    public string PasswordSalt { get; set; }

    public int UserType { get; set; }

    public bool IsActive { get; set; }
    
    public bool IsDeleted { get; set; }

    public DateTime CreationDate { get; set; }
    
    public DateTime ModificationDate { get; set; }
    
    public virtual ICollection<UserRoleMapping> UserRolesMappings { get; set; }
}