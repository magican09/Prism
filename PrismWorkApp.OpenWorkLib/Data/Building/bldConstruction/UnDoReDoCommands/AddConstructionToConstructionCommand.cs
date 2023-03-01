using PrismWorkApp.OpenWorkLib.Data.Service;
using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class AddConstructionToConstructionCommand : IUnDoRedoCommand
    {
        private bldConstruction _CurrentConstruction;
        private bldConstruction _AddedConstruction;

        private bldObject _AddConstructionLastParentObject;
        private bldConstruction _AddConstructionParentConstruction;

        public string Name { get; set; }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {

            throw new NotImplementedException();
        }

        public void Execute(object parameter = null)
        {

            _AddConstructionLastParentObject = _AddedConstruction.bldObject;
            _AddConstructionParentConstruction = _AddedConstruction.ParentConstruction;

            _AddedConstruction?.bldObject?.Constructions?.Remove(_AddedConstruction);
            _AddedConstruction?.ParentConstruction?.Constructions?.Remove(_AddedConstruction);
            _CurrentConstruction.Constructions.Add(_AddedConstruction);
        }

        public void UnExecute()
        {
            _AddedConstruction.bldObject = _AddConstructionLastParentObject;
            _AddedConstruction.ParentConstruction = _AddConstructionParentConstruction;

            _AddedConstruction?.bldObject?.Constructions?.Add(_AddedConstruction);
            _AddedConstruction?.ParentConstruction?.Constructions?.Add(_AddedConstruction);
            _CurrentConstruction.Constructions.Remove(_AddedConstruction);
        }
        public AddConstructionToConstructionCommand(bldConstruction construction, bldConstruction add_construction)
        {
            _CurrentConstruction = construction;
            _AddedConstruction = add_construction;

            _AddConstructionLastParentObject = _AddedConstruction.bldObject;
            _AddConstructionParentConstruction = _AddedConstruction.ParentConstruction;

            _AddedConstruction?.bldObject?.Constructions?.Remove(_AddedConstruction);
            _AddedConstruction?.ParentConstruction?.Constructions?.Remove(_AddedConstruction);
            _CurrentConstruction.Constructions.Add(_AddedConstruction);

        }
    }
}
