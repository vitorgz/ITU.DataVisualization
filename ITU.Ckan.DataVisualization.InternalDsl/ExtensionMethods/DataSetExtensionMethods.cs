using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl.ExtensionMethods
{
    public static class DataSetExtensionMethods
    {
        public static DataSet AddFields(this DataSet dataSet, IEnumerable<Field> fieldsList)
        {
            dataSet.fields = fieldsList;
            return dataSet;
        }

        public static DataSet AddRecods(this DataSet dataSet, IEnumerable<Field> fieldsList)
        {
            dataSet.fields = fieldsList;
            return dataSet;
        }

        public static Field GetXAxys(this DataSet dataSet)
        {
            return dataSet?.fields?.Where(x => x.xAxys).FirstOrDefault();
        }

        public static List<Field> GetYAxys(this DataSet dataSet)
        {
            var yAxys = dataSet?.fields?.Where(x => x.selected && !x.xAxys).ToList();
            return yAxys ?? new List<Field>();
        }
    }
}
