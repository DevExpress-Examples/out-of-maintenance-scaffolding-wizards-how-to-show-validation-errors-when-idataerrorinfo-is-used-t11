Imports System
Imports System.Collections.Generic
Imports System.Data.Entity
Imports System.Data.Entity.Validation
Imports System.Data.Entity.Infrastructure
Imports System.Linq
Imports System.Linq.Expressions
Namespace Common.DataModel.EntityFramework
    ''' <summary>
    ''' A DbUnitOfWork instance represents the implementation of the Unit Of Work pattern 
    ''' such that it can be used to query from a database and group together changes that will then be written back to the store as a unit. 
    ''' </summary>
    ''' <typeparam name="TContext">DbContext type.</typeparam>
    Public MustInherit Class DbUnitOfWork(Of TContext As DbContext)
        Inherits UnitOfWorkBase
        Implements IUnitOfWork
        Private _Context As TContext
        ''' <summary>
        ''' Initializes a new instance of DbUnitOfWork class using specified DbContext.
        ''' </summary>
        ''' <param name="context">Instance of TContext that will be used as a context for this unit of work.</param>
        Public Sub New(ByVal context As TContext)
            Me._Context = context
        End Sub
        ''' <summary>
        ''' Instance of underlying DbContext.
        ''' </summary>
        Public ReadOnly Property Context As TContext
            Get
                Return _Context
            End Get
        End Property
        Private Sub SaveChanges() Implements IUnitOfWork.SaveChanges
            Try
                Context.SaveChanges()
            Catch ex As DbEntityValidationException
                Throw DbExceptionsConverter.Convert(ex)
            Catch ex As DbUpdateException
                Throw DbExceptionsConverter.Convert(ex)
            End Try
        End Sub
        Private Function GetState(ByVal entity As Object) As EntityState Implements IUnitOfWork.GetState
            Return GetEntityState(Context.Entry(entity).State)
        End Function
        Private Sub Update(ByVal entity As Object) Implements IUnitOfWork.Update
        End Sub
        Private Sub Detach(ByVal entity As Object) Implements IUnitOfWork.Detach
            Context.Entry(entity).State = System.Data.Entity.EntityState.Detached
        End Sub
        Private Function GetEntityState(ByVal entityStates As System.Data.Entity.EntityState) As EntityState
            Select Case entityStates
                Case System.Data.Entity.EntityState.Added
                    Return EntityState.Added
                Case System.Data.Entity.EntityState.Deleted
                    Return EntityState.Deleted
                Case System.Data.Entity.EntityState.Detached
                    Return EntityState.Detached
                Case System.Data.Entity.EntityState.Modified
                    Return EntityState.Modified
                Case System.Data.Entity.EntityState.Unchanged
                    Return EntityState.Unchanged
                Case Else
                    Throw New NotImplementedException()
            End Select
        End Function
        Private Function HasChanges() As Boolean Implements IUnitOfWork.HasChanges
            Return Context.ChangeTracker.HasChanges()
        End Function
        Protected Function GetRepository(Of TEntity As Class, TPrimaryKey)(ByVal dbSetAccessor As Func(Of TContext, DbSet(Of TEntity)), ByVal getPrimaryKeyExpression As Expression(Of Func(Of TEntity, TPrimaryKey)), Optional ByVal setPrimaryKeyAction As Action(Of TEntity, TPrimaryKey) = Nothing) As IRepository(Of TEntity, TPrimaryKey)
            Return GetRepositoryCore(Of IRepository(Of TEntity, TPrimaryKey), TEntity)(Function() New DbRepository(Of TEntity, TPrimaryKey, TContext)(Me, dbSetAccessor, getPrimaryKeyExpression, setPrimaryKeyAction))
        End Function
        Protected Function GetReadOnlyRepository(Of TEntity As Class)(ByVal dbSetAccessor As Func(Of TContext, DbSet(Of TEntity))) As IReadOnlyRepository(Of TEntity)
            Return GetRepositoryCore(Of IReadOnlyRepository(Of TEntity), TEntity)(Function() New DbReadOnlyRepository(Of TEntity, TContext)(Me, dbSetAccessor))
        End Function
    End Class
End Namespace
