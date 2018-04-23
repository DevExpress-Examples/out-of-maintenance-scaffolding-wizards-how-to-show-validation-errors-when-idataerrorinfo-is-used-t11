Imports DevExpress.Mvvm.DataModel
Imports Scaffolding.ValidationErrors.Model
Imports System
Imports System.Collections.Generic
Imports System.Linq

Namespace Scaffolding.ValidationErrors.DepartmentContextDataModel

	''' <summary>
	''' IDepartmentContextUnitOfWork extends the IUnitOfWork interface with repositories representing specific entities.
	''' </summary>
	Public Interface IDepartmentContextUnitOfWork
		Inherits IUnitOfWork

		''' <summary>
		''' The Department entities repository.
		''' </summary>
		ReadOnly Property Departments() As IRepository(Of Department, Integer)
	End Interface
End Namespace
