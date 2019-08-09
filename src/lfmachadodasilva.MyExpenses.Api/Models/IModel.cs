using System.ComponentModel.DataAnnotations;

namespace lfmachadodasilva.MyExpenses.Api.Models
{
    public interface IModel
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        long Id { get; set; }
    }
}
