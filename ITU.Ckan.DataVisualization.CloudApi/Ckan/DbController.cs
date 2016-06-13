using ITU.Ckan.DataVisualization.EFDataBase;
using ITU.Ckan.DataVisualization.InternalDslApi;
using ITU.Ckan.DataVisualization.InternalDslApi.DTO;
using Newtonsoft.Json;
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
        public async Task<HttpResponseMessage> SaveVisualization(Visualization visual)
        {
            var visuaJSON = JsonConvert.SerializeObject(visual);

            var data = new VisualizationDB() { name = visual.name, visualizationAsJson = visuaJSON };

            Startup.context.Visualizations.Add(data);
            await Startup.context.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        [Route("api/GetVisualization")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetVisualization(VisualDTO dto)
        {
            var name = dto.name;

            var vs = Startup.context.Visualizations.Where(x => x.name == name).FirstOrDefault();
            
            var deserialize = Task.Factory.StartNew( () => JsonConvert.DeserializeObject(vs.visualizationAsJson, typeof(Visualization)));

            Visualization visual = deserialize.Result as Visualization;

            return Request.CreateResponse(HttpStatusCode.OK, visual);
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
