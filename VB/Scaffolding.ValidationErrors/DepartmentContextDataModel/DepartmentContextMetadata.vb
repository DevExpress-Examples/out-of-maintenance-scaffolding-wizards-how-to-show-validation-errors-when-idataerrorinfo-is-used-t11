Imports DevExpress.Mvvm.DataAnnotations
Imports Scaffolding.ValidationErrors.Localization
Imports Scaffolding.ValidationErrors.Model

Namespace Scaffolding.ValidationErrors.DepartmentContextDataModel

	Public Class DepartmentContextMetadataProvider
		Public Shared Sub BuildMetadata(ByVal builder As MetadataBuilder(Of Department))
			builder.DisplayName(DepartmentContextResources.Department)
			builder.Property(Function(x) x.DepartmentID).DisplayName(DepartmentContextResources.Department_DepartmentID)
			builder.Property(Function(x) x.Name).DisplayName(DepartmentContextResources.Department_Name)
			builder.Property(Function(x) x.Budget).DisplayName(DepartmentContextResources.Department_Budget)
			builder.Property(Function(x) x.StartDate).DisplayName(DepartmentContextResources.Department_StartDate)
		End Sub
	End Class
End Namespace