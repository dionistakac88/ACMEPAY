using Application.Data;
using Dapper;
using Domain.Enums;
using Domain.Order;
using Infrastructure.Data;
using static Domain.Enums.Enums;

namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;

        public OrderRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public async Task<IEnumerable<Order>> GetAllWithStatusCapturedAndPagination(int pageNo, int pageSize)
        {
            try
            {
                using (var conn = _connectionProvider.GetDatabaseConnection())
                {
                    int skip = (pageNo - 1) * pageSize;

                    var query = "Select Id, amount, currency, card_holder_number CardHolderNumber, holder_name HolderName, guid, status, " +
                        "expiration_month ExpirationMonth, expiration_year ExpirationYear, cvv Cvv, order_reference OrderReference " +
                        "from Orders " +
                        "where Status = @status " +
                        "ORDER BY Id " +
                        $"OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY";


                    var orders = (await conn.QueryAsync<Order>(query, new { Status = Enums.Status.Captured.ToString() })).ToList();


                    return orders;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Guid> Add(Order order)
        {
            try
            {
                using (var conn = _connectionProvider.GetDatabaseConnection())
                {
                    var query = "Insert into Orders (amount, currency, card_holder_number, holder_name, expiration_month, " +
                                                    "expiration_year, cvv, order_reference, status) " +
                                                    "OUTPUT INSERTED.guid " +
                                "VALUES (@Amount, @Currency, @CardHolderNumber, @HolderName, @ExpirationMonth, " +
                                "@ExpirationYear, @Cvv, @OrderReference, @Status)";

                    var guid = await conn.QuerySingleAsync<Guid>(query, order);

                    return guid;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateOrderStatus(Guid id, string status)
        {
            try
            {
                using (var conn = _connectionProvider.GetDatabaseConnection())
                {
                    var query = "Update Orders set status = @Status where guid = @Id";

                    var rowsAffected = await conn.ExecuteAsync(query, new
                    {
                        Id = id,
                        Status = status,
                    });

                    return rowsAffected > 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
