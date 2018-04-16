Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports System.Data.Entity
Imports System.Collections.Generic
Imports System.Collections.ObjectModel

Namespace Scaffolding.ValidationErrors.Common.DataModel.EntityFramework
	Public MustInherit Class DbReadOnlyRepository(Of TEntity As Class, TDbContext As DbContext)
		Implements IReadOnlyRepository(Of TEntity)
		Private ReadOnly dbSetAccessor As Func(Of TDbContext, DbSet(Of TEntity))
		Private ReadOnly unitOfWork_Renamed As DbUnitOfWork(Of TDbContext)
		Private dbSet_Renamed As DbSet(Of TEntity)

		Public Sub New(ByVal unitOfWork As DbUnitOfWork(Of TDbContext), ByVal dbSetAccessor As Func(Of TDbContext, DbSet(Of TEntity)))
			Me.dbSetAccessor = dbSetAccessor
			Me.unitOfWork_Renamed = unitOfWork
		End Sub
		Protected ReadOnly Property DbSet() As DbSet(Of TEntity)
			Get
				If dbSet_Renamed Is Nothing Then
					dbSet_Renamed = dbSetAccessor(unitOfWork_Renamed.Context)
					'dbSet.Load();
				End If
				Return dbSet_Renamed
			End Get
		End Property
		Protected ReadOnly Property Context() As TDbContext
			Get
				Return unitOfWork_Renamed.Context
			End Get
		End Property
		Protected Overridable Function GetEntities() As IQueryable(Of TEntity)
			Return DbSet
		End Function
		#Region "IReadOnlyRepository"
		Private Function IReadOnlyRepository_GetEntities() As IQueryable(Of TEntity) Implements IReadOnlyRepository(Of TEntity).GetEntities
			Return GetEntities()
		End Function
		Private ReadOnly Property IReadOnlyRepository_UnitOfWork() As IUnitOfWork Implements IReadOnlyRepository(Of TEntity).UnitOfWork
			Get
				Return unitOfWork_Renamed
			End Get
		End Property
		Private ReadOnly Property Local() As ObservableCollection(Of TEntity) Implements IReadOnlyRepository(Of TEntity).Local
			Get
				Return DbSet.Local
			End Get
		End Property
		#End Region
	End Class
End Namespace
