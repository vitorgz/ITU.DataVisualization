using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl.Factories
{
    public class VisualizationFactory : IVisualizationFactory
    {
        public ISourceFactory AddSource(List<Source> source)
        {
            throw new NotImplementedException();
        }

        public Visualization Create()
        {
            throw new NotImplementedException();
        }

               public IVisualizationFactory Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
