namespace Wallet.Api.Models
{
    public class User
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public User(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("Firstname is required", nameof(firstName));
            }
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("Lastname is required", nameof(lastName));
            }
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
