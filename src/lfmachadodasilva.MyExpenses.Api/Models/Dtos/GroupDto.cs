using System.Collections.Generic;

namespace lfmachadodasilva.MyExpenses.Api.Models.Dtos
{
    public class GroupAddDto
    {
        public string Name { get; set; }

        public IEnumerable<long> Users { get; set; }
    }

    public class GroupDto : DtoBase
    {
        public string Name { get; set; }

        public IEnumerable<long> Users { get; set; }

        public GroupDto()
        {
            Users = new List<long>();
        }
    }

    public class GroupWithValuesDto : DtoBase
    {
        public string Name { get; set; }

        public IEnumerable<UserDto> Users { get; set; }

        public GroupWithValuesDto()
        {
            Users = new List<UserDto>();
        }
    }
}
