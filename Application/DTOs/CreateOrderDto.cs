namespace Application.DTOs
{
    public class CreateOrderDto
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string CardHolderNumber { get; set; }
        public string HolderName { get; set; }
        public int ExpirationMonth { get; set; }
        public int ExpirationYear { get; set; }
        public int Cvv { get; set; }
        public string OrderReference { get; set; }
    }
}
