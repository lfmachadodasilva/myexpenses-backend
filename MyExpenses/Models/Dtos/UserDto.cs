namespace MyExpenses.Models.Dtos
{
    public class UserDto : DtoBase<string>
    {
        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string PhotoUrl { get; set; }
    }
}
