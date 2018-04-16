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
    ''' IDepartmentContextUnitOfWork extends the IUnitOfWork interface with repositories representing specific entities.
    ''' </summary>
    Public Interface IDepartmentContextUnitOfWork
        Inherits IUnitOfWork
        ''' <summary>
        ''' The Department entities repository.
        ''' </summary>
        ReadOnly Property Departments As IRepository(Of Department, Integer)
    End Interface
End Namespace
