using System.ComponentModel.DataAnnotations.Schema;

namespace lfmachadodasilva.MyExpenses.Api.Models.Dtos
{
    public class UserDto : DtoBase
    {
        public string Name { get; set; }

        public string Email { get; set; }
    }
}
