using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MyExpenses.Models
{
    public class UserModelBase : ModelBaseString
    {
    }

    [Table(ModelConstants.TableUser)]
    public class UserModel : UserModelBase
    {
        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string PhotoUrl { get; set; }
    }
}
