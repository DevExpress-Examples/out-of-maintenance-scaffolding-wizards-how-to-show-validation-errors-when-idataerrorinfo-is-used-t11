Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Linq.Expressions

Namespace Scaffolding.ValidationErrors.Common.Utils
	Public Class ExpressionHelper
		Private Shared ReadOnly TraitsCache As New Dictionary(Of Type, Object)()

		Public Shared Function GetValueEqualsExpression(Of TPropertyOwner, TProperty)(ByVal getPropertyExpression As Expression(Of Func(Of TPropertyOwner, TProperty)), ByVal constant As TProperty) As Expression(Of Func(Of TPropertyOwner, Boolean))
			Dim equalExpression As Expression = Expression.Equal(getPropertyExpression.Body, Expression.Constant(constant))
			Return Expression.Lambda(Of Func(Of TPropertyOwner, Boolean))(equalExpression, getPropertyExpression.Parameters.Single())
		End Function
		Public Shared Function GetEntityTraits(Of TOwner, TPropertyOwner, TProperty)(ByVal owner As TOwner, ByVal getPropertyExpression As Expression(Of Func(Of TPropertyOwner, TProperty)), ByVal setPropertyAction As Action(Of TPropertyOwner, TProperty)) As EntityTraits(Of TPropertyOwner, TProperty)
			Dim traits As Object = Nothing
			If (Not TraitsCache.TryGetValue(owner.GetType(), traits)) Then
				traits = New EntityTraits(Of TPropertyOwner, TProperty)(getPropertyExpression.Compile(), If(setPropertyAction, GetSetValueActionExpression(getPropertyExpression).Compile()), GetHasValueFunctionExpression(getPropertyExpression).Compile())
				TraitsCache(owner.GetType()) = traits
			End If
			Return CType(traits, EntityTraits(Of TPropertyOwner, TProperty))
		End Function

		Private Shared Function GetSetValueActionExpression(Of TPropertyOwner, TProperty)(ByVal getPropertyExpression As Expression(Of Func(Of TPropertyOwner, TProperty))) As Expression(Of Action(Of TPropertyOwner, TProperty))
			Dim body As MemberExpression = CType(getPropertyExpression.Body, MemberExpression)
			Dim thisParameter As ParameterExpression = getPropertyExpression.Parameters.Single()
			Dim propertyValueParameter As ParameterExpression = Expression.Parameter(GetType(TProperty), "propertyValue")
			Dim assignPropertyValueExpression As BinaryExpression = Expression.Assign(body, propertyValueParameter)
			Return Expression.Lambda(Of Action(Of TPropertyOwner, TProperty))(assignPropertyValueExpression, thisParameter, propertyValueParameter)
		End Function
		Private Shared Function GetHasValueFunctionExpression(Of TPropertyOwner, TProperty)(ByVal getPropertyExpression As Expression(Of Func(Of TPropertyOwner, TProperty))) As Expression(Of Func(Of TPropertyOwner, Boolean))
			Dim memberExpression As MemberExpression = CType(getPropertyExpression.Body, MemberExpression)
			If TypeOf memberExpression.Expression Is MemberExpression Then
				Dim equalExpression As Expression = Expression.NotEqual(memberExpression.Expression, Expression.Constant(Nothing))
				Return Expression.Lambda(Of Func(Of TPropertyOwner, Boolean))(equalExpression, getPropertyExpression.Parameters.Single())
			End If
			Return Function(x) True
		End Function
	End Class
	Public Class EntityTraits(Of TEntity, TPrimaryKey)
		Public Sub New(ByVal getPrimaryKeyFunction As Func(Of TEntity, TPrimaryKey), ByVal setPrimaryKeyAction As Action(Of TEntity, TPrimaryKey), ByVal hasPrimaryKeyFunction As Func(Of TEntity, Boolean))
			Me.GetPrimaryKey = getPrimaryKeyFunction
			Me.SetPrimaryKey = setPrimaryKeyAction
			Me.HasPrimaryKey = hasPrimaryKeyFunction
		End Sub
		Private privateGetPrimaryKey As Func(Of TEntity, TPrimaryKey)
		Public Property GetPrimaryKey() As Func(Of TEntity, TPrimaryKey)
			Get
				Return privateGetPrimaryKey
			End Get
			Private Set(ByVal value As Func(Of TEntity, TPrimaryKey))
				privateGetPrimaryKey = value
			End Set
		End Property
		Private privateSetPrimaryKey As Action(Of TEntity, TPrimaryKey)
		Public Property SetPrimaryKey() As Action(Of TEntity, TPrimaryKey)
			Get
				Return privateSetPrimaryKey
			End Get
			Private Set(ByVal value As Action(Of TEntity, TPrimaryKey))
				privateSetPrimaryKey = value
			End Set
		End Property
		Private privateHasPrimaryKey As Func(Of TEntity, Boolean)
		Public Property HasPrimaryKey() As Func(Of TEntity, Boolean)
			Get
				Return privateHasPrimaryKey
			End Get
			Private Set(ByVal value As Func(Of TEntity, Boolean))
				privateHasPrimaryKey = value
			End Set
		End Property
	End Class
End Namespace
