using ITU.Ckan.DataVisualization.InternalDslApi;
using ITU.Ckan.DataVisualization.InternalDslApi.DTO;
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
        [Route("api/SaveVisualization")]
        [HttpPost]
        public HttpResponseMessage SaveVisualization(Visualization name)
        {
            Startup.context.Visualizations.Add(name);
            Startup.context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        [Route("api/GetVisualization")]
        [HttpPost]
        public HttpResponseMessage GetVisualization(VisualDTO dto)
        {
            var name = dto.name;

            var vs = Startup.context.Visualizations.Where(x => x.name == name).FirstOrDefault();

            return Request.CreateResponse(HttpStatusCode.OK, vs);
        }

        [Route("api/GetVisualizationList")]
        [HttpPost]
        public HttpResponseMessage GetVisualizationList()
        {
            var vsNames = Startup.context.Visualizations.Select(x => x.name).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, vsNames);
        }
    }
}
