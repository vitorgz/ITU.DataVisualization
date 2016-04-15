﻿using ITU.Ckan.DataVisualization.CloudApi.Deserialize;
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
        public async Task<HttpResponseMessage> GetPackages(SourceDTO source)
        {
            //http://data.kk.dk/api/action/package_list
            var response = await GenericApi.GenericRestfulClient.Get<ListDeserialize>(source.sourceName, source.command);

            var pkgs = CloudApiHelpers.ConvertToListOfType<Package>(response);

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

                   CloudApiHelpers.ProcessJsonResponse(response, item.fields);
            }

            //Merge all Data in one DAtaTable
            var table = CloudApiHelpers.CreateDataTable(visual);


            //create RowData
            //CloudApiHelpers.MergeData(visual);


            return Request.CreateResponse(HttpStatusCode.OK, table);
        }

        [Route("api/GetTags")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetTags(SourceDTO source)
        {
            var response = await GenericApi.GenericRestfulClient.Get<ListDeserialize>(source.sourceName, source.command);

            var tags = CloudApiHelpers.ConvertToListOfType<Tag>(response);

            return Request.CreateResponse(HttpStatusCode.OK, tags);
        }

        [Route("api/GetGroups")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetGroups(SourceDTO source)
        {
            var response = await GenericApi.GenericRestfulClient.Get<ListDeserialize>(source.sourceName, source.command);

            var groups = CloudApiHelpers.ConvertToListOfType<Group>(response);

            return Request.CreateResponse(HttpStatusCode.OK, groups);
        }

        [Route("api/GetOrganizations")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetOrganizations(SourceDTO source)
        {
            var response = await GenericApi.GenericRestfulClient.Get<ListDeserialize>(source.sourceName, source.command);

            var orgs = CloudApiHelpers.ConvertToListOfType<Organization>(response);

            return Request.CreateResponse(HttpStatusCode.OK, orgs);
        }

        [Route("api/SendCommand")]
        [HttpPost]
        public async Task<HttpResponseMessage> SendCommand(SourceDTO source)
        {
            var response = await GenericApi.GenericRestfulClient.GetJson<object>(source.sourceName, source.command);

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }           
    }
}
