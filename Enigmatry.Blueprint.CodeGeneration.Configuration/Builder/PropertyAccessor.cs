using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Enigmatry.Blueprint.CodeGeneration.Configuration.Builder
{
    public class PropertyAccessor<T, TProperty>
    {
        public PropertyInfo PropertyInfo { get; }

        public PropertyAccessor(Expression<Func<T, TProperty>> propertyExpression)
        {
            Check.NotNull(propertyExpression, nameof(propertyExpression));
            PropertyInfo = propertyExpression.GetPropertyAccess();
        }
    }
}
