using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections.ObjectModel;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class RemoveWorkCommand : UnDoRedoCommandBase, IUnDoRedoCommand
    {
        private bldWork _removedWork;
        private bldConstruction _contruction;
        private ObservableCollection<bldWork> _PreviousWorks = new ObservableCollection<bldWork>();
        private ObservableCollection<bldWork> _NextWorks = new ObservableCollection<bldWork>();

        public string Name { get; set; }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter = null)
        {
            foreach (bldWork work in _removedWork.PreviousWorks)
            {
                _PreviousWorks.Add(work);
                work.NextWorks.Remove(_removedWork);
            }
            _removedWork.PreviousWorks.Clear();

            foreach (bldWork work in _removedWork.NextWorks)
            {
                _NextWorks.Add(work);
                work.PreviousWorks.Remove(_removedWork);
            }
            _removedWork.NextWorks.Clear();
            _contruction.Works.Remove(_removedWork);
        }

        public void UnExecute()
        {

            foreach (bldWork work in _PreviousWorks)
            {
                _removedWork.PreviousWorks.Add(work);
                work.NextWorks.Add(_removedWork);
            }
            _PreviousWorks.Clear();

            foreach (bldWork work in _NextWorks)
            {
                _removedWork.NextWorks.Add(work);
                work.PreviousWorks.Add(_removedWork);
            }
            _NextWorks.Clear();
            _contruction.Works.Add(_removedWork);
        }
        public RemoveWorkCommand(bldConstruction construction, bldWork _work)
        {
            _removedWork = _work;
            _contruction = construction;

            foreach (bldWork work in _removedWork.PreviousWorks)
            {
                _PreviousWorks.Add(work);
                work.NextWorks.Remove(_removedWork);
            }
            _removedWork.PreviousWorks.Clear();

            foreach (bldWork work in _removedWork.NextWorks)
            {
                _NextWorks.Add(work);
                work.PreviousWorks.Remove(_removedWork);
            }
            _removedWork.NextWorks.Clear();
            _contruction.Works.Remove(_removedWork);
        }
    }
}
