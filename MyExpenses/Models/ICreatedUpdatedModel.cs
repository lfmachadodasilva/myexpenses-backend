using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyExpenses.Models
{
    public interface ICreatedUpdatedModel
    {
        #region Created by
        string CreatedById { get; set; }
        UserModel CreatedBy { get; set; }
        DateTime Created { get; set; }
        #endregion

        #region Updated by
        string UpdatedById { get; set; }
        [ForeignKey(ModelConstants.ForeignKeyGroup)]
        UserModel UpdatedBy { get; set; }
        DateTime Updated { get; set; }
        #endregion
    }
}