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
            var visual = RootInstance.CurrentVisualization;

            var filters = visual.GetFilters();
            var data = await visual.GetData(filters);
            
            if (data == null)
                return View();

            //Convert
            var xAxisDAta = (data.column.Value as object[]).OfType<string>().ToArray();
            if(xAxisDAta == null) xAxisDAta = (data.column.Value as object[]).Cast<string>().ToArray();
            //var xAxisDAta = DslConverterHelpers.ConvertToStringArray(data.column.Value);

            var rows = from row in data.rows
                       //select new { data = DslConverterHelpers.ConvertToSpecificType(row.Value, row.Type.GetType()) };
                       select new { data = row.Value };

            var chartType = getChartType(visual.graph);

            DotNet.Highcharts.Highcharts chart = new DotNet.Highcharts.Highcharts("chart")
               .InitChart(new Chart() { DefaultSeriesType = chartType });

            chart.SetXAxis(new XAxis
            {
                Categories = xAxisDAta
                //Categories = new[] { "Jan", "Feb", "Mar", "Apr" }
            });

            var series = new Series[rows.Count()];
            for (int i = 0; i < rows.Count(); i++)
            {
                series[i] = new Series();
                series[i].Data = new Data(rows.ElementAt(i).data as object[]);
            }
            chart.SetSeries(series);

            return View(chart);
        }

        private DotNet.Highcharts.Enums.ChartTypes getChartType(Graph graph)
        {
            if(graph == null) return DotNet.Highcharts.Enums.ChartTypes.Line;

            switch (graph.name) {
                case "PieChart":
                    return DotNet.Highcharts.Enums.ChartTypes.Pie;
                case "LineChart":
                    return DotNet.Highcharts.Enums.ChartTypes.Line;
                case "BarChart":
                    return DotNet.Highcharts.Enums.ChartTypes.Bar;
                case "ColumnChart":
                    return DotNet.Highcharts.Enums.ChartTypes.Column;
                default:
                    return DotNet.Highcharts.Enums.ChartTypes.Line;
            }
        }
    }
}