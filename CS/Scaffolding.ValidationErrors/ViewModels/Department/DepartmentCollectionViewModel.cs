using System;
using System.Linq;
using DevExpress.Mvvm.POCO;
using Scaffolding.ValidationErrors.Common.Utils;
using Scaffolding.ValidationErrors.DepartmentContextDataModel;
using Scaffolding.ValidationErrors.Common.DataModel;
using Scaffolding.ValidationErrors.Model;
using Scaffolding.ValidationErrors.Common.ViewModel;

namespace Scaffolding.ValidationErrors.ViewModels {
    /// <summary>
    /// Represents the Departments collection view model.
    /// </summary>
    public partial class DepartmentCollectionViewModel : CollectionViewModel<Department, int, IDepartmentContextUnitOfWork> {

        /// <summary>
        /// Creates a new instance of DepartmentCollectionViewModel as a POCO view model.
        /// </summary>
        /// <param name="unitOfWorkFactory">A factory used to create a unit of work instance.</param>
        public static DepartmentCollectionViewModel Create(IUnitOfWorkFactory<IDepartmentContextUnitOfWork> unitOfWorkFactory = null) {
            return ViewModelSource.Create(() => new DepartmentCollectionViewModel(unitOfWorkFactory));
        }

        /// <summary>
        /// Initializes a new instance of the DepartmentCollectionViewModel class.
        /// This constructor is declared protected to avoid undesired instantiation of the DepartmentCollectionViewModel type without the POCO proxy factory.
        /// </summary>
        /// <param name="unitOfWorkFactory">A factory used to create a unit of work instance.</param>
        protected DepartmentCollectionViewModel(IUnitOfWorkFactory<IDepartmentContextUnitOfWork> unitOfWorkFactory = null)
            : base(unitOfWorkFactory ?? UnitOfWorkSource.GetUnitOfWorkFactory(), x => x.Departments) {
        }
    }
}