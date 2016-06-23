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
        IGenericFactory<T> AddGenericProperty(Expression<Func<T, object>> property, object value);

        IGenericFactory<T> AddIn(Action<T> action);

        T Create();
    }
}
