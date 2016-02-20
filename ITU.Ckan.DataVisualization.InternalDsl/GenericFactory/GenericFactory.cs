using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl
{
    public class GenericFactory<T> : IGenericFactory<T>
    {
        T entity;

        public GenericFactory(T entity)
        {
            this.entity = entity;
        }

        public IGenericFactory<T> AddPropertyValue(Expression<Func<T, object>> property, object value)
        {
            PropertyInfo propertyInfo = null;
            if (property.Body is MemberExpression)
            {
                propertyInfo = (property.Body as MemberExpression).Member as PropertyInfo;
            }
            else
            {
                propertyInfo = (((UnaryExpression)property.Body).Operand as MemberExpression).Member as PropertyInfo;
            }
            propertyInfo.SetValue(entity, value, null);

            return this;
        }

        public T Create()
        {
            return this.entity;
        }
    }
}
