using PrismWorkApp.OpenWorkLib.Data.Service;
using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class AddDocumentCommand<TEntity> : UnDoRedoCommandBase, IUnDoRedoCommand
        where TEntity : IbldDocument
    {
        private bldDocument _CurrentDoc;
        private TEntity _AddedDoc;

        public string Name { get; set; } = "Добавлен новый документ";

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter = null)
        {
            if (!_CurrentDoc.AttachedDocuments.Contains(_AddedDoc as bldDocument))
                _CurrentDoc.AttachedDocuments.Add(_AddedDoc as bldDocument);
        }

        public void UnExecute()
        {
            if (_CurrentDoc.AttachedDocuments.Contains(_AddedDoc as bldDocument))
                _CurrentDoc.AttachedDocuments.Remove(_AddedDoc as bldDocument);
        }
        public AddDocumentCommand(bldDocument current_doc, TEntity new_doc)
        {
            _CurrentDoc = current_doc;
            _AddedDoc = new_doc;

            if (!_CurrentDoc.AttachedDocuments.Contains(_AddedDoc as bldDocument))
                _CurrentDoc.AttachedDocuments.Add(_AddedDoc as bldDocument);


        }
    }
}
