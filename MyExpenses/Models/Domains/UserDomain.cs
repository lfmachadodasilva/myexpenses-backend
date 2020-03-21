namespace MyExpenses.Models.Domains
{
    public class UserDomain : DomainBase<string>
    {
        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string PhotoUrl { get; set; }
    }
}
