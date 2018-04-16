using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;

namespace Scaffolding.ValidationErrors {
    /// <summary>
    /// Represents the root POCO view model for the DepartmentContext data model.
    /// </summary>
    public class DepartmentContextViewModel {

        /// <summary>
        /// Creates a new instance of DepartmentContextViewModel as a POCO view model.
        /// </summary>
        public static DepartmentContextViewModel Create() {
            return ViewModelSource.Create(() => new DepartmentContextViewModel());
        }

        /// <summary>
        /// Initializes a new instance of the DepartmentContextViewModel class.
        /// This constructor is declared protected to avoid undesired instantiation of the DepartmentContextViewModel type without the POCO proxy factory.
        /// </summary>
        protected DepartmentContextViewModel() {
        }

        protected virtual IDocumentManagerService DocumentManagerService { get { return null; } }

        /// <summary>
        /// Creates and shows a document containing a collection view model.
        /// Since DepartmentContextViewModel is a POCO view model, an instance of this class will also expose the ShowDocumentCommand property that can be used as a binding source in views.
        /// </summary>
        /// <param name="p">Contains the document view name and title separated with ';'.</param>
        public void ShowDocument(string p) {
            string[] parameters = p.Split(';');
            ShowDocumentCore(parameters[0], parameters[1]);
        }

        void ShowDocumentCore(string viewName, string title) {
            IDocument document = DocumentManagerService.FindDocumentByIdOrCreate(viewName, x => CreateDocument(viewName, title));
            document.Show();
        }

        IDocument CreateDocument(string viewName, string title) {
            var document = DocumentManagerService.CreateDocument(viewName, null, this);
            document.Title = title;
            document.DestroyOnClose = false;
            return document;
        }
    }
}