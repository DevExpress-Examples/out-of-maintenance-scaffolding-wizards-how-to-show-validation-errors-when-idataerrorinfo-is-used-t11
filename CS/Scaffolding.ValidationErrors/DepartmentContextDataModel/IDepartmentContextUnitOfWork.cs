using DevExpress.Mvvm.DataModel;
using Scaffolding.ValidationErrors.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scaffolding.ValidationErrors.DepartmentContextDataModel {

    /// <summary>
    /// IDepartmentContextUnitOfWork extends the IUnitOfWork interface with repositories representing specific entities.
    /// </summary>
    public interface IDepartmentContextUnitOfWork : IUnitOfWork {

        /// <summary>
        /// The Department entities repository.
        /// </summary>
        IRepository<Department, int> Departments { get; }
    }
}
