Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports DevExpress.Mvvm

Namespace Scaffolding.ValidationErrors
	Public Class DepartmentContextViewModel
		Protected Overridable ReadOnly Property DocumentManagerService() As IDocumentManagerService
			Get
				Return Nothing
			End Get
		End Property

		Public Sub ShowDocument(ByVal p As String)
			Dim parameters() As String = p.Split(";"c)
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