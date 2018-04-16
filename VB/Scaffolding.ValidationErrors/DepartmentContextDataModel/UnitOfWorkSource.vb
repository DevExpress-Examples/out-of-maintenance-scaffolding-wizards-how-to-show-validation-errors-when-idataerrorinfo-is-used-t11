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
Imports DevExpress.Mvvm

Namespace Scaffolding.ValidationErrors.DepartmentContextDataModel
	Public NotInheritable Class UnitOfWorkSource
		#Region "inner classes"
		Private Class DbUnitOfWorkFactory
			Implements IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork)
			Public Shared ReadOnly Instance As IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork) = New DbUnitOfWorkFactory()
			Private Sub New()
			End Sub
			Private Function CreateUnitOfWork() As IDepartmentContextUnitOfWork Implements IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork).CreateUnitOfWork
				Return New DepartmentContextUnitOfWork(New DepartmentContext())
			End Function
		End Class
		Private Class DesignUnitOfWorkFactory
			Implements IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork)
			Public Shared ReadOnly Instance As IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork) = New DesignUnitOfWorkFactory()

			Private ReadOnly UnitOfWork As IDepartmentContextUnitOfWork = New DepartmentContextDesignTimeUnitOfWork()
			Private Sub New()
			End Sub
			Private Function CreateUnitOfWork() As IDepartmentContextUnitOfWork Implements IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork).CreateUnitOfWork
				Return UnitOfWork
			End Function
		End Class
		#End Region
		Private Sub New()
		End Sub
		Public Shared Function GetUnitOfWorkFactory() As IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork)
			Return GetUnitOfWorkFactory(ViewModelBase.IsInDesignMode)
		End Function
		Public Shared Function GetUnitOfWorkFactory(ByVal isInDesignTime As Boolean) As IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork)
			Return If(isInDesignTime, DesignUnitOfWorkFactory.Instance, DbUnitOfWorkFactory.Instance)
		End Function
		Public Shared Function CreateUnitOfWork(Optional ByVal isInDesignTime As Boolean = False) As IDepartmentContextUnitOfWork
			Return GetUnitOfWorkFactory(isInDesignTime).CreateUnitOfWork()
		End Function
	End Class
End Namespace