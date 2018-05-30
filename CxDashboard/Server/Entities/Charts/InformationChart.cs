using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Charts
{
    public class InformationChart
    {
        public string name { get; set; }
        public string Data { get; set; }
        public string color { get; set; }
        public string Marx { get; set; }
        public List<InformationChartData> ChartData { get; set; }
    }
}
