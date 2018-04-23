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
using DevExpress.Mvvm;

namespace Scaffolding.ValidationErrors.DepartmentContextDataModel {
    /// <summary>
    /// Provides methods to obtain the relevant IUnitOfWorkFactory.
    /// </summary>
    public static class UnitOfWorkSource {

        #region inner classes
        class DbUnitOfWorkFactory : IUnitOfWorkFactory<IDepartmentContextUnitOfWork> {
            public static readonly IUnitOfWorkFactory<IDepartmentContextUnitOfWork> Instance = new DbUnitOfWorkFactory();
            DbUnitOfWorkFactory() { }
            IDepartmentContextUnitOfWork IUnitOfWorkFactory<IDepartmentContextUnitOfWork>.CreateUnitOfWork() {
                return new DepartmentContextUnitOfWork(() => new DepartmentContext());
            }
        }

        class DesignUnitOfWorkFactory : IUnitOfWorkFactory<IDepartmentContextUnitOfWork> {
            public static readonly IUnitOfWorkFactory<IDepartmentContextUnitOfWork> Instance = new DesignUnitOfWorkFactory();
            DesignUnitOfWorkFactory() { }
            IDepartmentContextUnitOfWork IUnitOfWorkFactory<IDepartmentContextUnitOfWork>.CreateUnitOfWork() {
                return new DepartmentContextDesignTimeUnitOfWork();
            }
        }
        #endregion

        /// <summary>
        /// Returns the IUnitOfWorkFactory implementation based on the current mode (run-time or design-time).
        /// </summary>
        public static IUnitOfWorkFactory<IDepartmentContextUnitOfWork> GetUnitOfWorkFactory() {
            return GetUnitOfWorkFactory(ViewModelBase.IsInDesignMode);
        }

        /// <summary>
        /// Returns the IUnitOfWorkFactory implementation based on the given mode (run-time or design-time).
        /// </summary>
        /// <param name="isInDesignTime">Used to determine which implementation of IUnitOfWorkFactory should be returned.</param>
        public static IUnitOfWorkFactory<IDepartmentContextUnitOfWork> GetUnitOfWorkFactory(bool isInDesignTime) {
            return isInDesignTime ? DesignUnitOfWorkFactory.Instance : DbUnitOfWorkFactory.Instance;
        }
    }
}