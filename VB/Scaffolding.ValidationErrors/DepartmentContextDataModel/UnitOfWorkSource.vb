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
    ''' <summary>
    ''' Provides methods to obtain the relevant IUnitOfWorkFactory.
    ''' </summary>
    Public NotInheritable Class UnitOfWorkSource

        Private Sub New()
        End Sub


        #Region "inner classes"
        Private Class DbUnitOfWorkFactory
            Implements IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork)

            Public Shared ReadOnly Instance As IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork) = New DbUnitOfWorkFactory()
            Private Sub New()
            End Sub
            Private Function IUnitOfWorkFactoryGeneric_CreateUnitOfWork() As IDepartmentContextUnitOfWork Implements IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork).CreateUnitOfWork
                Return New DepartmentContextUnitOfWork(Function() New DepartmentContext())
            End Function
        End Class

        Private Class DesignUnitOfWorkFactory
            Implements IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork)

            Public Shared ReadOnly Instance As IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork) = New DesignUnitOfWorkFactory()
            Private Sub New()
            End Sub
            Private Function IUnitOfWorkFactoryGeneric_CreateUnitOfWork() As IDepartmentContextUnitOfWork Implements IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork).CreateUnitOfWork
                Return New DepartmentContextDesignTimeUnitOfWork()
            End Function
        End Class
        #End Region

        ''' <summary>
        ''' Returns the IUnitOfWorkFactory implementation based on the current mode (run-time or design-time).
        ''' </summary>
        Public Shared Function GetUnitOfWorkFactory() As IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork)
            Return GetUnitOfWorkFactory(ViewModelBase.IsInDesignMode)
        End Function

        ''' <summary>
        ''' Returns the IUnitOfWorkFactory implementation based on the given mode (run-time or design-time).
        ''' </summary>
        ''' <param name="isInDesignTime">Used to determine which implementation of IUnitOfWorkFactory should be returned.</param>
        Public Shared Function GetUnitOfWorkFactory(ByVal isInDesignTime As Boolean) As IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork)
            Return If(isInDesignTime, DesignUnitOfWorkFactory.Instance, DbUnitOfWorkFactory.Instance)
        End Function
    End Class
End Namespace