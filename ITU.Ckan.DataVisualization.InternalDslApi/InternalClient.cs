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
    public static class InternalClient
    {
        public static async Task<T> Get<T>(string url, string api)
        {
            return await GetCkanAsync<T>(url, api);
        }

        public static async Task<T> Get<T>(string url, string api, Dictionary<string, List<string>> filters) {
            return default(T);  
        }

        public static async Task<T> Get<T>(string url, string api, string id)
        {
            return default(T);

        }

        public static async Task<T> Get<T>(string url, string api, string id, int limit)
        {
            return default(T);
        }

        internal static async Task<T> GetCkanAsync<T>(string url, string api)
        {
            T result = default(T);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = await client.GetAsync(api).ConfigureAwait(false);
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
                    // Handle exception.
                    var s = e;
                    return default(T);
                }
            }
        }

    }
}
