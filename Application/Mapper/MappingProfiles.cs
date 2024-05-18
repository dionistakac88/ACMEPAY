using Application.DTOs;
using Domain.Order;

namespace Application.Mapper
{
    public class MappingProfiles : AutoMapper.Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreateOrderDto, Order>();
            CreateMap<Order, OrderResponseDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Guid));
            
        }
            
    }
}
