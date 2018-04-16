using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DevExpress.Mvvm;

namespace Scaffolding.ValidationErrors.Common.DataModel {
    /// <summary>
    /// DesignTimeReadOnlyRepository is an IReadOnlyRepository interface implementation that represents the collection of entities of a given type for design-time mode. 
    /// DesignTimeReadOnlyRepository objects are created from a DesignTimeInitOfWork class instance using the GetReadOnlyRepository method. 
    /// DesignTimeReadOnlyRepository provides only read-only operations against entities of a given type.
    /// </summary>
    /// <typeparam name="TEntity">A repository entity type.</typeparam>
    public class DesignTimeReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity>
        where TEntity : class {

        IQueryable<TEntity> queryableEntities;

        protected virtual IQueryable<TEntity> GetEntitiesCore() {
            if (queryableEntities == null)
                queryableEntities = DesignTimeHelper.CreateDesignTimeObjects<TEntity>(2).AsQueryable();
            return queryableEntities;
        }

        #region IReadOnlyRepository
        IQueryable<TEntity> IReadOnlyRepository<TEntity>.GetEntities() {
            return GetEntitiesCore();
        }

        IUnitOfWork IReadOnlyRepository<TEntity>.UnitOfWork {
            get { return DesignTimeUnitOfWork.Instance; }
        }

        ObservableCollection<TEntity> IReadOnlyRepository<TEntity>.Local {
            get { return new ObservableCollection<TEntity>(GetEntitiesCore()); }
        }
        #endregion
    }
}