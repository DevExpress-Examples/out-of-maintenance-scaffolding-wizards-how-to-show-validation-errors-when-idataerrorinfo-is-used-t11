using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Scaffolding.ValidationErrors.Common.Utils {
    public class ExpressionHelper {
        static readonly Dictionary<Type, object> TraitsCache = new Dictionary<Type, object>();

        public static Expression<Func<TPropertyOwner, bool>> GetValueEqualsExpression<TPropertyOwner, TProperty>(Expression<Func<TPropertyOwner, TProperty>> getPropertyExpression, TProperty constant) {
            Expression equalExpression = Expression.Equal(getPropertyExpression.Body, Expression.Constant(constant));
            return Expression.Lambda<Func<TPropertyOwner, bool>>(equalExpression, getPropertyExpression.Parameters.Single());
        }
        public static EntityTraits<TPropertyOwner, TProperty> GetEntityTraits<TOwner, TPropertyOwner, TProperty>(TOwner owner, Expression<Func<TPropertyOwner, TProperty>> getPropertyExpression, Action<TPropertyOwner, TProperty> setPropertyAction) {
            object traits = null;
            if(!TraitsCache.TryGetValue(owner.GetType(), out traits)) {
                traits = new EntityTraits<TPropertyOwner, TProperty>(getPropertyExpression.Compile(), setPropertyAction ?? GetSetValueActionExpression(getPropertyExpression).Compile(), GetHasValueFunctionExpression(getPropertyExpression).Compile());
                TraitsCache[owner.GetType()] = traits;
            }
            return (EntityTraits<TPropertyOwner, TProperty>)traits;
        }

        static Expression<Action<TPropertyOwner, TProperty>> GetSetValueActionExpression<TPropertyOwner, TProperty>(Expression<Func<TPropertyOwner, TProperty>> getPropertyExpression) {
            MemberExpression body = (MemberExpression)getPropertyExpression.Body;
            ParameterExpression thisParameter = getPropertyExpression.Parameters.Single();
            ParameterExpression propertyValueParameter = Expression.Parameter(typeof(TProperty), "propertyValue");
            BinaryExpression assignPropertyValueExpression = Expression.Assign(body, propertyValueParameter);
            return Expression.Lambda<Action<TPropertyOwner, TProperty>>(assignPropertyValueExpression, thisParameter, propertyValueParameter);
        }
        static Expression<Func<TPropertyOwner, bool>> GetHasValueFunctionExpression<TPropertyOwner, TProperty>(Expression<Func<TPropertyOwner, TProperty>> getPropertyExpression) {
            MemberExpression memberExpression = (MemberExpression)getPropertyExpression.Body;
            if(memberExpression.Expression is MemberExpression) {
                Expression equalExpression = Expression.NotEqual(memberExpression.Expression, Expression.Constant(null));
                return Expression.Lambda<Func<TPropertyOwner, bool>>(equalExpression, getPropertyExpression.Parameters.Single());
            }
            return x => true;
        }
    }
    public class EntityTraits<TEntity, TPrimaryKey> {
        public EntityTraits(Func<TEntity, TPrimaryKey> getPrimaryKeyFunction, Action<TEntity, TPrimaryKey> setPrimaryKeyAction, Func<TEntity, bool> hasPrimaryKeyFunction) {
            this.GetPrimaryKey = getPrimaryKeyFunction;
            this.SetPrimaryKey = setPrimaryKeyAction;
            this.HasPrimaryKey = hasPrimaryKeyFunction;
        }
        public Func<TEntity, TPrimaryKey> GetPrimaryKey { get; private set; }
        public Action<TEntity, TPrimaryKey> SetPrimaryKey { get; private set; }
        public Func<TEntity, bool> HasPrimaryKey { get; private set; }
    }
}
