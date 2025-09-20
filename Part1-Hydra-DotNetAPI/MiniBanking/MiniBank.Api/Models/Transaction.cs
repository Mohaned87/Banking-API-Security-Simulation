namespace MiniBank.Api.Models
{
    public class Transaction
    {
        public long Id { get; set; }
        public TxType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime UtcAt { get; set; } = DateTime.UtcNow;
        // للحوالات
        public int? FromAccountId { get; set; }
        public int? ToAccountId { get; set; }
        public string Description { get; set; } = "";
    }
}
