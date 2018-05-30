using System.Collections.Generic;

namespace Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<Report> Reports { get; set; }
    }
}
