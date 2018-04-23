Imports DevExpress.Mvvm.DataModel
Imports DevExpress.Mvvm.DataModel.EF6
Imports Scaffolding.ValidationErrors.Model
Imports System
Imports System.Collections.Generic
Imports System.Linq

Namespace Scaffolding.ValidationErrors.DepartmentContextDataModel

	''' <summary>
	''' A DepartmentContextUnitOfWork instance that represents the run-time implementation of the IDepartmentContextUnitOfWork interface.
	''' </summary>
	Public Class DepartmentContextUnitOfWork
		Inherits DbUnitOfWork(Of DepartmentContext)
		Implements IDepartmentContextUnitOfWork

		Public Sub New(ByVal contextFactory As Func(Of DepartmentContext))
			MyBase.New(contextFactory)
		End Sub

		Private ReadOnly Property IDepartmentContextUnitOfWork_Departments() As IRepository(Of Department, Integer) Implements IDepartmentContextUnitOfWork.Departments
			Get
				Return GetRepository(Function(x) x.Set(Of Department)(), Function(x As Department) x.DepartmentID)
			End Get
		End Property
	End Class
End Namespace
