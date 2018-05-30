using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CxDashboard.Entities.Charts
{
    public class Linear : Chart
    {
        public string xKey { get; set; }
        public LineData Data { get; set; }
        public string[] yKeys { get; set; }
        public string[] Labels { get; set; }
    }

    public class LineData
    {
        public string Y { get; set; }
        public int? A { get; set; }
        public int? B { get; set; }
    }
}
