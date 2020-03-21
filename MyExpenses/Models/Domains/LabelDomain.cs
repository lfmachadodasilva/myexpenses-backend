namespace MyExpenses.Models.Domains
{
    public class LabelDomain : DomainBase<long>
    {
        public string Name { get; set; }

        public long GroupId { get; set; }
    }

    public class LabelDomainsDto : LabelDomain
    {
        public string GroupName { get; set; }

        public decimal CurrentValue { get; set; }
        public decimal LastMonthValue { get; set; }
        public decimal AverageValue { get; set; }
    }
}
