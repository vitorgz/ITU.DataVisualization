using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl.IFactories
{
    public interface IPackageFactory
    {
        //IPackageFactory Initialize;
        IPackageFactory AddIn(Action<IPackageFactory> action);
        IPackageFactory AddDataSet(List<DataSet> dataSets);
        Package Create();
        IPackageFactory GetDataSetsById(string dataSetUrl, string id);
    }
}
