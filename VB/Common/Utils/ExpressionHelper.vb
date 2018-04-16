Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Linq.Expressions
Namespace Common.Utils
    ''' <summary>
    ''' Provides methods to perform operations with lambda expression trees.
    ''' </summary>
    Public Class ExpressionHelper
        Private Shared ReadOnly _TraitsCache As Dictionary(Of Type, Object) = New Dictionary(Of Type, Object)()
        ''' <summary>
        ''' Builds a lambda expression that compares an entity property value with a given constant value.
        ''' </summary>
        ''' <typeparam name="TPropertyOwner">An owner type of the property.</typeparam>
        ''' <typeparam name="TProperty">A primary key property type.</typeparam>
        ''' <param name="getPropertyExpression">A lambda expression that returns the property value for a given entity.</param>
        ''' <param name="constant">A constant value to compare with entity property value.</param>
        Public Shared Function GetValueEqualsExpression(Of TPropertyOwner, TProperty)(ByVal getPropertyExpression As Expression(Of Func(Of TPropertyOwner, TProperty)), ByVal constant As TProperty) As Expression(Of Func(Of TPropertyOwner, Boolean))
            Dim equalExpression As Expression = Expression.Equal(getPropertyExpression.Body, Expression.Convert(Expression.Constant(constant), getPropertyExpression.Body.Type))
            Return Expression.Lambda(Of Func(Of TPropertyOwner, Boolean))(equalExpression, getPropertyExpression.Parameters.[Single]())
        End Function
        ''' <summary>
        ''' Returns an instance of the EntityTraits class that encapsulates operations to obtain and set the primary key value of a given entity.
        ''' </summary>
        ''' <typeparam name="TOwner">A type used as a key to cache compiled lambda expressions.</typeparam>
        ''' <typeparam name="TPropertyOwner">An owner type of the primary key property.</typeparam>
        ''' <typeparam name="TProperty">A primary key property type.</typeparam>
        ''' <param name="owner">An instance of the TOwner type which type is used as a key to cache compiled lambda expressions.</param>
        ''' <param name="getPropertyExpression">A lambda expression that returns the primary key value for a given entity.</param>
        ''' <param name="setPropertyAction">A lambda expression that assigns the primary key value to a given entity.</param>
        Public Shared Function GetEntityTraits(Of TOwner, TPropertyOwner, TProperty)(ByVal owner As TOwner, ByVal getPropertyExpression As Expression(Of Func(Of TPropertyOwner, TProperty)), ByVal setPropertyAction As Action(Of TPropertyOwner, TProperty)) As EntityTraits(Of TPropertyOwner, TProperty)
            Dim traits As Object = Nothing
            If Not _TraitsCache.TryGetValue(owner.[GetType](), traits) Then
                traits = New EntityTraits(Of TPropertyOwner, TProperty)(getPropertyExpression.Compile(), If(setPropertyAction, GetSetValueActionExpression(getPropertyExpression).Compile()), GetHasValueFunctionExpression(getPropertyExpression).Compile())
                _TraitsCache(owner.[GetType]()) = traits
            End If
            Return CType(traits, EntityTraits(Of TPropertyOwner, TProperty))
        End Function
        Private Shared Function GetSetValueActionExpression(Of TPropertyOwner, TProperty)(ByVal getPropertyExpression As Expression(Of Func(Of TPropertyOwner, TProperty))) As Expression(Of Action(Of TPropertyOwner, TProperty))
            Dim body As MemberExpression = CType(getPropertyExpression.Body, MemberExpression)
            Dim thisParameter As ParameterExpression = getPropertyExpression.Parameters.[Single]()
            Dim propertyValueParameter As ParameterExpression = Expression.Parameter(GetType(TProperty), "propertyValue")
            Dim assignPropertyValueExpression As BinaryExpression = Expression.Assign(body, propertyValueParameter)
            Return Expression.Lambda(Of Action(Of TPropertyOwner, TProperty))(assignPropertyValueExpression, thisParameter, propertyValueParameter)
        End Function
        Private Shared Function GetHasValueFunctionExpression(Of TPropertyOwner, TProperty)(ByVal getPropertyExpression As Expression(Of Func(Of TPropertyOwner, TProperty))) As Expression(Of Func(Of TPropertyOwner, Boolean))
            Dim memberExpression As MemberExpression = CType(getPropertyExpression.Body, MemberExpression)
            If TypeOf memberExpression.Expression Is MemberExpression Then
                Dim equalExpression As Expression = Expression.NotEqual(memberExpression.Expression, Expression.Constant(Nothing))
                Return Expression.Lambda(Of Func(Of TPropertyOwner, Boolean))(equalExpression, getPropertyExpression.Parameters.[Single]())
            End If
            Return Function(x) True
        End Function
    End Class
    ''' <summary>
    ''' Incapsulates operations to obtain and set the primary key value of a given entity.
    ''' </summary>
    ''' <typeparam name="TEntity">An owner type of the primary key property.</typeparam>
    ''' <typeparam name="TPrimaryKey">A primary key property type.</typeparam>
    Public Class EntityTraits(Of TEntity, TPrimaryKey)
        Private _HasPrimaryKey As Func(Of TEntity, Boolean)
        Private _SetPrimaryKey As Action(Of TEntity, TPrimaryKey)
        Private _GetPrimaryKey As Func(Of TEntity, TPrimaryKey)
        ''' <summary>
        ''' Initializes a new instance of EntityTraits class.
        ''' </summary>
        ''' <param name="getPrimaryKeyFunction">A function that returns the primary key value of a given entity.</param>
        ''' <param name="setPrimaryKeyAction">An action that assigns the primary key value to a given entity.</param>
        ''' <param name="hasPrimaryKeyFunction">A function that determines whether given the entity has a primary key assigned.</param>
        Public Sub New(ByVal getPrimaryKeyFunction As Func(Of TEntity, TPrimaryKey), ByVal setPrimaryKeyAction As Action(Of TEntity, TPrimaryKey), ByVal hasPrimaryKeyFunction As Func(Of TEntity, Boolean))
            Me._GetPrimaryKey = getPrimaryKeyFunction
            Me._SetPrimaryKey = setPrimaryKeyAction
            Me._HasPrimaryKey = hasPrimaryKeyFunction
        End Sub
        ''' <summary>
        ''' The function that returns the primary key value of a given entity.
        ''' </summary>
        Public ReadOnly Property GetPrimaryKey As Func(Of TEntity, TPrimaryKey)
            Get
                Return _GetPrimaryKey
            End Get
        End Property
        ''' <summary>
        ''' The action that assigns the primary key value to a given entity.
        ''' </summary>
        Public ReadOnly Property SetPrimaryKey As Action(Of TEntity, TPrimaryKey)
            Get
                Return _SetPrimaryKey
            End Get
        End Property
        ''' <summary>
        ''' A function that determines whether the given entity has a primary key assigned (the primary key is not null). Always returns true if the primary key is a non-nullable value type.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property HasPrimaryKey As Func(Of TEntity, Boolean)
            Get
                Return _HasPrimaryKey
            End Get
        End Property
    End Class
End Namespace
