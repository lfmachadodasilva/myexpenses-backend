using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace lfmachadodasilva.MyExpenses.Api.Models
{
    [Table("User")]
    public class UserModel : ModelBase
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserGroupModel> UserGroups { get; set; }

        public UserModel()
        {
            UserGroups = new List<UserGroupModel>();
        }
    }
}
