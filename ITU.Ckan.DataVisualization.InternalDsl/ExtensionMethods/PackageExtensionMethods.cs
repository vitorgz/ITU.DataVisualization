using ITU.Ckan.DataVisualization.InternalDslApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public static Package GetPackageByName(this Source root, Expression<Func<Package, bool>> property)
        {
            Func<Package, bool> funcWhere = property.Compile();
            var s = root.packages.Where(funcWhere).FirstOrDefault();
            return s;
        }

        public static List<DataSet> GetSelecteDataSets(this Package pck)
        {
            return pck.dataSets.Where(x => x.selected).ToList();
        }

        public static List<DataSet> GetCVCFormatDataSets(this Package pck)
        {
            return pck.dataSets.Where(x => x.format == "CSV").ToList();
        }

        public static void GetDataSetsById(this Package package, string dataSetUrl, string id)
        {
            var task = Task.Run(async () =>
            {
                var dts = await InternalClient.GetDataSet<List<DataSet>>(dataSetUrl, id);
                package.dataSets = dts;
            });
            task.Wait();
        }

        public static Package AddIn(this Package pck, Action<Package> action)
        {
            action.Invoke(pck);

            return pck;
        }

    }
}
