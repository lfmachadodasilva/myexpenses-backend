using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lfmachadodasilva.MyExpenses.Api.Models.Dtos
{
    [Table("Expense")]
    public class ExpenseModel : ModelBase
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Value { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public ExpenseType Type { get; set; }

        #region Relations

        public long LabelId { get; set; }

        [Required]
        public long GroupId { get; set; }

        #endregion
    }
}
