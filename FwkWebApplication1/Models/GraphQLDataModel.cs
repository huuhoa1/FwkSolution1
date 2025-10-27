using System;
using System.Collections.Generic;

namespace FwkWebApplication1.Models
{
    /// <summary>
    /// Model for displaying GraphQL data in table format
    /// </summary>
    public class GraphQLDataModel
    {
        public string Title { get; set; }
        public List<Dictionary<string, object>> Data { get; set; }
        public List<string> Columns { get; set; }
        public string ErrorMessage { get; set; }
        public bool HasError { get; set; }

        public GraphQLDataModel()
        {
            Data = new List<Dictionary<string, object>>();
            Columns = new List<string>();
            HasError = false;
        }
    }

    /// <summary>
    /// Model for GraphQL query execution results
    /// </summary>
    public class GraphQLQueryResult
    {
        public string Query { get; set; }
        public List<Dictionary<string, object>> Results { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }

        public GraphQLQueryResult()
        {
            Results = new List<Dictionary<string, object>>();
            IsSuccess = true;
        }
    }
}
