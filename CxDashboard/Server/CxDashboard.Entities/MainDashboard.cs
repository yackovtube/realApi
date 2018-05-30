using System.Collections.Generic;

namespace CxDashboard.Entities
{
    public class MainDashboard
    {
        public int MainDashboardId { get; set; }
        public byte[] Timeline { get; set; }
        public List<ApplicationOlderVersionGrade> ApplicationVersions { get; set; }
    }
}
