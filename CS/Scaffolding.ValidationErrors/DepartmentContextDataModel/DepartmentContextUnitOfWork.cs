using System;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Collections.Generic;
using Scaffolding.ValidationErrors.Common.Utils;
using Scaffolding.ValidationErrors.Common.DataModel;
using Scaffolding.ValidationErrors.Common.DataModel.EntityFramework;
using Scaffolding.ValidationErrors.Model;

namespace Scaffolding.ValidationErrors.DepartmentContextDataModel {
    /// <summary>
    /// A DepartmentContextUnitOfWork instance that represents the run-time implementation of the IDepartmentContextUnitOfWork interface.
    /// </summary>
    public class DepartmentContextUnitOfWork : DbUnitOfWork<DepartmentContext>, IDepartmentContextUnitOfWork {

        public DepartmentContextUnitOfWork(Func<DepartmentContext> contextFactory)
            : base(contextFactory) {
        }

        IRepository<Department, int> IDepartmentContextUnitOfWork.Departments {
            get { return GetRepository(x => x.Set<Department>(), x => x.DepartmentID); }
        }
    }
}
