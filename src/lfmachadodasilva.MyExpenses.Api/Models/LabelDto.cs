namespace lfmachadodasilva.MyExpenses.Api.Models
{
    public class LabelDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public decimal CurrentValue { get; set; }

        public decimal LastValue { get; set; }

        public decimal AverageValue { get; set; }
    }
}
