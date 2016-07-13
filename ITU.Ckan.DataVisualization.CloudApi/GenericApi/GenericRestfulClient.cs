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
using System.Threading;
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

            for (int i = 0; i < filters.Item2.Count(); i++)
            {
                s.Append("\"" + filters.Item2.ElementAt(i) + "\"");
                if (i < filters.Item2.Count() - 1) s.Append(",");
            }            

            s.Append(" FROM ");
            s.Append("\"" + filters.Item1 + "\"");

            if (limit != 0)
                s.Append("&limit=" + limit);
            else
                s.Append("&limit=" + Properties.Resources.DefaultAmount);


            if (offset != 0)
                s.Append("&offset=" + offset);

            var path = api + s;

            var results = await GetCkanAsyncJson(url, path);
            
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
            var path = new StringBuilder();
            path.Append(api + "?resource_id=" + id);

            if (limit > 0)
                path.Append("&limit=" + limit);
            else
                path.Append("&limit=" + Properties.Resources.DefaultAmount);

            return await GetCkanAsync<T>(url, path.ToString());
        }

        public static async Task<object> Get(string url, string api, string id, int limit)
        {
            var path = new StringBuilder();
            path.Append(api + "?resource_id=" + id);

            if (limit > 0)
                path.Append("&limit=" + limit);
            else
                path.Append("&limit=" + Properties.Resources.DefaultAmount);

            return await GetCkanAsyncJson(url, path.ToString());            
        }

        public static async Task<object> Get(string url, string api, string id, int offtset, int limit)
        {
            var path = new StringBuilder();
            path.Append(api + "?resource_id=" + id);

            if (limit > 0)
                path.Append("&limit=" + limit);
            else
                path.Append("&limit=" + Properties.Resources.DefaultAmount);

            if (offtset > 0)
                path.Append("&offset=" + offtset);

            return await GetCkanAsyncJson(url, path.ToString());
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
                    var ct = new CancellationTokenSource();
                    ct.CancelAfter(10000);
                    HttpResponseMessage response = await client.GetAsync(api, ct.Token).ConfigureAwait(false);
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
                    var ct = new CancellationTokenSource();
                    ct.CancelAfter(10000);
                    HttpResponseMessage response = await client.GetAsync(api, ct.Token).ConfigureAwait(false);
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
