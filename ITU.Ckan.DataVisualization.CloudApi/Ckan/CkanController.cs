using ITU.Ckan.DataVisualization.CloudApi.Deserialize;
using ITU.Ckan.DataVisualization.CloudApi.Helpers;
using ITU.Ckan.DataVisualization.InternalDslApi.DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace ITU.Ckan.DataVisualization.CloudApi.Ckan
{
    public class CkanController : ApiController
    {
        [Route("api/GetPackages")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetPackages(SourceDTO source)
        {
            try
            {
                //http://data.kk.dk/api/action/package_list
                var response = await GenericApi.GenericRestfulClient.Get<ListDeserialize>(source.sourceName, source.command);

                var pkgs = CloudApiHelpers.ConvertToListOfType<Package>(response);

                return Request.CreateResponse(HttpStatusCode.OK, pkgs);
            }
            catch (HttpRequestException ex)
            {
                //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        [Route("api/GetDataSet")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetDataSet(DataSetDTO dto)
        {
            try {                

                //http://data.kk.dk/api/action/package_show?id=aalegraes
                var response = await GenericApi.GenericRestfulClient.Get<PackageDeserialize>(dto.source, "/api/action/package_show", dto.packageId);

                //find CSV datasets
                var csvFiles = response.result.resources.Where(x => x.format == "CSV");

                //check if it's a DataStore and if it's, get MetaData
                foreach (var file in csvFiles)
                {
                    var resp = await GenericApi.GenericRestfulClient.Get<DataSetDeserialize>(dto.source, "/api/action/datastore_search", file.id, 1);
                    file.fields = resp.result.fields;

                }

                //add data sets to package
                var dts = response.result.resources;

                return Request.CreateResponse(HttpStatusCode.OK, dts);
            }
            catch (HttpRequestException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        [Route("api/GetMetaData")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetMetaData(string url, string dataSourceId)
        {
            try
            {
                //http://data.kk.dk/api/action/datastore_search?resource_id=123014980123948702&limit=1
                var response = await GenericApi.GenericRestfulClient.Get<DataSetDeserialize>(url, "/api/action/datastore_search", dataSourceId, 1);
                var fields = response.result.fields;

                return Request.CreateResponse(HttpStatusCode.OK, fields);
            }
            catch (HttpRequestException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        [Route("api/GetDataResourceLimit")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetDataResourceLimit(SourceDTO source)
        {
           try
            {                
                //TODO
                //ONly one source
                //http://data.kk.dk/api/action/datastore_search?resource_id=123014980123948702&limit=1
                var response = await GenericApi.GenericRestfulClient.Get(
                    source.sourceName,
                    "/api/action/datastore_search",
                    source.dataSetId,
                    source.limit);

                //TODO parse response -> read first Fields, and them map to formatted object

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (HttpRequestException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
}

        [Route("api/GetDataResourceLimitOffset")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetDataResourceLimitOffset(SourceDTO source)
        {
            try
            {
                //TODO it only works with one Source! make it sense?                              

                //returns JSON
                //http://data.kk.dk/api/action/datastore_search?resource_id=123014980123948702&limit=1
                var response = await GenericApi.GenericRestfulClient.Get(
                    source.sourceName,
                    "/api/action/datastore_search",
                    source.dataSetId,
                    source.limit,
                    source.offset);

                //TODO parse response -> read first Fields, and them map to formatted object (not easy)

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (HttpRequestException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        [Route("api/GetData")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetData(VisualDTO visual)
        {
            try
            {
                foreach (var item in visual.sources)
                {
                    var flds = item.fields.Select(x => x.id.ToString()).ToList();
                    var values = new Tuple<string, List<string>>(item.dataSetId, flds);
                    
                    object response = null;
                    try
                    {
                        //sometimes "limit" does not work in the CKAN, so the entire source comes up
                        //if query is longer than 10 seconds, we reject the SQL query and continue working with "dataStore"
                        response = await GenericApi.GenericRestfulClient.
                       Get<object>(item.sourceName, "/api/action/datastore_search_sql?sql=", values, visual.limit, visual.offset);
                    }
                    catch
                    {
                        response = null;
                        response = await GenericApi.GenericRestfulClient.Get(item.sourceName, "/api/action/datastore_search",
                            item.dataSetId, item.limit, item.offset);
                    }                    


                    //if the SQL query fails (sometimes due to un-authorized permissions)
                    //query the whole table using the datasore_search command 
                    if (string.IsNullOrEmpty(response.ToString()))
                    {
                        response = null;
                        response = await GenericApi.GenericRestfulClient.Get(item.sourceName, "/api/action/datastore_search",
                            item.dataSetId, item.limit, item.offset);
                    }

                    item.fields.ForEach(x => x.type = CloudApiHelpers.ResolveType(x.type));
                    CloudApiHelpers.ProcessJsonResponse(response, item.fields);

                }

                //Merge all Data in one DataTable
                var table = CloudApiHelpers.CreateDataTable(visual);
                
                return Request.CreateResponse(HttpStatusCode.OK, table);
            }
            catch (HttpRequestException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        [Route("api/GetDataPieChart")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetDataPieChart(VisualDTO visual)
        {
            try
            {
                var source = visual.sources.FirstOrDefault();
                var xAxys = source.fields.FirstOrDefault();

                var values = new Tuple<string, List<string>>(source.dataSetId, new List<string>() { xAxys.id.ToString() });
                              
                object response = null;
                try
                {
                    response = await GenericApi.GenericRestfulClient.
                          Get<object>(source.sourceName, "/api/action/datastore_search_sql?sql=", values, visual.limit, visual.offset);
                }
                catch
                {
                    response = null;
                    response = await GenericApi.GenericRestfulClient.Get(source.sourceName, "/api/action/datastore_search",
                         source.dataSetId, source.limit, source.offset);
                }

                if (string.IsNullOrEmpty(response.ToString()))
                    response = await GenericApi.GenericRestfulClient.Get(source.sourceName, "/api/action/datastore_search",
                          source.dataSetId, source.limit, source.offset);

                CloudApiHelpers.ProcessPieChartJsonResponse(response, xAxys);

                var table = CloudApiHelpers.PieChartAnalizeAndCreateTable(source.fields.FirstOrDefault().record);

                return Request.CreateResponse(HttpStatusCode.OK, table);
            }
            catch (HttpRequestException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        [Route("api/GetTags")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetTags(SourceDTO source)
        {
            try
            {
                var response = await GenericApi.GenericRestfulClient.Get<ListDeserialize>(source.sourceName, source.command);

                var tags = CloudApiHelpers.ConvertToListOfType<Tag>(response);

                return Request.CreateResponse(HttpStatusCode.OK, tags);
            }
            catch (HttpRequestException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        [Route("api/GetGroups")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetGroups(SourceDTO source)
        {
            try
            {
                var response = await GenericApi.GenericRestfulClient.Get<ListDeserialize>(source.sourceName, source.command);

                var groups = CloudApiHelpers.ConvertToListOfType<Group>(response);

                return Request.CreateResponse(HttpStatusCode.OK, groups);
            }
            catch (HttpRequestException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        [Route("api/GetOrganizations")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetOrganizations(SourceDTO source)
        {
            try
            {
                var response = await GenericApi.GenericRestfulClient.Get<ListDeserialize>(source.sourceName, source.command);

                var orgs = CloudApiHelpers.ConvertToListOfType<Organization>(response);

                return Request.CreateResponse(HttpStatusCode.OK, orgs);
            }
            catch (HttpRequestException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        [Route("api/SendCommand")]
        [HttpPost]
        public async Task<HttpResponseMessage> SendCommand(SourceDTO source)
        {
            try
            {
                var response = await GenericApi.GenericRestfulClient.GetJson<object>(source.sourceName, source.command);

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (HttpRequestException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }           
    }
}
