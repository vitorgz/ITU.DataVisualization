using ITU.Ckan.DataVisualization.InternalDsl.Factories;
using ITU.Ckan.DataVisualization.InternalDslApi;
using ITU.Ckan.DataVisualization.InternalDslApi.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl.ExtensionMethods
{
    public static class VisualizationExtensionMethods
    {
        public static Visualization GetVisualization(this Root root, string name)
        {
            var s = root.visualizations.Where(x => x.name == name).FirstOrDefault();
            return s;
        }
        
        public static Visualization GetVisualizationById(this Root root, Expression<Func<Visualization, bool>> property)
        {
            Func<Visualization, bool> funcWhere = property.Compile();
            var s = root.visualizations.Where(funcWhere).FirstOrDefault();
            return s;
        }

        public static async Task<Table> GetData(this Visualization visual, VisualDTO filters)
        {
            var data = await InternalClient.Get<Table>(filters);
            
            return data;
        }

        public static void AddTable(this Visualization visual, Table tab)
        {
            visual.table = tab;
        }

        public static async Task<Table> GetPieChartData(this Visualization visual, VisualDTO filters)
        {
            var data = await InternalClient.GetPieChart<Table>(filters);            

            return data;
        }

        public static VisualDTO GetFilters(this Visualization visual)
        {
            var filters = new VisualDTO();
            var sourceDTO = new List<SourceDTO>();

            foreach (var source in visual.sources)
            {
                if (source.packages == null) continue;
                var dts = source.packages
                    .Where(x => x.dataSets != null)
                    .SelectMany(x => x.dataSets.Where(y => y.format == "CSV")).ToList();

                dts.ForEach(x =>
                {
                    var selcted = x.fields.Where(y => y.selected || y.xAxys).ToList();
                    if (selcted.Any())
                        sourceDTO.Add(new SourceDTO()
                        {
                            dataSetId = x.id,
                            fields = selcted,
                            sourceName = source.name
                        });
                }
                );
            }


            filters.sources = sourceDTO;
            return filters;
        }

        public static Visualization AddIn(this Visualization visual, Action<Visualization> action)
        {
            action.Invoke(visual);
            
            return visual;
        }

        public static Visualization AddIn(this Visualization visual, Expression<Func<Visualization, object>> property, object value)
        {
            PropertyInfo propertyInfo = null;
            if (property.Body is MemberExpression)
            {
                propertyInfo = (property.Body as MemberExpression).Member as PropertyInfo;
            }
            else
            {
                propertyInfo = (((UnaryExpression)property.Body).Operand as MemberExpression).Member as PropertyInfo;
            }
            propertyInfo.SetValue(visual, value, null);

            return visual;
        }
    }
}
