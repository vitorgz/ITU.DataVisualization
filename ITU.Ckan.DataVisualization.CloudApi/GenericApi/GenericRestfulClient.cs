using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
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

        public static async Task<object> GetJson<T>(string url, string command)
        {
            return await GetCkanAsyncJson(url, command);
        }

        public static async Task<object> Get<T>(string url, string api, Tuple<string, List<string>> filters, int limit, int offset)
        {
            StringBuilder s = new StringBuilder();
            s.Append("SELECT ");

            s.Append(string.Join(",", filters.Item2));

            s.Append(" FROM ");
            s.Append("\"" + filters.Item1 + "\"");

            if (limit != 0)
                s.Append(" LIMIT " + limit);

            if (offset != 0)
                s.Append(" OFFSET " + offset);

            var path = api + s;

            var results = await GetCkanAsyncJson(url, path);

            //object schema = GetJsonSchema(filters.Item2);
            //var results = await GetCkanAsync<T>(url, path);

            return results;
        }

        /*
        private static ResultsDeserialize GetJsonSchema(List<string> filters)
        {
            var dto = new ResultsDeserialize();
            dto.result = new RecordsDeserialize();
            dto.result.records = new List<RecordDeserialize>() { new RecordDeserialize() };
            dynamic res = dto.result.records;
            res = new ExpandoObject();

            foreach (var item in filters)
            {
                ((IDictionary<string, object>)res)[item] = item.GetType().TypeInitializer;
            }           

            return dto;
        }
        */

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

        public static async Task<object> Get(string url, string api, string id, int limit)
        {
            return await GetCkanAsyncJson(url, api + "?resource_id=" + id + "&limit=" + limit);            
        }

        public static async Task<object> Get(string url, string api, string id, int offtset, int limit)
        {
            return await GetCkanAsyncJson(url, api + "?resource_id=" + id + "&offset=" + offtset + "&limit=" + limit);
        }

        public static async Task<T> GetCkanAsync<T>(string url, string api)
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
                    throw new InvalidOperationException("An error has occurred while requesting data from CKAN");
                }
            }
        }

        internal static async Task<object> GetCkanAsyncJson(string url, string api)
        {
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
                        var data = response.Content.ReadAsStringAsync().Result;

                        var json = JsonConvert.DeserializeObject(data);
                        return json;
                    }
                    return string.Empty;
                }
                catch (HttpRequestException e)
                {
                    throw new InvalidOperationException("An error has occurred while requesting data from CKAN");
                }
            }
        }
    }
}
