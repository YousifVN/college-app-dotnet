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
        CreateMap<StudentDTO, Student>().ReverseMap();
        
        // ** make sure to add future configs after the reverse to avoid unexpected behaviors **
        
        // config for different property names
        // CreateMap<StudentDTO, Student>().ReverseMap().ForMember(n => n.Name, opt => opt.MapFrom(x => x.StudnetName));
        
        // config for ignore
        // CreateMap<StudentDTO, Student>().ReverseMap().ForMember(n => n.Email, opt => opt.Ignore());
        
        // config for transform
        // CreateMap<StudentDTO, Student>().ReverseMap().AddTransform<string>(n => string.IsNullOrEmpty(n) ? "No address found" : n);
    }
}