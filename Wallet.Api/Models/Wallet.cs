namespace Wallet.Api.Models
{
    public class Wallet
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public decimal Balance { get; private set; } = 0;

        public Wallet(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User Id is required", nameof(userId));
            }
            UserId = userId;
        }
    }
}
