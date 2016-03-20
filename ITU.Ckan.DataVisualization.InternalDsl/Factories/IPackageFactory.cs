using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl.Factories
{
    public interface IPackageFactory
    {
        IPackageFactory Initialize();
        IPackageFactory AddDataSet(List<DataSet> dataSets);
        Package Create();
        Task<IPackageFactory> GetDataSetsById(string dataSetUrl, string id);
    }
}
