using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyExpenses.Models
{
    public interface IModel<T>
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        T Id { get; set; }
    }

    public class ModelBaseNumber : IModel<long>
    {
        /// <inheritdoc />
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
    }

    public class ModelBaseString : IModel<string>
    {
        /// <inheritdoc />
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
    }
}
