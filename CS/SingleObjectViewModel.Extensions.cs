using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using Scaffolding.ValidationErrors.Common.DataModel;

namespace Scaffolding.ValidationErrors.Common.ViewModel {
    partial class SingleObjectViewModel<TEntity, TPrimaryKey, TUnitOfWork> {

        protected override bool HasValidationErrors() {
            IDataErrorInfo dataErrorInfo = Entity as IDataErrorInfo;
            if(dataErrorInfo == null)
                return base.HasValidationErrors();
            return TypeDescriptor.GetProperties(typeof(TEntity))
                .Cast<PropertyDescriptor>()
                .Where(n => !string.IsNullOrEmpty(dataErrorInfo[n.Name]))
                .Any();
        }
    }
}