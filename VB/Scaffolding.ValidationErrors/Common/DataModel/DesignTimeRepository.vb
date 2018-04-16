Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports System.Linq.Expressions
Imports System.Collections.Generic
Imports DevExpress.Mvvm
Imports Scaffolding.ValidationErrors.Common.Utils

Namespace Scaffolding.ValidationErrors.Common.DataModel
	Public MustInherit Class DesignTimeRepository(Of TEntity As Class, TPrimaryKey)
		Inherits DesignTimeReadOnlyRepository(Of TEntity)
		Implements IRepository(Of TEntity, TPrimaryKey)

		Private ReadOnly getPrimaryKeyExpression_Renamed As Expression(Of Func(Of TEntity, TPrimaryKey))
		Private ReadOnly entityTraits As EntityTraits(Of TEntity, TPrimaryKey)

		Public Sub New(ByVal getPrimaryKeyExpression As Expression(Of Func(Of TEntity, TPrimaryKey)), Optional ByVal setPrimaryKeyAction As Action(Of TEntity, TPrimaryKey) = Nothing)
			Me.getPrimaryKeyExpression_Renamed = getPrimaryKeyExpression
			Me.entityTraits = ExpressionHelper.GetEntityTraits(Me, getPrimaryKeyExpression, setPrimaryKeyAction)
		End Sub
		Protected Overridable Function CreateCore() As TEntity
			Return DesignTimeHelper.CreateDesignTimeObject(Of TEntity)()
		End Function
		Protected Overridable Function FindCore(ByVal key As TPrimaryKey) As TEntity
			Throw New InvalidOperationException()
		End Function
		Protected Overridable Sub RemoveCore(ByVal entity As TEntity)
			Throw New InvalidOperationException()
		End Sub
		Protected Overridable Function ReloadCore(ByVal entity As TEntity) As TEntity
			Throw New InvalidOperationException()
		End Function
		Protected Overridable Function GetPrimaryKeyCore(ByVal entity As TEntity) As TPrimaryKey
			Return entityTraits.GetPrimaryKey(entity)
		End Function
		Protected Overridable Sub SetPrimaryKeyCore(ByVal entity As TEntity, ByVal key As TPrimaryKey)
			Dim setPrimaryKeyaction = entityTraits.SetPrimaryKey
			setPrimaryKeyaction(entity, key)
		End Sub
		#Region "IRepository"
		Private Function Find(ByVal key As TPrimaryKey) As TEntity Implements IRepository(Of TEntity, TPrimaryKey).Find
			Return FindCore(key)
		End Function
		Private Sub Remove(ByVal entity As TEntity) Implements IRepository(Of TEntity, TPrimaryKey).Remove
			RemoveCore(entity)
		End Sub
		Private Function Create() As TEntity Implements IRepository(Of TEntity, TPrimaryKey).Create
			Return CreateCore()
		End Function
		Private Function Reload(ByVal entity As TEntity) As TEntity Implements IRepository(Of TEntity, TPrimaryKey).Reload
			Return ReloadCore(entity)
		End Function
		Private ReadOnly Property GetPrimaryKeyExpression() As Expression(Of Func(Of TEntity, TPrimaryKey)) Implements IRepository(Of TEntity, TPrimaryKey).GetPrimaryKeyExpression
			Get
				Return getPrimaryKeyExpression_Renamed
			End Get
		End Property
		Private Function GetPrimaryKey(ByVal entity As TEntity) As TPrimaryKey Implements IRepository(Of TEntity, TPrimaryKey).GetPrimaryKey
			Return GetPrimaryKeyCore(entity)
		End Function
		Private Function HasPrimaryKey(ByVal entity As TEntity) As Boolean Implements IRepository(Of TEntity, TPrimaryKey).HasPrimaryKey
			Return entityTraits.HasPrimaryKey(entity)
		End Function
		Private Sub SetPrimaryKey(ByVal entity As TEntity, ByVal key As TPrimaryKey) Implements IRepository(Of TEntity, TPrimaryKey).SetPrimaryKey
			SetPrimaryKeyCore(entity, key)
		End Sub
		#End Region
	End Class
End Namespace