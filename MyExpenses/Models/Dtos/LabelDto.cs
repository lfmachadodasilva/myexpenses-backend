using System.ComponentModel.DataAnnotations;

namespace MyExpenses.Models.Dtos
{
    public class LabelDto : DtoBase<long>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public long GroupId { get; set; }
    }

    public class LabelDetailsDto : LabelDto
    {
        public string GroupName { get; set; }

        public decimal CurrentValue { get; set; }
        public decimal LastMonthValue { get; set; }
        public decimal AverageValue { get; set; }
    }
}
