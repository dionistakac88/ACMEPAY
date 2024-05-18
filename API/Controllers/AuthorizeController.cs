using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Transactions;
using Dapper;
using API.Repository;
using API.Common.Enums;
using Microsoft.OpenApi.Extensions;
using API.DTOs;
using API.Common.Core;
using AutoMapper;
using static API.Common.Enums.Enums;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizeController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public AuthorizeController(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<OrderResponseDto>> GetOrders()
        {
            return await _orderRepository.GetAll();
        }

        [HttpPost]
        public async Task<ActionResult<OrderStatusResponseDto>> AddTransaction(CreateOrderDto orderDto)
        {
            try
            {
                var order = _mapper.Map<Order>(orderDto);
                var status = Enums.Status.Authorized.GetDisplayName();

                order.Status = status;
                var newOrderGuid = await _orderRepository.Add(order);

                var responseStatus = new OrderStatusResponseDto()
                {
                    Id = newOrderGuid,
                    Status = status
                };

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
                var update = await _orderRepository.UpdateOrderStatus(changeStatusRequestDto, id, status);

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
