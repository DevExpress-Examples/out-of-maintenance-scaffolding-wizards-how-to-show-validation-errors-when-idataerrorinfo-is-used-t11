Imports System.Data.Entity

Namespace Scaffolding.ValidationErrors.Model
    Public Class DepartmentContext
        Inherits DbContext

        Public Property Departments() As DbSet(Of Department)
    End Class
End Namespace
