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
        Package package; 
        public IPackageFactory Initialize()
        {
            package = new Package();
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

        public async Task<IPackageFactory> GetDataSetsById(string dataSetUrl, string id)
        {
            //todo change it to <List<DataSet>> make more sense
            var pck = await InternalClient.GetDataSet<Package>(dataSetUrl, id);
            this.package = pck;

            return this;
        }
    }
}
