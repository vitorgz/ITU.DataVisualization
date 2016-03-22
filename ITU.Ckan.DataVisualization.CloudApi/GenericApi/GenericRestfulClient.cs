using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.CloudApi.GenericApi
{
    public static class GenericRestfulClient
    {
        public static async Task<T> Get<T>(string url, string api)
        {
            return await GetCkanAsync<T>(url, api);
        }

        public static async Task<T> Get<T>(string url, string api, Dictionary<string, List<string>> filters) {
            //if(filters.Count == 1) return await GetCkanAsync<T>(url, api+"/q="+filters.First());

            StringBuilder s = new StringBuilder();
            s.Append("SELECT ");
            foreach (var item in filters.Values)
            {              
                s.Append(string.Join(",",item));
            }
            s.Append(" FROM ");
            s.Append("\"" + filters.Select(x => x.Key).FirstOrDefault() + "\"");

            var path = api + s;

            return await GetCkanAsync<T>(url, path);
            /*
            //read anonnymous Json!
            var dummyObject = new {
                help = "",
                sucess = true,
                result = new { records = new[] { new { status = 0, bkrn = 0 } }},
            };
            JsonConvert.DeserializeAnonymousType(data, dummyObject);

            //using Decode
            dynamic data = Json.Decode(json);
            */
        }

        public static async Task<T> Get<T>(string url, string api, string id)
        {
            if(api.Contains("datastore"))
                return await GetCkanAsync<T>(url, api + "?resource_id=" + id);
            else if(api.Contains("package"))
                return await GetCkanAsync<T>(url, api + "?id=" + id);

            return default(T);
        }

        public static async Task<T> Get<T>(string url, string api, string id, int limit)
        {
            return await GetCkanAsync<T>(url, api + "?resource_id=" + id + "&limit=" + limit);
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
