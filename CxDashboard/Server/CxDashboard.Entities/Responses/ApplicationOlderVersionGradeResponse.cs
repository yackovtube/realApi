using CxDashboard.Entities.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CxDashboard.Entities.Responses
{
    public class ApplicationOlderVersionGradeResponse
    {
        public int ApplicationOlderVersionGradeId { get; set; }
        public string ApplicationVersion { get; set; }
        public double RC1QualityGrade { get; set; }
        public string RC1DateTime { get; set; }
        public GradeAndInformationData RC1ActualGrade { get; set; }
        public double GAQualityGrade { get; set; }
        public string GADateTime { get; set; }
        public GradeAndInformationData GAActualGrade { get; set; }
    }
}
