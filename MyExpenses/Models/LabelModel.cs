using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MyExpenses.Models
{
    public interface ILabelFields
    {
        string Name { get; set; }

        string Icon { get; set; }
    }

    [Table(ModelConstants.TableLabel)]
    public class LabelModel : ModelBase, ILabelFields
    {
        [Required]
        public string Name { get; set; }

        public string Icon { get; set; }

        #region Relations

        [Required]
        public long GroupId { get; set; }

        [ForeignKey(ModelConstants.ForeignKeyGroup)]
        public GroupModel Group { get; set; }

        public ICollection<ExpenseModel> Expenses { get; set; }

        #endregion

        public LabelModel()
        {
            Expenses = new List<ExpenseModel>();
        }

        public override bool CheckIfIsForbidden(string user)
        {
            return Group != null && !Group.GroupUser.Any(gu => gu.UserId.Equals(user));
        }
    }

    public class LabelAddModel : ILabelFields
    {
        [Required]
        public string Name { get; set; }

        public string Icon { get; set; }
    }

    public class LabelManageModel : IModel<long>, ILabelFields
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Icon { get; set; }
    }

    public class LabelGetFullModel : ModelBase, ILabelFields
    {
        [Required]
        public string Name { get; set; }

        public string Icon { get; set; }

        public decimal CurrValue { get; set; }

        public decimal LastValue { get; set; }

        public decimal AvgValue { get; set; }
    }
}
