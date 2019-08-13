﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using lfmachadodasilva.MyExpenses.Api.Models.Dtos;

namespace lfmachadodasilva.MyExpenses.Api.Models
{
    [Table("Expense")]
    public class ExpenseModel : ModelBase
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Value { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public ExpenseType Type { get; set; }

        #region Relations

        public long LabelId { get; set; }

        [ForeignKey("LabelId")]
        public LabelModel Label { get; set; }

        [Required]
        public long GroupId { get; set; }

        [ForeignKey("GroupId")]
        public LabelModel Group { get; set; }

        #endregion
    }
}
