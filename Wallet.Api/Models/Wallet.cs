namespace Wallet.Api.Models
{
    public class Wallet
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public decimal Balance { get; private set; } = 0;
        public DateTime CreatedDt { get; private set; }
        public User Owner { get; private set; } = null!;
        private const decimal MAX_BALANCE = 999999999999999999m;

        public Wallet(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User Id is required", nameof(userId));
            }
            UserId = userId;
        }

        public void Deposit(decimal amount)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
            var newBalance = Balance + amount;
            if (newBalance > MAX_BALANCE)
            {
                throw new InvalidOperationException("Deposit would exceed the maximum balance");
            }
            Balance += amount;
        }
    }
}
