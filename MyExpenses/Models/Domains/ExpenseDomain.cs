using System;

namespace MyExpenses.Models.Domains
{
    public class ExpenseDomain : DomainBase<long>
    {
        public string Name { get; set; }

        public decimal Value { get; set; }

        public ExpenseType Type { get; set; }

        public DateTime Date { get; set; }

        public string Comments { get; set; }

        public long GroupdId { get; set; }

        public long LabelId { get; set; }
    }

    public class ExpenseDetailsDomain : ExpenseDomain
    {
        public string GroupdName { get; set; }

        public string LabelName { get; set; }
    }
}
