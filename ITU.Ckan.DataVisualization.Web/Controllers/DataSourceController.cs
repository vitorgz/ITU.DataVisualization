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
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "http://data.kk.dk/", Value = "http://data.kk.dk/" });
            items.Add(new SelectListItem { Text = "http://datahub.io/", Value = "http://datahub.io/" }); 
            items.Add(new SelectListItem { Text = "http://opendatadc.org/", Value = "http://opendatadc.org/" });
            items.Add(new SelectListItem { Text = "http://data.opendataportal.at/", Value = "http://data.opendataportal.at/" });
            items.Add(new SelectListItem { Text = "http://dati.openexpo2015.it/", Value = "http://dati.openexpo2015.it/" });
            items.Add(new SelectListItem { Text = "http://data.london.gov.uk/", Value = "http://data.london.gov.uk/" });
            items.Add(new SelectListItem { Text = "http://www.civicdata.io/", Value = "http://www.civicdata.io/", Selected = true });
            items.Add(new SelectListItem { Text = "Other", Value = "3" });

            ViewData["ckan"] = items;

            List<SelectListItem> chartItems = new List<SelectListItem>();
            chartItems.Add(new SelectListItem { Text = "PieChart", Value = "PieChart" });
            chartItems.Add(new SelectListItem { Text = "LineChart", Value = "LineChart" });
            chartItems.Add(new SelectListItem { Text = "ColumnChart", Value = "ColumnChart", Selected = true });
            chartItems.Add(new SelectListItem { Text = "BarChart", Value = "BarChart" });

            ViewData["chart"] = chartItems;

            restartSeries(RootInstance.CurrentVisualization);

            return View();
        }

        [HttpPost]
        public void SelectChart(string chart)
        {
            var visual = RootInstance.CurrentVisualization;
            if(visual.graph == null)
                visual.graph = new Graph();

            if (chart == "PieChart")
                restartSeries(visual);

            visual.graph = RootInstance.Current.graphs.Where(x => x.name == chart).FirstOrDefault();   
        }

        private void restartSeries(Visualization visual)
        {
            var yList = visual.sources.Where(x=>x.packages!=null)
                .SelectMany(x => x.packages.Where(e => e != null && e.dataSets != null)
                .SelectMany(y => y.dataSets.Where(e => e != null && e.fields != null)
                .SelectMany(z => z.GetYAxys())));

            //var yList = new List<Field>();

            //foreach (var source in visual.sources)
            //{
            //    if (source.packages != null)
            //        foreach (var package in source.packages)
            //        {
            //            if (package != null && package.dataSets != null)
            //                foreach (var dataset in package.dataSets)
            //                {
            //                    if (dataset != null)
            //                        yList.(dataset.GetYAxys());
            //                }
            //        }
            //}

            yList.ToList().ForEach(x => x.selected = false);
        }

        // POST: DataSource
        public async Task<JsonResult> GetPackages(string id)
        {
            var vis = RootInstance.CurrentVisualization;
            var source = vis.GetSourceById(x => x.name == id);

            if (source.packages == null)
            {
                var pck = SourceFactory.Initialize.AddIn(x =>
                            {
                                x.GetPackages(id);
                                //x.GetGroups(id);
                                //x.GetTag(id);
                            }).Create();

                source.packages = pck.packages; //TODO remove "Result" for non async
            }

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
            var vis = RootInstance.CurrentVisualization;
            var source = vis.GetSourceById(x => x.name == src);            

            var pkg = source.GetPackageByName(x => x.name == pck);
            source.packages.ToList().ForEach(x => x.selected = false);
            pkg.selected = true;

            if (pkg.dataSets == null)
            {
                var ds = await new PackageFactory().Initialize().GetDataSetsById(source.name, pck);
                var newPkg = ds.Create();
                pkg.dataSets = newPkg.dataSets;
            }

            List<SelectListItem> items = new List<SelectListItem>();
            if (pkg == null) return Json(items);
            items.Add(new SelectListItem { Text = "Select Data Set", Value = "Select Data Set" });
            foreach (var item in pkg.dataSets)
            {
                items.Add(new SelectListItem { Text = item.name, Value = item.name });
            }

            return Json(items);
        }

        public async Task<JsonResult> GetFields(string src, string pck, string dts)
        {
            var visual = RootInstance.CurrentVisualization;
            var source = visual.GetSourceById(x => x.name == src);
            var ds = source.GetPackageByName(x => x.name == pck).dataSets;
            var fields = ds.Where(x=>x.name == dts && x.format == "CSV").FirstOrDefault().fields;

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
        [HttpPost]
        public void SelectXAxys(string src, string pck, string dts, string fld)
        {
            var visual = RootInstance.CurrentVisualization;
            var source = visual.GetSourceById(x => x.name == src);
            var ds = source.GetPackageByName(x=>x.name == pck).dataSets;
            var fields = ds.Where(x => x.format == "CSV" && x.name == dts).FirstOrDefault().fields;

            var select = fields.Where(x => x.id.ToString() == fld).FirstOrDefault();
            var nonSelect = fields.Where(x => x.id.ToString() != fld).ToList();
            nonSelect.ForEach(x => x.xAxys = false);
            select.xAxys = true;
        }

        [HandleError()]
        [HttpPost]
        public void SelectField(string src, string pck, string dts, string fld)
        {
            var visual = RootInstance.CurrentVisualization;
            var source = visual.GetSourceById(x => x.name == src);
            var ds = source.GetPackageByName(x => x.name == pck).dataSets;
            var fields = ds.Where(x => x.format == "CSV" && x.name == dts).FirstOrDefault().fields;

            var select = fields.Where(x => x.id.ToString() == fld).FirstOrDefault();
            var nonSelect = fields.Where(x => x.id.ToString() != fld).ToList();
            nonSelect.ForEach(x => x.selected = false);
            select.selected = true;

            if (DslConverterHelpers.ResolveType(select.type) != typeof(int) &&
                DslConverterHelpers.ResolveType(select.type) != typeof(double) &&
                DslConverterHelpers.ResolveType(select.type) != typeof(decimal) &&
                DslConverterHelpers.ResolveType(select.type) != typeof(float) &&
                DslConverterHelpers.ResolveType(select.type) != typeof(long))
                throw new Exception("non numeric type");
        }


        public ActionResult ChartRedirect()
        {
            return RedirectToAction("Index", "Draw");
        }
    }
}