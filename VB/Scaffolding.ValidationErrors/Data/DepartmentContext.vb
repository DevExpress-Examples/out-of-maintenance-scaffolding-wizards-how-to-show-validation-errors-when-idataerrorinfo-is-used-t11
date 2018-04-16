Imports Microsoft.VisualBasic
Imports System.Data.Entity

Namespace Scaffolding.ValidationErrors.Model
	Public Class DepartmentContext
		Inherits DbContext
		Private privateDepartments As DbSet(Of Department)
		Public Property Departments() As DbSet(Of Department)
			Get
				Return privateDepartments
			End Get
			Set(ByVal value As DbSet(Of Department))
				privateDepartments = value
			End Set
		End Property
	End Class
End Namespace
