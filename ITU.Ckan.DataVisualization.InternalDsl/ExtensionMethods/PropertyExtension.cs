using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl
{
    public static class PropertyExtension
    {
        public static TProperty AddGenericProperties<TProperty>(this TProperty classWithProperties, string name, object value)
            where TProperty : IPropertable
        {
            if (classWithProperties.properties == null)
                classWithProperties.properties = new List<Property>();

            classWithProperties.properties.Add(new Property() { name = name, value = value });

            return classWithProperties;
        }
    }
}
