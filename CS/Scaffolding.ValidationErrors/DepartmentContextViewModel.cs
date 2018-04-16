using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Mvvm;

namespace Scaffolding.ValidationErrors {
    public class DepartmentContextViewModel {
        protected virtual IDocumentManagerService DocumentManagerService { get { return null; } }

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