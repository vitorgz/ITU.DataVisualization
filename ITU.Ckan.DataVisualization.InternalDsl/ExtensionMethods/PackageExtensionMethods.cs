using ITU.Ckan.DataVisualization.InternalDslApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl.ExtensionMethods
{
    public static class PackageExtensionMethods
    {
        public static async Task<List<DataSet>> GetDataSets(this Package pck, string dataSourceId, string packageid)
        {
            return await InternalClient.GetDataSet<List<DataSet>>(dataSourceId, packageid);
        }

    }
}
