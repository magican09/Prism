using PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class AddWorkCommand : IUnDoRedoCommand
    {
         private bldWork _Added_work;
         private ObservableCollection<bldWork> _RemovedPreviousWorks = new ObservableCollection<bldWork>();
         private ObservableCollection<bldWork> _RemovedNextsWorks = new ObservableCollection<bldWork>();
        private bldConstruction _LastConstruction;
        private bldConstruction _CurrentConstruction;
        public string Name { get; set ; }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter = null)
        {
            foreach (bldWork work in _RemovedPreviousWorks)
            {
                _Added_work.PreviousWorks.Remove(work);
                work.NextWorks.Remove(_Added_work);
            }
            foreach (bldWork work in _RemovedNextsWorks)
            {
                _Added_work.NextWorks.Remove(work);
                work.PreviousWorks.Remove(_Added_work);
            }
            _Added_work.bldConstruction = _LastConstruction;
            _CurrentConstruction.Works.Add(_Added_work);
            if (_LastConstruction != null)
            {
                _LastConstruction.Works.Remove(_Added_work);
            }
        }

        public void UnExecute()
        {
            foreach (bldWork work in _RemovedPreviousWorks)
            {
                _Added_work.PreviousWorks.Add(work);
                work.NextWorks.Add(_Added_work);
            }
            foreach (bldWork work in _RemovedNextsWorks)
            {
                _Added_work.NextWorks.Add(work);
                work.PreviousWorks.Add(_Added_work);
            }
            _Added_work.bldConstruction = _CurrentConstruction;
            _CurrentConstruction.Works.Remove(_Added_work);
            if (_LastConstruction != null)
            {
              _LastConstruction.Works.Add(_Added_work);
            }
        }
        public AddWorkCommand(bldConstruction construction,bldWork add_work)
        {
            _Added_work = add_work;
            _CurrentConstruction = construction;
            List<bldWork> previousWorks = new List<bldWork>(_Added_work.PreviousWorks);
            List<bldWork> nextWorks = new List<bldWork>(_Added_work.NextWorks);

            if (_Added_work.bldConstruction != null)
            {
                _LastConstruction = _Added_work.bldConstruction;
                _LastConstruction.Works.Remove(_Added_work);
            }
           foreach(bldWork  work in previousWorks) //Удяем рабуту из предыдущих работ
            {
                _Added_work.PreviousWorks.Remove(work); 
                _RemovedPreviousWorks.Add(work);
                work.NextWorks.Remove(_Added_work);
               
            }
            foreach (bldWork work in nextWorks)//Удяем рабуту из последующих работ
            {
                _Added_work.NextWorks.Remove(work);
                _RemovedNextsWorks.Add(work);
                work.PreviousWorks.Remove(_Added_work);
            }
            _CurrentConstruction.Works.Add(_Added_work);
        }
    }
}
