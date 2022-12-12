using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class AddWorkGroupToConstructionCommand : IUnDoRedoCommand
    {
        private ObservableCollection<bldWork> _AddedWorks = new ObservableCollection<bldWork>();
        private ObservableCollection<Tuple<bldWork, bldWork>> _Works_PreviousWork_Collection = new ObservableCollection<Tuple<bldWork, bldWork>>();
        private ObservableCollection<Tuple<bldWork, bldWork>> _Works_NextWork_Collection = new ObservableCollection<Tuple<bldWork, bldWork>>();
        private ObservableCollection<Tuple<bldWork, bldWork>> _External_PreviousWorks_Tuple_Collection;
        private ObservableCollection<Tuple<bldWork, bldWork>> _External_NextWork_Tuple_Collection;
        private bldConstruction _LastConstruction;
        private bldConstruction _CurrentConstruction;

        public string Name { get; set; }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter = null)
        {
            foreach (Tuple<bldWork, bldWork> tuple in _External_PreviousWorks_Tuple_Collection)
            {
                tuple.Item1.PreviousWorks.Remove(tuple.Item2);
                tuple.Item2.NextWorks.Remove(tuple.Item1);
            }
            foreach (Tuple<bldWork, bldWork> tuple in _External_NextWork_Tuple_Collection)
            {
                tuple.Item1.NextWorks.Remove(tuple.Item2);
                tuple.Item2.PreviousWorks.Remove(tuple.Item1);
            }

            foreach (bldWork work in _AddedWorks)
            {
                _CurrentConstruction.Works.Add(work);
                _LastConstruction.Works.Remove(work);
                work.bldConstruction = _CurrentConstruction;
            }

        }

        public void UnExecute()
        {
            foreach (Tuple<bldWork, bldWork> tuple in _External_PreviousWorks_Tuple_Collection)
            {
                tuple.Item1.PreviousWorks.Add(tuple.Item2);
                tuple.Item2.NextWorks.Add(tuple.Item1);
            }
            foreach (Tuple<bldWork, bldWork> tuple in _External_NextWork_Tuple_Collection)
            {
                tuple.Item1.NextWorks.Add(tuple.Item2);
                tuple.Item2.PreviousWorks.Add(tuple.Item1);
            }

            foreach (bldWork work in _AddedWorks)
            {
                _CurrentConstruction.Works.Remove(work);
                _LastConstruction.Works.Add(work);
                work.bldConstruction = _LastConstruction;
            }
        }
      


        public AddWorkGroupToConstructionCommand(bldConstruction construction, ObservableCollection<bldWork> add_works_group)
        {
            _AddedWorks = new ObservableCollection<bldWork>(add_works_group);
            _LastConstruction = add_works_group[0].bldConstruction;
            _CurrentConstruction = construction;

            foreach (bldWork work in _AddedWorks)
            {
                foreach (bldWork previous_work in work.PreviousWorks)
                    _Works_PreviousWork_Collection.Add(new Tuple<bldWork, bldWork>(work, previous_work));
                foreach (bldWork next_work in work.NextWorks)
                    _Works_NextWork_Collection.Add(new Tuple<bldWork, bldWork>(work, next_work));
            }
            #region Находим все внешние предыдущие и последующие работы 
            _External_PreviousWorks_Tuple_Collection =
               new ObservableCollection<Tuple<bldWork, bldWork>>(
                   _Works_PreviousWork_Collection.Where(pw => _AddedWorks.Where(w => w.Id == pw.Item2.Id).FirstOrDefault() == null).ToList());
            _External_NextWork_Tuple_Collection =
                new ObservableCollection<Tuple<bldWork, bldWork>>(
                    _Works_NextWork_Collection.Where(pw => _AddedWorks.Where(w => w.Id == pw.Item2.Id).FirstOrDefault() == null).ToList());
            #endregion

            foreach (Tuple<bldWork, bldWork> tuple in _External_PreviousWorks_Tuple_Collection)
            {
                tuple.Item1.PreviousWorks.Remove(tuple.Item2);
                tuple.Item2.NextWorks.Remove(tuple.Item1);
            }
            foreach (Tuple<bldWork, bldWork> tuple in _External_NextWork_Tuple_Collection)
            {
                tuple.Item1.NextWorks.Remove(tuple.Item2);
                tuple.Item2.PreviousWorks.Remove(tuple.Item1);
            }

            foreach (bldWork work in _AddedWorks)
            {
                _CurrentConstruction.Works.Add(work);
                _LastConstruction.Works.Remove(work);
                 work.bldConstruction = _CurrentConstruction;
            }
        }
    }
    public class WorkState
    {
        public Guid WorkId;
        public bldWork Work;
        public ObservableCollection<bldWork> PreviousWorks = new ObservableCollection<bldWork>();
        public ObservableCollection<bldWork> NextWorks = new ObservableCollection<bldWork>();
    }

}
