Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Data.Entity
Imports System.Data.Entity.Validation
Imports System.Data.Entity.Infrastructure
Imports System.Linq

Namespace Scaffolding.ValidationErrors.Common.DataModel.EntityFramework
	Public MustInherit Class DbUnitOfWork(Of TContext As DbContext)
		Implements IUnitOfWork
		Public Sub New(ByVal context As TContext)
			Context = context
		End Sub
		Private privateContext As TContext
		Public Property Context() As TContext
			Get
				Return privateContext
			End Get
			Private Set(ByVal value As TContext)
				privateContext = value
			End Set
		End Property
		Private Sub SaveChanges() Implements IUnitOfWork.SaveChanges
			Try
				Context.SaveChanges()
			Catch ex As DbEntityValidationException
				Throw DbExceptionsConverter.Convert(ex)
			Catch ex As DbUpdateException
				Throw DbExceptionsConverter.Convert(ex)
			End Try
		End Sub
		Private Function GetState(ByVal entity As Object) As EntityState Implements IUnitOfWork.GetState
			Return GetEntityState(Context.Entry(entity).State)
		End Function
		Private Sub Update(ByVal entity As Object) Implements IUnitOfWork.Update
		End Sub
		Private Sub Detach(ByVal entity As Object) Implements IUnitOfWork.Detach
			Context.Entry(entity).State = System.Data.Entity.EntityState.Detached
		End Sub
		Private Function GetEntityState(ByVal entityStates As System.Data.Entity.EntityState) As EntityState
			Select Case entityStates
				Case System.Data.Entity.EntityState.Added
					Return EntityState.Added
				Case System.Data.Entity.EntityState.Deleted
					Return EntityState.Deleted
				Case System.Data.Entity.EntityState.Detached
					Return EntityState.Detached
				Case System.Data.Entity.EntityState.Modified
					Return EntityState.Modified
				Case System.Data.Entity.EntityState.Unchanged
					Return EntityState.Unchanged
				Case Else
					Throw New NotImplementedException()
			End Select
		End Function
	End Class
End Namespace