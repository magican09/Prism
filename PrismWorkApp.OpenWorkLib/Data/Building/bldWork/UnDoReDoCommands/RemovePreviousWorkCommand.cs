using PrismWorkApp.OpenWorkLib.Data.Service;
using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class RemovePreviousWorkCommand : IUnDoRedoCommand
    {
        private bldWork _CurrentWork;
        private bldWork _RemovedPreviosWork;
        private bldConstruction _CurrentWorkConstruction;
        private bldConstruction _RemovedPreviosWorkConstruction;
        public string Name { get; set; }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter = null)
        {
            _CurrentWork.bldConstruction = _RemovedPreviosWorkConstruction;
            _RemovedPreviosWork.bldConstruction = _CurrentWorkConstruction;
            _RemovedPreviosWork.NextWorks.Remove(_CurrentWork);
            _CurrentWork.PreviousWorks.Remove(_RemovedPreviosWork);
        }

        public void UnExecute()
        {
            _CurrentWork.bldConstruction = _CurrentWorkConstruction;
            _RemovedPreviosWork.bldConstruction = _RemovedPreviosWorkConstruction;
            _RemovedPreviosWork.NextWorks.Add(_CurrentWork);
            _CurrentWork.PreviousWorks.Add(_RemovedPreviosWork);
        }
        public RemovePreviousWorkCommand(bldWork work, bldWork previous_work)
        {
            _CurrentWork = work;
            _RemovedPreviosWork = previous_work;
            _CurrentWorkConstruction = _CurrentWork.bldConstruction;
            _RemovedPreviosWorkConstruction = _RemovedPreviosWork.bldConstruction;

            _CurrentWork.bldConstruction = _RemovedPreviosWorkConstruction;
            _RemovedPreviosWork.bldConstruction = _CurrentWorkConstruction;
            _RemovedPreviosWork.NextWorks.Remove(_CurrentWork);
            _CurrentWork.PreviousWorks.Remove(_RemovedPreviosWork);

        }
    }
}
