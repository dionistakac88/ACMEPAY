using Domain.Order;

namespace Application.Data
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllWithStatusCapturedAndPagination(int pageNo = 1, int pageSize = 10);

        Task<Guid> Add(Order order);

        Task<bool> UpdateOrderStatus(Guid id, string status);
    }
}