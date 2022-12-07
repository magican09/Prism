using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data 
{
    public class AddLaboratoryReportCommand : IUnDoRedoCommand
    {
        private bldWork _CurrentWork;
        private bldLaboratoryReport _AddedReport;

        public string Name { get; set; } = "Добавлен докумет к работе";

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter = null)
        {
            _CurrentWork.LaboratoryReports.Add(_AddedReport);
            _CurrentWork.AOSRDocument.AttachedDocuments.Add(_AddedReport);
        }

        public void UnExecute()
        {
            _CurrentWork.LaboratoryReports.Remove(_AddedReport);
            _CurrentWork.AOSRDocument.AttachedDocuments.Remove(_AddedReport);
        }
        public AddLaboratoryReportCommand(bldWork work, bldLaboratoryReport report)
        {
            _CurrentWork = work;
            _AddedReport = report;

            _CurrentWork.LaboratoryReports.Add(_AddedReport);
            _CurrentWork.AOSRDocument.AttachedDocuments.Add(_AddedReport);


        }
    }
}
