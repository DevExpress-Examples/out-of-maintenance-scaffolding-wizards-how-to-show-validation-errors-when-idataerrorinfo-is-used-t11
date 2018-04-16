using System;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Collections.Generic;
using Scaffolding.ValidationErrors.Common.Utils;
using Scaffolding.ValidationErrors.Model;
using Scaffolding.ValidationErrors.Common.DataModel;
using Scaffolding.ValidationErrors.Common.DataModel.EntityFramework;

namespace Scaffolding.ValidationErrors.DepartmentContextDataModel {
    public class DepartmentRepository : DbRepository<Department, int, DepartmentContext>, IDepartmentRepository {
        public DepartmentRepository(DbUnitOfWork<DepartmentContext> unitOfWork)
            : base(unitOfWork, context => context.Set<Department>(), x => x.DepartmentID) {
        }
    }
}