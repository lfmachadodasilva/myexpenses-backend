namespace lfmachadodasilva.MyExpenses.Api.Models.Dtos
{
    public class PaymentDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        // Relations
        public long GroupId { get; set; }
    }

    public class PaymentWithValueDto : PaymentDto
    {
        public decimal CurrentValue { get; set; }

        public decimal LastValue { get; set; }

        public decimal AverageValue { get; set; }
    }
}
