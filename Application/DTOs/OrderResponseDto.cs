namespace Application.DTOs
{
    public class OrderResponseDto
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string CardHolderNumber { get; set; }
        public string HolderName { get; set; }
        public Guid Id { get; set; }
        public string Status { get; set; }

    }
}
