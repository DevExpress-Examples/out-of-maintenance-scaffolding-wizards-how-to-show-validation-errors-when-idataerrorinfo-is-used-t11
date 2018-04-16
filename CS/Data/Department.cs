using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Scaffolding.ValidationErrors.Model {
    public class Department : IDataErrorInfo {
        public int DepartmentID { get; set; }
        public string Name { get; set; }
        public decimal Budget { get; set; }
        public DateTime StartDate { get; set; }

        public string Error {
            get {
                return this["Name"] != null || this["Budget"] != null ? "Correct values" : null;
            }
        }

        public string this[string columnName] {
            get {
                switch(columnName) {
                    case "Name":
                        return string.IsNullOrEmpty(Name) ? "Name cannot be null" : null;
                    case "Budget":
                        return Budget < 0 ? "Budget cannot be negative" : null;
                    default:
                        return null;
                }
            }
        }
    }
}