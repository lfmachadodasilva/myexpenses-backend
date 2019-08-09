using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace lfmachadodasilva.MyExpenses.Api.Models
{
    [Table("Group")]
    public class GroupModel : ModelBase
    {
        public string Name { get; set; }

        public IEnumerable<UserGroupModel> UserGroups { get; set; }

        public GroupModel()
        {
            UserGroups = new List<UserGroupModel>();
        }
    }
}
