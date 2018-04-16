Imports System
Imports System.Linq
Imports DevExpress.Mvvm.POCO
Imports Common.Utils
Imports DepartmentContextDataModel
Imports Common.DataModel
Imports Scaffolding.ValidationErrors.Model
Imports Common.ViewModel
Namespace ViewModels
    ''' <summary>
    ''' Represents the Departments collection view model.
    ''' </summary>
    Partial Public Class DepartmentCollectionViewModel
        Inherits CollectionViewModel(Of Department, Integer, IDepartmentContextUnitOfWork)
        ''' <summary>
        ''' Creates a new instance of DepartmentCollectionViewModel as a POCO view model.
        ''' </summary>
        ''' <param name="unitOfWorkFactory">A factory used to create a unit of work instance.</param>
        Public Shared Function Create(Optional ByVal unitOfWorkFactory As IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork) = Nothing) As DepartmentCollectionViewModel
            Return ViewModelSource.Create(Function() New DepartmentCollectionViewModel(unitOfWorkFactory))
        End Function
        ''' <summary>
        ''' Initializes a new instance of the DepartmentCollectionViewModel class.
        ''' This constructor is declared protected to avoid undesired instantiation of the DepartmentCollectionViewModel type without the POCO proxy factory.
        ''' </summary>
        ''' <param name="unitOfWorkFactory">A factory used to create a unit of work instance.</param>
        Protected Sub New(Optional ByVal unitOfWorkFactory As IUnitOfWorkFactory(Of IDepartmentContextUnitOfWork) = Nothing)
            MyBase.New(If(unitOfWorkFactory, UnitOfWorkSource.GetUnitOfWorkFactory()), Function(x) x.Departments)
        End Sub
    End Class
End Namespace
