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
    public class DepartmentContextDesignTimeUnitOfWork : DesignTimeUnitOfWork, IDepartmentContextUnitOfWork {
        static DesignTimeDepartmentRepository departmentsRepository = new DesignTimeDepartmentRepository();

        public DepartmentContextDesignTimeUnitOfWork() {
        }

        bool IDepartmentContextUnitOfWork.HasChanges() {
            return false;
        }
        IDepartmentRepository IDepartmentContextUnitOfWork.Departments {
            get { return departmentsRepository; }
        }
        public void Detach(object entity) {
        }
    }
}
