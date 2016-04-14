using ITU.Ckan.DataVisualization.CloudApi.Deserialize;
using ITU.Ckan.DataVisualization.CloudApi.Helpers;
using ITU.Ckan.DataVisualization.InternalDslApi.DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            var response = await GenericApi.GenericRestfulClient.Get<PackageListDeserialize>(source.name, "api/action/package_list");

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
            var response = await GenericApi.GenericRestfulClient.Get<PackageDeserialize>(source.name, "/api/action/package_show", source.packages.FirstOrDefault().name);

            //find CSV datasets
            var csvFiles = response.result.resources.Where(x => x.format == "CSV");

            //check if it's a DataStore and if it's, get MetaData
            foreach (var file in csvFiles)
            {
                var resp = await GenericApi.GenericRestfulClient.Get<DataSetDeserialize>(source.name, "/api/action/datastore_search", file.id, 1);
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
            var response = await GenericApi.GenericRestfulClient.Get<DataSetDeserialize>(url, "/api/action/datastore_search", dataSourceId, 1);
            var fields = response.result.fields;

            return Request.CreateResponse(HttpStatusCode.OK, fields);
        }

        [Route("api/GetDataLimit")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetDataLimit(SourceDTO source)
        {
            //http://data.kk.dk/api/action/datastore_search?resource_id=123014980123948702&limit=1
            var response = await GenericApi.GenericRestfulClient.Get(
                source.sourceName, 
                "/api/action/datastore_search", 
                source.dataSetId, 
                source.limit);

            //TODO parse response -> read first Fields, and them map to formatted object

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [Route("api/GetDataLimitOffset")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetDataLimitOffset(SourceDTO source)
        {
            //http://data.kk.dk/api/action/datastore_search?resource_id=123014980123948702&limit=1
            var response = await GenericApi.GenericRestfulClient.Get(
                source.sourceName,
                "/api/action/datastore_search",
                source.dataSetId, 
                source.limit, 
                source.offset);

            //TODO parse response -> read first Fields, and them map to formatted object

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [Route("api/GetData")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetData(VisualDTO visual)
        {
            //http://data.kk.dk/api/action/datastore_search?resource_id=123014980123948702

            // var dict = new Dictionary<string, List<string>>();

            foreach (var item in visual.sources)
            {
                var flds = item.fields.Select(x => x.id.ToString()).ToList();
                    var values = new Tuple<string, List<string>>(item.dataSetId, flds);

                    //for each data source
                    var response = await GenericApi.GenericRestfulClient.
                        Get<object>(item.sourceName, "/api/action/datastore_search_sql?sql=", values);

                    processJsonResponse(response, item.fields);
            }

            //Merge all Data in one DAtaTable
            var table = CreateDataTable(visual);


            //create RowData
            MergeData(visual);


            return Request.CreateResponse(HttpStatusCode.OK, table);
        }

        [Route("api/SendCommand")]
        [HttpPost]
        public async Task<HttpResponseMessage> SendCommand(SourceDTO source)
        {
            var response = await GenericApi.GenericRestfulClient.GetJson<object>(source.sourceName, source.command);

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        private void MergeData(VisualDTO visual)
        {
            
        }

        private Table CreateDataTable(VisualDTO visual)
        {
            //select X-Axys
           var table = new Table();

            //map xAxys //TODO move to the fluent
            var fields = visual.sources.SelectMany(x => x.fields);
            var xField = fields.Where(y => y.xAxys).FirstOrDefault();
            var dataType = CloudApiHelpers.ResolveType(xField.type);
            var data = CloudApiHelpers.ConvertArrayToSpecificType(xField.record.value, dataType); //(xField.record.value as object[]).OfType().ToArray(); ;
;

            if (table.column == null) table.column = new Column();
            table.column.Value = data;
            table.column.Type = dataType;
            table.column.name = xField.name;

            //map yAxys
            var rows = new List<Row>();
            var series = fields.Where(y => y.selected && !y.xAxys);
            foreach (var item in series)
            {
                var row = new Row() { name = item.name, Type = CloudApiHelpers.ResolveType(item.type), Value = CloudApiHelpers.ConvertArrayToSpecificType(item.record.value, CloudApiHelpers.ResolveType(item.type)) };
                rows.Add(row);
            }

            //if (table.rows == null) //TODO perhaps Initilize() fluent api
            table.rows = rows;

            return table;

        }

        private void processJsonResponse(object response, List<Field> fields)
        {
            JObject json = JObject.Parse(response.ToString());

            foreach (var item in fields)
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
