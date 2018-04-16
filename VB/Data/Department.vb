Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations

Namespace Scaffolding.ValidationErrors.Model
	Public Class Department
		Implements IDataErrorInfo
		Private privateDepartmentID As Integer
		Public Property DepartmentID() As Integer
			Get
				Return privateDepartmentID
			End Get
			Set(ByVal value As Integer)
				privateDepartmentID = value
			End Set
		End Property
		Private privateName As String
		Public Property Name() As String
			Get
				Return privateName
			End Get
			Set(ByVal value As String)
				privateName = value
			End Set
		End Property
		Private privateBudget As Decimal
		Public Property Budget() As Decimal
			Get
				Return privateBudget
			End Get
			Set(ByVal value As Decimal)
				privateBudget = value
			End Set
		End Property
		Private privateStartDate As DateTime
		Public Property StartDate() As DateTime
			Get
				Return privateStartDate
			End Get
			Set(ByVal value As DateTime)
				privateStartDate = value
			End Set
		End Property

		Public ReadOnly Property [Error]() As String Implements IDataErrorInfo.Error
			Get
				Return If(Me("Name") IsNot Nothing OrElse Me("Budget") IsNot Nothing, "Correct values", Nothing)
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