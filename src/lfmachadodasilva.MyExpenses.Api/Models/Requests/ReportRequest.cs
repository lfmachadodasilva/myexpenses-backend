using System;
using System.ComponentModel.DataAnnotations;

namespace lfmachadodasilva.MyExpenses.Api.Models.Requests
{
    public class ReportRequest
    {
        [Required]
        public long GroupId { get; set; }

        public int Year { get; set; } = DateTime.Today.Year;
    }
}
