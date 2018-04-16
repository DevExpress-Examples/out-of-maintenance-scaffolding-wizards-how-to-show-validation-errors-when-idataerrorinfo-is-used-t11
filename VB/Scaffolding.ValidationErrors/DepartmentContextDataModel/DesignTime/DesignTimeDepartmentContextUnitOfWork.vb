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
	Public Class DepartmentContextDesignTimeUnitOfWork
		Inherits DesignTimeUnitOfWork
		Implements IDepartmentContextUnitOfWork
		Private Shared departmentsRepository As New DesignTimeDepartmentRepository()

		Public Sub New()
		End Sub

		Private Function HasChanges() As Boolean Implements IDepartmentContextUnitOfWork.HasChanges
			Return False
		End Function
		Private ReadOnly Property Departments() As IDepartmentRepository Implements IDepartmentContextUnitOfWork.Departments
			Get
				Return departmentsRepository
			End Get
		End Property
		Public Overloads Sub Detach(ByVal entity As Object) Implements Scaffolding.ValidationErrors.Common.DataModel.IUnitOfWork.Detach
		End Sub
	End Class
End Namespace
