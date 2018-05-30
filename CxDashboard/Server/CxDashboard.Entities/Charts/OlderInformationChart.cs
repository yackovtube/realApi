using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CxDashboard.Entities.Charts
{
    public class OlderInformationChart: BaseInformationChart
    {
        public string ApplicationVersion { get; set; }
        public List<InformationChartData> ChartData { get; set; }
    }
}
