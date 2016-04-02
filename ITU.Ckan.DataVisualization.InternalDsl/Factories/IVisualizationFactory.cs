using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl.Factories
{
    public interface IVisualizationFactory
    {
        IVisualizationFactory Initialize();
        ISourceFactory AddSource(List<Source> source);
        Task<IVisualizationFactory> GetSources();
        Visualization Create();
    }
}
