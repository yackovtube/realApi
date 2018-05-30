namespace Entities.Charts
{
    public class Pie
    {
        public string Name { get; set; }
        public string Element { get; set; }
        public string Type { get; set; }
        public object Xkey { get; set; }
        public object Ykeys { get; set; }
        public object Labels { get; set; }
        public object PointSize { get; set; }
        public object HideHover { get; set; }
        public bool Resize { get; set; }
        public Datum[] Data { get; set; }
        public string TotalData { get; set; }
        public string DataShowType { get; set; }
    }

    public class Datum
    {
        public string Label { get; set; }
        public int Value { get; set; }
    }
}
