using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using ITU.Ckan.DataVisualization.InternalDsl.ExtensionMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ITU.Ckan.DataVisualization.Web.Controllers
{
    public class DrawController : Controller
    {
        // GET: Draw
        public async Task<ActionResult> Index()
        {

            var visual = RootInstance.Current.GetVisualization("test");

            var data = await visual.GetData();


            //var type = GetDataType(data.table.column.Type);
            var xAxisDAta = (data.table.column.Value as object[]).OfType<string>().Distinct().ToArray();

            var rows = from row in data.table.rows
                       select new { data = ConvertToType(row.Value, row.Type.GetType()) };



            DotNet.Highcharts.Highcharts chart = new DotNet.Highcharts.Highcharts("chart")
               .InitChart(new Chart() { DefaultSeriesType = DotNet.Highcharts.Enums.ChartTypes.Line });

            chart.SetXAxis(new XAxis
            {
                Categories = xAxisDAta
                //Categories = new[] { "Jan", "Feb", "Mar", "Apr" }
            });

            //it works
            //var arrayData = rows.FirstOrDefault().data as object[];
            //chart.SetSeries(new[] {
            //    new Series()
            //{
            //    //Data = arrayData
            //    Name = "test",
            //    Data = new Data(arrayData)
            //    //Data = new Data(new object[] { 29.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4 })
            //} });

            var series = new Series[rows.Count()];
            for (int i = 0; i < rows.Count(); i++)
            {
                series[i] = new Series();
                series[i].Data = new Data(rows.ElementAt(i).data as object[]);
            }
            chart.SetSeries(series);

            return View(chart);
        }

        private Array ConvertToType(object value, Type type) //TODO, use helpers?
        {
            var objArr = (value as object[]);
            var arr = Array.CreateInstance(type, objArr.Length);
            //Array.Copy(objArr, arr, objArr.Length);
            arr = Array.ConvertAll(objArr, elem => Convert.ChangeType(elem, type));

            return arr;
            /*
            MethodInfo method = typeof(Queryable).GetMethod("OfType");
            MethodInfo generic = method.MakeGenericMethod(new Type[] { type });
            // Use .NET 4 covariance
            var result = (IEnumerable<object>)generic.Invoke
                  (null, new object[] { value });
            object[] array = result.ToArray();

            return array;
            */
        }
    }
}