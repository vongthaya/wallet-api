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

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void CreateWallet_InvalidOwnerName_ThrowsArgumentException(string? ownerName)
        {
            var e = Assert.Throws<ArgumentException>(() => new WalletModel(ownerName!));
            Assert.Equal("ownerName", e.ParamName);
        }

        [Fact]
        public void CreateWallet_Balance_Zero()
        {
            var wallet = new WalletModel("Kou");
            Assert.Equal(0m, wallet.Balance);
        }
    }
}
