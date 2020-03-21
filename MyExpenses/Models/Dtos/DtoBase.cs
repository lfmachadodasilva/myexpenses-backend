namespace MyExpenses.Models.Dtos
{
    public interface IDto<T>
    {
        /// <summary>
        /// Id
        /// </summary>
        T Id { get; set; }
    }

    public class DtoBase<T> : IModel<T>
    {
        /// <inheritdoc />
        public T Id { get; set; }
    }
}
