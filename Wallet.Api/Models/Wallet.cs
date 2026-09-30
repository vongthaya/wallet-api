namespace Wallet.Api.Models
{
    public class Wallet
    {
        public string OwnerName { get; }

        public Wallet(string ownerName)
        {
            OwnerName = ownerName;
        }
    }
}
