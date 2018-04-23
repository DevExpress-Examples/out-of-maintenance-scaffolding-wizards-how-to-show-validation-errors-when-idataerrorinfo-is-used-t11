using DevExpress.Mvvm.DataModel;
using DevExpress.Mvvm.DataModel.DesignTime;
using Scaffolding.ValidationErrors.Model;
using System;
using System.Collections.Generic;
using System.Linq;

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

        IRepository<Department, int> IDepartmentContextUnitOfWork.Departments
        {
            get { return GetRepository((Department x) => x.DepartmentID); }
        }
    }
}
