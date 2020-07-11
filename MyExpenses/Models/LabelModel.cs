using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyExpenses.Models
{
    [Table(ModelConstants.TableLabel)]
    public class LabelModel : ModelBaseNumber
    {
        public string Name { get; set; }

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
    }

    public class LabelValueModel : ModelBaseNumber
    {
        public decimal Value { get; set; }
    }

    public class LabelAddModel
    {
        [Required]
        public string Name { get; set; }
    }

    public class LabelManageModel : ModelBaseNumber
    {
        [Required]
        public string Name { get; set; }
    }

    public class LabelGetFullModel : LabelManageModel
    {
        public decimal CurrValue { get; set; }

        public decimal LastValue { get; set; }

        public decimal AvgValue { get; set; }
    }
}
