using System.Collections.Generic;

namespace lfmachadodasilva.MyExpenses.Api.Models.Dtos
{
    public class GroupDto : DtoBase
    {
        public string Name { get; set; }

        public IEnumerable<UserDto> Users { get; set; }

        public GroupDto()
        {
            Users = new List<UserDto>();
        }
    }
}
