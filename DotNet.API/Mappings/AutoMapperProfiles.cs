using AutoMapper;
using DotNet.API.Models.Domain;
using DotNet.API.Models.DTO;

namespace DotNet.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserDTO, UserDomain>()
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x.FullName))
                .ReverseMap();
            CreateMap<Region, RegionDto>().ReverseMap();
        }

        public class UserDTO
        {
            public string FullName { get; set; }
        }

        public class UserDomain
        {
            public string Name { get; set; }
        }
    }
}
