using Application.DTOs;

namespace Application.Services
{
    public interface ITransactionService
    {
        Task<TransactionStatusResponseDto> AddTransaction(CreateTransactionDto transactionDto);

        Task<IEnumerable<TransactionResponseDto>> GetAllWithStatusAndPagination(int pageNo = 1, int pageSize = 10);

        Task<bool> UpdateTransactionStatus(Guid id, string status);
    }
}
