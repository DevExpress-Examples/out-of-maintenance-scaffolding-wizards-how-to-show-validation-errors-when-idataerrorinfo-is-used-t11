Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports System.Data
Imports System.Data.Entity
Imports System.Linq.Expressions
Imports System.Collections.Generic
Imports Scaffolding.ValidationErrors.Common.Utils
Imports Scaffolding.ValidationErrors.Common.DataModel
Imports System.Data.Entity.Validation
Imports System.Data.Entity.Infrastructure

Namespace Scaffolding.ValidationErrors.Common.DataModel.EntityFramework
	Public MustInherit Class DbRepository(Of TEntity As Class, TPrimaryKey, TDbContext As DbContext)
		Inherits DbReadOnlyRepository(Of TEntity, TDbContext)
		Implements IRepository(Of TEntity, TPrimaryKey)

		Private ReadOnly getPrimaryKeyExpression_Renamed As Expression(Of Func(Of TEntity, TPrimaryKey))
		Private ReadOnly entityTraits As EntityTraits(Of TEntity, TPrimaryKey)

		Public Sub New(ByVal unitOfWork As DbUnitOfWork(Of TDbContext), ByVal dbSetAccessor As Func(Of TDbContext, DbSet(Of TEntity)), ByVal getPrimaryKeyExpression As Expression(Of Func(Of TEntity, TPrimaryKey)), Optional ByVal setPrimaryKeyAction As Action(Of TEntity, TPrimaryKey) = Nothing)
			MyBase.New(unitOfWork, dbSetAccessor)
			Me.getPrimaryKeyExpression_Renamed = getPrimaryKeyExpression
			Me.entityTraits = ExpressionHelper.GetEntityTraits(Me, getPrimaryKeyExpression, setPrimaryKeyAction)
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
			Return entityTraits.GetPrimaryKey(entity)
		End Function
		Protected Overridable Sub SetPrimaryKeyCore(ByVal entity As TEntity, ByVal key As TPrimaryKey)
			Dim setPrimaryKeyaction = entityTraits.SetPrimaryKey
			setPrimaryKeyaction(entity, key)
		End Sub
		#Region "IRepository"
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
		Private ReadOnly Property GetPrimaryKeyExpression() As Expression(Of Func(Of TEntity, TPrimaryKey)) Implements IRepository(Of TEntity, TPrimaryKey).GetPrimaryKeyExpression
			Get
				Return Me.getPrimaryKeyExpression_Renamed
			End Get
		End Property
		Private Sub SetPrimaryKey(ByVal entity As TEntity, ByVal key As TPrimaryKey) Implements IRepository(Of TEntity, TPrimaryKey).SetPrimaryKey
			SetPrimaryKeyCore(entity, key)
		End Sub
		Private Function GetPrimaryKey(ByVal entity As TEntity) As TPrimaryKey Implements IRepository(Of TEntity, TPrimaryKey).GetPrimaryKey
			Return GetPrimaryKeyCore(entity)
		End Function
		Private Function HasPrimaryKey(ByVal entity As TEntity) As Boolean Implements IRepository(Of TEntity, TPrimaryKey).HasPrimaryKey
			Return entityTraits.HasPrimaryKey(entity)
		End Function
		#End Region
	End Class
End Namespace
