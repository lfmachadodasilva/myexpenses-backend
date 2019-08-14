using System;
using System.ComponentModel.DataAnnotations;

namespace lfmachadodasilva.MyExpenses.Api.Models.Dtos
{
    public class ExpenseDto : DtoBase
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

    public class ExpenseWithValuesDto : ExpenseDto
    {
        public string LabelName { get; set; }
    }
}
