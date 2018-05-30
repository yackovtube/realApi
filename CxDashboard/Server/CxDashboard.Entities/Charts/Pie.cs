namespace CxDashboard.Entities.Charts
{
    public class Pie : Chart
    {

        public Datum[] Data { get; set; }
        public string DataShowType { get; set; }
    }

    public class Datum
    {
        public string Label { get; set; }
        public double Value { get; set; }
    }
}
