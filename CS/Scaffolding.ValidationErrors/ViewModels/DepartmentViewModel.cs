using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using Scaffolding.ValidationErrors.Common.Utils;
using Scaffolding.ValidationErrors.DepartmentContextDataModel;
using Scaffolding.ValidationErrors.Common.DataModel;
using Scaffolding.ValidationErrors.Model;
using Scaffolding.ValidationErrors.Common.ViewModel;

namespace Scaffolding.ValidationErrors.ViewModels {
    public partial class DepartmentViewModel : SingleObjectViewModel<Department, int, IDepartmentContextUnitOfWork> {
        public DepartmentViewModel()
            : this(UnitOfWorkSource.GetUnitOfWorkFactory()) {
        }
        public DepartmentViewModel(IUnitOfWorkFactory<IDepartmentContextUnitOfWork> unitOfWorkFactory)
            : base(unitOfWorkFactory, x => x.Departments) {
        }
    }
}
