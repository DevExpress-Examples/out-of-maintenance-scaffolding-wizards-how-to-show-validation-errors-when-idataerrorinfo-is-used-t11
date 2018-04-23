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
    ''' IDepartmentContextUnitOfWork extends the IUnitOfWork interface with repositories representing specific entities.
    ''' </summary>
    Public Interface IDepartmentContextUnitOfWork
        Inherits IUnitOfWork

        ''' <summary>
        ''' The Department entities repository.
        ''' </summary>
        ReadOnly Property Departments() As IRepository(Of Department, Integer)
    End Interface
End Namespace
