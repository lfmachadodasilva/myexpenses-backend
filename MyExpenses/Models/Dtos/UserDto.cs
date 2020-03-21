using System.ComponentModel.DataAnnotations;

namespace MyExpenses.Models.Dtos
{
    public class UserDto : DtoBase<string>
    {
        public string DisplayName { get; set; }

        [Required]
        public string Email { get; set; }

        public string PhotoUrl { get; set; }
    }
}
