using Application.Data;
using Application.DTOs;
using AutoMapper;
using Domain;
using Domain.Enums;
using Domain.Order;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAllWithStatusAndPagination(int pageNo = 1, int pageSize = 10)
        {
            var order = (await _orderRepository.GetAllWithStatusCapturedAndPagination(pageNo, pageSize)).Select(
                    row => new OrderResponseDto
                    {
                        Amount = row.Amount,
                        Currency = row.Currency,
                        CardHolderNumber = Util.MaskCardNumber(row.CardHolderNumber),
                        HolderName = row.HolderName,
                        Id = row.Guid,
                        Status = row.Status
                    });

            return order;
        }

        public async Task<OrderStatusResponseDto> AddOrder(CreateOrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            var status = Enums.Status.Authorized.ToString();

            order.Status = status;
            var newOrderGuid = await _orderRepository.Add(order);

            var responseStatus = new OrderStatusResponseDto()
            {
                Id = newOrderGuid,
                Status = status
            };

            return responseStatus;
        }

        public async Task<bool> UpdateOrderStatus(Guid id, string status)
        {
            return await _orderRepository.UpdateOrderStatus(id, status);
        }
    }
}
