Imports System
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
    ''' The base interface for objects passed as a parameter to a single object view model and used to initialize its entity.
    ''' </summary>
    ''' <typeparam name="TEntity">A type of the entity that will be initialized.</typeparam>
    ''' <typeparam name="TUnitOfWork">A unit of work type.</typeparam>
    Public Interface IEntityInitializer(Of TEntity As Class, TUnitOfWork As IUnitOfWork)
        ''' <summary>
        ''' Initializes the given entity.
        ''' </summary>
        ''' <param name="unitOfWork">A unit of work that owns the given entity.</param>
        ''' <param name="entity">An entity to initialize.</param>
        Sub InitializeEntity(ByVal unitOfWork As TUnitOfWork, ByVal entity As TEntity)
    End Interface
    ''' <summary>
    ''' The base class for objects passed as a parameter to a single object view model and used to set the value of a certain property of its entity.
    ''' </summary>
    ''' <typeparam name="TEntity">A type of entity that will be initialized.</typeparam>
    ''' <typeparam name="TUnitOfWork">A unit of work type.</typeparam>
    ''' <typeparam name="TProperty">A type of property that will be set.</typeparam>
    Public MustInherit Class EntityPropertyInitializerBase(Of TEntity As Class, TUnitOfWork As IUnitOfWork, TProperty)
        Implements IEntityInitializer(Of TEntity, TUnitOfWork)
        Private ReadOnly _setPropertyAction As Action(Of TEntity, TProperty)
        ''' <summary>
        ''' Initializes a new instance of the EntityPropertyInitializerBase class.
        ''' </summary>
        ''' <param name="setPropertyAction">An action that assigns the property value to the entity.</param>
        Public Sub New(ByVal setPropertyAction As Action(Of TEntity, TProperty))
            Me._setPropertyAction = setPropertyAction
        End Sub
        Protected MustOverride Function TryGetPropertyValue(ByVal unitOfWork As TUnitOfWork, ByVal entity As TEntity, ByRef propertyValue As TProperty) As Boolean
        Private Sub InitializeEntity(ByVal unitOfWork As TUnitOfWork, ByVal entity As TEntity) Implements IEntityInitializer(Of TEntity, TUnitOfWork).InitializeEntity
            Dim propertyValue As TProperty
            If TryGetPropertyValue(unitOfWork, entity, propertyValue) Then
                _setPropertyAction(entity, propertyValue)
            End If
        End Sub
    End Class
    ''' <summary>
    ''' The class for objects passed a parameter to a single object view model and used to set the value of the entity navigation property.
    ''' </summary>
    ''' <typeparam name="TEntity">A type of the entity that will be initialized.</typeparam>
    ''' <typeparam name="TRelatedEntity">A type of the related entity.</typeparam>
    ''' <typeparam name="TRelatedEntityKey">A related entity primary key type.</typeparam>
    ''' <typeparam name="TUnitOfWork">A unit of work type.</typeparam>
    Public Class EntityNavigationPropertyInitializer(Of TEntity As Class, TRelatedEntity As Class, TRelatedEntityKey, TUnitOfWork As IUnitOfWork)
        Inherits EntityPropertyInitializerBase(Of TEntity, TUnitOfWork, TRelatedEntity)
        Private ReadOnly _relatedEntityKey As TRelatedEntityKey
        Private ReadOnly _getRepositoryFunc As Func(Of TUnitOfWork, IRepository(Of TRelatedEntity, TRelatedEntityKey))
        ''' <summary>
        ''' Initializes a new instance of the EntityNavigationPropertyInitializer class.
        ''' </summary>
        ''' <param name="relatedEntityKey">>A related entity primary key value.</param>
        ''' <param name="setRelatedEntityAction">An action that assigns the navigation property value to the entity.</param>
        ''' <param name="getRepositoryFunc">A function that returns the repository representing navigation entities.</param>
        Public Sub New(ByVal relatedEntityKey As TRelatedEntityKey, ByVal setRelatedEntityAction As Action(Of TEntity, TRelatedEntity), ByVal getRepositoryFunc As Func(Of TUnitOfWork, IRepository(Of TRelatedEntity, TRelatedEntityKey)))
            MyBase.New(setRelatedEntityAction)
            Me._relatedEntityKey = relatedEntityKey
            Me._getRepositoryFunc = getRepositoryFunc
        End Sub
        Protected Overrides Function TryGetPropertyValue(ByVal unitOfWork As TUnitOfWork, ByVal entity As TEntity, ByRef propertyValue As TRelatedEntity) As Boolean
            Dim relatedEntityRepository As IRepository(Of TRelatedEntity, TRelatedEntityKey) = _getRepositoryFunc(unitOfWork)
            propertyValue = relatedEntityRepository.GetEntities().Where(ExpressionHelper.GetValueEqualsExpression(relatedEntityRepository.GetPrimaryKeyExpression, _relatedEntityKey)).FirstOrDefault()
            Return propertyValue IsNot Nothing
        End Function
    End Class
End Namespace
