using PrismWorkApp.OpenWorkLib.Data.Service;
using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class RemoveMaterialCommand : IUnDoRedoCommand
    {
        private bldWork _CurrentWork;
        private bldMaterial _RemovedMaterial;

        public string Name { get; set; } = "Удален материал из работе";

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter = null)
        {
            _CurrentWork.Materials.Remove(_RemovedMaterial);
            foreach (bldDocument document in _RemovedMaterial.Documents)
                _CurrentWork.AOSRDocument.AttachedDocuments.Remove(document);
        }

        public void UnExecute()
        {
            _CurrentWork.Materials.Add(_RemovedMaterial);
            foreach (bldDocument document in _RemovedMaterial.Documents)
                _CurrentWork.AOSRDocument.AttachedDocuments.Add(document);
        }
        public RemoveMaterialCommand(bldWork work, bldMaterial material)
        {
            _CurrentWork = work;
            _RemovedMaterial = material;
            _CurrentWork.Materials.Remove(_RemovedMaterial);
            foreach (bldDocument document in _RemovedMaterial.Documents)
                _CurrentWork.AOSRDocument.AttachedDocuments.Remove(document);
        }
    }
}
