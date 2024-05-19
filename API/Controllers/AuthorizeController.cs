using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using Application.DTOs;
using Domain.Enums;
using Application.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizeController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public AuthorizeController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IEnumerable<TransactionResponseDto>> GetTransactionsWithCapturedStatus()
        {
            return await _transactionService.GetAllWithStatusAndPagination();
        }

        [HttpPost]
        public async Task<ActionResult<TransactionStatusResponseDto>> AddTransaction(CreateTransactionDto transactionDto)
        {
            var responseStatus = await _transactionService.AddTransaction(transactionDto);

            return Ok(responseStatus);
        }

        [HttpPut("{id}/voids")]
        public async Task<ActionResult<TransactionStatusResponseDto>> ChangeTransactionStatusToVoid(Guid id, ChangeStatusRequestDto changeStatusRequestDto)
        {
            return await UpdateTransactionStatus(changeStatusRequestDto, id, Enums.Status.Voided.GetDisplayName());
        }

        [HttpPut("{id}/capture")]
        public async Task<ActionResult<TransactionStatusResponseDto>> ChangeTransactionStatusToCapture(Guid id, ChangeStatusRequestDto changeStatusRequestDto)
        {
            return await UpdateTransactionStatus(changeStatusRequestDto, id, Enums.Status.Captured.GetDisplayName());
        }

        private async Task<ActionResult<TransactionStatusResponseDto>>  UpdateTransactionStatus(ChangeStatusRequestDto changeStatusRequestDto, Guid id, string status)
        {
            var isUpdated = await _transactionService.UpdateTransactionStatus(id, status);

            if (isUpdated)
            {
                var responseStatus = new TransactionStatusResponseDto()
                {
                    Id = id,
                    Status = status
                };

                return Ok(responseStatus);
            }
            else
            {
                return BadRequest("Not Updated");
            }
            
        }
    }
}
