using ITU.Ckan.DataVisualization.InternalDsl.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITU.Ckan.DataVisualization.Web.Controllers
{
    public class DataSourceController : Controller
    {
        [Route("DataSource/CategoryChosen")]
        public ViewResult CategoryChosen(string MovieType)
        {

            ViewBag.messageString = MovieType;

            return View("Information");

        }
        public ActionResult SelectCategory()
        {

            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Action", Value = "0" });

            items.Add(new SelectListItem { Text = "Drama", Value = "1" });

            items.Add(new SelectListItem { Text = "Comedy", Value = "2", Selected = true });

            items.Add(new SelectListItem { Text = "Science Fiction", Value = "3" });

            ViewBag.MovieType = items;

            return View();

        }


        // GET: DataSource
        public ActionResult Index()
        {
            //new SourceFactory().GetSources();
            return SelectCategory();
        }
    }
}