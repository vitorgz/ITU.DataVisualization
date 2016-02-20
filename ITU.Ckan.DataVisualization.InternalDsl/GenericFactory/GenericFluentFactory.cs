using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl
{
    public static class GenericFluentFactory<T>
    {
        public static IGenericFactory<T> Init(T entity)
        {
            return new GenericFactory<T>(entity);
        }
    }
}
