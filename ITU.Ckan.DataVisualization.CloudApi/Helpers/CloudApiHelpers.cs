using ITU.Ckan.DataVisualization.CloudApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.CloudApi.Helpers
{
    public class CloudApiHelpers
    {
        public static object ConvertArrayToSpecificType(object data, Type dataType)
        {
            var objArr = (data as object[]);
            var arr = Array.CreateInstance(dataType, objArr.Length);

            if (dataType == typeof(int)) //TODO change by 0, not remove values
                objArr = objArr.Where(x => x != null).ToArray();

            arr = Array.ConvertAll(objArr, elem => Convert.ChangeType(elem, dataType));

            return arr;
        }

        public static Type ResolveType(object type)
        {
            switch (type.ToString())
            {
                case "int": return typeof(int);
                case "int2": return typeof(int);
                case "int4": return typeof(int);
                case "int8": return typeof(long);
                case "timestamp": return typeof(DateTime);
                case "float4": return typeof(float);
                case "time": return typeof(TimeSpan);
                case "float8": return typeof(double);
                case "bool": return typeof(bool);
                case "decimal": return typeof(decimal);
                case "text": return typeof(string);
                case "numeric": return typeof(int);
                default: return typeof(string);
            }
        }
    }
}
