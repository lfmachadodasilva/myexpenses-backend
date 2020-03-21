using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyExpenses.Models.Domains
{
    public interface IDomain<T>
    {
        /// <summary>
        /// Id
        /// </summary>
        T Id { get; set; }
    }

    public class DomainBase<T> : IModel<T>
    {
        /// <inheritdoc />
        public T Id { get; set; }
    }
}
