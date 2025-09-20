using System.Security.Principal;

namespace MiniBank.Api.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string NationalId { get; set; } = ""; // For Testing
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
