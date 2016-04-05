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
        public IVisualizationFactory AddSource(List<Source> sources)
        {
            visual.sources = new List<Source>(sources);
            return this;
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

        public List<Source> GetSources()
        {
            return visual.sources.ToList();
        }

        public IVisualizationFactory Initialize()
        {
            visual = new Visualization();
            return this;
        }
    }
}
