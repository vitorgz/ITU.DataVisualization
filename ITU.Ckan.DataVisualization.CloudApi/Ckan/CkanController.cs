using ITU.Ckan.DataVisualization.CloudApi.DTO;
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
        public async Task<HttpResponseMessage> GetPackages()
        {
            //http://data.kk.dk/api/action/package_list
            var response = await GenericApi.GenericRestfulClient.Get<PackageListDTO>("http://data.kk.dk/", "api/action/package_list");

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [Route("api/GetPackagesById")]
        public async Task<HttpResponseMessage> GetPackagesById()
        {
            //http://data.kk.dk/api/action/package_show?id=aalegraes
            var response = await GenericApi.GenericRestfulClient.Get<PackageDTO>("http://data.kk.dk/", "/api/action/package_show", "aalegraes");

            //check if any of those is a CSV (or do it in the client)
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [Route("api/GetMetaData")]
        public async Task<HttpResponseMessage> GetMetaData(string id)
        {
            //http://data.kk.dk/api/action/datastore_search?resource_id=123014980123948702&limit=1
            var response = await GenericApi.GenericRestfulClient.Get<DataSetDTO>("http://data.kk.dk/", "/api/action/datastore_search", id);

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [Route("api/GetData")]
        public async Task<HttpResponseMessage> GetData(Visualization visual)
        {
            //http://data.kk.dk/api/action/datastore_search?resource_id=123014980123948702

            var dict = new Dictionary<string, List<string>>();

            foreach (var source in visual.sources)
            {
                //get only selected packages
                var pckgs = source.packages.Where(x => x.author == "s");

                //get which fields are selected for each data source
                foreach (var dts in pckgs.SelectMany(x => x.dataSets))
                {
                    var flds = dts.fields.Where(x => x.selected).Select(x => x.name).ToList();
                    dict.Add(dts.id, flds);
                }

                //for each data source
                var response = await GenericApi.GenericRestfulClient.Get<DataSetDTO>(source.name, "/api/action/datastore_search", dict);
            }

            //merge process!

            return Request.CreateResponse(HttpStatusCode.OK, new List<int>());
        }
    }
}
