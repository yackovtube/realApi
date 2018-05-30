using System.ComponentModel.DataAnnotations;

namespace CxDashboard.Entities
{
    public class Report
    {
        public int ReportId { get; set; }
        public string ReportName { get; set; }
        [Required]
        public Category Category { get; set; }
    }
}
