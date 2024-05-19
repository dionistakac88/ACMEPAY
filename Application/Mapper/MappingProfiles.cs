using Application.DTOs;
using Domain.Transaction;

namespace Application.Mapper
{
    public class MappingProfiles : AutoMapper.Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreateTransactionDto, Transaction>();
            CreateMap<Transaction, TransactionResponseDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Guid));
            
        }
            
    }
}
