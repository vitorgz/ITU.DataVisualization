using ITU.Ckan.DataVisualization.InternalDsl.ExtensionMethods;
using ITU.Ckan.DataVisualization.InternalDsl.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ITU.Ckan.DataVisualization.Web.Controllers
{
    public class DataSourceController : Controller
    {
        public ActionResult Index()
        {
            //TODO
            RootInstance.Current.visualizations = new List<Visualization>() { new Visualization() { name = "test" } };
            var visual = RootInstance.Current.GetVisualization("test");
            visual.sources = new List<Source>() { new Source() { name = "http://data.kk.dk/" } };

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "http://data.kk.dk/", Value = "http://data.kk.dk/" });
            items.Add(new SelectListItem { Text = "http://data.kk.dk/", Value = "http://data.kk.dk/" });
            items.Add(new SelectListItem { Text = "http://datahub.io", Value = "http://datahub.io/" });
            items.Add(new SelectListItem { Text = "http://data.amsterdam.nl/", Value = "http://data.amsterdam.nl/", Selected = true });
            items.Add(new SelectListItem { Text = "Other", Value = "3" });

            ViewData["ckan"] = items;

            return View();
        }


        // POST: DataSource
        public async Task<JsonResult> GetPackages(string id)
        {
            //new SourceFactory().GetSources();

            //we neeed to added to the current instance "RootInstance"
            var pck = await new SourceFactory().Initialize().GetPackages(id);

            var vis = RootInstance.Current.GetVisualization("test");
            var source = vis.GetSourceById(x => x.name == "http://data.kk.dk/");
            source.packages = pck.Create().packages;

            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in source.packages)
            {
                items.Add(new SelectListItem { Text = item.name, Value = item.name });
            }

            //ViewBag.Packages = items;

            return Json(new SelectList(items, "Value", "Text"));
        }

        public async Task<JsonResult> GetDataSets(string id)
        {
            var vis = RootInstance.Current.GetVisualization("test");
            var source = vis.GetSourceById(x => x.name == "http://data.kk.dk/");            

            var ds = await new PackageFactory().Initialize().GetDataSetsById(source.name, id);
            var newPkg = ds.Create();

            var pkg = source.packages.Where(x => x.name == id).FirstOrDefault();
            pkg.selected = true;
            pkg.dataSets = newPkg.dataSets;

            List<SelectListItem> items = new List<SelectListItem>();
            if (pkg == null) return Json(items);
            foreach (var item in pkg.dataSets)
            {
                items.Add(new SelectListItem { Text = item.name, Value = item.name });
            }

            return Json(items);
        }

        public async Task<JsonResult> VerifyDataStore(string id)
        {
            //get package
            //var pck = await new SourceFactory().Initialize().GetPackages(new Source() { name = "s" });

            //get datasets

            //call for example


            return Json(true);
        }

        public ActionResult ChartRedirect()
        {
            return RedirectToAction("Index", "Chart");
        }
    }
}