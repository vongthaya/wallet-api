using Wallet.Api.Models;

namespace Wallet.Api.Test
{
    public class UserTest
    {
        [Fact]
        public void CreateUser_SetFirstName_LastName()
        {
            var kou = new User("Kou", "Phounsavanh");
            Assert.Equal("Kou", kou.FirstName);
            Assert.Equal("Phounsavanh", kou.LastName);
        }

        [Theory]
        [InlineData("", "Phounsavanh")]
        [InlineData(" ", "Phounsavanh")]
        [InlineData(null, "Phounsavanh")]
        public void CreateUser_InvalidFirstName(string? firstName, string lastName)
        {
            var e = Assert.Throws<ArgumentException>(() => new User(firstName!, lastName));
            Assert.Equal("firstName", e.ParamName);
        }

        [Theory]
        [InlineData("Kou", "")]
        [InlineData("Kou", " ")]
        [InlineData("Kou", null)]
        public void CreateUser_InvalidLastName(string firstName, string? lastName)
        {
            var e = Assert.Throws<ArgumentException>(() => new User(firstName, lastName!));
            Assert.Equal("lastName", e.ParamName);
        }
    }
}
