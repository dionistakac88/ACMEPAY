using API.DTOs;
using Domain;

namespace API.Repository
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderResponseDto>> GetAll(int pageNo = 1, int pageSize = 10);

        Task<Guid> Add(Order order);

        Task<bool> UpdateOrderStatus(ChangeStatusRequestDto order, Guid id, string status);
    }
}