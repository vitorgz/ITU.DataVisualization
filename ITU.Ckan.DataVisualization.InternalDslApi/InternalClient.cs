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
        public static async Task<T> GetPackages<T>(string url)
        {
            var api = "/api/GetPackages";
            var content = new Source() { name = url };
            return await GetCkanAsync<T>(api, content);
        }

        public static async Task<T> GetTags<T>(string url)
        {
            var api = "/api/action/tag_list";
            var content = new Source() { name = url };
            return await GetCkanAsync<T>(api, content);
        }

        public static async Task<T> GetOrganizations<T>(string url)
        {
            var api = "/api/action/organization_list";
            var content = new Source() { name = url };
            return await GetCkanAsync<T>(api, content);
        }

        public static async Task<T> GetGroups<T>(string url)
        {
            var api = "/api/action/group_list";
            var content = new Source() { name = url };
            return await GetCkanAsync<T>(api, content);
        }

        public static async Task<T> GetDataSet<T>(string url, string id)
        {
            var api = "/api/GetDataSet";
            var content = new Source() { name = url };
            content.packages = new List<Package>() { new Package() { name = id} };
            return await GetCkanAsync<T>(api, content);
        }

        public static async Task<T> GetMetaData<T>(string url, string dataSetId)
        {
            var api = "/api/GetMetaData";
            var content = new DataSet() { resource_id = dataSetId }; //TODO url missing
            return await GetCkanAsync<T>(api, content);
        }

        public static async Task<T> Get<T>(string url, Dictionary<string, List<string>> filters) {
            return default(T);  //TODO 
        }

        public static async Task<T> Get<T>(Visualization visual)
        {
            var api = "/api/GetData";
            return await GetCkanAsync<T>(api, visual);
        }

        public static async Task<T> Get<T>(Visualization visual, int limit)
        {
            //TODO
            var api = "/api/GetDataLimitOffset";
            return await GetCkanAsync<T>(api, visual);
        }

        public static async Task<T> Get<T>(Visualization visual, int limit, int offset)
        {
            //TODO
            var api = "/api/GetDataLimit";
            return await GetCkanAsync<T>(api, visual);
        }

        public static async Task<T> Get<T>(string url, string id, int limit)
        {
            return default(T);
        }

        public static async Task<T> Get<T>(string url, string id, int offset, int limit)
        {
            return default(T);
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
                    // Handle exception.
                    var s = e;
                    return default(T);
                }
            }
        }

    }
}
