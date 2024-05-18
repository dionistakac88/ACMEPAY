using API.Common;
using API.Common.Enums;
using API.DTOs;
using Dapper;
using Domain;
using Microsoft.OpenApi.Extensions;
using System.Text.RegularExpressions;

namespace API.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;

        public OrderRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAll(int pageNo, int pageSize)
        {
            try
            {
                using (var conn = _connectionProvider.GetDatabaseConnection())
                {
                    int skip = (pageNo - 1) * pageSize;

                    var query = "Select amount, currency, card_holder_number CardHolderNumber, holder_name HolderName, guid Id, status " +
                        "from Orders " +
                        "where Status = @status " +
                        "ORDER BY Id " +
                        $"OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY";

                    var orders = (await conn.QueryAsync<OrderResponseDto>(query, new { status = Enums.Status.Captured.GetDisplayName() }))
                        .Select(
                        row => new OrderResponseDto
                        {
                            Amount = row.Amount,
                            Currency = row.Currency,
                            CardHolderNumber = Util.MaskCardNumber(row.CardHolderNumber),
                            HolderName = row.HolderName,
                            Id = row.Id,
                            Status = row.Status
                        });


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

        public async Task<bool> UpdateOrderStatus(ChangeStatusRequestDto changeOrderStatusRequestDto, Guid id, string status)
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
