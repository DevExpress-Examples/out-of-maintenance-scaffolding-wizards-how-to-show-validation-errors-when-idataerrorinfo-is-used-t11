Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel.DataAnnotations
Imports System.Linq
Imports System.Linq.Expressions
Imports DevExpress.Mvvm
Imports DevExpress.Mvvm.POCO
Imports DevExpress.Mvvm.DataAnnotations
Imports Scaffolding.ValidationErrors.Common.Utils
Imports Scaffolding.ValidationErrors.Common.DataModel
Imports MessageBoxButton = System.Windows.MessageBoxButton
Imports MessageBoxImage = System.Windows.MessageBoxImage
Imports MessageBoxResult = System.Windows.MessageBoxResult

Namespace Scaffolding.ValidationErrors.Common.ViewModel
	Public Interface ISingleObjectViewModel(Of TEntity, TPrimaryKey)
		ReadOnly Property Entity() As TEntity
		ReadOnly Property PrimaryKey() As TPrimaryKey
	End Interface
	Public Interface IDetailEntityInfo
	End Interface
	Public Class DetailEntityInfo(Of TDetailEntity As Class)
		Implements IDetailEntityInfo
		Private privateDetailEntityKey As Object
		Public Property DetailEntityKey() As Object
			Get
				Return privateDetailEntityKey
			End Get
			Private Set(ByVal value As Object)
				privateDetailEntityKey = value
			End Set
		End Property
		Public Sub New(ByVal detailEntityKey As Object)
			Me.DetailEntityKey = detailEntityKey
		End Sub
	End Class
End Namespace