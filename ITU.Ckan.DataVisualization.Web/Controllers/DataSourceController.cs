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
        [Route("DataSource/CategoryChosen")]
        public void CategoryChosen(string MovieType)
        {

            GetPackages();

           // return View("Information");

        }
        public ActionResult Index()
        {

            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "CPH", Value = "0" });

            items.Add(new SelectListItem { Text = "GZ", Value = "1" });

            items.Add(new SelectListItem { Text = "BCN", Value = "2", Selected = true });

            items.Add(new SelectListItem { Text = "Other", Value = "3" });

            ViewBag.CkanInstances = items;
            ViewBag.Packages = new List<SelectListItem>();

            return View();

        }


        // GET: DataSource
        public async Task<ActionResult> GetPackages()
        {
            //new SourceFactory().GetSources();
            //return SelectCategory();
            var sd = await new SourceFactory().Initialize().GetPackages(new Source() { name = "s" });

            ViewBag.Packages = sd;

            return View();
        }
    }
}