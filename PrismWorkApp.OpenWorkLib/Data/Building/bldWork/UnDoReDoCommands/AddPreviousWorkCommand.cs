using PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class AddPreviousWorkCommand : IUnDoRedoCommand
    {
        private bldWork _CurrentWork;
        private bldWork _AddPreviosWork;
        private bldConstruction _CurrentWorkConstruction;
        private bldConstruction _AddPreviosWorkConstruction;
        
        public string Name { get ; set; }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter = null)
        {

            _CurrentWork.bldConstruction = _AddPreviosWorkConstruction;
            _AddPreviosWork.bldConstruction = _CurrentWorkConstruction;
            _AddPreviosWork.NextWorks.Add(_CurrentWork);
            _CurrentWork.PreviousWorks.Add(_AddPreviosWork);
        }

        public void UnExecute()
        {

            _CurrentWork.bldConstruction = _CurrentWorkConstruction ;
            _AddPreviosWork.bldConstruction = _AddPreviosWorkConstruction; 
            _AddPreviosWork.NextWorks.Remove(_CurrentWork);
            _CurrentWork.PreviousWorks.Remove(_AddPreviosWork);
        }
        public AddPreviousWorkCommand(bldWork work, bldWork previous_work)
        {
            _CurrentWork = work;
            _AddPreviosWork = previous_work;
         
            _CurrentWorkConstruction = _CurrentWork.bldConstruction;
            _AddPreviosWorkConstruction = _AddPreviosWork.bldConstruction;

            _CurrentWork.bldConstruction = _AddPreviosWorkConstruction;
            _AddPreviosWork.bldConstruction = _CurrentWorkConstruction;
            _AddPreviosWork.NextWorks.Add(_CurrentWork);
            _CurrentWork.PreviousWorks.Add(_AddPreviosWork);

        }
    }
}
