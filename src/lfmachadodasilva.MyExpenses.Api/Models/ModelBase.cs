using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lfmachadodasilva.MyExpenses.Api.Models
{
    public class ModelBase : IModel
    {
        /// <inheritdoc />
        [Key]
        public long Id { get; set; }
    }
}
