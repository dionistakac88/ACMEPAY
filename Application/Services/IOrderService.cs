using Application.DTOs;

namespace Application.Services
{
    public interface IOrderService
    {
        Task<OrderStatusResponseDto> AddOrder(CreateOrderDto orderDto);

        Task<IEnumerable<OrderResponseDto>> GetAllWithStatusAndPagination(int pageNo = 1, int pageSize = 10);

        Task<bool> UpdateOrderStatus(Guid id, string status);
    }
}
