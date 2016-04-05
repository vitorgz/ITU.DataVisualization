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
        public static async Task<Package> GetDataSets(this Package pck, string dataSourceId, string packageid)
        {
            //todo can be moved to List<DataSet>
            return await InternalClient.GetDataSet<Package>(dataSourceId, packageid);
        }

        public static List<DataSet> GetSelecteDataSets(this Package pck)
        {
            return pck.dataSets.Where(x => x.selected).ToList();
        }
        public static List<DataSet> GetCVCFormatDataSets(this Package pck)
        {
            return pck.dataSets.Where(x => x.format == "CSV").ToList();
        }

    }
}
