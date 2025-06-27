using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApp.Data.Config;

public class RolePrivilegeConfig : IEntityTypeConfiguration<RolePrivilege>
{
    public void Configure(EntityTypeBuilder<RolePrivilege> builder)
    {
        builder.ToTable("RolePrivileges");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();

        builder.Property(n => n.RolePrivilegeName).HasMaxLength(250).IsRequired();
        builder.Property(n => n.Description);
        builder.Property(n => n.IsActive).IsRequired();
        builder.Property(n => n.IsDeleted).IsRequired();
        builder.Property(n => n.CreationDate).IsRequired();
    }
} 