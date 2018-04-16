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
    public static class UnitOfWorkSource {
        #region inner classes
        class DbUnitOfWorkFactory : IUnitOfWorkFactory<IDepartmentContextUnitOfWork> {
            public static readonly IUnitOfWorkFactory<IDepartmentContextUnitOfWork> Instance = new DbUnitOfWorkFactory();
            DbUnitOfWorkFactory() { }
            IDepartmentContextUnitOfWork IUnitOfWorkFactory<IDepartmentContextUnitOfWork>.CreateUnitOfWork() {
                return new DepartmentContextUnitOfWork(new DepartmentContext());
            }
        }
        class DesignUnitOfWorkFactory : IUnitOfWorkFactory<IDepartmentContextUnitOfWork> {
            public static readonly IUnitOfWorkFactory<IDepartmentContextUnitOfWork> Instance = new DesignUnitOfWorkFactory();

            readonly IDepartmentContextUnitOfWork UnitOfWork = new DepartmentContextDesignTimeUnitOfWork();
            DesignUnitOfWorkFactory() { }
            IDepartmentContextUnitOfWork IUnitOfWorkFactory<IDepartmentContextUnitOfWork>.CreateUnitOfWork() {
                return UnitOfWork;
            }
        }
        #endregion
        public static IUnitOfWorkFactory<IDepartmentContextUnitOfWork> GetUnitOfWorkFactory() {
            return GetUnitOfWorkFactory(ViewModelBase.IsInDesignMode);
        }
        public static IUnitOfWorkFactory<IDepartmentContextUnitOfWork> GetUnitOfWorkFactory(bool isInDesignTime) {
            return isInDesignTime ? DesignUnitOfWorkFactory.Instance : DbUnitOfWorkFactory.Instance;
        }
        public static IDepartmentContextUnitOfWork CreateUnitOfWork(bool isInDesignTime = false) {
            return GetUnitOfWorkFactory(isInDesignTime).CreateUnitOfWork();
        }
    }
}