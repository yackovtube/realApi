using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CxDashboard.Entities
{
    public class EngineVersion
    {
        [Key]
        public int VersionId { get; set; }
        public string EngineVersionNumber { get; set; }
        public bool IsCurrentVersion { get; set; }
    }
}
