namespace Wallet.Api.Models
{
    public class Wallet
    {
        public string OwnerName { get; }
        public decimal Balance { get; private set; } = 0;

        public Wallet(string ownerName)
        {
            if (string.IsNullOrWhiteSpace(ownerName))
            {
                throw new ArgumentException("OwnerName is required", nameof(ownerName));
            }
            OwnerName = ownerName;
        }
    }
}
