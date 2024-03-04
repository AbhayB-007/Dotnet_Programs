using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using Teamarcs.Connect.API.Helper;

namespace SQLQueryRunner.Controllers
{
    public class QueryController : Controller
    {
        private IConfiguration config;
        public QueryController(IConfiguration configuration)
        {
            config = configuration;
        }
        public IActionResult Index()
        {
            ViewBag.AllConnectionStrings = config.GetSection("ConnectionStrings").Get<Dictionary<string, string>>().Keys.ToList();
            return View();
        }
        public ContentResult GetData(string rawQuery, string connKey)
        {
            var message = "";
            try
            {
                SQLHelper helper = new SQLHelper(config, connKey);
                using (System.Data.SqlClient.SqlConnection connection = helper.createConnection())
                {
                    var dataTables = SQLHelper.ExecuteDataset(connection, CommandType.Text, rawQuery);
                    if (dataTables.Tables.Count > 0)
                    {
                        return SerializeJSON(dataTables);
                    }
                    else
                    {
                        message = "Query Executed Successfully";
                    }
                }
                return new ContentResult() { StatusCode = 400, Content = message };
            }
            catch (Exception ex)
            {
                return new ContentResult() { StatusCode = 400, Content = $" InnerException : {ex.InnerException} | Error Message : {ex.Message}" };
            }
        }

        private ContentResult SerializeJSON(object toSerialize)
        {
            var resultData = toSerialize; //Whatever value you are serializing
            ContentResult result = new ContentResult();
            result.StatusCode = 200;
            result.Content = JsonConvert.SerializeObject(resultData);
            result.ContentType = "application/json";
            return result;
        }

        public string ToHtml(DataSet dataset)
        {
            StringBuilder htmlBuilder = new StringBuilder();

            foreach (DataTable dataTable in dataset.Tables)
            {
                // Create the HTML table start tag
                htmlBuilder.Append("<table>");

                // Create the table header row
                htmlBuilder.Append("<tr>");
                foreach (DataColumn column in dataTable.Columns)
                {
                    htmlBuilder.AppendFormat("<th>{0}</th>", column.ColumnName);
                }
                htmlBuilder.Append("</tr>");

                // Create the table rows
                foreach (DataRow row in dataTable.Rows)
                {
                    htmlBuilder.Append("<tr>");
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        htmlBuilder.AppendFormat("<td>{0}</td>", row[column.ToString().Trim()]);
                    }
                    htmlBuilder.Append("</tr>");
                }
                // Create the HTML table end tag
                htmlBuilder.Append("</table>");
            }
            return htmlBuilder.ToString();
        }

    }
}
