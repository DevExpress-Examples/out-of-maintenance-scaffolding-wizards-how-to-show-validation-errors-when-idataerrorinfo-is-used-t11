using System;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Collections.Generic;
using Scaffolding.ValidationErrors.Common.Utils;
using Scaffolding.ValidationErrors.Common.DataModel;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;

namespace Scaffolding.ValidationErrors.Common.DataModel.EntityFramework {
    public abstract class DbRepository<TEntity, TPrimaryKey, TDbContext> : DbReadOnlyRepository<TEntity, TDbContext>, IRepository<TEntity, TPrimaryKey>
        where TEntity : class
        where TDbContext : DbContext {

        readonly Expression<Func<TEntity, TPrimaryKey>> getPrimaryKeyExpression;
        readonly EntityTraits<TEntity, TPrimaryKey> entityTraits;

        public DbRepository(DbUnitOfWork<TDbContext> unitOfWork, Func<TDbContext, DbSet<TEntity>> dbSetAccessor, Expression<Func<TEntity, TPrimaryKey>> getPrimaryKeyExpression, Action<TEntity, TPrimaryKey> setPrimaryKeyAction = null)
            : base(unitOfWork, dbSetAccessor) {
            this.getPrimaryKeyExpression = getPrimaryKeyExpression;
            this.entityTraits = ExpressionHelper.GetEntityTraits(this, getPrimaryKeyExpression, setPrimaryKeyAction);
        }
        protected virtual TEntity CreateCore() {
            TEntity newEntity = DbSet.Create();
            DbSet.Add(newEntity);
            return newEntity;
        }
        protected virtual TEntity FindCore(TPrimaryKey key) {
            return DbSet.Find(key);
        }
        protected virtual void RemoveCore(TEntity entity) {
            try {
                DbSet.Remove(entity);
            } catch (DbEntityValidationException ex) {
                throw DbExceptionsConverter.Convert(ex);
            } catch (DbUpdateException ex) {
                throw DbExceptionsConverter.Convert(ex);
            }
        }
        protected virtual TEntity ReloadCore(TEntity entity) {
            Context.Entry(entity).Reload();
            return FindCore(GetPrimaryKeyCore(entity));
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
            get { return this.getPrimaryKeyExpression; }
        }
        void IRepository<TEntity, TPrimaryKey>.SetPrimaryKey(TEntity entity, TPrimaryKey key) {
            SetPrimaryKeyCore(entity, key);
        }
        TPrimaryKey IRepository<TEntity, TPrimaryKey>.GetPrimaryKey(TEntity entity) {
            return GetPrimaryKeyCore(entity);
        }
        bool IRepository<TEntity, TPrimaryKey>.HasPrimaryKey(TEntity entity) {
            return entityTraits.HasPrimaryKey(entity);
        }
        #endregion
    }
}
