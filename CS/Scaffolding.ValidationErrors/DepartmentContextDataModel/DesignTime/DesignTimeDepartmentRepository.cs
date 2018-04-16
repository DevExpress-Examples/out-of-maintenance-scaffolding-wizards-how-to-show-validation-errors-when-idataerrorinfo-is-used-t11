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
    public class DesignTimeDepartmentRepository : DesignTimeRepository<Department, int>, IDepartmentRepository {
        public DesignTimeDepartmentRepository()
            : base(x => x.DepartmentID) {
        }
    }
}