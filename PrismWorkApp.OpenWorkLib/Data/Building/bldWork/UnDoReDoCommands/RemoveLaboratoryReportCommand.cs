using PrismWorkApp.OpenWorkLib.Data.Service;
using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class RemoveLaboratoryReportCommand : IUnDoRedoCommand
    {
        private bldWork _CurrentWork;
        private bldLaboratoryReport _RemovedReport;

        public string Name { get; set; } = "Удален документ из работы";

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter = null)
        {
            _CurrentWork.LaboratoryReports.Remove(_RemovedReport);
            _CurrentWork.AOSRDocument.AttachedDocuments.Remove(_RemovedReport);

        }
        public void UnExecute()
        {
            _CurrentWork.LaboratoryReports.Add(_RemovedReport);
            _CurrentWork.AOSRDocument.AttachedDocuments.Add(_RemovedReport);

        }
        public RemoveLaboratoryReportCommand(bldWork work, bldLaboratoryReport report)
        {
            _CurrentWork = work;
            _RemovedReport = report;

            _CurrentWork.LaboratoryReports.Remove(_RemovedReport);
            _CurrentWork.AOSRDocument.AttachedDocuments.Remove(_RemovedReport);


        }
    }
}
