Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports DevExpress.Mvvm
Imports DevExpress.Mvvm.POCO
Namespace ViewModels
    ''' <summary>
    ''' Represents the root POCO view model for the DepartmentContext data model.
    ''' </summary>
    Public Class DepartmentContextViewModel
        ''' <summary>
        ''' Creates a new instance of DepartmentContextViewModel as a POCO view model.
        ''' </summary>
        Public Shared Function Create() As DepartmentContextViewModel
            Return ViewModelSource.Create(Function() New DepartmentContextViewModel())
        End Function
        ''' <summary>
        ''' Initializes a new instance of the DepartmentContextViewModel class.
        ''' This constructor is declared protected to avoid undesired instantiation of the DepartmentContextViewModel type without the POCO proxy factory.
        ''' </summary>
        Protected Sub New()
        End Sub
        Protected Overridable ReadOnly Property DocumentManagerService As IDocumentManagerService
            Get
                Return Nothing
            End Get
        End Property
        ''' <summary>
        ''' Creates and shows a document containing a collection view model.
        ''' Since DepartmentContextViewModel is a POCO view model, an instance of this class will also expose the ShowDocumentCommand property that can be used as a binding source in views.
        ''' </summary>
        ''' <param name="p">Contains the document view name and title separated with ';'.</param>
        Public Sub ShowDocument(ByVal p As String)
            Dim parameters As String() = p.Split(";"c)
            ShowDocumentCore(parameters(0), parameters(1))
        End Sub
        Private Sub ShowDocumentCore(ByVal viewName As String, ByVal title As String)
            Dim document As IDocument = DocumentManagerService.FindDocumentByIdOrCreate(viewName, Function(x) CreateDocument(viewName, title))
            document.Show()
        End Sub
        Private Function CreateDocument(ByVal viewName As String, ByVal title As String) As IDocument
            Dim document = DocumentManagerService.CreateDocument(viewName, Nothing, Me)
            document.Title = title
            document.DestroyOnClose = False
            Return document
        End Function
    End Class
End Namespace
