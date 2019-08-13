using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace lfmachadodasilva.MyExpenses.Api.Models
{
    [Table("Label")]
    public class LabelModel : ModelBase
    {
        public string Name { get; set; }

        // Relations
        public long GroupId { get; set; }

        public IEnumerable<ExpenseModel> Expenses { get; set; }
    }
}
