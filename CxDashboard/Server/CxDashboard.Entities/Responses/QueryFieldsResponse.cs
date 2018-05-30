using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CxDashboard.Entities.Responses
{
    public class QueryFieldsResponse
    {
        public List<QueryDefinitionResponse> GroupByFields { get; set; }
        public List<QueryDefinitionResponse> AggregationFields { get; set; }
    }
}
