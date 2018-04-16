Imports System.Linq
Imports System.Collections.Generic
Imports System.ComponentModel
Imports Common.DataModel

Namespace Common.ViewModel
    Partial Public MustInherit Class SingleObjectViewModel(Of TEntity As Class, TPrimaryKey, TUnitOfWork As IUnitOfWork)
        Protected Overrides Function HasValidationErrors() As Boolean
            Dim dataErrorInfo As IDataErrorInfo = TryCast(Entity, IDataErrorInfo)
            If dataErrorInfo Is Nothing Then
                Return MyBase.HasValidationErrors()
            End If
            Return TypeDescriptor.GetProperties(GetType(TEntity)).Cast(Of PropertyDescriptor)().Where(Function(n) Not String.IsNullOrEmpty(dataErrorInfo(n.Name))).Any()
        End Function
    End Class
End Namespace