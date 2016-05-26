using ITU.Ckan.DataVisualization.InternalDslApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ITU.Ckan.DataVisualization.CloudApi.Ckan
{
    public class DbController : ApiController
    {
        [Route("api/GetVisualization")]
        [HttpPost]
        public HttpResponseMessage GetVisualization(string name)
        {
            var visual = DBClient.GetVisualizationByName(name);

            return Request.CreateResponse(HttpStatusCode.OK, visual);
        }

        [Route("api/GetVisualizationList")]
        public HttpResponseMessage GetVisualizationList()
        {
            var visuals = DBClient.GetListVisualizations();

            return Request.CreateResponse(HttpStatusCode.OK, visuals);
        }
    }
}
