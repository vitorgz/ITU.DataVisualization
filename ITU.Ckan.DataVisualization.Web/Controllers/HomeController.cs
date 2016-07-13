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
                new Source() { name = "http://www.odaa.dk/" },
                new Source() { name = "http://www.civicdata.com/" },
                new Source() { name = "http://opendatadc.org/" },
                new Source() { name = "http://open.stavanger.kommune.no/" },
                new Source() { name = "https://edo.ckan.io/" },
                new Source() { name = "http://data.humdata.org/" },
                new Source() { name = "http://data.gov.sk/" },
                new Source() { name = "http://catalogue.data.gov.bc.ca/" }   
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
            RootInstance.CurrentVisualization = null;
            var visual = await DBClient.GetVisualizationByName<Visualization>(vs);
            RootInstance.CurrentVisualization = visual;
            
            return Json(Url.Action("DrawChart", "Draw"));
        }

    }
}
