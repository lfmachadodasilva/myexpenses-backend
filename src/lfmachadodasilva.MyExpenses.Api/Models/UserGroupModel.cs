using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lfmachadodasilva.MyExpenses.Api.Models
{
    [Table("UserGroup")]
    public class UserGroupModel
    {
        [Key]
        public long GroupId { get; set; }

        [Key]
        public long UserId { get; set; }
    }
}
