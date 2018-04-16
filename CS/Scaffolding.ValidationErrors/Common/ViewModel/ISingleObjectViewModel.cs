using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.DataAnnotations;
using Scaffolding.ValidationErrors.Common.Utils;
using Scaffolding.ValidationErrors.Common.DataModel;
using MessageBoxButton = System.Windows.MessageBoxButton;
using MessageBoxImage = System.Windows.MessageBoxImage;
using MessageBoxResult = System.Windows.MessageBoxResult;

namespace Scaffolding.ValidationErrors.Common.ViewModel {
    public interface ISingleObjectViewModel<TEntity, TPrimaryKey> {
        TEntity Entity { get; }
        TPrimaryKey PrimaryKey { get; }
    }
    public interface IDetailEntityInfo { }
    public class DetailEntityInfo<TDetailEntity> : IDetailEntityInfo
        where TDetailEntity : class {
        public object DetailEntityKey { get; private set; }
        public DetailEntityInfo(object detailEntityKey) {
            this.DetailEntityKey = detailEntityKey;
        }
    }
}