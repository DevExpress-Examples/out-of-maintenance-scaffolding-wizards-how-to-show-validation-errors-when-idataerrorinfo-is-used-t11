Imports System
Imports System.Collections.Generic
Imports System.Data.Entity

Namespace Scaffolding.ValidationErrors.Model
	Public Class DepartmentContextInitializer
		Inherits DropCreateDatabaseIfModelChanges(Of DepartmentContext)

		Protected Overrides Sub Seed(ByVal context As DepartmentContext)
			MyBase.Seed(context)

			context.Departments.Add(New Department With {.Budget = 138, .Name = "Music", .StartDate = New Date(1988, 9, 14)})
			context.Departments.Add(New Department With {.Budget = 572, .Name = "Journalism", .StartDate = New Date(1994, 5, 23)})
			context.Departments.Add(New Department With {.Budget = 349, .Name = "Management", .StartDate = New Date(1994, 3, 7)})

			context.SaveChanges()
		End Sub
	End Class
End Namespace