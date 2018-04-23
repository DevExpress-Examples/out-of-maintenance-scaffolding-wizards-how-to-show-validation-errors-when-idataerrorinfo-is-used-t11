using System;
using System.Linq;
using System.ComponentModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using System.Collections.ObjectModel;
using Scaffolding.ValidationErrors.Common.Utils;
using Scaffolding.ValidationErrors.Common.DataModel;

namespace Scaffolding.ValidationErrors.Common.ViewModel {
    public class LookUpEntitiesViewModel<TEntity, TProjection, TPrimaryKey, TUnitOfWork> : EntitiesViewModel<TEntity, TProjection, TUnitOfWork>, IDocumentContent
        where TEntity : class
        where TProjection : class
        where TUnitOfWork : IUnitOfWork {

        public static LookUpEntitiesViewModel<TEntity, TProjection, TPrimaryKey, TUnitOfWork> Create(
            IUnitOfWorkFactory<TUnitOfWork> unitOfWorkFactory,
            Func<TUnitOfWork, IReadOnlyRepository<TEntity>> getRepositoryFunc,
            Func<IRepositoryQuery<TEntity>, IQueryable<TProjection>> projection = null) {
            return ViewModelSource.Create(() => new LookUpEntitiesViewModel<TEntity, TProjection, TPrimaryKey, TUnitOfWork>(unitOfWorkFactory, getRepositoryFunc, projection));
        }

        protected LookUpEntitiesViewModel(
            IUnitOfWorkFactory<TUnitOfWork> unitOfWorkFactory,
            Func<TUnitOfWork, IReadOnlyRepository<TEntity>> getRepositoryFunc,
            Func<IRepositoryQuery<TEntity>, IQueryable<TProjection>> projection
            ) : base(unitOfWorkFactory, getRepositoryFunc, projection) {
        }

        protected override IEntitiesChangeTracker CreateEntitiesChangeTracker() {
            return new EntitiesChangeTracker<TPrimaryKey>(this);
        }
    }
}