using System;

namespace CxDashboard.Entities.Requests
{
    public class QueriesDataRequest
    {
        public string id { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public Createdby createdBy { get; set; }
        public DateTime createdDate { get; set; }
        public Lastmodifiedby lastModifiedBy { get; set; }
        public DateTime lastModifiedDate { get; set; }
        public bool isFolder { get; set; }
        public bool hasChildren { get; set; }
        public Child[] children { get; set; }
        public bool isPublic { get; set; }
        public _Links _links { get; set; }
        public string url { get; set; }
    }

    public class Createdby
    {
        public string id { get; set; }
        public string displayName { get; set; }
    }

    public class Child
    {
        public string id { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public DateTime createdDate { get; set; }
        public Lastmodifiedby1 lastModifiedBy { get; set; }
        public DateTime lastModifiedDate { get; set; }
        public bool isPublic { get; set; }
        public _Links1 _links { get; set; }
        public string url { get; set; }
    }

    public class Lastmodifiedby1
    {
        public string id { get; set; }
        public string displayName { get; set; }
    }

    public class _Links1
    {
        public Self1 self { get; set; }
        public Html1 html { get; set; }
        public Parent1 parent { get; set; }
        public Wiql wiql { get; set; }
    }

    public class Self1
    {
        public string href { get; set; }
    }

    public class Html1
    {
        public string href { get; set; }
    }

    public class Parent1
    {
        public string href { get; set; }
    }
}
