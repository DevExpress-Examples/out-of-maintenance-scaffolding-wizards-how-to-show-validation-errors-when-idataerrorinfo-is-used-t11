Imports Microsoft.VisualBasic
Imports System
Imports System.Linq

Namespace Scaffolding.ValidationErrors.Common.DataModel
	Public Interface IUnitOfWork
		Sub SaveChanges()
		Function GetState(ByVal entity As Object) As EntityState
		Sub Update(ByVal entity As Object)
		Sub Detach(ByVal entity As Object)
	End Interface
	Public Interface IUnitOfWorkFactory(Of TUnitOfWork As IUnitOfWork)
		Function CreateUnitOfWork() As TUnitOfWork
	End Interface
End Namespace
