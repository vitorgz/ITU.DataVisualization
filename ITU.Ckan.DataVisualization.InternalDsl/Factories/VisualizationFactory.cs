using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl.Factories
{
    public class VisualizationFactory : IVisualizationFactory
    {
        Visualization visual;
        public ISourceFactory AddSource(List<Source> source)
        {
            throw new NotImplementedException();
        }

        public Visualization Create()
        {
            return visual;
        }

        public IVisualizationFactory Initialize()
        {
            visual = new Visualization();
            return this;
        }
    }
}
