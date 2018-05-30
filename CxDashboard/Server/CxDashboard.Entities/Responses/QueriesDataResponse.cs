namespace CxDashboard.Entities.Responses
{
    public class QueriesDataResponse
    {
        public Report[] AllReports { get; set; }
        public QueryData[] AllAvailableQueries { get; set; }
    }



    public class QueryData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public WorkItems workItems { get; set; }
    }
}
