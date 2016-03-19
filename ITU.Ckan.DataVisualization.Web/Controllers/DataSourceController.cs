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
            RootInstance.Current.visualizations = new List<Visualization>() { new Visualization() { name = "test"} };
            var vis = RootInstance.Current.visualizations.Where(x => x.name == "test").FirstOrDefault();
            vis.sources = new List<Source>() { new Source() { name = "CPH" } };

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "CPH", Value = "0" });
            items.Add(new SelectListItem { Text = "GZ", Value = "1" });
            items.Add(new SelectListItem { Text = "BCN", Value = "2", Selected = true });
            items.Add(new SelectListItem { Text = "Other", Value = "3" });

            ViewData["ckan"] = items;

            return View();
        }


        // GET: DataSource
        public async Task<JsonResult> GetPackages(string id)
        {
            //new SourceFactory().GetSources();
            

            //we neeed to added to the current instance "RootInstance"
            var pck = await new SourceFactory().Initialize().GetPackages(new Source() { name = "s" });

            var vis = RootInstance.Current.visualizations.Where(x => x.name == "test").FirstOrDefault();
            var source = vis.sources.Where(x => x.name == "CPH").FirstOrDefault();

            source.packages = pck.Create().packages;

            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in source.packages)
            {
                items.Add(new SelectListItem { Text = item.name});
            }

            //ViewBag.Packages = items;

            return Json(new SelectList(items, "Value", "Text"));
        }
    }
}