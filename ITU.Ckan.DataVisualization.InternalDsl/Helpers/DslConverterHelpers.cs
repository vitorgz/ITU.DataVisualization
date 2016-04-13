using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl.Helpers
{
    public static class DslConverterHelpers
    {
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
