using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using ITU.Ckan.DataVisualization.InternalDslApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ITU.Ckan.DataVisualization.Web.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var vsList = await DBClient.GetListVisualizations<List<string>>();

            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in vsList)
            {
                items.Add(new SelectListItem { Text = item, Value = item });
            }

            ViewData["visualizations"] = items;

            return View();
        }

        [HttpPost]
        public ActionResult Index(Visualization visual)
        {
            var visualInstance = new Visualization() { name = visual.name };

            if (visualInstance.sources == null)
            {
                visualInstance.sources = new List<Source>() {
                new Source() { name = "http://data.kk.dk/" },
                new Source() { name = "http://datahub.io/" },
                new Source() { name = "http://opendatadc.org/" },
                new Source() { name = "http://data.opendataportal.at/" },
                new Source() { name = "http://data.london.gov.uk/" },
                new Source() { name = "http://dati.openexpo2015.it/" },
                new Source() { name = "http://www.civicdata.io/" },
               };
            }

            RootInstance.CurrentVisualization = visualInstance;
            RootInstance.Current.visualizations.Concat(new List<Visualization>() { visualInstance });

            return RedirectToAction("Index", "DataSource");
        }

        [HttpPost]
        public async Task<ActionResult> SelectVisualization(string vs)
        {
            if (string.IsNullOrEmpty(vs)) return null;
            RootInstance.CurrentVisualization = await DBClient.GetVisualizationByName<Visualization>(vs);

            //return RedirectToAction("DrawChart", "Draw");
            return Json(Url.Action("DrawChart", "Draw"));
        }

    }
}
