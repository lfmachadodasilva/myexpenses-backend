using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lfmachadodasilva.MyExpenses.Api.Models
{
    [Table("UserGroup")]
    public class UserGroupModel
    {
        [Key]
        public Guid GroupId { get; set; }

        [Key]
        public Guid UserId { get; set; }
    }
}
