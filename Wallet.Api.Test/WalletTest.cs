using WalletModel = Wallet.Api.Models.Wallet;

namespace Wallet.Api.Test
{
    public class WalletTest
    {
        [Fact]
        public void CreateWallet_SetOwnerName()
        {
            var kou = new WalletModel("Kou");
            Assert.Equal("Kou", kou.OwnerName);
        }
    }
}
