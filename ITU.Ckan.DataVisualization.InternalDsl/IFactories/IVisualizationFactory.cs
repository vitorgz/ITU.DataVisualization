using ITU.Ckan.DataVisualization.InternalDslApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl.IFactories
{
    public interface IVisualizationFactory
    {
        //IVisualizationFactory Initialize();
        IVisualizationFactory AddIn(Action<IVisualizationFactory> action);
        IVisualizationFactory AddSource(List<Source> source);
        IVisualizationFactory addTable(Table data);
        //List<Source> GetSources();
        Visualization Create();
        IVisualizationFactory GetData(VisualDTO filters);
        IVisualizationFactory GetPieChartData(VisualDTO filters);


    }
}
