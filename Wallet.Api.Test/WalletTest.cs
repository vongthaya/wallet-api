using WalletModel = Wallet.Api.Models.Wallet;

namespace Wallet.Api.Test
{
    public class WalletTest
    {
        [Fact]
        public void CreateWallet_SetUserId()
        {
            var wallet = new WalletModel(5);
            Assert.Equal(5, wallet.UserId);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CreateWallet_InvalidUserId_ThrowsArgumentException(int userId)
        {
            var e = Assert.Throws<ArgumentException>(() => new WalletModel(userId));
            Assert.Equal("userId", e.ParamName);
        }

        [Fact]
        public void CreateWallet_Balance_Zero()
        {
            var wallet = new WalletModel(5);
            Assert.Equal(0m, wallet.Balance);
        }
    }
}
