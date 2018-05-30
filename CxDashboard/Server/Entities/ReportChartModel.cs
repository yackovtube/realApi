using Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class ReportChartModel
    {
        [Key]
        public int ReportChartId { get; set; }
        [Required(ErrorMessage = "Chart name is required", AllowEmptyStrings = false)]
        public string ChartName { get; set; }
        [Required(ErrorMessage = "Query name is required", AllowEmptyStrings = false)]
        public string QueryId { get; set; }
        [Required(ErrorMessage = "Group by is required", AllowEmptyStrings = false)]
        public string ColumnReferenceName { get; set; }
        public string aggrigationColumnReferenceName { get; set; }
        public int AggregationType { get; set; }
        [Required(ErrorMessage = "Report category is required")]
        [Range(1, double.MaxValue, ErrorMessage = "You have to select report category")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Report name is required")]
        [Range(1, double.MaxValue, ErrorMessage = "You have to select report name")]
        public int ReportName { get; set; }
        public DataShowType DataShowType { get; set; }
    }
}
