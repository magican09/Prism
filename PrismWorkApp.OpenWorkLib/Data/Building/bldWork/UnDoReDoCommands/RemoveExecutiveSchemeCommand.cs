using PrismWorkApp.OpenWorkLib.Data.Service;
using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class RemoveExecutiveSchemeCommand : IUnDoRedoCommand
    {
        private bldWork _CurrentWork;
        private bldExecutiveScheme _RemovedReport;

        public string Name { get; set; } = "Удален документ из работы";

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter = null)
        {
            _CurrentWork.ExecutiveSchemes.Remove(_RemovedReport);
            _CurrentWork.AOSRDocument.AttachedDocuments.Remove(_RemovedReport);

        }
        public void UnExecute()
        {
            _CurrentWork.ExecutiveSchemes.Add(_RemovedReport);
            _CurrentWork.AOSRDocument.AttachedDocuments.Add(_RemovedReport);

        }
        public RemoveExecutiveSchemeCommand(bldWork work, bldExecutiveScheme scheme)
        {
            _CurrentWork = work;
            _RemovedReport = scheme;

            _CurrentWork.ExecutiveSchemes.Remove(_RemovedReport);
            _CurrentWork.AOSRDocument.AttachedDocuments.Remove(_RemovedReport);


        }
    }
}
