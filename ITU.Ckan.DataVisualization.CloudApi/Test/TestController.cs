using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ITU.Ckan.DataVisualization.CloudApi.Test
{
    public class TestController : ApiController
    {
        [Route("api/test")]
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent("Hello from OWIN!")
            };
        }

        public string Get(string id)
        {
            return string.Format("The parameter value is {0}", id);
        }
    }
}
