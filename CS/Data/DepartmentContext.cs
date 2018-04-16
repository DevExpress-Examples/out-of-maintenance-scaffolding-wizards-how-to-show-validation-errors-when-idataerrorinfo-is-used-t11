using System.Data.Entity;

namespace Scaffolding.ValidationErrors.Model {
    public class DepartmentContext : DbContext {
        public DbSet<Department> Departments { get; set; }
    }
}
