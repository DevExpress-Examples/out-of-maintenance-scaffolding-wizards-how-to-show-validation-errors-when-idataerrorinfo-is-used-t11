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
    ''' A DepartmentContextDesignTimeUnitOfWork instance that represents the design-time implementation of the IDepartmentContextUnitOfWork interface.
    ''' </summary>
    Public Class DepartmentContextDesignTimeUnitOfWork
        Inherits DesignTimeUnitOfWork
        Implements IDepartmentContextUnitOfWork

        ''' <summary>
        ''' Initializes a new instance of the DepartmentContextDesignTimeUnitOfWork class.
        ''' </summary>
        Public Sub New()
        End Sub

        Private ReadOnly Property IDepartmentContextUnitOfWork_Departments() As IRepository(Of Department, Integer) Implements IDepartmentContextUnitOfWork.Departments
            Get
                Return GetRepository(Function(x As Department) x.DepartmentID)
            End Get
        End Property
    End Class
End Namespace
