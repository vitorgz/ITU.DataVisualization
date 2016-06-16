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
                //todo change it to <List<DataSet>> make more sense
                var pck = await InternalClient.GetDataSet<Package>(dataSetUrl, id);
            package = pck;
            });
            task.Wait();

            return this;
        }
    }
}
