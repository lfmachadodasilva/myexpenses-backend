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

    [Table(ModelConstants.TableExpense)]
    public class ExpenseModel : ModelBaseNumber
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
}
