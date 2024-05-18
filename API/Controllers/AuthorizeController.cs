using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using AutoMapper;
using Domain.Order;
using Application.DTOs;
using Domain.Enums;
using Application.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizeController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public AuthorizeController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IEnumerable<OrderResponseDto>> GetOrdersWithCapturedStatus()
        {
            return await _orderService.GetAllWithStatusAndPagination();
        }

        [HttpPost]
        public async Task<ActionResult<OrderStatusResponseDto>> AddOrder(CreateOrderDto orderDto)
        {
            try
            {

                var responseStatus = await _orderService.AddOrder(orderDto);

                return Ok(responseStatus);
            } 
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("{id}/voids")]
        public async Task<ActionResult<OrderStatusResponseDto>> ChangeOrderStatusToVoid(Guid id, ChangeStatusRequestDto changeStatusRequestDto)
        {
            return await UpdateOrderStatus(changeStatusRequestDto, id, Enums.Status.Voided.GetDisplayName());
        }

        [HttpPut("{id}/capture")]
        public async Task<ActionResult<OrderStatusResponseDto>> ChangeOrderStatusToCapture(Guid id, ChangeStatusRequestDto changeStatusRequestDto)
        {
            return await UpdateOrderStatus(changeStatusRequestDto, id, Enums.Status.Captured.GetDisplayName());
        }

        private async Task<ActionResult<OrderStatusResponseDto>>  UpdateOrderStatus(ChangeStatusRequestDto changeStatusRequestDto, Guid id, string status)
        {
            try
            {
                var update = await _orderService.UpdateOrderStatus(id, status);

                var responseStatus = new OrderStatusResponseDto()
                {
                    Id = id,
                    Status = status
                };

                return Ok(responseStatus);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
