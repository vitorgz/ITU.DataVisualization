using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl.ExtensionMethods
{
    public static class SourceExtensionMethods
    {
        public static Source GetSource(this Visualization root, string name)
        {
            var s = root.sources.Where(x => x.name == name).FirstOrDefault();
            return s;
        }

        public static Source GetSourceById(this Visualization root, Expression<Func<Source, bool>> property)
        {
            Func<Source, bool> funcWhere = property.Compile();
            var s = root.sources.Where(funcWhere).FirstOrDefault();
            return s;
        }
    }
}
