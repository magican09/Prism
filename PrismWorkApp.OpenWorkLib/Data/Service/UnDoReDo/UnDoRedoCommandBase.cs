using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{ 
    public abstract class UnDoRedoCommandBase: IUnDoRedoCommand
    {
        public event EventHandler CanExecuteChanged;
        public Guid Id { get; set; } = Guid.NewGuid();
        public ObservableCollection<IJornalable> ChangedObjects { get; set; } = new ObservableCollection<IJornalable>();
        public DateTime Date { get; set; } =  DateTime.Now;
        public IUnDoReDoSystem UnDoReDo_System { get; protected set; }
        private int _index;
        public int Index
        {
            get
            {
                if (UnDoReDo_System != null && 
                    UnDoReDo_System._UnDoCommands.Where(cm => cm.Id == this.Id).FirstOrDefault()!=null)
                    _index = UnDoReDo_System._UnDoCommands.ToList().IndexOf(
                       UnDoReDo_System._UnDoCommands.Where(cm=>cm.Id==this.Id).FirstOrDefault());
                else _index = -1;
                return _index;
            }
            set { _index = value; }
        }

        public string Name { get; set ; }

        public void Execute(object parameter = null)
        {
          
        }

        public bool CanExecute(object parameter = null)
        {
            return  true;
        }

        public void UnExecute()
        {
            
        }
    }
}
