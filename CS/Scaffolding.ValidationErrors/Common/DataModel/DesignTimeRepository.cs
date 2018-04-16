using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using DevExpress.Mvvm;
using Scaffolding.ValidationErrors.Common.Utils;

namespace Scaffolding.ValidationErrors.Common.DataModel {
    public abstract class DesignTimeRepository<TEntity, TPrimaryKey> : DesignTimeReadOnlyRepository<TEntity>, IRepository<TEntity, TPrimaryKey>
        where TEntity : class {

        readonly Expression<Func<TEntity, TPrimaryKey>> getPrimaryKeyExpression;
        readonly EntityTraits<TEntity, TPrimaryKey> entityTraits;

        public DesignTimeRepository(Expression<Func<TEntity, TPrimaryKey>> getPrimaryKeyExpression, Action<TEntity, TPrimaryKey> setPrimaryKeyAction = null) {
            this.getPrimaryKeyExpression = getPrimaryKeyExpression;
            this.entityTraits = ExpressionHelper.GetEntityTraits(this, getPrimaryKeyExpression, setPrimaryKeyAction);
        }
        protected virtual TEntity CreateCore() {
            return DesignTimeHelper.CreateDesignTimeObject<TEntity>();
        }
        protected virtual TEntity FindCore(TPrimaryKey key) {
            throw new InvalidOperationException();
        }
        protected virtual void RemoveCore(TEntity entity) {
            throw new InvalidOperationException();
        }
        protected virtual TEntity ReloadCore(TEntity entity) {
            throw new InvalidOperationException();
        }
        protected virtual TPrimaryKey GetPrimaryKeyCore(TEntity entity) {
            return entityTraits.GetPrimaryKey(entity);
        }
        protected virtual void SetPrimaryKeyCore(TEntity entity, TPrimaryKey key) {
            var setPrimaryKeyaction = entityTraits.SetPrimaryKey;
            setPrimaryKeyaction(entity, key);
        }
        #region IRepository
        TEntity IRepository<TEntity, TPrimaryKey>.Find(TPrimaryKey key) {
            return FindCore(key);
        }
        void IRepository<TEntity, TPrimaryKey>.Remove(TEntity entity) {
            RemoveCore(entity);
        }
        TEntity IRepository<TEntity, TPrimaryKey>.Create() {
            return CreateCore();
        }
        TEntity IRepository<TEntity, TPrimaryKey>.Reload(TEntity entity) {
            return ReloadCore(entity);
        }
        Expression<Func<TEntity, TPrimaryKey>> IRepository<TEntity, TPrimaryKey>.GetPrimaryKeyExpression {
            get { return getPrimaryKeyExpression; }
        }
        TPrimaryKey IRepository<TEntity, TPrimaryKey>.GetPrimaryKey(TEntity entity) {
            return GetPrimaryKeyCore(entity);
        }
        bool IRepository<TEntity, TPrimaryKey>.HasPrimaryKey(TEntity entity) {
            return entityTraits.HasPrimaryKey(entity);
        }
        void IRepository<TEntity, TPrimaryKey>.SetPrimaryKey(TEntity entity, TPrimaryKey key) {
            SetPrimaryKeyCore(entity, key);
        }
        #endregion
    }
}