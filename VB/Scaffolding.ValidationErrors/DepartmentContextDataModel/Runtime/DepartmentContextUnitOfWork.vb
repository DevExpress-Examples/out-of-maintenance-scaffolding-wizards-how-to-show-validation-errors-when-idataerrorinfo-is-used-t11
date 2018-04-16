Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports System.Data
Imports System.Data.Entity
Imports System.Linq.Expressions
Imports System.Collections.Generic
Imports Scaffolding.ValidationErrors.Common.Utils
Imports Scaffolding.ValidationErrors.Common.DataModel
Imports Scaffolding.ValidationErrors.Common.DataModel.EntityFramework
Imports Scaffolding.ValidationErrors.Model

Namespace Scaffolding.ValidationErrors.DepartmentContextDataModel
	Public Class DepartmentContextUnitOfWork
		Inherits DbUnitOfWork(Of DepartmentContext)
		Implements IDepartmentContextUnitOfWork
		Private departmentsRepository As Lazy(Of IDepartmentRepository)

		Public Sub New(ByVal context As DepartmentContext)
			MyBase.New(context)
			departmentsRepository = New Lazy(Of IDepartmentRepository)(Function() New DepartmentRepository(Me))
		End Sub
		Private Function HasChanges() As Boolean Implements IDepartmentContextUnitOfWork.HasChanges
			Return Context.ChangeTracker.HasChanges()
		End Function
		Private ReadOnly Property Departments() As IDepartmentRepository Implements IDepartmentContextUnitOfWork.Departments
			Get
				Return departmentsRepository.Value
			End Get
		End Property
	End Class
End Namespace
