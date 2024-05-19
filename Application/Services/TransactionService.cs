using Application.Data;
using Application.DTOs;
using AutoMapper;
using Domain;
using Domain.Enums;
using Domain.Transaction;

namespace Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TransactionResponseDto>> GetAllWithStatusAndPagination(int pageNo = 1, int pageSize = 10)
        {
            var transaction = (await _transactionRepository.GetAllWithStatusCapturedAndPagination(pageNo, pageSize)).Select(
                    row => new TransactionResponseDto
                    {
                        Amount = row.Amount,
                        Currency = row.Currency,
                        CardHolderNumber = Util.MaskCardNumber(row.CardHolderNumber),
                        HolderName = row.HolderName,
                        Id = row.Guid,
                        Status = row.Status
                    });

            return transaction;
        }

        public async Task<TransactionStatusResponseDto> AddTransaction(CreateTransactionDto transactionDto)
        {
            var transaction = _mapper.Map<Transaction>(transactionDto);
            var status = Enums.Status.Authorized.ToString();

            transaction.Status = status;
            var newTransactionGuid = await _transactionRepository.Add(transaction);

            var responseStatus = new TransactionStatusResponseDto()
            {
                Id = newTransactionGuid,
                Status = status
            };

            return responseStatus;
        }

        public async Task<bool> UpdateTransactionStatus(Guid id, string status)
        {
            return await _transactionRepository.UpdateTransactionStatus(id, status);
        }
    }
}
