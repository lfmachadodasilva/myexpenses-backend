using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyExpenses.Models
{
    public interface IModel<T>
    {
        /// <summary>
        /// Id
        /// </summary>
        T Id { get; set; }
    }

    public interface IModelValidate
    {
        bool CheckIfIsForbidden(string user);
    }

    public class ModelBase : IModel<long>, ICreatedUpdatedModel, IModelValidate
    {
        /// <inheritdoc />
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        #region Created by
        [NotMapped]
        public string CreatedById { get; set; }
        [NotMapped]
        [ForeignKey(ModelConstants.ForeignKeyGroup)]
        public UserModel CreatedBy { get; set; }
        [NotMapped]
        public DateTime Created { get; set; }
        #endregion

        #region Updated by
        [NotMapped]
        public string UpdatedById { get; set; }
        [NotMapped]
        [ForeignKey(ModelConstants.ForeignKeyGroup)]
        public UserModel UpdatedBy { get; set; }
        [NotMapped]
        public DateTime Updated { get; set; }
        #endregion

        public virtual bool CheckIfIsForbidden(string user) => false;
    }

    public class ModelBaseString : IModel<string>
    {
        /// <inheritdoc />
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
    }
}
