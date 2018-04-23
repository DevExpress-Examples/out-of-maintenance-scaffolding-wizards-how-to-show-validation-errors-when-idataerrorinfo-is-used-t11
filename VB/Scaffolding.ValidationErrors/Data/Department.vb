Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations

Namespace Scaffolding.ValidationErrors.Model
	Public Class Department
		Implements IDataErrorInfo

		Public Property DepartmentID() As Integer
		Public Property Name() As String
		Public Property Budget() As Decimal
		Public Property StartDate() As Date

		Public ReadOnly Property [Error]() As String Implements IDataErrorInfo.Error
			Get
				Return Nothing
			End Get
		End Property

		Default Public ReadOnly Property Item(ByVal columnName As String) As String Implements IDataErrorInfo.Item
			Get
				Select Case columnName
					Case "Name"
						Return If(String.IsNullOrEmpty(Name), "Name cannot be null", Nothing)
					Case "Budget"
						Return If(Budget < 0, "Budget cannot be negative", Nothing)
					Case Else
						Return Nothing
				End Select
			End Get
		End Property
	End Class
End Namespace