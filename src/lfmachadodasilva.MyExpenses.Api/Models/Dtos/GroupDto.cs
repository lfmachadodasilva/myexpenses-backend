using System.Collections.Generic;

namespace lfmachadodasilva.MyExpenses.Api.Models.Dtos
{
    public class GroupAdd
    {
        public string Name { get; set; }

        public IEnumerable<long> Users { get; set; }
    }

    public class GroupBaseDto : DtoBase
    {
        public string Name { get; set; }
    }

    public class GroupDto : GroupBaseDto
    {
        public IEnumerable<long> Users { get; set; }

        public GroupDto()
        {
            Users = new List<long>();
        }
    }

    public class GroupWithValuesDto : GroupBaseDto
    {
        public IEnumerable<UserDto> Users { get; set; }

        public GroupWithValuesDto()
        {
            Users = new List<UserDto>();
        }
    }
}
