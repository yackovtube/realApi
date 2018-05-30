using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Requests
{
    public class QueryDefinitionRequest
    {
        public string id { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public DateTime createdDate { get; set; }
        public Lastmodifiedby lastModifiedBy { get; set; }
        public DateTime lastModifiedDate { get; set; }
        public string queryType { get; set; }
        public Column[] columns { get; set; }
        public Sortcolumn[] sortColumns { get; set; }
        public string wiql { get; set; }
        public bool isPublic { get; set; }
        public Linkclauses linkClauses { get; set; }
        public string filterOptions { get; set; }
        public Sourceclauses sourceClauses { get; set; }
        public Targetclauses targetClauses { get; set; }
        public _Links _links { get; set; }
        public string url { get; set; }
    }

    public class Lastmodifiedby
    {
        public string id { get; set; }
        public string displayName { get; set; }
    }

    public class Linkclauses
    {
        public string logicalOperator { get; set; }
        public Claus[] clauses { get; set; }
    }

    public class Claus
    {
        public Field field { get; set; }
        public Operator _operator { get; set; }
        public string value { get; set; }
    }

    public class Field
    {
        public string referenceName { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Operator
    {
        public string referenceName { get; set; }
        public string name { get; set; }
    }

    public class Sourceclauses
    {
        public string logicalOperator { get; set; }
        public Claus1[] clauses { get; set; }
    }

    public class Claus1
    {
        public string logicalOperator { get; set; }
        public Claus2[] clauses { get; set; }
        public Field field { get; set; }
        public Operator1 _operator { get; set; }
        public string value { get; set; }
    }

    public class Operator1
    {
        public string referenceName { get; set; }
        public string name { get; set; }
    }

    public class Claus2
    {
        public Field field { get; set; }
        public Operator2 _operator { get; set; }
        public string value { get; set; }
    }

    public class Operator2
    {
        public string referenceName { get; set; }
        public string name { get; set; }
    }

    public class Targetclauses
    {
        public string logicalOperator { get; set; }
        public Claus3[] clauses { get; set; }
    }

    public class Claus3
    {
        public Field field { get; set; }
        public Operator3 _operator { get; set; }
        public string value { get; set; }
    }

    public class Operator3
    {
        public string referenceName { get; set; }
        public string name { get; set; }
    }

    public class _Links
    {
        public Self self { get; set; }
        public Html html { get; set; }
        public Parent parent { get; set; }
        public Wiql wiql { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class Html
    {
        public string href { get; set; }
    }

    public class Parent
    {
        public string href { get; set; }
    }

    public class Wiql
    {
        public string href { get; set; }
    }

    public class Column
    {
        public string referenceName { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Sortcolumn
    {
        public Field field { get; set; }
        public bool descending { get; set; }
    }
}