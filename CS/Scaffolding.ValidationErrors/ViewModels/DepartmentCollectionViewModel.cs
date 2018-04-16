using System;
using System.Linq;
using Scaffolding.ValidationErrors.Common.Utils;
using Scaffolding.ValidationErrors.DepartmentContextDataModel;
using Scaffolding.ValidationErrors.Common.DataModel;
using Scaffolding.ValidationErrors.Model;
using Scaffolding.ValidationErrors.Common.ViewModel;

namespace Scaffolding.ValidationErrors.ViewModels {
    public partial class DepartmentCollectionViewModel : CollectionViewModel<Department, int, IDepartmentContextUnitOfWork> {
        public DepartmentCollectionViewModel()
            : this(UnitOfWorkSource.GetUnitOfWorkFactory()) {
        }
        public DepartmentCollectionViewModel(IUnitOfWorkFactory<IDepartmentContextUnitOfWork> unitOfWorkFactory)
            : base(unitOfWorkFactory, x => x.Departments) {
        }
    }
}