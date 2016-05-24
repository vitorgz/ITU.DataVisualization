using ITU.Ckan.DataVisualization.InternalDslApi;
using ITU.Ckan.DataVisualization.InternalDslApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl.Factories
{
    public class VisualizationFactory : IVisualizationFactory
    {
        static Visualization visual;
        public IVisualizationFactory AddSource(List<Source> sources)
        {
            visual.sources = new List<Source>(sources);
            return this;
        }

        public IVisualizationFactory addTable(Table data)
        {
            visual.table = data;
            return this;
        }


        public Visualization Create()
        {
            return visual;
        }

        public async Task<IVisualizationFactory> GetData(VisualDTO filters)
        {
            var table = await InternalClient.Get<Table>(filters);
            visual.table = new Table();
            visual.table = table;
            return this;
        }

        public async Task<IVisualizationFactory> GetPieChartData(VisualDTO filters)
        {
            //TODO
            return this;
        }

        public List<Source> GetSources()
        {
            return visual.sources.ToList();
        }

        static VisualizationFactory visualf;

        public static IVisualizationFactory Initialize
        {
            get
            {
                if (visual == null)
                    visual = new Visualization();
                if (visualf == null)
                    visualf = new VisualizationFactory();

                return visualf;
            }
        }
    }
}

