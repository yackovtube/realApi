using CxDashboard.Entities.Requests;
using System;

namespace CxDashboard.Entities
{
    public abstract class WorkItems
    {
        public string queryType { get; set; }
        public string queryResultType { get; set; }
        public DateTime asOf { get; set; }
        public Column[] columns { get; set; }
        public Sortcolumn[] sortColumns { get; set; }

        public abstract string[] GetAllIds();

        public abstract int WorkItemCount
        {
            get;
        }

    }

    public class Flat : WorkItems
    {
        public override int WorkItemCount
        {
            get
            {
                return workItems.Length;
            }
        }

        public WorkItem[] workItems { get; set; }
        public override string[] GetAllIds()
        {
            string[] stringArr;
            if (workItems.Length > 200)
            {
                int arrSize = workItems.Length / 200;
                stringArr = new string[arrSize + 1];
            }
            else
                stringArr = new string[1];

            int counter = 0;
            int arrPpsition = 0;
            string ids = "";
            foreach (var item in workItems)
            {
                if (counter == 200)
                {
                    ids = ids.Remove(ids.Length - 1);
                    stringArr[arrPpsition] = ids;
                    arrPpsition++;
                    ids = "";
                    counter = 0;
                }
                ids += item.id + ",";
                counter++;
            }
            ids = ids.Remove(ids.Length - 1);
            stringArr[arrPpsition] = ids;

            return stringArr;
        }
    }

    public class OneHop : WorkItems
    {
        public override int WorkItemCount
        {
            get
            {
                return workItemRelations.Length;
            }
        }

        public Workitemrelation[] workItemRelations { get; set; }

        public override string[] GetAllIds()
        {
            string[] stringArr;
            if (workItemRelations.Length > 200)
            {
                int arrSize = workItemRelations.Length / 200;
                stringArr = new string[arrSize + 1];
            }
            else
                stringArr = new string[1];

            int counter = 0;
            int arrPpsition = 0;
            string ids = "";
            foreach (var item in workItemRelations)
            {
                if (counter == 200)
                {
                    ids = ids.Remove(ids.Length - 1);
                    stringArr[arrPpsition] = ids;
                    arrPpsition++;
                    ids = "";
                    counter = 0;
                }
                ids += item.target.id + ",";
                counter++;
            }
            ids = ids.Remove(ids.Length - 1);
            stringArr[arrPpsition] = ids;

            return stringArr;
        }
    }

    public class Workitemrelation
    {
        public WorkItem target { get; set; }
        public string rel { get; set; }
        public WorkItem source { get; set; }
    }

    public class WorkItem
    {
        public int id { get; set; }
        public string url { get; set; }
    }
}
