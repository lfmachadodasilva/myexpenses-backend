using System.ComponentModel.DataAnnotations;

namespace lfmachadodasilva.MyExpenses.Api.Models
{
    public class ModelBase : IModel
    {
        [Key]
        public long Id { get; set; }
    }
}
