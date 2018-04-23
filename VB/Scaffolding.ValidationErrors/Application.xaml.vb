Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Linq
Imports System.Windows
Imports DevExpress.Xpf.Core
Imports System.Data.Entity
Imports Scaffolding.ValidationErrors.Model
Imports DevExpress.Internal

Namespace Scaffolding.ValidationErrors
	''' <summary>
	''' Interaction logic for App.xaml
	''' </summary>
	Partial Public Class App
		Inherits Application

		Protected Overrides Sub OnStartup(ByVal e As StartupEventArgs)
			MyBase.OnStartup(e)
			ApplicationThemeHelper.UpdateApplicationThemeName()
			DbEngineDetector.PatchConnectionStringsAndConfigureEntityFrameworkDefaultConnectionFactory()
			Database.SetInitializer(Of DepartmentContext)(New DepartmentContextInitializer())
		End Sub
		Protected Overrides Sub OnExit(ByVal e As ExitEventArgs)
			ApplicationThemeHelper.SaveApplicationThemeName()
		End Sub
	End Class
End Namespace
