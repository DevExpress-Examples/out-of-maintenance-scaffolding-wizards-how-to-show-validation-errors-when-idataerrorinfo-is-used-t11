using DevExpress.Mvvm.DataAnnotations;
using Scaffolding.ValidationErrors.Localization;
using Scaffolding.ValidationErrors.Model;

namespace Scaffolding.ValidationErrors.DepartmentContextDataModel {

    public class DepartmentContextMetadataProvider {
        public static void BuildMetadata(MetadataBuilder<Department> builder) {
            builder.DisplayName(DepartmentContextResources.Department);
            builder.Property(x => x.DepartmentID).DisplayName(DepartmentContextResources.Department_DepartmentID);
            builder.Property(x => x.Name).DisplayName(DepartmentContextResources.Department_Name);
            builder.Property(x => x.Budget).DisplayName(DepartmentContextResources.Department_Budget);
            builder.Property(x => x.StartDate).DisplayName(DepartmentContextResources.Department_StartDate);
        }
    }
}