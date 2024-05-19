using Domain.Transaction;

namespace Application.Data
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAllWithStatusCapturedAndPagination(int pageNo = 1, int pageSize = 10);

        Task<Guid> Add(Transaction transaction);

        Task<bool> UpdateTransactionStatus(Guid id, string status);
    }
}