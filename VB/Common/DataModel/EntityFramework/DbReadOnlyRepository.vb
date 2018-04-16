Imports System
Imports System.Linq
Imports System.Data.Entity
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Namespace Common.DataModel.EntityFramework
    ''' <summary>
    ''' A DbReadOnlyRepository is a IReadOnlyRepository interface implementation representing the collection of all entities in the unit of work, or that can be queried from the database, of a given type. 
    ''' DbReadOnlyRepository objects are created from a DbUnitOfWork using the GetReadOnlyRepository method. 
    ''' DbReadOnlyRepository provides only read-only operations against entities of a given type.
    ''' </summary>
    ''' <typeparam name="TEntity">Repository entity type.</typeparam>
    ''' <typeparam name="TDbContext">DbContext type.</typeparam>
    Public Class DbReadOnlyRepository(Of TEntity As Class, TDbContext As DbContext)
        Implements IReadOnlyRepository(Of TEntity)
        Private ReadOnly _dbSetAccessor As Func(Of TDbContext, DbSet(Of TEntity))
        Private ReadOnly _unitOfWork As DbUnitOfWork(Of TDbContext)
        Private _dbSet As DbSet(Of TEntity)
        ''' <summary>
        ''' Initializes a new instance of DbReadOnlyRepository class.
        ''' </summary>
        ''' <param name="unitOfWork">Owner unit of work that provides context for repository entities.</param>
        ''' <param name="dbSetAccessor">Function that returns DbSet entities from Entity Framework DbContext.</param>
        Public Sub New(ByVal unitOfWork As DbUnitOfWork(Of TDbContext), ByVal dbSetAccessor As Func(Of TDbContext, DbSet(Of TEntity)))
            Me._dbSetAccessor = dbSetAccessor
            Me._unitOfWork = unitOfWork
        End Sub
        Protected ReadOnly Property DbSet As DbSet(Of TEntity)
            Get
                If _dbSet Is Nothing Then
                    _dbSet = _dbSetAccessor(_unitOfWork.Context)
                End If
                Return _dbSet
            End Get
        End Property
        Protected ReadOnly Property Context As TDbContext
            Get
                Return _unitOfWork.Context
            End Get
        End Property
        Protected Overridable Function GetEntities() As IQueryable(Of TEntity)
            Return DbSet
        End Function
        Private Function GetEntities_Impl() As IQueryable(Of TEntity) Implements IReadOnlyRepository(Of TEntity).GetEntities
            Return GetEntities()
        End Function
        Private ReadOnly Property UnitOfWork As IUnitOfWork Implements IReadOnlyRepository(Of TEntity).UnitOfWork
            Get
                Return _unitOfWork
            End Get
        End Property
        Private ReadOnly Property Local As ObservableCollection(Of TEntity) Implements IReadOnlyRepository(Of TEntity).Local
            Get
                Return DbSet.Local
            End Get
        End Property
    End Class
End Namespace
