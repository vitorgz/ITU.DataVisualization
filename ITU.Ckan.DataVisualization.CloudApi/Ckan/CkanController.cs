using ITU.Ckan.DataVisualization.CloudApi.DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://data.kk.dk/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = await client.GetAsync("api/action/package_list").ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStreamAsync().Result;
                        
                        var jsonSerializer = new DataContractJsonSerializer(typeof(PackageListDTO));
                        object objResponse = jsonSerializer.ReadObject(data);
                        
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent("Hello from OWIN!")
                        };
                    }
                }
                catch (HttpRequestException e)
                {
                    // Handle exception.
                    var s = e;
                    return null;
                }
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent("no data!")
            };
        }

        [Route("api/GetPackagesById")]
        public async Task<HttpResponseMessage> GetPackagesById()
        {
            //http://data.kk.dk/api/action/package_list
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://data.kk.dk/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = await client.GetAsync("/api/action/package_show?id=aalegraes").ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStreamAsync().Result;

                        var jsonSerializer = new DataContractJsonSerializer(typeof(PackageDTO));
                        object objResponse = jsonSerializer.ReadObject(data);

                        return new HttpResponseMessage()
                        {
                            Content = new StringContent("Hello from OWIN!")
                        };
                    }
                }
                catch (HttpRequestException e)
                {
                    // Handle exception.
                    var s = e;
                    return null;
                }
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent("no data!")
            };
        }
    }
}
