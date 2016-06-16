using ITU.Ckan.DataVisualization.InternalDsl.IFactories;
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
        private static Visualization visual;

        private static VisualizationFactory visualf;

        public static IVisualizationFactory Initialize
        {
            get
            {
                if (visual == null)
                    visual = new Visualization();
                if (visualf == null)
                    visualf = new VisualizationFactory();

                return visualf as IVisualizationFactory;
            }
        }

        public IVisualizationFactory AddIn(Action<IVisualizationFactory> action)
        {
            var expression = VisualizationFactory.Initialize;
            action.Invoke(expression);

            return this;
        }

        public Visualization Create()
        {
            return visual;
        }

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

        public IVisualizationFactory GetData(VisualDTO filters)
        {
            var task = Task.Run(async () =>
            {
                var table = await InternalClient.Get<Table>(filters);
                visual.table = new Table();
                visual.table = table;
            });
            task.Wait();
            return this;
        }

        public IVisualizationFactory GetPieChartData(VisualDTO filters)
        {
            var task = Task.Run(async () =>
            {
                var table = await InternalClient.GetPieChart<Table>(filters);
                visual.table = new Table();
                visual.table = table;
            });
            task.Wait();
            return this;
        }

        public List<Source> GetSources()
        {
            return visual.sources.ToList();
        }  
       
    }
}

