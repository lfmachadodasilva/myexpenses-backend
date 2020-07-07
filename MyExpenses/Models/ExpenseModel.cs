using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyExpenses.Models
{
    public enum ExpenseType
    {
        Incoming = 0,
        Outcoming
    }

    public abstract class ExpenseBaseFieldsModel
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
    }

    public abstract class ExpenseBaseFieldsWithIdModel : ModelBaseNumber
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
    }

    [Table(ModelConstants.TableExpense)]
    public class ExpenseModel : ExpenseBaseFieldsWithIdModel
    {
        #region Relations

        public long LabelId { get; set; }

        [ForeignKey(ModelConstants.ForeignKeyLabel)]
        public LabelModel Label { get; set; }

        [Required]
        public long GroupId { get; set; }

        [ForeignKey(ModelConstants.ForeignKeyGroup)]
        public GroupModel Group { get; set; }

        #endregion
    }

    public class ExpenseFullModel : ExpenseBaseFieldsWithIdModel
    {
        public LabelManageModel Label { get; set; }
    }

    public class ExpenseAddModel : ExpenseBaseFieldsModel
    {
        public long LabelId { get; set; }
        public long GroupId { get; set; }
    }

    public class ExpenseManageModel : ExpenseBaseFieldsWithIdModel
    {
        public long LabelId { get; set; }
        public long GroupId { get; set; }
    }
}
