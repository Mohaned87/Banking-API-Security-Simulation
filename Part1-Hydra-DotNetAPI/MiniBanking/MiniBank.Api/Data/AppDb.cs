using Microsoft.EntityFrameworkCore;
using MiniBank.Api.Models;

namespace MiniBank.Api.Data
{
    public class AppDb : DbContext
    {
        public AppDb(DbContextOptions<AppDb> options) : base(options) { }

        public DbSet<AppUser> Users => Set<AppUser>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Transaction> Transactions => Set<Transaction>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<Account>().HasIndex(a => a.AccountNumber).IsUnique();
            b.Entity<AppUser>().HasIndex(u => u.Email).IsUnique();
            base.OnModelCreating(b);
        }

    }

}
