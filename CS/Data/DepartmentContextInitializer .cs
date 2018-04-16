using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Scaffolding.ValidationErrors.Model {
    public class DepartmentContextInitializer : DropCreateDatabaseIfModelChanges<DepartmentContext> {
        protected override void Seed(DepartmentContext context) {
            base.Seed(context);

            context.Departments.Add(new Department { Budget = 138, Name = "Music", StartDate = new DateTime(1988, 9, 14) });
            context.Departments.Add(new Department { Budget = 572, Name = "Journalism", StartDate = new DateTime(1994, 5, 23) });
            context.Departments.Add(new Department { Budget = 349, Name = "Management", StartDate = new DateTime(1994, 3, 7) });

            context.SaveChanges();
        }
    }
}