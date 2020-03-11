using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyExpenses.Models
{
    [Table(ModelConstants.TableGroupUser)]
    public class GroupUserModel
    {
        [Required]
        public long GroupId { get; set; }

        [ForeignKey("GroupId")]
        public GroupModel Group { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public UserModel User { get; set; }
    }
}
