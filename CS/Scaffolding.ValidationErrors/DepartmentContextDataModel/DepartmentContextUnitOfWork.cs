using DevExpress.Mvvm.DataModel;
using DevExpress.Mvvm.DataModel.EF6;
using Scaffolding.ValidationErrors.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scaffolding.ValidationErrors.DepartmentContextDataModel {

    /// <summary>
    /// A DepartmentContextUnitOfWork instance that represents the run-time implementation of the IDepartmentContextUnitOfWork interface.
    /// </summary>
    public class DepartmentContextUnitOfWork : DbUnitOfWork<DepartmentContext>, IDepartmentContextUnitOfWork {

        public DepartmentContextUnitOfWork(Func<DepartmentContext> contextFactory)
            : base(contextFactory) {
        }

        IRepository<Department, int> IDepartmentContextUnitOfWork.Departments
        {
            get { return GetRepository(x => x.Set<Department>(), (Department x) => x.DepartmentID); }
        }
    }
}
