using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl.Helpers
{
    public static class DslConverterHelpers
    {

        public static Type ResolveType(object type)
        {
            switch (type.ToString().ToLower())
            {
                case "int": return typeof(int);
                case "int2": return typeof(int);
                case "int4": return typeof(int);
                case "int8": return typeof(long);
                case "int16": return typeof(int);
                case "int32": return typeof(int);
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

        public static string[] ConvertToStringArray(object value)
        {
            try
            {
                var objArr = (value as object[]);
                var arr = Array.CreateInstance(typeof(string), objArr.Length);
                arr = Array.ConvertAll(objArr, elem => Convert.ChangeType(elem, typeof(string)));

                return arr.OfType<string>().ToArray();
            }
            catch
            {
                throw new FormatException("fail to convert data");
            }
        }

        public static Array ConvertToSpecificType(object value, Type type)
        {
            try
            {
                var objArr = (value as object[]);
                var arr = Array.CreateInstance(type, objArr.Length);
                arr = Array.ConvertAll(objArr, elem => Convert.ChangeType(elem, type));

                return arr;
            }
            catch
            {
                throw new FormatException("fail to convert data");
            }

            /*
            MethodInfo method = typeof(Queryable).GetMethod("OfType");
            MethodInfo generic = method.MakeGenericMethod(new Type[] { type });
            // Use .NET 4 covariance
            var result = (IEnumerable<object>)generic.Invoke
                  (null, new object[] { value });
            object[] array = result.ToArray();

            return array;
            */
        }
    }
}
