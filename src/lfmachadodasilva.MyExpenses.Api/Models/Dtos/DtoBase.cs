using System.ComponentModel.DataAnnotations;

namespace lfmachadodasilva.MyExpenses.Api.Models.Dtos
{
    public class DtoBase : IDto
    {
        /// <inheritdoc />
        [Required]
        public long Id { get; set; }
    }
}
