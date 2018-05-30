using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CxDashboard.Entities.Charts
{
    public class GradeAndInformationData
    {
        public string Grade { get; set; }
        public List<InformationChartData> DataList { get; set; }
    }
}
