using Microsoft.EntityFrameworkCore;
using Wallet.Api.Models;
using WalletModel = Wallet.Api.Models.Wallet;

namespace Wallet.Api.Data
{
    public class WalletDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<WalletModel> Wallets {get; set; }

        public WalletDbContext(DbContextOptions<WalletDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<WalletModel>(entity =>
            {
                entity.Property(w => w.Balance)
                    .HasPrecision(18, 0)
                    .HasDefaultValue(0m);
                entity.ToTable(t => t.HasCheckConstraint("ck_wallets_balance_nonnegative", "balance >= 0"));
                entity.HasOne<User>()
                    .WithOne()
                    .HasForeignKey<WalletModel>(w => w.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(w => w.CreatedDt)
                    .HasDefaultValueSql("now()");
            });
        }
    }
}
