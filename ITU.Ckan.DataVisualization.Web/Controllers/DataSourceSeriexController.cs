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
    public class DataSourceSeriexController : Controller
    {
        
        [ChildActionOnly]
        public ActionResult _xAxisView(string parameter1)
        {            
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "http://data.kk.dk/", Value = "http://data.kk.dk/" });
            items.Add(new SelectListItem { Text = "http://open.stavanger.kommune.no/", Value = "http://open.stavanger.kommune.no/" });
            items.Add(new SelectListItem { Text = "http://opendatadc.org/", Value = "http://opendatadc.org/" });
            items.Add(new SelectListItem { Text = "http://www.civicdata.com/", Value = "http://www.civicdata.com/" });
            items.Add(new SelectListItem { Text = "https://edo.ckan.io/", Value = "https://edo.ckan.io/" });
            items.Add(new SelectListItem { Text = "http://data.gov.sk/", Value = "http://data.gov.sk/" });
            items.Add(new SelectListItem { Text = "http://data.humdata.org/", Value = "http://data.humdata.org/" });
            items.Add(new SelectListItem { Text = "http://www.odaa.dk/", Value = "http://www.odaa.dk/" });
            items.Add(new SelectListItem { Text = "http://catalogue.data.gov.bc.ca/", Value = "http://catalogue.data.gov.bc.ca/" });

            ViewData["ckan"] = items;
            
            return PartialView();
        }        

        // POST: DataSource
        public async Task<JsonResult> GetPackages(string id)
        {
            var vis = RootInstance.CurrentVisualization;
            var source = vis.GetSourceById(x => x.name == id);

            if (source.packages == null)
            {
                var pck = new SourceFactory().AddIn(x =>
                {
                    x.GetPackages(id);
                    //x.GetGroups(id);
                    //x.GetTag(id);
                }).Create();

                //valid call after factory
                //pck.GetTags(id);

                //valid
                //var sd = new Source();
                //sd.AddIn(x => { x.GetGroups(id).GetTags(id); });
                
                //valid
                //var pcks = new SourceFactory().Initialize().GetTag(id).GetGroups(id).Create();

                source.packages = pck.packages;
            }

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Select Package", Value = "Select Package" });
            foreach (var item in source.packages)
            {
                items.Add(new SelectListItem { Text = item.name, Value = item.name });
            }

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
                //var ds = await new PackageFactory().Initialize().GetDataSetsById(source.name, pck);
                var ds = new PackageFactory().Initialize().AddIn(x=> { x.GetDataSetsById(source.name, pck); }).Create();
                //var newPkg = ds.Create();
                pkg.dataSets = ds.dataSets;
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
            items.Add(new SelectListItem { Text = "Select Field", Value = "Select Field" });
            foreach (var item in fields)
            {
                items.Add(new SelectListItem { Text = (item.id + "- (" + item.type + ")"), Value = item.id.ToString() });
            }
            
            return Json(items);
        }
        

        [HttpPost]
        public void SelectXAxys(string src, string pck, string dts, string fld)
        {
            var visual = RootInstance.CurrentVisualization;
            var source = visual.GetSourceById(x => x.name == src);
            var ds = source.GetPackageByName(x=>x.name == pck).dataSets;
            var fields = ds.Where(x => x.format == "CSV" && x.name == dts).FirstOrDefault().fields;

            var select = fields.Where(x => x.id.ToString() == fld).FirstOrDefault();
            //var nonSelect = fields.Where(x => x.id.ToString() != fld).ToList();
            //nonSelect.ForEach(x => x.xAxys = false);
            //select.xAxys = true;

            var dataset = ds.Where(x => x.name == dts).FirstOrDefault();
            var fieldsList = new List<Field>() { select };

            RootInstance.SerieX.dataSetId = dataset != null ? dataset.id : string.Empty;
            RootInstance.SerieX.fields = fieldsList;
            RootInstance.SerieX.sourceName = src;
            RootInstance.SerieX.packageName = pck;


        }

    }
}