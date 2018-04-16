Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports System.Collections.Generic
Imports System.ComponentModel
Imports Scaffolding.ValidationErrors.Common.DataModel

Namespace Scaffolding.ValidationErrors.Common.ViewModel
    Partial Public Class SingleObjectViewModel(Of TEntity As Class, TPrimaryKey, TUnitOfWork As IUnitOfWork)
        Inherits SingleObjectViewModelBase(Of TEntity, TPrimaryKey, TUnitOfWork)
        Protected Overrides Function HasValidationErrors() As Boolean
            Dim dataErrorInfo As IDataErrorInfo = TryCast(Entity, IDataErrorInfo)
            If dataErrorInfo Is Nothing Then
                Return MyBase.HasValidationErrors()
            End If
            Return TypeDescriptor.GetProperties(GetType(TEntity)).Cast(Of PropertyDescriptor)().Where(Function(n) (Not String.IsNullOrEmpty(dataErrorInfo(n.Name)))).Any()
        End Function
    End Class
End Namespace