using ITU.Ckan.DataVisualization.InternalDsl.ExtensionMethods;
using ITU.Ckan.DataVisualization.InternalDsl.Factories;
using ITU.Ckan.DataVisualization.InternalDsl.Helpers;
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
            List<SelectListItem> chartItems = new List<SelectListItem>();
            chartItems.Add(new SelectListItem { Text = "PieChart", Value = "PieChart" });
            chartItems.Add(new SelectListItem { Text = "LineChart", Value = "LineChart" });
            chartItems.Add(new SelectListItem { Text = "ColumnChart", Value = "ColumnChart", Selected = true });
            chartItems.Add(new SelectListItem { Text = "BarChart", Value = "BarChart" });

            ViewData["chart"] = chartItems;

            var visual = RootInstance.CurrentVisualization;
            visual.restartSeries();

            return View();
        }

        [HttpPost]
        public void SelectChart(string chart)
        {
            var visual = RootInstance.CurrentVisualization;
            if(visual.graph == null)
                visual.graph = new Graph();

            if (chart == "PieChart")
                visual.restartSeries();

            visual.graph = RootInstance.Current.graphs.Where(x => x.name == chart).FirstOrDefault();   
        }

        [HandleError()]
        [HttpPost]
        public ActionResult ChartRedirect()
        {
            var visual = RootInstance.CurrentVisualization;
            if (visual.graph == null)
            {
                return Json(new { ok = false });
            }

            RootInstance.SelectFields();

            return Json(new { ok = true,
                newurl = new UrlHelper(Request.RequestContext).Action("Index", "Draw") });       
            //return RedirectToAction("Index", "Draw");
        }
    }
}