using ITU.Ckan.DataVisualization.InternalDsl.IFactories;
using ITU.Ckan.DataVisualization.InternalDslApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl.Factories
{
    public class PackageFactory : IPackageFactory
    {
        private static Package package;

        private static IPackageFactory packagef;

        public static IPackageFactory Initialize
        {
            get
            {
                if (packagef == null)
                    packagef = new PackageFactory();
                if (package == null)
                    package = new Package();
                return packagef as IPackageFactory;
            }
        }

        public IPackageFactory AddIn(Action<IPackageFactory> action)
        {
            var expression = PackageFactory.Initialize;
            action.Invoke(expression);

            return this;
        }

        public IPackageFactory AddDataSet(List<DataSet> dataSets)
        {
            package.dataSets = new List<DataSet>(dataSets);
            return this;
        }

        public Package Create()
        {
            return package;
        }

        public IPackageFactory GetDataSetsById(string dataSetUrl, string id)
        {
            var task = Task.Run(async () =>
            {
                var dts = await InternalClient.GetDataSet<List<DataSet>>(dataSetUrl, id);
            package.dataSets = dts;
            });
            task.Wait();

            return this;
        }
    }
}
