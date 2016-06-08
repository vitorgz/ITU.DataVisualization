using ITU.Ckan.DataVisualization.InternalDslApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDslApi
{
    public static class DBClient
    {

        public static void CreateVisualization(string name)
        {
               
        }

        public static async Task<T> GetVisualizationByName<T>(string name)
        {
            var dto = new VisualDTO() { name = name };
            var api = "/api/GetVisualization";
            return await GetCkanAsync<T>(api, dto);
        }

        public static async Task<T> GetListVisualizations<T>()
        {
            var api = "/api/GetVisualizationList";
            return await GetCkanAsync<T>(api, true);
        }

        public static async Task<T> SaveVisualization<T>(Visualization vs)
        {
            var api = "/api/SaveVisualization";
            return await GetCkanAsync<T>(api, vs);
        }

        internal static async Task<T> GetCkanAsync<T>(string api, object content)
        {
            T result = default(T);
            using (var client = new HttpClient())
            {
                var cloudURL = Properties.Resources.CloudURL;
                cloudURL += api;

                client.BaseAddress = new Uri(cloudURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(cloudURL, content).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStreamAsync().Result;

                        var jsonSerializer = new DataContractJsonSerializer(typeof(T));
                        result = (T)jsonSerializer.ReadObject(data);
                    }
                    return result;
                }
                catch (HttpRequestException e)
                {
                    throw new InvalidOperationException("An error has occurred while requesting data from CKAN");
                }
            }
        }
    }
}
