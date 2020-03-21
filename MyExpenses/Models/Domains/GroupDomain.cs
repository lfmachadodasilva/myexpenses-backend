using System.Collections.Generic;

namespace MyExpenses.Models.Domains
{
    public class GroupDomain : DomainBase<long>
    {
        public string Name { get; set; }

        public ICollection<string> Users { get; set; }
    }

    public class GroupDetailsDomain : DomainBase<long>
    {
        public string Name { get; set; }

        public ICollection<UserDomain> Users { get; set; }
    }
}
