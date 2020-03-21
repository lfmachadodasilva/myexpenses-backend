using System;
using System.ComponentModel.DataAnnotations;

namespace MyExpenses.Models.Dtos
{
    public class ExpenseDto : DtoBase<long>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Value { get; set; }

        [Required]
        public ExpenseType Type { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string Comments { get; set; }

        [Required]
        public long GroupdId { get; set; }

        [Required]
        public long LabelId { get; set; }
    }

    public class ExpenseDetailsDto : ExpenseDto
    {
        public string GroupdName { get; set; }

        public string LabelName { get; set; }
    }
}
