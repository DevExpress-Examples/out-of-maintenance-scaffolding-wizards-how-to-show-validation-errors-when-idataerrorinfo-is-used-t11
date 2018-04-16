Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports DevExpress.Mvvm

Namespace Scaffolding.ValidationErrors.Common.DataModel
	Public MustInherit Class DesignTimeReadOnlyRepository(Of TEntity As Class)
		Implements IReadOnlyRepository(Of TEntity)
		Private queryableEntities As IQueryable(Of TEntity)

		Protected Overridable Function GetEntitiesCore() As IQueryable(Of TEntity)
			If queryableEntities Is Nothing Then
				queryableEntities = DesignTimeHelper.CreateDesignTimeObjects(Of TEntity)(2).AsQueryable()
			End If
			Return queryableEntities
		End Function
		#Region "IReadOnlyRepository"
		Private Function GetEntities() As IQueryable(Of TEntity) Implements IReadOnlyRepository(Of TEntity).GetEntities
			Return GetEntitiesCore()
		End Function
		Private ReadOnly Property IReadOnlyRepository_UnitOfWork() As IUnitOfWork Implements IReadOnlyRepository(Of TEntity).UnitOfWork
			Get
				Return DesignTimeUnitOfWork.Instance
			End Get
		End Property
		Private ReadOnly Property Local() As ObservableCollection(Of TEntity) Implements IReadOnlyRepository(Of TEntity).Local
			Get
				Return New ObservableCollection(Of TEntity)(GetEntitiesCore())
			End Get
		End Property
		#End Region
	End Class
End Namespace