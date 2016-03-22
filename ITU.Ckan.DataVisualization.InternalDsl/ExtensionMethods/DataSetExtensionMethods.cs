using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl.ExtensionMethods
{
    public static class DataSetExtensionMethods
    {
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
