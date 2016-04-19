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
            var source = vis.GetSourceById(x => x.name == id);
            source.packages = pck.Create().packages;

            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in source.packages)
            {
                items.Add(new SelectListItem { Text = item.name, Value = item.name });
            }

            //ViewBag.Packages = items;

            return Json(new SelectList(items, "Value", "Text"));
        }

        public async Task<JsonResult> GetDataSets(string src, string pck)
        {
            var vis = RootInstance.Current.GetVisualization("test");
            var source = vis.GetSourceById(x => x.name == src);            

            var ds = await new PackageFactory().Initialize().GetDataSetsById(source.name, pck);
            var newPkg = ds.Create();

            var pkg = source.packages.Where(x => x.name == pck).FirstOrDefault();
            source.packages.ToList().ForEach(x => x.selected = false);
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

        public async Task<JsonResult> GetFields(string src, string pck, string dts)
        {
            var visual = RootInstance.Current.GetVisualization("test");
            var source = visual.GetSourceById(x => x.name == src);
            var ds = source.packages.Where(x => x.name == pck).FirstOrDefault().dataSets;
            var fields = ds.Where(x=>x.name == dts).Where(x => x.format == "CSV").FirstOrDefault().fields;

            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in fields)
            {
                items.Add(new SelectListItem { Text = (item.id + "- (" + item.type + ")"), Value = item.id.ToString() });
            }

            //ViewData["fields"] = items;

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

        //TODO set select flag!
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