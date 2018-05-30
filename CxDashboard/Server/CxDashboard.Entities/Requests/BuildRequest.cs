using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CxDashboard.Entities.Requests
{
    public class BuildRequest
    {
        public List<Value> value { get; set; }
        public int count { get; set; }
    }

    public class Value
    {
        public string uri { get; set; }
        public int id { get; set; }
        public string buildNumber { get; set; }
        public string url { get; set; }
        public DateTime startTime { get; set; }
        public DateTime finishTime { get; set; }
        public string reason { get; set; }
        public string status { get; set; }
    }
}
