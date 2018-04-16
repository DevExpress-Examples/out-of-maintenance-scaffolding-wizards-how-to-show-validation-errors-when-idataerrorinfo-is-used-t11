Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports System.Data
Imports System.Data.Entity
Imports System.Linq.Expressions
Imports System.Collections.Generic
Imports Scaffolding.ValidationErrors.Common.Utils
Imports Scaffolding.ValidationErrors.Model
Imports Scaffolding.ValidationErrors.Common.DataModel
Imports Scaffolding.ValidationErrors.Common.DataModel.EntityFramework

Namespace Scaffolding.ValidationErrors.DepartmentContextDataModel
	Public Class DepartmentRepository
		Inherits DbRepository(Of Department, Integer, DepartmentContext)
		Implements IDepartmentRepository
		Public Sub New(ByVal unitOfWork As DbUnitOfWork(Of DepartmentContext))
			MyBase.New(unitOfWork, Function(context) context.Set(Of Department)(), Function(x) x.DepartmentID)
		End Sub
	End Class
End Namespace