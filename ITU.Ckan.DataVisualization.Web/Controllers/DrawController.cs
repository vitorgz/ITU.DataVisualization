using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using ITU.Ckan.DataVisualization.InternalDsl.ExtensionMethods;
using ITU.Ckan.DataVisualization.InternalDsl.Helpers;
using ITU.Ckan.DataVisualization.InternalDslApi.DTO;
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
            Table data;
            if(visual.graph==null) throw new Exception("Chart was not selected");
            if (visual.graph.name != "PieChart")
                data = await visual.GetData(filters);
            else
                data = await visual.GetPieChartData(filters);           


            if (data == null)
                return View();

            string[] xAxisDAta = null;
            if (data.column != null)
            {
                xAxisDAta = (data.column.Value as object[]).OfType<string>().ToArray();
                if (xAxisDAta == null) xAxisDAta = (data.column.Value as object[]).Cast<string>().ToArray();
                //var xAxisDAta = DslConverterHelpers.ConvertToStringArray(data.column.Value);
            }

            var rows = from row in data.rows
                           //select new { data = DslConverterHelpers.ConvertToSpecificType(row.Value, row.Type.GetType()) };
                       select new { data = row.Value, name = row.name };

            var chartType = getChartType(visual.graph);
        
            DotNet.Highcharts.Highcharts chart = new DotNet.Highcharts.Highcharts("chart")
               .InitChart(new Chart() { DefaultSeriesType = chartType });

            chart.SetXAxis(new XAxis
            {
                Categories = xAxisDAta
            });

            var series = new Series[rows.Count()];
            for (int i = 0; i < rows.Count(); i++)
            {
                series[i] = new Series();

                //series[i].Data = new Data(new object[] { rows.ElementAt(i).data }.ToArray() );
                object[] dataTo = rows.ElementAt(i).data as object[];
                series[i].Data = new Data(dataTo.ToArray());                
                series[i].Name = rows.ElementAt(i).name;
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