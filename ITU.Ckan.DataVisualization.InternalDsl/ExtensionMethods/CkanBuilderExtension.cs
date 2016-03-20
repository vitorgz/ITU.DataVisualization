using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl
{
    public static class CkanBuilderExtension
    {
        public static TProperty addAttributes<TProperty, T>(this TProperty classWithProperties, List<T> name)
           where TProperty : ICkanBuilder
           where T : IEntity
        {

            if (classWithProperties.GetType() == typeof(Group))
            {
                (classWithProperties as Group).properties = new List<Property>();
            }

            if (classWithProperties.GetType() == typeof(Source))
            {
                var source = (classWithProperties as Source);

                Type typeParameterType = typeof(T);
                if (typeof(T) == typeof(Group)) source.groups = (name as List<Group>);
            }
            
            return classWithProperties;

        }

        public static DataSet addFields(this DataSet dataSet, IEnumerable<Field> fieldsList)
        {
            dataSet.fields = fieldsList;
            return dataSet;
        }

        public static DataSet addRecods(this DataSet dataSet, IEnumerable<Field> fieldsList)
        {
            dataSet.fields = fieldsList;
            return dataSet;
        }

       

        
    }
}
