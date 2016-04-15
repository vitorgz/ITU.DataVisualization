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
    public static class InternalClient
    {
        public static async Task<T> GetPackages<T>(string url)
        {
            var cloudApi = "/api/GetPackages";
            var ckanApi = "api/action/package_list";
            var content = new SourceDTO() { sourceName = url, command = ckanApi };
            return await GetCkanAsync<T>(cloudApi, content);
        }

        public static async Task<T> GetTags<T>(string url)
        {
            var cloudApi = "/api/GetTags";
            var ckanApi = "/api/action/tag_list";
            var content = new SourceDTO() { sourceName = url, command = ckanApi };
            return await GetCkanAsync<T>(cloudApi, content);
        }

        public static async Task<T> GetOrganizations<T>(string url)
        {
            var cloudApi = "/api/GetOrganizations";
            var ckanApi = "/api/action/organization_list";
            var content = new SourceDTO() { sourceName = url, command = ckanApi };
            return await GetCkanAsync<T>(cloudApi, content);
        }

        public static async Task<T> GetGroups<T>(string url)
        {
            var cloudApi = "/api/GetGroups";
            var ckanApi = "/api/action/group_list";
            var content = new SourceDTO() { sourceName = url, command = ckanApi };
            return await GetCkanAsync<T>(cloudApi, content);
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

        public static async Task<T> Get<T>(VisualDTO filtersData)
        {
            var api = "/api/GetData";
            return await GetCkanAsync<T>(api, filtersData);
        }

        public static async Task<T> GetDataFromSource<T>(SourceDTO source)
        {
            if (source.offset != 0)
            {
                return await GetCkanAsync<T>("/api/GetDataLimitOffset", source);
            }

            var api = "/api/GetDataLimit";
            return await GetCkanAsync<T>(api, source);
        }

        public static async Task<T> Get<T>(string url, string id, int limit)
        {
            var dto = new SourceDTO(){ sourceName = url, dataSetId = id, limit = limit };
            var api = "/api/GetDataLimit";
            return await GetCkanAsync<T>(api, dto);
        }

        public static async Task<T> Get<T>(string url, string id, int offset, int limit)
        {
            var dto = new SourceDTO() { sourceName = url, dataSetId = id, limit = limit, offset = offset };
            var api = "/api/GetDataLimit";
            return await GetCkanAsync<T>(api, dto);
        }

        public static async Task<T> SendCommand<T>(string url, string command)
        {
            var dto = new SourceDTO() { sourceName = url, command = command };
            var api = "/api/SendCommand";
            return await GetCkanAsync<T>(api, dto);
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
