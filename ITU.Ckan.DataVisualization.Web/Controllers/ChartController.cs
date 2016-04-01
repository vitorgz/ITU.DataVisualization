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

        public void GetDataForChart(string idX, string idY)
        {
            var visual = RootInstance.Current.GetVisualization("test");
            var source = visual.GetSourceById(x => x.name == "http://data.kk.dk/");
            var ds = source.packages.Where(x => x.selected).FirstOrDefault().dataSets;
            var fields = ds.Where(x => x.format == "CSV").FirstOrDefault().fields;

            var select = fields.Where(x => x.id.ToString() == idX || x.id.ToString() == idY).ToList();
            var nonSelect = fields.Where(x => x.id.ToString() != idX || x.id.ToString() != idY).ToList();
            nonSelect.ForEach(x => x.selected = false);
            select.ForEach(x => x.selected = true);
            select.Where(x => x.id.ToString() == idX).FirstOrDefault().xAxys = true;
            
        }

        public ActionResult ChartRedirect()
        {
            return RedirectToAction("Index", "Draw");
        }
    }
}