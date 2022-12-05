using PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class AddNextWorkCommand : IUnDoRedoCommand
    {
        private bldWork _CurrentWork;
        private bldConstruction _CurrentWorkConstruction;

        private bldWork _AddNextWork;
        private bldConstruction _AddNextWorkConstruction;


        public string Name { get ; set; }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter = null)
        {

            _CurrentWork.bldConstruction = _AddNextWorkConstruction;
            _AddNextWork.bldConstruction = _CurrentWorkConstruction;
            _AddNextWork.PreviousWorks.Add(_CurrentWork);
            _CurrentWork.NextWorks.Add(_AddNextWork);
        }

        public void UnExecute()
        {
            _CurrentWork.bldConstruction = _CurrentWorkConstruction ;
            _AddNextWork.bldConstruction = _AddNextWorkConstruction;
            _AddNextWork.PreviousWorks.Remove(_CurrentWork);
            _CurrentWork.NextWorks.Remove(_AddNextWork);
        }
        public AddNextWorkCommand(bldWork work, bldWork next_work)
        {
            _CurrentWork = work;
            _AddNextWork = next_work;
            _CurrentWorkConstruction = _CurrentWork.bldConstruction;
            _AddNextWorkConstruction = _AddNextWork.bldConstruction;

            _CurrentWork.bldConstruction = _AddNextWorkConstruction;
            _AddNextWork.bldConstruction = _CurrentWorkConstruction;
            _AddNextWork.PreviousWorks.Add(_CurrentWork);
            _CurrentWork.NextWorks.Add(_AddNextWork);

        }
    }
}
