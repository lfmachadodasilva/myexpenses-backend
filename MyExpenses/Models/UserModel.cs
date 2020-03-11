using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyExpenses.Models
{
    [Table(ModelConstants.TableUser)]
    public class UserModel : ModelBaseString
    {
        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string PhotoUrl { get; set; }

        public ICollection<GroupUserModel> GroupUser { get; set; }
    }
}
