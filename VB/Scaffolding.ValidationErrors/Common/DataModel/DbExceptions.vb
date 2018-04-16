Imports Microsoft.VisualBasic
Imports System
Imports System.Linq

Namespace Scaffolding.ValidationErrors.Common.DataModel
	Public Class DbException
		Inherits Exception
		Public Sub New(ByVal errorMessage As String, ByVal errorCaption As String, ByVal innerException As Exception)
			MyBase.New(innerException.Message, innerException)
			ErrorMessage = errorMessage
			ErrorCaption = errorCaption
		End Sub
		Private privateErrorMessage As String
		Public Property ErrorMessage() As String
			Get
				Return privateErrorMessage
			End Get
			Private Set(ByVal value As String)
				privateErrorMessage = value
			End Set
		End Property
		Private privateErrorCaption As String
		Public Property ErrorCaption() As String
			Get
				Return privateErrorCaption
			End Get
			Private Set(ByVal value As String)
				privateErrorCaption = value
			End Set
		End Property
	End Class
End Namespace