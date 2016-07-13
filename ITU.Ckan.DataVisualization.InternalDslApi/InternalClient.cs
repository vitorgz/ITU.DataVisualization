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
        /// <summary>
        /// GEt the list of pacakges for a source
        /// </summary>
        /// <typeparam name="T">List<Packages></typeparam>
        /// <param name="url"></param>
        /// <returns>List<PAckage></returns>
        public static async Task<T> GetPackages<T>(string url)
        {
            var cloudApi = "/api/GetPackages";
            var ckanApi = "api/action/package_list";
            var content = new SourceDTO() { sourceName = url, command = ckanApi };
            return await GetCkanAsync<T>(cloudApi, content);
        }

        /// <summary>
        /// GEt the list of tags for a source
        /// </summary>
        /// <typeparam name="T">List<Tag></typeparam>
        /// <param name="url"></param>
        /// <returns>List<Tag></returns>
        public static async Task<T> GetTags<T>(string url)
        {
            var cloudApi = "/api/GetTags";
            var ckanApi = "/api/action/tag_list";
            var content = new SourceDTO() { sourceName = url, command = ckanApi };
            return await GetCkanAsync<T>(cloudApi, content);
        }

        /// <summary>
        /// GEt the list of organizations for a source
        /// </summary>
        /// <typeparam name="T">List<Organization></typeparam>
        /// <param name="url"></param>
        /// <returns>List<Organization></returns>
        public static async Task<T> GetOrganizations<T>(string url)
        {
            var cloudApi = "/api/GetOrganizations";
            var ckanApi = "/api/action/organization_list";
            var content = new SourceDTO() { sourceName = url, command = ckanApi };
            return await GetCkanAsync<T>(cloudApi, content);
        }

        /// <summary>
        /// GEt the list of groups for a source
        /// </summary>
        /// <typeparam name="T">List<Group></typeparam>
        /// <param name="url"></param>
        /// <returns>List<Group></returns>
        public static async Task<T> GetGroups<T>(string url)
        {
            var cloudApi = "/api/GetGroups";
            var ckanApi = "/api/action/group_list";
            var content = new SourceDTO() { sourceName = url, command = ckanApi };
            return await GetCkanAsync<T>(cloudApi, content);
        }

        /// <summary>
        /// List of DataSet for a source
        /// </summary>
        /// <typeparam name="T">List<DataSet></typeparam>
        /// <param name="url">source url</param>
        /// <param name="id">packageId</param>
        /// <returns>List<DataSet></returns>
        public static async Task<T> GetDataSet<T>(string url, string id)
        {
            var api = "/api/GetDataSet";
            var content = new DataSetDTO() { source = url, packageId = id };
            return await GetCkanAsync<T>(api, content);
        }

        /// <summary>
        /// Gets an List of Fields
        /// </summary>
        /// <param name="url">from the source</param>
        /// <param name="dataSetId">datasetID</param>
        /// <returns>List<Fields></returns>
        public static async Task<List<Field>> GetMetaData(SourceDTO dto)
        {
            var api = "/api/GetMetaData";
            return await GetCkanAsync<List<Field>>(api, dto);
        }

        /// <summary>
        /// Get data from many data sources
        /// </summary>
        /// <typeparam name="T">must be of type Table</typeparam>
        /// <param name="filtersData">parameters needed for getting data </param>
        /// <returns>A table</returns>
        public static async Task<T> Get<T>(VisualDTO filtersData)
        {
            var api = "/api/GetData";
            return await GetCkanAsync<T>(api, filtersData);
        }

        /// <summary>
        /// Get data for PieCharts, from many data sources
        /// </summary>
        /// <typeparam name="T">must be of type Table</typeparam>
        /// <param name="filtersData">parameters needed for getting data </param>
        /// <returns>Table of contents</returns>
        public static async Task<T> GetPieChart<T>(VisualDTO filtersData)
        {
            var api = "/api/GetDataPieChart";
            return await GetCkanAsync<T>(api, filtersData);
        }

        /// <summary>
        /// Get data from only one source
        /// </summary>
        /// <param name="source">parameters needed for getting data from one source</param>
        /// <returns>A table with the results </returns>
        public static async Task<Table> GetDataFromOneSource(SourceDTO source)
        {
            var api = "/api/GetDataResourceLimitOffset";
            return await GetCkanAsync<Table>(api, source);
        }

        /// <summary>
        /// Get data from only one data source
        /// </summary>
        /// <param name="url">from the data source</param>
        /// <param name="id">datasetId</param>
        /// <param name="offset">offset</param>
        /// <param name="limit">limit</param>
        /// <returns>Table of contents</returns>
        public static async Task<Table> Get(string url, string id, int offset, int limit)
        {
            var dto = new SourceDTO() { sourceName = url, dataSetId = id, limit = limit, offset = offset };
            var api = "/api/GetDataResourceLimitOffset";
            return await GetCkanAsync<Table>(api, dto);
        }

        /// <summary>
        /// send arbitrary command
        /// </summary>
        /// <param name="url">url from the host</param>
        /// <param name="command">string with the URL command to send</param>
        /// <returns>Json file</returns>
        public static async Task<string> SendCommand(string url, string command)
        {
            var dto = new SourceDTO() { sourceName = url, command = command };
            var api = "/api/SendCommand";
            return await GetCkanAsync<string>(api, dto);
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
