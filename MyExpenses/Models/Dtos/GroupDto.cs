using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyExpenses.Models.Dtos
{
    public class GroupDto : DtoBase<long>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public ICollection<string> Users { get; set; }
    }

    public class GroupDetailsDto : DtoBase<long>
    {
        public string Name { get; set; }

        public ICollection<UserDto> Users { get; set; }
    }
}
