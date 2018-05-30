using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CxDashboard.Entities.Requests
{
    public class EngineAPIData
    {
        public int hiddenGood { get; set; }
        public int hiddenBad { get; set; }
        public int hiddenUnknown { get; set; }
        public int hiddenReplaced { get; set; }
        public int recurrentGood { get; set; }
        public int recurrentBad { get; set; }
        public int recurrentUnknown { get; set; }
        public int recurrentReplaced { get; set; }
        public int newGood { get; set; }
        public int newBad { get; set; }
        public int newUnknown { get; set; }
        public int newReplaced { get; set; }
    }
}
