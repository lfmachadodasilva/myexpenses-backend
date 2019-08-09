namespace lfmachadodasilva.MyExpenses.Api.Models.Dtos
{
    public class LabelDto : DtoBase
    {
        public string Name { get; set; }

        // Relations
        public long GroupId { get; set; }
    }

    public class LabelWithValuesDto : LabelDto
    {
        public decimal CurrentValue { get; set; }

        public decimal LastValue { get; set; }

        public decimal AverageValue { get; set; }
    }
}
