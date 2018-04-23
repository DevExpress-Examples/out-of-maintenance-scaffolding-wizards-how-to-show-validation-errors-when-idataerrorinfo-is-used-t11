using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using DevExpress.Xpf.Core;
using System.Data.Entity;
using Scaffolding.ValidationErrors.Model;

namespace Scaffolding.ValidationErrors {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            Database.SetInitializer<DepartmentContext>(new DepartmentContextInitializer());
            base.OnStartup(e);
        }
        private void OnAppStartup_UpdateThemeName(object sender, StartupEventArgs e) {
            DevExpress.Xpf.Core.ApplicationThemeHelper.UpdateApplicationThemeName();
        }
    }
}
