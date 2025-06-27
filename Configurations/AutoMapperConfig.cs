using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Models;

namespace CollegeApp.Configurations;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        // CreateMap<Student, StudentDTO>();
        // CreateMap<StudentDTO, Student>();
        
        // we can use this instead of the two lines above
        // CreateMap<StudentDTO, Student>().ReverseMap();
        
        // ** make sure to add future configs after the reverse to avoid unexpected behaviors **
        
        // config for different property names
        // CreateMap<StudentDTO, Student>().ReverseMap().ForMember(n => n.Name, opt => opt.MapFrom(x => x.StudentName));
        
        // config for ignore
        // CreateMap<StudentDTO, Student>().ReverseMap().ForMember(n => n.Email, opt => opt.Ignore());
        
        // config for transform
        
        // this will work for all string properties, which is wrong in this case
        // CreateMap<StudentDTO, Student>().ReverseMap().AddTransform<string>(n => string.IsNullOrEmpty(n) ? "No address found" : n);
        
        // this will only work for the specified property
        CreateMap<StudentDTO, Student>().ReverseMap()
            .ForMember(n => n.Address, opt => opt.
                MapFrom(n => string.IsNullOrEmpty(n.Address) ? "No address found" : n.Address));
        
        
        CreateMap<RoleDTO, Role>().ReverseMap();
    }
}