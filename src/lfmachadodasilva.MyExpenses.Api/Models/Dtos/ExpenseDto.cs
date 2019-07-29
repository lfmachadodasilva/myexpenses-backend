using System;
using System.ComponentModel.DataAnnotations;

namespace lfmachadodasilva.MyExpenses.Api.Models.Dtos
{
    public class ExpenseDto
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Value { get; set; }

        [Required]
        public DateTime Date { get; set; }

        #region Relations

        public long LabelId { get; set; }

        public long PaymentId { get; set; }

        [Required]
        public long GroupId { get; set; }

        #endregion
    }

    public class ExpenseWithValuesDto : ExpenseDto
    {
        public string LabelName { get; set; }

        public string PaymentName { get; set; }

        public decimal LastValue { get; set; }

        public decimal AverageValue { get; set; }
    }
}
