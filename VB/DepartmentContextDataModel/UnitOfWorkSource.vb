Imports System
Imports System.Linq
Imports System.Data
Imports System.Data.Entity
Imports System.Linq.Expressions
Imports System.Collections.Generic
Imports Common.Utils
Imports Common.DataModel
Imports Common.DataModel.EntityFramework
Imports Scaffolding.ValidationErrors.Model
Imports DevExpress.Mvvm
Namespace DepartmentContextDataModel
    ''' <summary>
    ''' Provides methods to obtain the relevant IUnitOfWorkFactory.
    ''' </summary>
    Public Module UnitOfWorkSource
        Friend Class DbUnitOfWorkFactory
            Implements IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork)
            Public Shared ReadOnly Instance As IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork) = New DbUnitOfWorkFactory()
            Private Sub New()
            End Sub
            Private Function CreateUnitOfWork() As IDepartmentContextUnitOfWork Implements IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork).CreateUnitOfWork
                Return New DepartmentContextUnitOfWork(New DepartmentContext())
            End Function
        End Class
        Friend Class DesignUnitOfWorkFactory
            Implements IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork)
            Public Shared ReadOnly Instance As IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork) = New DesignUnitOfWorkFactory()
            Private ReadOnly _UnitOfWork As IDepartmentContextUnitOfWork = New DepartmentContextDesignTimeUnitOfWork()
            Private Sub New()
            End Sub
            Private Function CreateUnitOfWork() As IDepartmentContextUnitOfWork Implements IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork).CreateUnitOfWork
                Return _UnitOfWork
            End Function
        End Class
        ''' <summary>
        ''' Returns the IUnitOfWorkFactory implementation based on the current mode (run-time or design-time).
        ''' </summary>
        Public Function GetUnitOfWorkFactory() As IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork)
            Return GetUnitOfWorkFactory(ViewModelBase.IsInDesignMode)
        End Function
        ''' <summary>
        ''' Returns the IUnitOfWorkFactory implementation based on the given mode (run-time or design-time).
        ''' </summary>
        ''' <param name="isInDesignTime">Used to determine which implementation of IUnitOfWorkFactory should be returned.</param>
        Public Function GetUnitOfWorkFactory(ByVal isInDesignTime As Boolean) As IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork)
            Return If(isInDesignTime, DesignUnitOfWorkFactory.Instance, DbUnitOfWorkFactory.Instance)
        End Function
    End Module
End Namespace
