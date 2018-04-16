using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace Scaffolding.ValidationErrors.Common.DataModel.EntityFramework {
    public static class DbExceptionsConverter {
        public static DbException Convert(DbUpdateException exception) {
            Exception originalException = exception;
            while(originalException.InnerException != null) {
                originalException = originalException.InnerException;
            }
            return new DbException(originalException.Message, CommonResources.Exception_UpdateErrorCaption, exception);
        }
        public static DbException Convert(DbEntityValidationException exception) {
            StringBuilder stringBuilder = new StringBuilder();
            foreach(DbEntityValidationResult validationResult in exception.EntityValidationErrors) {
                foreach(DbValidationError error in validationResult.ValidationErrors) {
                    if(stringBuilder.Length > 0)
                        stringBuilder.AppendLine();
                    stringBuilder.Append(error.PropertyName + ": " + error.ErrorMessage);
                }
            }
            return new DbException(stringBuilder.ToString(), CommonResources.Exception_ValidationErrorCaption, exception);
        }
    }
}