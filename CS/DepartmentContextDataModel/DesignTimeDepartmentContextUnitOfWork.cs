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
    /// A DepartmentContextDesignTimeUnitOfWork instance that represents the design-time implementation of the IDepartmentContextUnitOfWork interface.
    /// </summary>
    public class DepartmentContextDesignTimeUnitOfWork : DesignTimeUnitOfWork, IDepartmentContextUnitOfWork {

        /// <summary>
        /// Initializes a new instance of the DepartmentContextDesignTimeUnitOfWork class.
        /// </summary>
        public DepartmentContextDesignTimeUnitOfWork() {
        }

        IRepository<Department, int> IDepartmentContextUnitOfWork.Departments {
            get { return GetRepository((Department x) => x.DepartmentID); }
        }
    }
}
