using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITU.Ckan.DataVisualization.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {            
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
                new Source() { name = "http://data.amsterdam.nl/" },
               };
            }

            RootInstance.CurrentVisualization = visualInstance;
            RootInstance.Current.visualizations.Concat(new List<Visualization>() { visualInstance });

            return RedirectToAction("Index", "DataSource");
        }
    }
}
