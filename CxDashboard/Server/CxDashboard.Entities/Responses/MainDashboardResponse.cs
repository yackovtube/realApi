using CxDashboard.Entities.Charts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CxDashboard.Entities.Responses
{
    public class MainDashboardResponse
    {
        public string TimeLine { get; set; }
        public List<ApplicationOlderVersionGradeResponse> OldVersionApplicationList { get; set; }
        public List<OlderInformationChart> OldVersionEngineList { get; set; }
        public InformationChart CurrentApplicationKPIsGrade { get; set; }
        public OlderInformationChart CurrentEngineKPIsGrade { get; set; }
        public IEnumerable<BaseInformationChart> SmokeGrades { get; set; }
        public IEnumerable<BaseInformationChart> EngineCodeCoverage { get; set; }
        public IEnumerable<BaseInformationChart> ApplicationCodeCoverage { get; set; }
    }
}
