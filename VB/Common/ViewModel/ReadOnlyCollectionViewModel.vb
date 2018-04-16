Imports System
Imports System.Linq
Imports System.Linq.Expressions
Imports System.Collections.Generic
Imports DevExpress.Mvvm.POCO
Imports DevExpress.Mvvm.DataAnnotations
Imports Common.Utils
Imports Common.DataModel
Namespace Common.ViewModel
    ''' <summary>
    ''' The base class for POCO view models exposing a read-only collection of entities of a given type. 
    ''' This is a partial class that provides the extension point to add custom properties, commands and override methods without modifying the auto-generated code.
    ''' </summary>
    ''' <typeparam name="TEntity">An entity type.</typeparam>
    ''' <typeparam name="TUnitOfWork">A unit of work type.</typeparam>
    Partial Public Class ReadOnlyCollectionViewModel(Of TEntity As Class, TUnitOfWork As IUnitOfWork)
        Inherits ReadOnlyCollectionViewModelBase(Of TEntity, TUnitOfWork)
        ''' <summary>
        ''' Initializes a new instance of the ReadOnlyCollectionViewModel class.
        ''' </summary>
        ''' <param name="unitOfWorkFactory">A factory used to create a unit of work instance.</param>
        ''' <param name="getRepositoryFunc">A function that returns the repository representing entities of a given type.</param>
        Public Sub New(ByVal unitOfWorkFactory As IUnitOfWorkFactory(Of TUnitOfWork), ByVal getRepositoryFunc As Func(Of TUnitOfWork, IReadOnlyRepository(Of TEntity)))
            MyBase.New(unitOfWorkFactory, getRepositoryFunc)
        End Sub
    End Class
    ''' <summary>
    ''' The base class for POCO view models exposing a read-only collection of entities of a given type. 
    ''' It is not recommended to inherit directly from this class. Use the ReadOnlyCollectionViewModel class instead.
    ''' </summary>
    ''' <typeparam name="TEntity">An entity type.</typeparam>
    ''' <typeparam name="TUnitOfWork">A unit of work type.</typeparam>
    <POCOViewModel> _
    Public MustInherit Class ReadOnlyCollectionViewModelBase(Of TEntity As Class, TUnitOfWork As IUnitOfWork)
        Private _repository As IReadOnlyRepository(Of TEntity)
        Protected ReadOnly _unitOfWorkFactory As IUnitOfWorkFactory(Of TUnitOfWork)
        Private ReadOnly _getRepositoryFunc As Func(Of TUnitOfWork, IReadOnlyRepository(Of TEntity))
        Private _entities As IList(Of TEntity)
        ''' <summary>
        ''' Initializes a new instance of the ReadOnlyCollectionViewModelBase class.
        ''' </summary>
        ''' <param name="unitOfWorkFactory">A factory used to create a unit of work instance.</param>
        ''' <param name="getRepositoryFunc">A function that returns the repository representing entities of a given type.</param>
        Public Sub New(ByVal unitOfWorkFactory As IUnitOfWorkFactory(Of TUnitOfWork), ByVal getRepositoryFunc As Func(Of TUnitOfWork, IReadOnlyRepository(Of TEntity)))
            Me._unitOfWorkFactory = unitOfWorkFactory
            Me._getRepositoryFunc = getRepositoryFunc
            If Not Me.IsInDesignMode() Then
                OnInitializeInRuntime()
            End If
        End Sub
        ''' <summary>
        ''' The collection of entities loaded from the unit of work.
        ''' </summary>
        Public ReadOnly Property Entities As IList(Of TEntity)
            Get
                If _entities Is Nothing Then
                    _entities = GetEntities()
                End If
                Return _entities
            End Get
        End Property
        ''' <summary>
        ''' The selected enity.
        ''' Since ReadOnlyCollectionViewModelBase is a POCO view model, this property will raise INotifyPropertyChanged.PropertyEvent when modified so it can be used as a binding source in views.
        ''' </summary>
        Public Overridable Property SelectedEntity As TEntity
        ''' <summary>
        ''' The lambda expression used to filter which entities will be loaded locally from the unit of work.
        ''' </summary>
        Public Overridable Property FilterExpression As Expression(Of Func(Of TEntity, Boolean))
        ''' <summary>
        ''' Recreates the unit of work and reloads entities.
        ''' Since CollectionViewModelBase is a POCO view model, an instance of this class will also expose the RefreshCommand property that can be used as a binding source in views.
        ''' </summary>
        Public Overridable Sub Refresh()
            If Me._entities Is Nothing Then
                Return
            End If
            Me._repository = GetRepository()
            Me._entities = GetEntities()
            Me.RaisePropertyChanged(Function(x) Entities)
        End Sub
        Protected Overridable Sub OnInitializeInRuntime()
        End Sub
        Protected ReadOnly Property Repository As IReadOnlyRepository(Of TEntity)
            Get
                If _repository Is Nothing Then
                    _repository = GetRepository()
                End If
                Return _repository
            End Get
        End Property
        Protected Overridable Sub OnSelectedEntityChanged()
        End Sub
        Protected Overridable Sub OnFilterExpressionChanged()
            Refresh()
        End Sub
        Private Function GetRepository() As IReadOnlyRepository(Of TEntity)
            Return _getRepositoryFunc(_unitOfWorkFactory.CreateUnitOfWork())
        End Function
        Protected Overridable Function GetEntities() As IList(Of TEntity)
            Dim queryable As IQueryable(Of TEntity) = GetFilteredQueryableEntities()
            queryable.Load()
            Return Repository.Local
        End Function
        Protected Function GetFilteredQueryableEntities() As IQueryable(Of TEntity)
            Dim queryable = Repository.GetEntities()
            If FilterExpression IsNot Nothing Then
                queryable = queryable.Where(FilterExpression)
            End If
            Return queryable
        End Function
    End Class
End Namespace
