using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl
{
    public interface IGenericFactory<T>
    {
        IGenericFactory<T> AddPropertyValue(Expression<Func<T, object>> property, object value);
        T Create();
    }
}
