using ITU.Ckan.DataVisualization.InternalDslApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl.Factories
{
    public interface IVisualizationFactory
    {
       //IVisualizationFactory Initialize();
        IVisualizationFactory AddSource(List<Source> source);
        //List<Source> GetSources();
        Visualization Create();
        Task<IVisualizationFactory> GetData(VisualDTO filters);
    }
}
