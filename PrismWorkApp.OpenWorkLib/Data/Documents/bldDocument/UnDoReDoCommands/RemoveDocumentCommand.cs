using PrismWorkApp.OpenWorkLib.Data.Service;
using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class RemoveDocumentCommand: UnDoRedoCommandBase, IUnDoRedoCommand
   {
        private bldDocument _CurrentDoc;
        private bldDocument _RemovedDoc;

        public string Name { get; set; } = "Добавлен новый документ";

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter = null)
        {

            if (_CurrentDoc.AttachedDocuments.Contains(_RemovedDoc))
                _CurrentDoc.AttachedDocuments.Remove(_RemovedDoc);
        }

        public void UnExecute()
        {
           if (!_CurrentDoc.AttachedDocuments.Contains(_RemovedDoc))
                    _CurrentDoc.AttachedDocuments.Add(_RemovedDoc);
        }
        public RemoveDocumentCommand(bldDocument current_doc,IbldDocument removed_doc)
        {
            _CurrentDoc = current_doc;
            _RemovedDoc = removed_doc as bldDocument;

            if(_CurrentDoc.AttachedDocuments.Contains(_RemovedDoc)) 
                _CurrentDoc.AttachedDocuments.Remove(_RemovedDoc);


        }
    }
}
