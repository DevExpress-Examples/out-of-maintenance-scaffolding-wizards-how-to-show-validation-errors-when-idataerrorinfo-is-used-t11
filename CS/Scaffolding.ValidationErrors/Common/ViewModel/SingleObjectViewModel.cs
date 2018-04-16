using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.DataAnnotations;
using Scaffolding.ValidationErrors.Common.Utils;
using Scaffolding.ValidationErrors.Common.DataModel;
using MessageBoxButton = System.Windows.MessageBoxButton;
using MessageBoxImage = System.Windows.MessageBoxImage;
using MessageBoxResult = System.Windows.MessageBoxResult;

namespace Scaffolding.ValidationErrors.Common.ViewModel {
    public abstract partial class SingleObjectViewModel<TEntity, TPrimaryKey, TUnitOfWork> : SingleObjectViewModelBase<TEntity, TPrimaryKey, TUnitOfWork>
        where TEntity : class
        where TUnitOfWork : IUnitOfWork {
        protected SingleObjectViewModel(IUnitOfWorkFactory<TUnitOfWork> unitOfWorkFactory, Func<TUnitOfWork, IRepository<TEntity, TPrimaryKey>> getRepositoryFunc)
            : base(unitOfWorkFactory, getRepositoryFunc) {
        }
    }
    [POCOViewModel]
    public abstract class SingleObjectViewModelBase<TEntity, TPrimaryKey, TUnitOfWork> : ISingleObjectViewModel<TEntity, TPrimaryKey>, ISupportParameter, IDocumentViewModel
        where TEntity : class
        where TUnitOfWork : IUnitOfWork {
        object title;
        readonly Func<TUnitOfWork, IRepository<TEntity, TPrimaryKey>> getRepositoryFunc;

        protected SingleObjectViewModelBase(IUnitOfWorkFactory<TUnitOfWork> unitOfWorkFactory, Func<TUnitOfWork, IRepository<TEntity, TPrimaryKey>> getRepositoryFunc) {
            UnitOfWorkFactory = unitOfWorkFactory;
            this.getRepositoryFunc = getRepositoryFunc;
            UnitOfWork = UnitOfWorkFactory.CreateUnitOfWork();
            if(this.IsInDesignMode())
                this.Entity = this.Repository.Local.FirstOrDefault();
            else
                OnInitializeInRuntime();
        }
        protected virtual void OnInitializeInRuntime() {
            Messenger.Default.Register<EntityMessage<TEntity>>(this, x => OnEntityMessage(x));
        }

        protected virtual void OnEntityMessage(EntityMessage<TEntity> x) {
            if(Entity == null) return;
            if(x.MessageType == EntityMessageType.Deleted && object.Equals(GetPrimaryKey(x.Entity), GetPrimaryKey(Entity)))
                Close();
        }
        protected IUnitOfWorkFactory<TUnitOfWork> UnitOfWorkFactory { get; private set; }
        protected TUnitOfWork UnitOfWork { get; private set; }
        public object Title { get { return title; } }
        public virtual TEntity Entity { get; set; }
        protected virtual void OnEntityChanged() {
            Update();
            if(this.Entity != null && Repository.HasPrimaryKey(Entity))
                RefreshLookUpCollections(GetPrimaryKey(this.Entity));
        }
        protected virtual void RefreshLookUpCollections(TPrimaryKey key) {
        }
        protected IRepository<TEntity, TPrimaryKey> Repository { get { return getRepositoryFunc(UnitOfWork); } }

        [Required]
        protected virtual IMessageBoxService MessageBoxService { get { return null; } }
        [ServiceProperty(ServiceSearchMode.PreferParents)]
        protected virtual IDocumentManagerService DocumentManagerService { get { return null; } }

        protected virtual void OnParameterChanged(object parameter) {
            if(Entity != null && IsNew())
                Remove(Entity);
            IDetailEntityInfo detailEntity = parameter as IDetailEntityInfo;
            if(detailEntity != null) {
                Entity = CreateEntity();
                TryInitializeDetail(detailEntity);
            } else if(parameter is TPrimaryKey) {
                this.parameter = parameter;
                Entity = Repository.Find((TPrimaryKey)parameter);
            } else if(parameter == null) {
                Entity = CreateEntity();
            } else {
                Entity = null;
            }
        }
        protected virtual TEntity CreateEntity() {
            return Repository.Create();
        }

        [Display(AutoGenerateField = false)]
        public void Update() {
            UpdateTitle();
            UpdateCommands();
        }
        void UpdateTitle() {
            if(Entity == null)
                title = null;
            else if(IsNew())
                title = GetTitleForNewEntity();
            else
                title = GetTitle(GetState() == EntityState.Modified);
            this.RaisePropertyChanged(x => x.Title);
        }
        protected virtual void UpdateCommands() {
            this.RaiseCanExecuteChanged(x => x.Save());
            this.RaiseCanExecuteChanged(x => x.Delete());
        }
        [Command]
        public bool Save() {
            try {
                bool isNewEntity = IsNew();
                if(!isNewEntity)
                    UnitOfWork.Update(Entity);
                UnitOfWork.SaveChanges();
                if(isNewEntity)
                    Messenger.Default.Send(new EntityMessage<TEntity>(Entity, EntityMessageType.Added));
                else
                    Messenger.Default.Send(new EntityMessage<TEntity>(Entity, EntityMessageType.Changed));

                Parameter = GetPrimaryKey(Entity);
                OnEntityChanged();
                this.RaisePropertyChanged(x => x.Entity);
                return true;
            } catch (DbException e) {
                MessageBoxService.Show(e.ErrorMessage, e.ErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        public virtual bool CanSave() {
            return Entity != null && !HasValidationErrors();
        }
        public void Delete() {
            if(MessageBoxService.Show(string.Format(CommonResources.Confirmation_Delete, typeof(TEntity).Name), CommonResources.Confirmation_Caption, MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;
            try {
                OnBeforeEntityDeleted(Entity);
                Repository.Remove(Entity);
                UnitOfWork.SaveChanges();
                TEntity toMessage = Entity;
                Entity = null;
                Messenger.Default.Send(new EntityMessage<TEntity>(toMessage, EntityMessageType.Deleted));
                Close();
            } catch (DbException e) {
                MessageBoxService.Show(e.ErrorMessage, e.ErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        protected virtual void OnBeforeEntityDeleted(TEntity entity) { }
        public bool CanDelete() {
            return Entity != null && !IsNew();
        }
        public void Close() {
            if(!TryClose() || DocumentManagerService == null)
                return;
            OnClose();
            IDocument document = DocumentManagerService.FindDocument(this);
            if(document != null)
                document.Close();
        }
        protected virtual void OnClose() {
            Messenger.Default.Unregister(this);
        }
        protected virtual bool TryClose() {
            if(HasValidationErrors()) {
                MessageBoxResult warningResult = MessageBoxService.Show(CommonResources.Warning_SomeFieldsContainInvalidData, CommonResources.Warning_Caption, MessageBoxButton.OKCancel);
                return warningResult == MessageBoxResult.OK;
            }
            if(!NeedSave()) return true;
            MessageBoxResult result = MessageBoxService.Show(CommonResources.Confirmation_Save, CommonResources.Confirmation_Caption, MessageBoxButton.YesNoCancel);
            bool close;
            if(result == MessageBoxResult.Yes)
                close = Save();
            else
                close = result != MessageBoxResult.Cancel;
            if(close)
                OnClose();
            return close;
        }
        protected bool IsNew() {
            return GetState() == EntityState.Added;
        }
        protected bool NeedSave() {
            if(Entity == null)
                return false;
            EntityState state = GetState();
            return state == EntityState.Modified || state == EntityState.Added;
        }
        protected virtual bool HasValidationErrors() { return false; }
        string GetTitle(bool entityModified) {
            if(entityModified)
                return GetTitle() + CommonResources.Entity_Changed;
            else
                return GetTitle();
        }
        protected virtual string GetTitleForNewEntity() {
            return typeof(TEntity).Name + CommonResources.Entity_New;
        }
        protected virtual string GetTitle() {
            return typeof(TEntity).Name + " " + Convert.ToString(GetPrimaryKey(Entity));
        }

        protected virtual bool TryInitializeDetail(IDetailEntityInfo detailEntityInfo) {
            return true;
        }
        protected bool TryInitializeDetail<TDetailEntity, TDetailEntityKey>(object parameter,
            IRepository<TDetailEntity, TDetailEntityKey> detailsRepository,
            Action<TEntity, TDetailEntity> assignAction) where TDetailEntity : class {
            DetailEntityInfo<TDetailEntity> detailEntityInfo = parameter as DetailEntityInfo<TDetailEntity>;
            if(detailEntityInfo == null) return false;
            TDetailEntityKey key = (TDetailEntityKey)detailEntityInfo.DetailEntityKey;
            TDetailEntity detailEntity = detailsRepository.GetEntities().Where(ExpressionHelper.GetValueEqualsExpression(detailsRepository.GetPrimaryKeyExpression, key)).FirstOrDefault();
            if(detailEntity != null) {
                assignAction(Entity, detailEntity);
                this.RaisePropertyChanged(x => x.Entity);
            }
            return true;
        }

        protected EntityState GetState() {
            try {
                return UnitOfWork.GetState(Entity);
            } catch (InvalidOperationException) {
                if(Parameter is TPrimaryKey)
                    Repository.SetPrimaryKey(Entity, (TPrimaryKey)Parameter);
                return UnitOfWork.GetState(Entity);
            }
        }
        void Remove(TEntity entity) {
            if(Repository.HasPrimaryKey(entity))
                entity = Repository.Find(GetPrimaryKey(entity));
            Repository.Remove(Entity);
        }
        protected virtual TPrimaryKey GetPrimaryKey(TEntity entity) {
            return this.Repository.GetPrimaryKey(entity);
        }

        protected void DestroyDocument(IDocument document) {
            if(document != null)
                document.Close();
        }
        protected virtual LookUpCollectionViewModel<TEntity, TDetailEntity, TDetailPrimaryKey, TUnitOfWork> CreateLookUpCollectionViewModel<TDetailEntity, TDetailPrimaryKey>(Func<TUnitOfWork, IRepository<TDetailEntity, TDetailPrimaryKey>> getRepositoryFunc, Expression<Func<TDetailEntity, bool>> filterExpression, TPrimaryKey masterEntityKey) where TDetailEntity : class {
            var lookUpViewModel = ViewModelSource.Create(() => new LookUpCollectionViewModel<TEntity, TDetailEntity, TDetailPrimaryKey, TUnitOfWork>(UnitOfWorkFactory, getRepositoryFunc, filterExpression, masterEntityKey));
            lookUpViewModel.SetParentViewModel(this);
            return lookUpViewModel;
        }
        protected virtual ReadOnlyLookUpCollectionViewModel<TEntity, TDetailEntity, TUnitOfWork> CreateReadOnlyLookUpCollectionViewModel<TDetailEntity>(Func<TUnitOfWork, IReadOnlyRepository<TDetailEntity>> getRepositoryFunc, Expression<Func<TDetailEntity, bool>> filterExpression) where TDetailEntity : class {
            var lookUpViewModel = ViewModelSource.Create(() => new ReadOnlyLookUpCollectionViewModel<TEntity, TDetailEntity, TUnitOfWork>(UnitOfWorkFactory, getRepositoryFunc, filterExpression));
            lookUpViewModel.SetParentViewModel(this);
            return lookUpViewModel;
        }
        protected virtual IList<TLookUpEntity> GetLookUpEntities<TLookUpEntity>(IReadOnlyRepository<TLookUpEntity> repository) where TLookUpEntity : class {
            repository.GetEntities().Load();
            return repository.Local;
        }
        #region ISupportParameter
        static readonly object NotSetParameter = new object();
        object parameter = NotSetParameter;

        object Parameter {
            get { return object.Equals(parameter, NotSetParameter) ? null : parameter; }
            set {
                if(object.Equals(parameter, value))
                    return;
                OnParameterChanged(value);
            }
        }
        object ISupportParameter.Parameter { get { return Parameter; } set { Parameter = value; } }
        #endregion
        #region IDocumentViewModel
        object IDocumentViewModel.Title { get { return Title; } }
        bool IDocumentViewModel.Close() { return TryClose(); }
        #endregion
        #region ISingleObjectViewModel
        TEntity ISingleObjectViewModel<TEntity, TPrimaryKey>.Entity { get { return Entity; } }
        TPrimaryKey ISingleObjectViewModel<TEntity, TPrimaryKey>.PrimaryKey { get { return GetPrimaryKey(Entity); } }
        #endregion
    }
}