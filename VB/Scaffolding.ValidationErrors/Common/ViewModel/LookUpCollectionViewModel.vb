Imports Microsoft.VisualBasic
Imports System
Imports System.Linq.Expressions
Imports DevExpress.Mvvm
Imports Scaffolding.ValidationErrors.Common.Utils
Imports Scaffolding.ValidationErrors.Common.DataModel

Namespace Scaffolding.ValidationErrors.Common.ViewModel
	Partial Public Class LookUpCollectionViewModel(Of TMasterEntity As Class, TEntity As Class, TPrimaryKey, TUnitOfWork As IUnitOfWork)
		Inherits LookUpCollectionViewModelBase(Of TMasterEntity, TEntity, TPrimaryKey, TUnitOfWork)
		Public Sub New(ByVal unitOfWorkFactory As IUnitOfWorkFactory(Of TUnitOfWork), ByVal getRepositoryFunc As Func(Of TUnitOfWork, IRepository(Of TEntity, TPrimaryKey)), ByVal filterExpression As Expression(Of Func(Of TEntity, Boolean)), ByVal masterEntityKey As Object)
			MyBase.New(unitOfWorkFactory, getRepositoryFunc, filterExpression, masterEntityKey)
		End Sub
	End Class
	Public MustInherit Class LookUpCollectionViewModelBase(Of TMasterEntity As Class, TEntity As Class, TPrimaryKey, TUnitOfWork As IUnitOfWork)
		Inherits CollectionViewModel(Of TEntity, TPrimaryKey, TUnitOfWork)
		Private masterEntityKey As Object

		Public Sub New(ByVal unitOfWorkFactory As IUnitOfWorkFactory(Of TUnitOfWork), ByVal getRepositoryFunc As Func(Of TUnitOfWork, IRepository(Of TEntity, TPrimaryKey)), ByVal filterExpression As Expression(Of Func(Of TEntity, Boolean)), ByVal masterEntityKey As Object)
			MyBase.New(unitOfWorkFactory, getRepositoryFunc, filterExpression)
			Me.masterEntityKey = masterEntityKey
		End Sub
		Public Overrides Sub [New]()
			If DocumentManagerService Is Nothing Then
				Return
			End If
			Dim document As IDocument = CreateDocument(New DetailEntityInfo(Of TMasterEntity)(masterEntityKey))
			document.Show()
		End Sub
	End Class
End Namespace