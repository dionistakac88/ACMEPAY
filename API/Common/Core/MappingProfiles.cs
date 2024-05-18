using API.DTOs;
using Domain;

namespace API.Common.Core
{
    public class MappingProfiles : AutoMapper.Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreateOrderDto, Order>();
        }
            
    }
}
