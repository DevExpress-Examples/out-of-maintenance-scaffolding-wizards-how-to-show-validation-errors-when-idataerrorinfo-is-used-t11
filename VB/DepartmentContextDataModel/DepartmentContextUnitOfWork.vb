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
Namespace DepartmentContextDataModel
    ''' <summary>
    ''' A DepartmentContextUnitOfWork instance that represents the run-time implementation of the IDepartmentContextUnitOfWork interface.
    ''' </summary>
    Public Class DepartmentContextUnitOfWork
        Inherits DbUnitOfWork(Of DepartmentContext)
        Implements IDepartmentContextUnitOfWork
        ''' <summary>
        ''' Initializes a new instance of the DepartmentContextUnitOfWork class.
        ''' </summary>
        ''' <param name="context">An underlying DbContext.</param>
        Public Sub New(ByVal context As DepartmentContext)
            MyBase.New(context)
        End Sub
        Private ReadOnly Property Departments As IRepository(Of Department, Integer) Implements IDepartmentContextUnitOfWork.Departments
            Get
                Return GetRepository(Function(x) x.[Set](Of Department)(), Function(x) x.DepartmentID)
            End Get
        End Property
    End Class
End Namespace
