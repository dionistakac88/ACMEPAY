using Application.Data;
using Dapper;
using Domain.Enums;
using Domain.Transaction;
using Infrastructure.Data;
using static Domain.Enums.Enums;

namespace Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;

        public TransactionRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public async Task<IEnumerable<Transaction>> GetAllWithStatusCapturedAndPagination(int pageNo, int pageSize)
        {
            using (var conn = _connectionProvider.GetDatabaseConnection())
            {
                int skip = (pageNo - 1) * pageSize;

                var query = "Select Id, amount, currency, card_holder_number CardHolderNumber, holder_name HolderName, guid, status, " +
                    "expiration_month ExpirationMonth, expiration_year ExpirationYear, cvv Cvv, order_reference OrderReference " +
                    "from Transactions " +
                    "where Status = @status " +
                    "ORDER BY Id " +
                    $"OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY";


                var transactions = (await conn.QueryAsync<Transaction>(query, new { Status = Enums.Status.Captured.ToString() })).ToList();


                return transactions;
            }
        }

        public async Task<Guid> Add(Transaction transaction)
        {
            using (var conn = _connectionProvider.GetDatabaseConnection())
            {
                var query = "Insert into Transactions (amount, currency, card_holder_number, holder_name, expiration_month, " +
                                                "expiration_year, cvv, order_reference, status) " +
                                                "OUTPUT INSERTED.guid " +
                            "VALUES (@Amount, @Currency, @CardHolderNumber, @HolderName, @ExpirationMonth, " +
                            "@ExpirationYear, @Cvv, @OrderReference, @Status)";

                var guid = await conn.QuerySingleAsync<Guid>(query, transaction);

                return guid;
            }
        }

        public async Task<bool> UpdateTransactionStatus(Guid id, string status)
        {
            using (var conn = _connectionProvider.GetDatabaseConnection())
            {
                var query = "Update Transactions set status = @Status where guid = @Id";

                var rowsAffected = await conn.ExecuteAsync(query, new
                {
                    Id = id,
                    Status = status,
                });

                return rowsAffected > 0;
            }
        }
    }
}
