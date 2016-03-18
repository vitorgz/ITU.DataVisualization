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
            throw new NotImplementedException();
        }

        public Package Create()
        {
            throw new NotImplementedException();
        }

        
    }
}
