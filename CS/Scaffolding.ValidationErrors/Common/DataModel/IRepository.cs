using System;
using System.Linq;
using System.Linq.Expressions;

namespace Scaffolding.ValidationErrors.Common.DataModel {
    public interface IRepository<TEntity, TPrimaryKey> : IReadOnlyRepository<TEntity> where TEntity : class {
        TEntity Find(TPrimaryKey key);
        void Remove(TEntity enity);
        TEntity Create();
        TEntity Reload(TEntity entity);
        Expression<Func<TEntity, TPrimaryKey>> GetPrimaryKeyExpression { get; }
        TPrimaryKey GetPrimaryKey(TEntity entity);
        bool HasPrimaryKey(TEntity entity);
        void SetPrimaryKey(TEntity entity, TPrimaryKey key);
    }
}
