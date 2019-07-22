using System;
using System.ComponentModel.DataAnnotations;

namespace lfmachadodasilva.MyExpenses.Api.Models.Requests
{
    public class SearchRequest
    {
        [Required]
        public long GroupId { get; set; }

        public int Month { get; set; } = DateTime.Today.Month;

        public int Year { get; set; } = DateTime.Today.Year;
    }
}
