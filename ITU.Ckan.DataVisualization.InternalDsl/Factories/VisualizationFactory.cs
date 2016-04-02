using ITU.Ckan.DataVisualization.InternalDslApi;
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

        public async Task<IVisualizationFactory> GetData()
        {
            //TODO this will override some values, take care!
            visual = await InternalClient.Get<Visualization>(visual);
            return this;
        }

        public async Task<IVisualizationFactory> GetSources()
        {
            throw new NotImplementedException();
        }

        public IVisualizationFactory Initialize()
        {
            visual = new Visualization();
            return this;
        }
    }
}
