using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using MyExpenses.Helpers;

namespace MyExpenses.Models
{
    public interface IGroupFields
    {
        string Name { get; set; }
    }

    [Table(ModelConstants.TableGroup)]
    public class GroupModel : ModelBase, IGroupFields
    {
        [Required]
        public string Name { get; set; }

        public ICollection<GroupUserModel> GroupUser { get; set; }

        public override bool CheckIfIsForbidden(string user)
        {
            return GroupUser != null &&
                   !GroupUser.Any(gu => gu.UserId.Equals(user));
        }
    }

    public class GroupGetModel : IModel<long>, IGroupFields
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
    }

    public class GroupAddModel : IGroupFields
    {
        [Required]
        public string Name { get; set; }

        public ICollection<UserModelBase> Users { get; set; }
    }

    public class GroupGetFullModel : ModelBase, IGroupFields
    {
        [Required]
        public string Name { get; set; }

        public ICollection<UserModel> Users { get; set; }
    }

    public class GroupManageModel : GroupGetModel
    {
        public ICollection<UserModelBase> Users { get; set; }
    }
}
