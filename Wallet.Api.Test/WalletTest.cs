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

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Deposit_NonPositiveAmount_Throws(decimal amount)
        {
            var wallet = new WalletModel(userId: 1);

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => wallet.Deposit(amount));

            Assert.Equal("amount", ex.ParamName);
        }

        [Fact]
        public void Deposit_ExceedMaxBalance_Throws_InvalidOperationException()
        {
            var wallet = new WalletModel(userId: 1);
            wallet.Deposit(999999999999999999);

            Assert.Throws<InvalidOperationException>(() => wallet.Deposit(amount: 1));
            Assert.Equal(999999999999999999, wallet.Balance);
        }

    }
}
