using ITU.Ckan.DataVisualization.InternalDsl.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Threading.Tasks;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Helpers;

namespace ITU.Ckan.DataVisualization.Web.Controllers
{
    public class ChartController : Controller
    {
        // GET: Chart
        public ActionResult Index()
        {
            var visual = RootInstance.Current.GetVisualization("test");
            var source = visual.GetSourceById(x => x.name == "http://data.kk.dk/");
            var ds = source.packages.Where(x => x.selected).FirstOrDefault().dataSets;
            var fields = ds.Where(x => x.format == "CSV").FirstOrDefault().fields;

            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in fields)
            {
                items.Add(new SelectListItem { Text = (item.id + "- (" + item.type + ")"), Value = item.id.ToString() });
            }

            ViewData["fields"] = items;

            return View();
        }

        public async Task<ActionResult> GetDataForChart(string idX, string idY)
        {
            var visual = RootInstance.Current.GetVisualization("test");
            var source = visual.GetSourceById(x => x.name == "http://data.kk.dk/");
            var ds = source.packages.Where(x => x.selected).FirstOrDefault().dataSets;
            var fields = ds.Where(x => x.format == "CSV").FirstOrDefault().fields;

            var select = fields.Where(x => x.id.ToString() == idX || x.id.ToString() == idY).ToList();
            select.ForEach(x => x.selected = true);
            select.Where(x => x.id.ToString() == idX).FirstOrDefault().xAxys = true;

            var data = await visual.GetData();
            var xAxisDAta = (data.table.column.Value as object[]).OfType<string>().ToArray();

            var arrayData = new Data(data.table.rows.Select(x=>x.Value as object[]).FirstOrDefault()); //not working

            DotNet.Highcharts.Highcharts chart = new DotNet.Highcharts.Highcharts("chart")
               .InitChart(new Chart() { DefaultSeriesType = DotNet.Highcharts.Enums.ChartTypes.Area })
               .SetXAxis(new XAxis
               {
                   Categories = xAxisDAta
               })
               .SetSeries(new Series()
               {
                   Data = arrayData

                   //Data = new Data(new object[] { 29.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4 })
               });

            return View(chart);
        }
    }
}