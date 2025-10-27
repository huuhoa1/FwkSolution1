using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FwkWebApplication1.Models;
using FwkWebApplication1.Services;
using Newtonsoft.Json.Linq;

namespace FwkWebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly GraphQLClientService _graphQLService;

        public HomeController()
        {
            _graphQLService = new GraphQLClientService("http://localhost:4000");
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// GraphQL page - displays a form to execute GraphQL queries
        /// </summary>
        public ActionResult GraphQL()
        {
            return View();
        }

        /// <summary>
        /// Execute GraphQL query and return results
        /// </summary>
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> ExecuteGraphQLQuery(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Json(new { success = false, message = "Query cannot be empty" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var result = await _graphQLService.GetDataAsync(query);

                // Extract data from the response
                var jsonResult = JObject.FromObject(result);
                var data = jsonResult["data"];

                if (data == null)
                {
                    var errors = jsonResult["errors"];
                    var errorMessage = "No data returned";
                    if (errors != null)
                    {
                        var errorMessages = new List<string>();
                        foreach (var error in errors)
                        {
                            errorMessages.Add((string)error["message"]);
                        }
                        errorMessage = string.Join("; ", errorMessages);
                    }

                    return Json(new
                    {
                        success = false,
                        message = errorMessage
                    }, JsonRequestBehavior.AllowGet);
                }

                // Convert result to a displayable format
                var dataModel = ConvertToDataModel(data);

                return Json(new
                {
                    success = true,
                    data = dataModel,
                    rawData = jsonResult.ToString()
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"Error executing query: {ex.Message}"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Convert JToken data to displayable table format
        /// </summary>
        private dynamic ConvertToDataModel(JToken data)
        {
            var result = new List<Dictionary<string, object>>();
            var columns = new HashSet<string>();

            if (data.Type == JTokenType.Array)
            {
                // If data is an array, iterate through items
                foreach (var item in data)
                {
                    var row = new Dictionary<string, object>();
                    foreach (var property in item.Children<JProperty>())
                    {
                        columns.Add(property.Name);
                        row[property.Name] = property.Value.ToString();
                    }
                    result.Add(row);
                }
            }
            else if (data.Type == JTokenType.Object)
            {
                // If data is an object, check if it contains array properties
                var obj = (JObject)data;

                // Find the first array property to display
                var arrayProperty = obj.Properties()
                    .FirstOrDefault(p => p.Value.Type == JTokenType.Array);

                if (arrayProperty != null && arrayProperty.Value.Count() > 0)
                {
                    // Use the first array property
                    foreach (var item in arrayProperty.Value)
                    {
                        var row = new Dictionary<string, object>();
                        if (item.Type == JTokenType.Object)
                        {
                            foreach (var property in item.Children<JProperty>())
                            {
                                columns.Add(property.Name);
                                row[property.Name] = property.Value.ToString();
                            }
                        }
                        else
                        {
                            columns.Add(arrayProperty.Name);
                            row[arrayProperty.Name] = item.ToString();
                        }
                        result.Add(row);
                    }
                }
                else
                {
                    // No array found, convert object properties directly
                    var row = new Dictionary<string, object>();
                    foreach (var property in obj.Properties())
                    {
                        columns.Add(property.Name);
                        if (property.Value.Type == JTokenType.Array || property.Value.Type == JTokenType.Object)
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        else
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                    }
                    result.Add(row);
                }
            }

            return new
            {
                rows = result,
                columns = columns.ToList()
            };
        }
    }
}