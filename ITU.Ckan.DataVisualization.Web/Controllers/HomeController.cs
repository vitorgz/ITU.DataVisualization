using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using ITU.Ckan.DataVisualization.InternalDslApi;
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
            DBClient.CreateVisualization(visual.name);


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
    }
}
