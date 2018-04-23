using System;
using System.Linq;

namespace Scaffolding.ValidationErrors.Common.ViewModel {
    public abstract partial class SingleObjectViewModel<TEntity, TPrimaryKey, TUnitOfWork> {
        protected override bool TryClose() {
            var res = base.TryClose();
            if (Entity != null && IsNew() && res)
                Entity = null;
            return res;
        }
    }
}