Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports Scaffolding.ValidationErrors.Common.Utils
Imports Scaffolding.ValidationErrors.DepartmentContextDataModel
Imports Scaffolding.ValidationErrors.Common.DataModel
Imports Scaffolding.ValidationErrors.Model
Imports Scaffolding.ValidationErrors.Common.ViewModel

Namespace Scaffolding.ValidationErrors.ViewModels
	Partial Public Class DepartmentCollectionViewModel
		Inherits CollectionViewModel(Of Department, Integer, IDepartmentContextUnitOfWork)
		Public Sub New()
			Me.New(UnitOfWorkSource.GetUnitOfWorkFactory())
		End Sub
		Public Sub New(ByVal unitOfWorkFactory As IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork))
			MyBase.New(unitOfWorkFactory, Function(x) x.Departments)
		End Sub
	End Class
End Namespace