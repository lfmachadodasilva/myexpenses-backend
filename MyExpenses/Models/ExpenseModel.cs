using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MyExpenses.Models
{
    public enum ExpenseType
    {
        Incoming = 0,
        Outcoming
    }

    public interface IExpenseFields
    {
        ExpenseType Type { get; set; }

        string Name { get; set; }

        decimal Value { get; set; }

        DateTime Date { get; set; }

        string Comments { get; set; }

        string PhotoUrl { get; set; }
    }

    [Table(ModelConstants.TableExpense)]
    public class ExpenseModel : ModelBase, IExpenseFields
    {
        [Required]
        public ExpenseType Type { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Value { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string Comments { get; set; }

        public string PhotoUrl { get; set; }

        #region Relations

        public long LabelId { get; set; }

        [ForeignKey(ModelConstants.ForeignKeyLabel)]
        public LabelModel Label { get; set; }

        [Required]
        public long GroupId { get; set; }

        [ForeignKey(ModelConstants.ForeignKeyGroup)]
        public GroupModel Group { get; set; }

        #endregion

        public override bool CheckIfIsForbidden(string user)
        {
            return Group != null && Label != null &&
                   (!Group.GroupUser.Any(gu => gu.UserId.Equals(user)) ||
                   !Label.Group.GroupUser.Any(gu => gu.UserId.Equals(user)));
        }
    }

    public class ExpenseFullModel : ModelBase, IExpenseFields
    {
        [Required]
        public ExpenseType Type { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Value { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string Comments { get; set; }

        public string PhotoUrl { get; set; }

        [Required]
        public LabelManageModel Label { get; set; }
    }

    public class ExpenseAddModel : IExpenseFields
    {
        [Required]
        public ExpenseType Type { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Value { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string Comments { get; set; }

        public string PhotoUrl { get; set; }

        public long LabelId { get; set; }
        public long GroupId { get; set; }
    }

    public class ExpenseManageModel : IModel<long>, IExpenseFields
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public ExpenseType Type { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Value { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string Comments { get; set; }

        public string PhotoUrl { get; set; }

        public long LabelId { get; set; }
        public long GroupId { get; set; }
    }
}
