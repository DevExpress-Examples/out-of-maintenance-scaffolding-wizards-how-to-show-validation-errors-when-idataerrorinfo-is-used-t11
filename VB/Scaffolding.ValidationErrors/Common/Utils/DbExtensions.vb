Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Linq

Namespace Scaffolding.ValidationErrors.Common.Utils
	Public Module DbExtensions
        <System.Runtime.CompilerServices.Extension> _
        Public Sub Load(ByVal source As IQueryable)
            Dim enumerator As IEnumerator = source.GetEnumerator()
            Do While enumerator.MoveNext()
            Loop
        End Sub
	End Module
End Namespace
