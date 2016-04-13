using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using ITU.Ckan.DataVisualization.InternalDsl.ExtensionMethods;
using ITU.Ckan.DataVisualization.InternalDsl.Helpers;
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

            var filters = visual.GetFilters();
            var data = await visual.GetData(filters);

            //var type = GetDataType(data.table.column.Type);
            //var xAxisDAta = (data.table.column.Value as object[]).OfType<string>().Distinct().ToArray();
            if (data == null)
                return View();

            //Convert
            //var xAxisDAta = (data.column.Value as object[]).OfType<string>().ToArray();
            var xAxisDAta = DslConverterHelpers.ConvertToStringArray(data.column.Value);

            var rows = from row in data.rows
                       select new { data = DslConverterHelpers.ConvertToSpecificType(row.Value, row.Type.GetType()) };

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
    }
}