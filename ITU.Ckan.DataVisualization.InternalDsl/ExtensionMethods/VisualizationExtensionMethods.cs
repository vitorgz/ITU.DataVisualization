using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    }
}
