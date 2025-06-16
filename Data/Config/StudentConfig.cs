using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApp.Data.Config;

public class StudentConfig : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();
        
        builder.Property(n => n.Name).IsRequired().HasMaxLength(250);
        builder.Property(n => n.Address).IsRequired(false).HasMaxLength(500);
        builder.Property(n => n.Name).IsRequired().HasMaxLength(250);
        
        builder.HasData(new List<Student>()
        {
            new() { Id = 1, Name = "yousif", Email = "yousif@gmail.com", Address = "Baghdad", DOB = new DateTime(2022, 8, 3)},
            new() { Id = 2, Name = "ali", Email = "ali@gmail.com", Address = "Erbil", DOB = new DateTime(2005, 3, 19) },
            new() { Id = 3, Name = "hassan", Email = "hassan@gmail.com", Address = "Basra", DOB = new DateTime(2014, 9, 27) },
            new() { Id = 4, Name = "ali", Email = "ali2@gmail.com", Address = "Samawah", DOB = new DateTime(2003, 8, 18) }
        });
    }
}