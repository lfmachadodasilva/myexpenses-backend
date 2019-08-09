using System.Collections.Generic;

namespace lfmachadodasilva.MyExpenses.Api.Models.Dtos
{
    public class ReportDto
    {
        public ReportByYear ByYear { get; set; }

        public IEnumerable<ReportByLabelDto> ByLabel { get; set; }

        public IEnumerable<ReportByMonth> ByMonth { get; set; }
    }

    public class ReportByLabelDto
    {
        /// <summary>
        /// Label name
        /// </summary>
        public string LabelName { get; set; }

        /// <summary>
        /// Average value
        /// </summary>
        public decimal AverageValue { get; set; }
    }

    /// <summary>
    /// Class to report values by year
    /// </summary>
    public class ReportByYear
    {
        /// <summary>
        /// Year
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Total value
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Total value left
        /// </summary>
        public decimal Left { get; set; }

        /// <summary>
        /// Total average value
        /// </summary>
        public decimal TotalAverage { get; set; }

        /// <summary>
        /// Total average left
        /// </summary>
        public decimal LeftAverage { get; set; }
    }

    /// <summary>
    /// Class to report values by month
    /// </summary>
    public class ReportByMonth
    {
        /// <summary>
        /// Month
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Total value spent in the month
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Total value left in the month
        /// </summary>
        public decimal Left { get; set; }
    }
}
