using ITU.Ckan.DataVisualization.InternalDsl.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<JsonResult> GetDataForChart(string idX, string idY)
        {
            return Json(true);
        }
    }
}