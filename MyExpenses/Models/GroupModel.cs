using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyExpenses.Models
{
    [Table(ModelConstants.TableGroup)]
    public class GroupModel : ModelBaseNumber
    {
        [Required]
        public string Name { get; set; }

        public ICollection<GroupUserModel> GroupUser { get; set; }
    }
}
