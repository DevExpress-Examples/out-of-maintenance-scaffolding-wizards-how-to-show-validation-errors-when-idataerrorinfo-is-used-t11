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
    public class DepartmentContextUnitOfWork : DbUnitOfWork<DepartmentContext>, IDepartmentContextUnitOfWork {
        Lazy<IDepartmentRepository> departmentsRepository;

        public DepartmentContextUnitOfWork(DepartmentContext context)
            : base(context) {
            departmentsRepository = new Lazy<IDepartmentRepository>(() => new DepartmentRepository(this));
        }
        bool IDepartmentContextUnitOfWork.HasChanges() {
            return Context.ChangeTracker.HasChanges();
        }
        IDepartmentRepository IDepartmentContextUnitOfWork.Departments {
            get { return departmentsRepository.Value; }
        }
    }
}
