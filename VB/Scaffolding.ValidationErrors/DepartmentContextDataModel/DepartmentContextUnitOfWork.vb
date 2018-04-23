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

Namespace Scaffolding.ValidationErrors.DepartmentContextDataModel
    ''' <summary>
    ''' A DepartmentContextUnitOfWork instance that represents the run-time implementation of the IDepartmentContextUnitOfWork interface.
    ''' </summary>
    Public Class DepartmentContextUnitOfWork
        Inherits DbUnitOfWork(Of DepartmentContext)
        Implements IDepartmentContextUnitOfWork

        Public Sub New(ByVal contextFactory As Func(Of DepartmentContext))
            MyBase.New(contextFactory)
        End Sub

        Private ReadOnly Property IDepartmentContextUnitOfWork_Departments() As IRepository(Of Department, Integer) Implements IDepartmentContextUnitOfWork.Departments
            Get
                Return GetRepository(Function(x) x.Set(Of Department)(), Function(x) x.DepartmentID)
            End Get
        End Property
    End Class
End Namespace
