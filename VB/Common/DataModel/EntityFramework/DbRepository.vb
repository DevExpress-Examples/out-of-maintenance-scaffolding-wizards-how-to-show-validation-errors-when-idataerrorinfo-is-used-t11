Imports System
Imports System.Linq
Imports System.Data
Imports System.Data.Entity
Imports System.Linq.Expressions
Imports System.Collections.Generic
Imports Common.Utils
Imports Common.DataModel
Imports System.Data.Entity.Validation
Imports System.Data.Entity.Infrastructure
Namespace Common.DataModel.EntityFramework
    ''' <summary>
    ''' A DbRepository is a IRepository interface implementation representing the collection of all entities in the unit of work, or that can be queried from the database, of a given type. 
    ''' DbRepository objects are created from a DbUnitOfWork using the GetRepository method. 
    ''' DbRepository provides only write operations against entities of a given type in addition to the read-only operation provided DbReadOnlyRepository base class.
    ''' </summary>
    ''' <typeparam name="TEntity">Repository entity type.</typeparam>
    ''' <typeparam name="TPrimaryKey">Entity primary key type.</typeparam>
    ''' <typeparam name="TDbContext">DbContext type.</typeparam>
    Public Class DbRepository(Of TEntity As Class, TPrimaryKey, TDbContext As DbContext)
        Inherits DbReadOnlyRepository(Of TEntity, TDbContext)
        Implements IRepository(Of TEntity, TPrimaryKey)
        Private ReadOnly _getPrimaryKeyExpression As Expression(Of Func(Of TEntity, TPrimaryKey))
        Private ReadOnly _entityTraits As EntityTraits(Of TEntity, TPrimaryKey)
        ''' <summary>
        ''' Initializes a new instance of DbRepository class.
        ''' </summary>
        ''' <param name="unitOfWork">Owner unit of work that provides context for repository entities.</param>
        ''' <param name="dbSetAccessor">Function that returns DbSet entities from Entity Framework DbContext.</param>
        ''' <param name="getPrimaryKeyExpression">Lambda-expression that returns entity primary key.</param>
        ''' <param name="setPrimaryKeyAction">Action that provides a way to set entity primary key in case primary key is a nullable type, otherwise this parameter can be ommited.</param>
        Public Sub New(ByVal unitOfWork As DbUnitOfWork(Of TDbContext), ByVal dbSetAccessor As Func(Of TDbContext, DbSet(Of TEntity)), ByVal getPrimaryKeyExpression As Expression(Of Func(Of TEntity, TPrimaryKey)), Optional ByVal setPrimaryKeyAction As Action(Of TEntity, TPrimaryKey) = Nothing)
            MyBase.New(unitOfWork, dbSetAccessor)
            Me._getPrimaryKeyExpression = getPrimaryKeyExpression
            Me._entityTraits = ExpressionHelper.GetEntityTraits(Me, getPrimaryKeyExpression, setPrimaryKeyAction)
        End Sub
        Protected Overridable Function CreateCore() As TEntity
            Dim newEntity As TEntity = DbSet.Create()
            DbSet.Add(newEntity)
            Return newEntity
        End Function
        Protected Overridable Function FindCore(ByVal key As TPrimaryKey) As TEntity
            Return DbSet.Find(key)
        End Function
        Protected Overridable Sub RemoveCore(ByVal entity As TEntity)
            Try
                DbSet.Remove(entity)
            Catch ex As DbEntityValidationException
                Throw DbExceptionsConverter.Convert(ex)
            Catch ex As DbUpdateException
                Throw DbExceptionsConverter.Convert(ex)
            End Try
        End Sub
        Protected Overridable Function ReloadCore(ByVal entity As TEntity) As TEntity
            Context.Entry(entity).Reload()
            Return FindCore(GetPrimaryKeyCore(entity))
        End Function
        Protected Overridable Function GetPrimaryKeyCore(ByVal entity As TEntity) As TPrimaryKey
            Return _entityTraits.GetPrimaryKey(entity)
        End Function
        Protected Overridable Sub SetPrimaryKeyCore(ByVal entity As TEntity, ByVal key As TPrimaryKey)
            Dim setPrimaryKeyaction = _entityTraits.SetPrimaryKey
            setPrimaryKeyaction(entity, key)
        End Sub
        Private Function Find(ByVal key As TPrimaryKey) As TEntity Implements IRepository(Of TEntity, TPrimaryKey).Find
            Return FindCore(key)
        End Function
        Private Sub Remove(ByVal entity As TEntity) Implements IRepository(Of TEntity, TPrimaryKey).Remove
            RemoveCore(entity)
        End Sub
        Private Function Create() As TEntity Implements IRepository(Of TEntity, TPrimaryKey).Create
            Return CreateCore()
        End Function
        Private Function Reload(ByVal entity As TEntity) As TEntity Implements IRepository(Of TEntity, TPrimaryKey).Reload
            Return ReloadCore(entity)
        End Function
        Private ReadOnly Property GetPrimaryKeyExpression As Expression(Of Func(Of TEntity, TPrimaryKey)) Implements IRepository(Of TEntity, TPrimaryKey).GetPrimaryKeyExpression
            Get
                Return Me._getPrimaryKeyExpression
            End Get
        End Property
        Private Sub SetPrimaryKey(ByVal entity As TEntity, ByVal key As TPrimaryKey) Implements IRepository(Of TEntity, TPrimaryKey).SetPrimaryKey
            SetPrimaryKeyCore(entity, key)
        End Sub
        Private Function GetPrimaryKey(ByVal entity As TEntity) As TPrimaryKey Implements IRepository(Of TEntity, TPrimaryKey).GetPrimaryKey
            Return GetPrimaryKeyCore(entity)
        End Function
        Private Function HasPrimaryKey(ByVal entity As TEntity) As Boolean Implements IRepository(Of TEntity, TPrimaryKey).HasPrimaryKey
            Return _entityTraits.HasPrimaryKey(entity)
        End Function
    End Class
End Namespace
