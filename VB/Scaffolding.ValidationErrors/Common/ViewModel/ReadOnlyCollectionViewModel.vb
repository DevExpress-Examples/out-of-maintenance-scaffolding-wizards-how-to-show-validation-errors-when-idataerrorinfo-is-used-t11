Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports System.Linq.Expressions
Imports System.Collections.Generic
Imports DevExpress.Mvvm.POCO
Imports DevExpress.Mvvm.DataAnnotations
Imports Scaffolding.ValidationErrors.Common.Utils
Imports Scaffolding.ValidationErrors.Common.DataModel

Namespace Scaffolding.ValidationErrors.Common.ViewModel
	Public MustInherit Partial Class ReadOnlyCollectionViewModel(Of TEntity As Class, TUnitOfWork As IUnitOfWork)
		Inherits ReadOnlyCollectionViewModelBase(Of TEntity, TUnitOfWork)
		Public Sub New(ByVal unitOfWorkFactory As IUnitOfWorkFactory(Of TUnitOfWork), ByVal getRepositoryFunc As Func(Of TUnitOfWork, IReadOnlyRepository(Of TEntity)), Optional ByVal filterExpression As Expression(Of Func(Of TEntity, Boolean)) = Nothing)
			MyBase.New(unitOfWorkFactory, getRepositoryFunc, filterExpression)
		End Sub
	End Class
	<POCOViewModel> _
	Public MustInherit Class ReadOnlyCollectionViewModelBase(Of TEntity As Class, TUnitOfWork As IUnitOfWork)
		Private refreshOnFilterExpressionChanged As Boolean = False
		Private repository_Renamed As IReadOnlyRepository(Of TEntity)
		Protected ReadOnly unitOfWorkFactory As IUnitOfWorkFactory(Of TUnitOfWork)
		Private ReadOnly getRepositoryFunc As Func(Of TUnitOfWork, IReadOnlyRepository(Of TEntity))
		Private entities_Renamed As IList(Of TEntity)

		Public Sub New(ByVal unitOfWorkFactory As IUnitOfWorkFactory(Of TUnitOfWork), ByVal getRepositoryFunc As Func(Of TUnitOfWork, IReadOnlyRepository(Of TEntity)), ByVal filterExpression As Expression(Of Func(Of TEntity, Boolean)))
			Me.unitOfWorkFactory = unitOfWorkFactory
			Me.getRepositoryFunc = getRepositoryFunc
			FilterExpression = filterExpression
			Me.refreshOnFilterExpressionChanged = True
			If (Not Me.IsInDesignMode()) Then
				OnInitializeInRuntime()
			End If
		End Sub
		Protected Overridable Sub OnInitializeInRuntime()
		End Sub
		Public Overridable Sub Refresh()
			Me.repository_Renamed = GetRepository()
			Me.entities_Renamed = GetEntities()
			Me.RaisePropertyChanged(Function(x) Entities)
		End Sub

		Protected ReadOnly Property Repository() As IReadOnlyRepository(Of TEntity)
			Get
				If repository_Renamed Is Nothing Then
					repository_Renamed = GetRepository()
				End If
				Return repository_Renamed
			End Get
		End Property
		Public ReadOnly Property Entities() As IList(Of TEntity)
			Get
				If entities_Renamed Is Nothing Then
					entities_Renamed = GetEntities()
				End If
				Return entities_Renamed
			End Get
		End Property
		Private privateSelectedEntity As TEntity
		Public Overridable Property SelectedEntity() As TEntity
			Get
				Return privateSelectedEntity
			End Get
			Set(ByVal value As TEntity)
				privateSelectedEntity = value
			End Set
		End Property
		Protected Overridable Sub OnSelectedEntityChanged()
		End Sub
		Private privateFilterExpression As Expression(Of Func(Of TEntity, Boolean))
		Public Overridable Property FilterExpression() As Expression(Of Func(Of TEntity, Boolean))
			Get
				Return privateFilterExpression
			End Get
			Set(ByVal value As Expression(Of Func(Of TEntity, Boolean)))
				privateFilterExpression = value
			End Set
		End Property
		Protected Overridable Sub OnFilterExpressionChanged()
			If refreshOnFilterExpressionChanged Then
				Refresh()
			End If
		End Sub
		Private Function GetRepository() As IReadOnlyRepository(Of TEntity)
			Return getRepositoryFunc(unitOfWorkFactory.CreateUnitOfWork())
		End Function
		Protected Overridable Function GetEntities() As IList(Of TEntity)
			Dim queryable = Repository.GetEntities()
			If FilterExpression IsNot Nothing Then
				queryable = queryable.Where(FilterExpression)
			End If
			queryable.Load()
			Return Repository.Local
		End Function
	End Class
End Namespace