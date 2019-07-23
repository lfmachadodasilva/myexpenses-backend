namespace lfmachadodasilva.MyExpenses.Api.Models
{
    public class LabelModel : ModelBase
    {
        public string Name { get; set; }

        // Relations
        public long GroupId { get; set; }
    }

    public class LabelWithValuesModel : LabelModel
    {
        public decimal CurrentValue { get; set; }

        public decimal LastValue { get; set; }

        public decimal AverageValue { get; set; }
    }
}
