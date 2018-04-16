Imports System
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Linq
Imports System.Linq.Expressions
Imports DevExpress.Mvvm
Imports DevExpress.Mvvm.POCO
Imports DevExpress.Mvvm.DataAnnotations
Imports Common.Utils
Imports Common.DataModel
Imports MessageBoxButton = System.Windows.MessageBoxButton
Imports MessageBoxImage = System.Windows.MessageBoxImage
Imports MessageBoxResult = System.Windows.MessageBoxResult
Namespace Common.ViewModel
    ''' <summary>
    ''' The base class for a POCO view models exposing a collection of entities of a given type and CRUD operations against these entities. 
    ''' This is a partial class that provides extension point to add custom properties, commands and override methods without modifying the auto-generated code.
    ''' </summary>
    ''' <typeparam name="TEntity">An entity type.</typeparam>
    ''' <typeparam name="TPrimaryKey">A primary key value type.</typeparam>
    ''' <typeparam name="TUnitOfWork">A unit of work type.</typeparam>
    Partial Public Class CollectionViewModel(Of TEntity As Class, TPrimaryKey, TUnitOfWork As IUnitOfWork)
        Inherits CollectionViewModelBase(Of TEntity, TPrimaryKey, TUnitOfWork)
        ''' <summary>
        ''' Initializes a new instance of the CollectionViewModel class.
        ''' </summary>
        ''' <param name="unitOfWorkFactory">A factory used to create a unit of work instance.</param>
        ''' <param name="getRepositoryFunc">A function that returns a repository representing entities of a given type.</param>
        ''' <param name="newEntityInitializerFactory">An optional parameter that provides a function to create an entity initializer. This parameter is used in the detail collection view models when creating a single object view model for a new entity.</param>
        Public Sub New(ByVal unitOfWorkFactory As IUnitOfWorkFactory(Of TUnitOfWork), ByVal getRepositoryFunc As Func(Of TUnitOfWork, IRepository(Of TEntity, TPrimaryKey)), Optional ByVal newEntityInitializerFactory As Func(Of IEntityInitializer(Of TEntity, TUnitOfWork)) = Nothing)
            MyBase.New(unitOfWorkFactory, getRepositoryFunc, newEntityInitializerFactory)
        End Sub
    End Class
    ''' <summary>
    ''' The base class for POCO view models exposing a collection of entities of a given type and CRUD operations against these entities.
    ''' It is not recommended to inherit directly from this class. Use the CollectionViewModel class instead.
    ''' </summary>
    ''' <typeparam name="TEntity">An entity type.</typeparam>
    ''' <typeparam name="TPrimaryKey">A primary key value type.</typeparam>
    ''' <typeparam name="TUnitOfWork">A unit of work type.</typeparam>
    Public MustInherit Class CollectionViewModelBase(Of TEntity As Class, TPrimaryKey, TUnitOfWork As IUnitOfWork)
        Inherits ReadOnlyCollectionViewModel(Of TEntity, TUnitOfWork)
        Implements IDocumentContent
        Private _DocumentOwner As IDocumentOwner
        Private ReadOnly _newEntityInitializerFactory As Func(Of IEntityInitializer(Of TEntity, TUnitOfWork))
        ''' <summary>
        ''' Initializes a new instance of the CollectionViewModelBase class.
        ''' </summary>
        ''' <param name="unitOfWorkFactory">A factory used to create a unit of work instance.</param>
        ''' <param name="getRepositoryFunc">A function that returns a repository representing entities of a given type.</param>
        ''' <param name="newEntityInitializerFactory">An optional parameter that provides a function to create an entity initializer. This parameter is used in detail collection view models when creating a single object view model for a new entity.</param>
        Protected Sub New(ByVal unitOfWorkFactory As IUnitOfWorkFactory(Of TUnitOfWork), ByVal getRepositoryFunc As Func(Of TUnitOfWork, IRepository(Of TEntity, TPrimaryKey)), ByVal newEntityInitializerFactory As Func(Of IEntityInitializer(Of TEntity, TUnitOfWork)))
            MyBase.New(unitOfWorkFactory, getRepositoryFunc)
            Me._newEntityInitializerFactory = newEntityInitializerFactory
        End Sub
        Protected Shadows ReadOnly Property Repository As IRepository(Of TEntity, TPrimaryKey)
            Get
                Return CType(MyBase.Repository, IRepository(Of TEntity, TPrimaryKey))
            End Get
        End Property
        <Required> _
        Protected Overridable ReadOnly Property MessageBoxService As IMessageBoxService
            Get
                Return Nothing
            End Get
        End Property
        <ServiceProperty(ServiceSearchMode.PreferParents)> _
        Protected Overridable ReadOnly Property DocumentManagerService As IDocumentManagerService
            Get
                Return Nothing
            End Get
        End Property
        ''' <summary>
        ''' Creates and shows a document containing a single object view model for new entity.
        ''' Since CollectionViewModelBase is a POCO view model, an instance of this class will also expose the NewCommand property that can be used as a binding source in views.
        ''' </summary>
        Public Overridable Sub [New]()
            Dim document As IDocument = CreateDocument(If(_newEntityInitializerFactory IsNot Nothing, _newEntityInitializerFactory(), Nothing))
            If document IsNot Nothing Then
                document.Show()
            End If
        End Sub
        ''' <summary>
        ''' Creates and shows a document containing a single object view model for the existing entity.
        ''' Since CollectionViewModelBase is a POCO view model, an instance of this class will also expose the EditCommand property that can be used as a binding source in views.
        ''' </summary>
        ''' <param name="entity">Entity to edit.</param>
        Public Overridable Sub Edit(ByVal entity As TEntity)
            Dim primaryKey As TPrimaryKey = GetPrimaryKey(entity)
            entity = Repository.Reload(entity)
            If entity Is Nothing OrElse Repository.UnitOfWork.GetState(entity) = EntityState.Detached Then
                DestroyDocument(FindEntityDocument(primaryKey))
                Return
            End If
            ShowDocument(GetPrimaryKey(entity))
        End Sub
        ''' <summary>
        ''' Determines whether an entity can be edited.
        ''' Since CollectionViewModelBase is a POCO view model, this method will be used as a CanExecute callback for EditCommand.
        ''' </summary>
        ''' <param name="entity">An entity to edit.</param>
        Public Function CanEdit(ByVal entity As TEntity) As Boolean
            Return entity IsNot Nothing
        End Function
        ''' <summary>
        ''' Deletes a given entity from the unit of work and saves changes if confirmed by a user.
        ''' Since CollectionViewModelBase is a POCO view model, an instance of this class will also expose the DeleteCommand property that can be used as a binding source in views.
        ''' </summary>
        ''' <param name="entity">An entity to edit.</param>
        Public Overridable Sub Delete(ByVal entity As TEntity)
            If MessageBoxService.Show(String.Format(CommonResources.Confirmation_Delete, GetType(TEntity).Name), CommonResources.Confirmation_Caption, MessageBoxButton.YesNo) <> MessageBoxResult.Yes Then
                Return
            End If
            Try
                Entities.Remove(entity)
                Repository.Remove(entity)
                Repository.UnitOfWork.SaveChanges()
                Messenger.[Default].Send(New EntityMessage(Of TEntity)(entity, EntityMessageType.Deleted))
            Catch e As DbException
                Refresh()
                MessageBoxService.Show(e.ErrorMessage, e.ErrorCaption, MessageBoxButton.OK, MessageBoxImage.[Error])
            End Try
        End Sub
        ''' <summary>
        ''' Determines whether an entity can be deleted.
        ''' Since CollectionViewModelBase is a POCO view model, this method will be used as a CanExecute callback for DeleteCommand.
        ''' </summary>
        ''' <param name="entity">An entity to edit.</param>
        Public Overridable Function CanDelete(ByVal entity As TEntity) As Boolean
            Return entity IsNot Nothing
        End Function
        ''' <summary>
        ''' Recreates unit of work and reloads entities.
        ''' Since CollectionViewModelBase is a POCO view model, an instance of this class will also expose the RefreshCommand property that can be used as a binding source in views.
        ''' </summary>
        Public Overrides Sub Refresh()
            Dim entity As TEntity = SelectedEntity
            MyBase.Refresh()
            If entity IsNot Nothing AndAlso Repository.HasPrimaryKey(entity) Then
                SelectedEntity = FindNewEntity(GetPrimaryKey(entity))
            End If
        End Sub
        ''' <summary>
        ''' Updates a given entity state and saves changes.
        ''' Since CollectionViewModelBase is a POCO view model, instance of this class will also expose the SaveCommand property that can be used as a binding source in views.
        ''' </summary>
        ''' <param name="entity">Entity to update and save.</param>
        <Display(AutoGenerateField:=False)> _
        Public Overridable Sub Save(ByVal entity As TEntity)
            Try
                Repository.UnitOfWork.Update(entity)
                Repository.UnitOfWork.SaveChanges()
                Messenger.[Default].Send(New EntityMessage(Of TEntity)(entity, EntityMessageType.Changed))
            Catch e As DbException
                MessageBoxService.Show(e.ErrorMessage, e.ErrorCaption, MessageBoxButton.OK, MessageBoxImage.[Error])
            End Try
        End Sub
        ''' <summary>
        ''' Determines whether entity local changes can be saved.
        ''' Since CollectionViewModelBase is a POCO view model, this method will be used as a CanExecute callback for SaveCommand.
        ''' </summary>
        ''' <param name="entity">Entity to edit.</param>
        Public Overridable Function CanSave(ByVal entity As TEntity) As Boolean
            Return entity IsNot Nothing
        End Function
        ''' <summary>
        ''' Notifies that SelectedEntity has been changed by raising the PropertyChanged event.
        ''' Since CollectionViewModelBase is a POCO view model, an instance of this class will also expose the UpdateSelectedEntityCommand property that can be used as a binding source in views.
        ''' </summary>
        <Display(AutoGenerateField:=False)> _
        Public Overridable Sub UpdateSelectedEntity()
            Me.RaisePropertyChanged(Function(x) x.SelectedEntity)
        End Sub
        Protected Overrides Sub OnInitializeInRuntime()
            MyBase.OnInitializeInRuntime()
            Messenger.[Default].Register(Of EntityMessage(Of TEntity))(Me, Sub(x) OnMessage(x))
        End Sub
        Private Sub OnMessage(ByVal message As EntityMessage(Of TEntity))
            If Not Repository.HasPrimaryKey(message.Entity) Then
                Return
            End If
            Dim key As TPrimaryKey = GetPrimaryKey(message.Entity)
            Select Case message.MessageType
                Case EntityMessageType.Added
                    OnEntityAdded(key)
                    Exit Select
                Case EntityMessageType.Changed
                    OnEntityChanged(key)
                    Exit Select
                Case EntityMessageType.Deleted
                    OnEntityDeleted(key)
                    Exit Select
            End Select
        End Sub
        Protected Overridable Function OnEntityAdded(ByVal key As TPrimaryKey) As TEntity
            Return FindNewEntity(key)
        End Function
        Protected Overridable Function OnEntityChanged(ByVal key As TPrimaryKey) As TEntity
            Dim entity As TEntity = FindEntity(key)
            If entity Is Nothing Then
                Return Nothing
            End If
            entity = Repository.Reload(entity)
            If FilterExpression IsNot Nothing AndAlso Not (New TEntity() {entity}.AsQueryable().Any(FilterExpression)) Then
                Repository.UnitOfWork.Detach(entity)
            Else
                Dim index As Integer = Repository.Local.IndexOf(entity)
                If index >= 0 Then
                    Repository.Local.Move(index, index)
                End If
            End If
            If Object.ReferenceEquals(entity, SelectedEntity) Then
                UpdateSelectedEntity()
            End If
            Return entity
        End Function
        Protected Overridable Sub OnEntityDeleted(ByVal key As TPrimaryKey)
            Dim entity As TEntity = Repository.Local.FirstOrDefault(Function(x) Object.Equals(Repository.GetPrimaryKey(x), key))
            If entity IsNot Nothing Then
                Repository.Remove(entity)
                Repository.UnitOfWork.Detach(entity)
            End If
        End Sub
        Protected Function FindNewEntity(ByVal key As TPrimaryKey) As TEntity
            Dim entities = GetFilteredQueryableEntities()
            Return entities.Where(ExpressionHelper.GetValueEqualsExpression(Repository.GetPrimaryKeyExpression, key)).FirstOrDefault()
        End Function
        Protected Function FindEntity(ByVal key As TPrimaryKey) As TEntity
            If FilterExpression Is Nothing Then
                Return Repository.Find(key)
            End If
            Return If(Repository.Local.AsQueryable().FirstOrDefault(ExpressionHelper.GetValueEqualsExpression(Repository.GetPrimaryKeyExpression, key)), FindNewEntity(key))
        End Function
        Protected Overrides Sub OnSelectedEntityChanged()
            MyBase.OnSelectedEntityChanged()
            Me.RaiseCanExecuteChanged(Sub(x) x.Edit(SelectedEntity))
            Me.RaiseCanExecuteChanged(Sub(x) x.Delete(SelectedEntity))
            Me.RaiseCanExecuteChanged(Sub(x) x.Save(SelectedEntity))
        End Sub
        Protected Overridable Function GetPrimaryKey(ByVal entity As TEntity) As TPrimaryKey
            Return Repository.GetPrimaryKey(entity)
        End Function
        Protected ReadOnly Property DocumentOwner As IDocumentOwner
            Get
                Return _DocumentOwner
            End Get
        End Property
        Protected Overridable Sub OnDestroy()
            Messenger.[Default].Unregister(Me)
        End Sub
        Private Sub ShowDocument(ByVal key As TPrimaryKey)
            Dim document As IDocument = If(FindEntityDocument(key), CreateDocument(key))
            If document IsNot Nothing Then
                document.Show()
            End If
        End Sub
        Protected Overridable Function CreateDocument(ByVal parameter As Object) As IDocument
            If DocumentManagerService Is Nothing Then
                Return Nothing
            End If
            Return DocumentManagerService.CreateDocument(GetType(TEntity).Name + "View", parameter, Me)
        End Function
        Protected Sub DestroyDocument(ByVal document As IDocument)
            If document IsNot Nothing Then
                document.Close()
            End If
        End Sub
        Protected Function FindEntityDocument(ByVal key As TPrimaryKey) As IDocument
            If DocumentManagerService Is Nothing Then
                Return Nothing
            End If
            For Each document As IDocument In DocumentManagerService.Documents
                Dim entityViewModel As ISingleObjectViewModel(Of TEntity, TPrimaryKey) = TryCast(document.Content, ISingleObjectViewModel(Of TEntity, TPrimaryKey))
                If entityViewModel IsNot Nothing AndAlso Object.Equals(entityViewModel.PrimaryKey, key) Then
                    Return document
                End If
            Next
            Return Nothing
        End Function
        Private ReadOnly Property Title As Object Implements IDocumentContent.Title
            Get
                Return Nothing
            End Get
        End Property
        Private Sub OnClose(ByVal e As CancelEventArgs) Implements IDocumentContent.OnClose
        End Sub
        Private Sub OnDestroy_Impl() Implements IDocumentContent.OnDestroy
            OnDestroy()
        End Sub
        Private Property DocumentOwner_Impl As IDocumentOwner Implements IDocumentContent.DocumentOwner
            Get
                Return DocumentOwner
            End Get
            Set(value As IDocumentOwner)
                _DocumentOwner = value
            End Set
        End Property
    End Class
End Namespace
