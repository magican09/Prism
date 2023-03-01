using PrismWorkApp.OpenWorkLib.Data.Service;
using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class RemoveNextWorkCommand : IUnDoRedoCommand
    {
        private bldWork _CurrentWork;
        private bldWork _RemovedNextWork;
        private bldConstruction _CurrentWorkConstruction;
        private bldConstruction _RemovedNextWorkConstruction;

        public string Name { get; set; }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter = null)
        {
            _CurrentWork.bldConstruction = _RemovedNextWorkConstruction;
            _RemovedNextWork.bldConstruction = _CurrentWorkConstruction;
            _RemovedNextWork.PreviousWorks.Remove(_CurrentWork);
            _CurrentWork.NextWorks.Remove(_RemovedNextWork);
        }

        public void UnExecute()
        {
            _CurrentWork.bldConstruction = _CurrentWorkConstruction;
            _RemovedNextWork.bldConstruction = _RemovedNextWorkConstruction;
            _RemovedNextWork.PreviousWorks.Add(_CurrentWork);
            _CurrentWork.NextWorks.Add(_RemovedNextWork);
        }
        public RemoveNextWorkCommand(bldWork work, bldWork next_work)
        {
            _CurrentWork = work;
            _RemovedNextWork = next_work;
            _CurrentWorkConstruction = _CurrentWork.bldConstruction;
            _RemovedNextWorkConstruction = _RemovedNextWork.bldConstruction;

            _CurrentWork.bldConstruction = _RemovedNextWorkConstruction;
            _RemovedNextWork.bldConstruction = _CurrentWorkConstruction;

            _RemovedNextWork.PreviousWorks.Remove(_CurrentWork);
            _CurrentWork.NextWorks.Remove(_RemovedNextWork);

        }
    }
}
