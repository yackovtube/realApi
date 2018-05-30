using System;
using System.ComponentModel.DataAnnotations;

namespace CxDashboard.Entities
{
    public class ApplicationOlderVersionGrade
    {
        [Key]
        public int ApplicationVersionGradeId { get; set; }
        [Required]
        public string ApplicationVersion { get; set; }
        public double RC1QuelityGrade { get; set; }
        public DateTimeOffset RC1DateTime { get; set; }
        public double GAQuelityGrade { get; set; }
        public DateTimeOffset GADateTime { get; set; }
        public MainDashboard MainDashboard { get; set; }
    }
}
