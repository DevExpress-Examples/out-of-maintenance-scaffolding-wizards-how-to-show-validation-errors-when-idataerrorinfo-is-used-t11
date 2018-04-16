using System;
using System.Linq;
using System.Collections.ObjectModel;

namespace Scaffolding.ValidationErrors.Common.DataModel {
    public interface IReadOnlyRepository<TEntity> where TEntity : class {
        IQueryable<TEntity> GetEntities();
        IUnitOfWork UnitOfWork { get; }
        ObservableCollection<TEntity> Local { get; }
    }
}
