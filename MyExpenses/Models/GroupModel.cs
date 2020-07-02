using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyExpenses.Models
{
    [Table(ModelConstants.TableGroup)]
    public class GroupModel : ModelBaseNumber
    {
        [Required]
        public string Name { get; set; }

        public ICollection<GroupUserModel> GroupUser { get; set; }
    }

    public class GroupGetModel : ModelBaseNumber
    {
        [Required]
        public string Name { get; set; }
    }

    public class GroupAddModel
    {
        [Required]
        public string Name { get; set; }

        public ICollection<UserModelBase> Users { get; set; }
    }

    public class GroupGetFullModel : GroupGetModel
    {
        public ICollection<UserModel> Users { get; set; }
    }

    public class GroupManageModel : GroupGetModel
    {
        public ICollection<UserModelBase> Users { get; set; }
    }
}
