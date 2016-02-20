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
                (classWithProperties as Group).Properties = new List<Property>();
            }

            if (classWithProperties.GetType() == typeof(Source))
            {
                var source = (classWithProperties as Source);

                Type typeParameterType = typeof(T);
                if (typeof(T) == typeof(Group)) source.Groups = (name as List<Group>);
            }
            
            return classWithProperties;

        }

        public static DataSet addFields(this DataSet dataSet, IEnumerable<Field> fieldsList)
        {
            dataSet.Fields = fieldsList;
            return dataSet;
        }

        public static DataSet addRecods(this DataSet dataSet, IEnumerable<Field> fieldsList)
        {
            dataSet.Fields = fieldsList;
            return dataSet;
        }

        public static Source GetSource(this Root root, string name)
        {
            var s = root.Sources.Where(x => x.Name == name).FirstOrDefault();
            return s;
        }

        public static Source GetSourceById(this Root root, Expression<Func<Source, bool>> property)
        {
            Func<Source, bool> funcWhere = property.Compile();
            var s = root.Sources.Where(funcWhere).FirstOrDefault();
            return s;
        }
    }
}
