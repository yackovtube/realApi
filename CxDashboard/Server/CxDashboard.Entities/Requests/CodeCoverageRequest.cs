using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CxDashboard.Entities.Requests
{
    public class CodeCoverageRequest
    {
        public Coveragedata[] coverageData { get; set; }
        public Build build { get; set; }
        public Deltabuild deltaBuild { get; set; }
    }

    public class Build
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Deltabuild
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Coveragedata
    {
        public Coveragestat[] coverageStats { get; set; }
        public string buildPlatform { get; set; }
        public string buildFlavor { get; set; }
    }

    public class Coveragestat
    {
        public string label { get; set; }
        public int position { get; set; }
        public int total { get; set; }
        public int covered { get; set; }
        public bool isDeltaAvailable { get; set; }
        public float delta { get; set; }
    }

}
