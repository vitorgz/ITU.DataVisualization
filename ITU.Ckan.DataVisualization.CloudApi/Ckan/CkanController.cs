using ITU.Ckan.DataVisualization.CloudApi.DTO;
using ITU.Ckan.DataVisualization.CloudApi.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ITU.Ckan.DataVisualization.CloudApi.Ckan
{
    public class CkanController : ApiController
    {
        [Route("api/GetPackages")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetPackages(Source source)
        {
            //http://data.kk.dk/api/action/package_list
            var response = await GenericApi.GenericRestfulClient.Get<PackageListDTO>(source.name, "api/action/package_list");

            var pkgs = new List<Package>();
            response.result.ForEach(x => pkgs.Add(new Package() { name = x}));

            return Request.CreateResponse(HttpStatusCode.OK, pkgs);
        }

        [Route("api/GetDataSet")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetDataSet(Source source)
        {
            var pkg = new Package();
            pkg.name = source.packages.FirstOrDefault().name;
            pkg.dataSets = new List<DataSet>();

            //http://data.kk.dk/api/action/package_show?id=aalegraes
            var response = await GenericApi.GenericRestfulClient.Get<PackageDTO>(source.name, "/api/action/package_show", source.packages.FirstOrDefault().name);

            //find CSV datasets
            var csvFiles = response.result.resources.Where(x => x.format == "CSV");

            //check if it's a DataStore and if it's, get MetaData
            foreach (var file in csvFiles)
            {
                var resp = await GenericApi.GenericRestfulClient.Get<DataSetDTO>(source.name, "/api/action/datastore_search", file.id, 1);
                file.fields = resp.result.fields;
                
            }
            
            //add data sets to package
            response.result.resources.ForEach(x => (pkg.dataSets as List<DataSet>).Add(x));

            return Request.CreateResponse(HttpStatusCode.OK, pkg);
        }

        [Route("api/GetMetaData")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetMetaData(string url, string dataSourceId)
        {
            //http://data.kk.dk/api/action/datastore_search?resource_id=123014980123948702&limit=1
            var response = await GenericApi.GenericRestfulClient.Get<DataSetDTO>(url, "/api/action/datastore_search", dataSourceId, 1);
            var fields = response.result.fields;

            return Request.CreateResponse(HttpStatusCode.OK, fields);
        }

        [Route("api/GetDataLimit")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetDataLimit(string url, string id, int limit)
        {
            //http://data.kk.dk/api/action/datastore_search?resource_id=123014980123948702&limit=1
            var response = await GenericApi.GenericRestfulClient.Get(url, "/api/action/datastore_search", id, limit);

            //TODO parse response -> read first Fields, and them map to formatted object

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [Route("api/GetDataLimitOffset")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetDataLimitOffset(string url, string id, int limit, int offset)
        {
            //http://data.kk.dk/api/action/datastore_search?resource_id=123014980123948702&limit=1
            var response = await GenericApi.GenericRestfulClient.Get(url, "/api/action/datastore_search", id, limit, offset);

            //TODO parse response -> read first Fields, and them map to formatted object

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [Route("api/GetData")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetData(Visualization visual)
        {
            //http://data.kk.dk/api/action/datastore_search?resource_id=123014980123948702

           // var dict = new Dictionary<string, List<string>>();

            foreach (var source in visual.sources)
            {
                //get only selected packages
                var pckgs = source.packages.Where(x => x.selected == true);

                //get which fields are selected for each data source
                foreach (var dts in pckgs.SelectMany(x => x.dataSets.Where(y=>y.format=="CSV")))
                {
                    var flds = dts.fields.Where(x => x.selected).Select(x => x.id.ToString()).ToList();
                    var values = new Tuple<string, List<string>>(dts.id, flds);

                    //for each data source
                    var response = await GenericApi.GenericRestfulClient.
                        Get<object>(source.name, "/api/action/datastore_search_sql?sql=", values);

                    processJsonResponse(response, dts);
                }
            }

            //Merge all Data in one DAtaTable
            CreateDataTable(visual);


            //create RowData
            MergeData(visual);


            return Request.CreateResponse(HttpStatusCode.OK, visual);
        }

        private void MergeData(Visualization visual)
        {
            
        }

        private void CreateDataTable(Visualization visual)
        {
            //select X-Axys
            if (visual.table == null) visual.table = new Table();

            var table = visual.table;

            //map xAxys //TODO passe to the fluent
            var ds = visual.sources?.SelectMany(x => x.packages.Where(d => d.selected))?.SelectMany(x => x.dataSets);
            var field = ds.Where(x=>x.format=="CSV").SelectMany(x => x.fields).Where(y => y.xAxys);
            var xField = field.FirstOrDefault();
            var dataType = CloudApiHelpers.ResolveType(xField.type);
            var data = CloudApiHelpers.ConvertArrayToSpecificType(xField.record.value, dataType); //(xField.record.value as object[]).OfType().ToArray(); ;
;

            if (table.column == null) table.column = new Column();
            table.column.Value = data;
            table.column.Type = dataType;
            table.column.name = xField.name;

            //map yAxys
            var rows = new List<Row>();
            var series = ds.Where(x => x.format == "CSV").SelectMany(x => x.fields.Where(y => y.selected && !y.xAxys));
            foreach (var item in series)
            {
                var row = new Row() { name = item.name, Type = CloudApiHelpers.ResolveType(item.type), Value = CloudApiHelpers.ConvertArrayToSpecificType(item.record.value, CloudApiHelpers.ResolveType(item.type)) };
                rows.Add(row);
            }

            //if (table.rows == null) //TODO perhaps Initilize() fluent api
            table.rows = rows;

        }

        private void processJsonResponse(object response, DataSet dts)
        {
            JObject json = JObject.Parse(response.ToString());

            foreach (var item in dts.fields.Where(x=>x.selected))
            {
                var jsonValues = from p in json["result"]["records"]
                                 select (string)p[item.id.ToString()];


                if (item.record == null) {
                    item.record = new Record();     
                }

                item.record.name = item.id.ToString();
                item.record.value = jsonValues.ToArray();
            }           

            //dts.records = json
        }
    }
}
