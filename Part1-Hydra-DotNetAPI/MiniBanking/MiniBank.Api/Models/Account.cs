namespace MiniBank.Api.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = null!;
        public string Currency { get; set; } = "USD";
        public decimal Balance { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
