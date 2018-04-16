Imports Microsoft.VisualBasic
Imports System
Imports System.Data.Entity.Infrastructure
Imports System.Data.Entity.Validation
Imports System.Linq
Imports System.Text

Namespace Scaffolding.ValidationErrors.Common.DataModel.EntityFramework
	Public NotInheritable Class DbExceptionsConverter
		Private Sub New()
		End Sub
		Public Shared Function Convert(ByVal exception As DbUpdateException) As DbException
			Dim originalException As Exception = exception
			Do While originalException.InnerException IsNot Nothing
				originalException = originalException.InnerException
			Loop
			Return New DbException(originalException.Message, CommonResources.Exception_UpdateErrorCaption, exception)
		End Function
		Public Shared Function Convert(ByVal exception As DbEntityValidationException) As DbException
			Dim stringBuilder As New StringBuilder()
			For Each validationResult As DbEntityValidationResult In exception.EntityValidationErrors
				For Each [error] As DbValidationError In validationResult.ValidationErrors
					If stringBuilder.Length > 0 Then
						stringBuilder.AppendLine()
					End If
					stringBuilder.Append([error].PropertyName & ": " & [error].ErrorMessage)
				Next [error]
			Next validationResult
			Return New DbException(stringBuilder.ToString(), CommonResources.Exception_ValidationErrorCaption, exception)
		End Function
	End Class
End Namespace