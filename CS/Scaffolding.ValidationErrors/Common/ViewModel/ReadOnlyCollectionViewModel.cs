using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.DataAnnotations;
using Scaffolding.ValidationErrors.Common.Utils;
using Scaffolding.ValidationErrors.Common.DataModel;

namespace Scaffolding.ValidationErrors.Common.ViewModel {
    public abstract partial class ReadOnlyCollectionViewModel<TEntity, TUnitOfWork> : ReadOnlyCollectionViewModelBase<TEntity, TUnitOfWork>
        where TEntity : class
        where TUnitOfWork : IUnitOfWork {
        public ReadOnlyCollectionViewModel(IUnitOfWorkFactory<TUnitOfWork> unitOfWorkFactory, Func<TUnitOfWork, IReadOnlyRepository<TEntity>> getRepositoryFunc, Expression<Func<TEntity, bool>> filterExpression = null)
            : base(unitOfWorkFactory, getRepositoryFunc, filterExpression) {
        }
    }
    [POCOViewModel]
    public abstract class ReadOnlyCollectionViewModelBase<TEntity, TUnitOfWork>
        where TEntity : class
        where TUnitOfWork : IUnitOfWork {
        bool refreshOnFilterExpressionChanged = false;
        IReadOnlyRepository<TEntity> repository;
        protected readonly IUnitOfWorkFactory<TUnitOfWork> unitOfWorkFactory;
        readonly Func<TUnitOfWork, IReadOnlyRepository<TEntity>> getRepositoryFunc;
        IList<TEntity> entities;

        public ReadOnlyCollectionViewModelBase(IUnitOfWorkFactory<TUnitOfWork> unitOfWorkFactory, Func<TUnitOfWork, IReadOnlyRepository<TEntity>> getRepositoryFunc, Expression<Func<TEntity, bool>> filterExpression) {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.getRepositoryFunc = getRepositoryFunc;
            FilterExpression = filterExpression;
            this.refreshOnFilterExpressionChanged = true;
            if(!this.IsInDesignMode())
                OnInitializeInRuntime();
        }
        protected virtual void OnInitializeInRuntime() { }
        public virtual void Refresh() {
            this.repository = GetRepository();
            this.entities = GetEntities();
            this.RaisePropertyChanged(x => Entities);
        }

        protected IReadOnlyRepository<TEntity> Repository {
            get {
                if(repository == null)
                    repository = GetRepository();
                return repository;
            }
        }
        public IList<TEntity> Entities {
            get {
                if(entities == null)
                    entities = GetEntities();
                return entities;
            }
        }
        public virtual TEntity SelectedEntity { get; set; }
        protected virtual void OnSelectedEntityChanged() { }
        public virtual Expression<Func<TEntity, bool>> FilterExpression { get; set; }
        protected virtual void OnFilterExpressionChanged() {
            if(refreshOnFilterExpressionChanged)
                Refresh();
        }
        IReadOnlyRepository<TEntity> GetRepository() {
            return getRepositoryFunc(unitOfWorkFactory.CreateUnitOfWork());
        }
        protected virtual IList<TEntity> GetEntities() {
            var queryable = Repository.GetEntities();
            if(FilterExpression != null)
                queryable = queryable.Where(FilterExpression);
            queryable.Load();
            return Repository.Local;
        }
    }
}