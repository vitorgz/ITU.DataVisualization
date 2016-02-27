using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl
{
    public static class PropertyExtension
    {
        public static TProperty WithProperties<TProperty>(this TProperty classWithProperties, string name)
            where TProperty : IPropertable
        {
            classWithProperties.properties = new List<Property>(); //foreach list
            return classWithProperties;
        }
    }
}
