using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CxDashboard.Entities.Responses
{
    public class ReportChartModelResponse
    {
        public int ReportChartId { get; set; }
        public string ChartName { get; set; }
        public string QueryId { get; set; }
        public string ColumnReferenceName { get; set; }
        public string aggregationColumnReferenceName { get; set; }
        public string ChartType { get; set; }
        public string CategoryId { get; set; }
        public string ReportName { get; set; }
        public string DataShowType { get; set; }
    }
}
