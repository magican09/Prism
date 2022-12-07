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

        public string Name { get; set; } = "Добавлен материал к работе";

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter = null)
        {
            _CurrentWork.Materials.Add(_AddedMaterial);
            foreach(bldDocument document in _AddedMaterial.Documents)
                _CurrentWork.AOSRDocument.AttachedDocuments.Add(document);
        }

        public void UnExecute()
        {
            _CurrentWork.Materials.Remove(_AddedMaterial);
            foreach(bldDocument document in _AddedMaterial.Documents)
                _CurrentWork.AOSRDocument.AttachedDocuments.Remove(document);
        }
        public AddLaboratoryReportCommand(bldWork work, bldLaboratoryReport report)
        {
            _CurrentWork = work;
            _AddedReport = report;

            _CurrentWork

        }
    }
}
