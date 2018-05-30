using System.Collections.Generic;

namespace CxDashboard.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<Report> Reports { get; set; }
    }
}
