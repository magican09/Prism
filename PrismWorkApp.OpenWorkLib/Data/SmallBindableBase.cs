using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public abstract class SmallBindableBase:INotifyPropertyChanged,IJornalable
    {
        public Guid Id { get; set; }
        public Guid StoredId { get; set; }

        event SaveChangesEventHandler IJornalable.SaveChanges
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }
        #region InotifyPropertyChanged
        protected virtual bool BaseSetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = "")
        {
            if (object.Equals(val, member)) return false;
            if (b_jornal_recording_flag)
            {
                PropertyBeforeChanged(this, new PropertyBeforeChangeEvantArgs(propertyName, member, val));
            }
            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
        protected bool SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = null, bool jornal_mode = false)
        {
            return BaseSetProperty<T>(ref member, val, propertyName);
        }
        #endregion

        #region IJornalable
        private ObservableCollection<IUnDoRedoCommand> _changesJornal = new ObservableCollection<IUnDoRedoCommand>();
        [NotMapped]
        public ObservableCollection<IUnDoRedoCommand> ChangesJornal
        {
            get { return _changesJornal; }
            set { SetProperty(ref _changesJornal, value); }
        }

        public ObservableCollection<IUnDoReDoSystem> UnDoReDoSystems { get; set; }

        public void JornalingOff()
        {
            if (b_jornal_recording_flag == true)
                b_jornal_recording_flag = false;
        }
        public void JornalingOn()
        {
            if (b_jornal_recording_flag == false)
                b_jornal_recording_flag = true;
        }
        private bool b_jornal_recording_flag = true;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event PropertyBeforeChangeEventHandler PropertyBeforeChanged = delegate { };
        public event UnDoReDoCommandCreateEventHandler UnDoReDoCommandCreated = delegate { };
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        /// <summary>
        /// Вызывается для добалвения UnDoRedo команды в систему UnDoRedo
        /// </summary>
        /// <param name="command"></param>
        public void InvokeUnDoReDoCommandCreatedEvent(IUnDoRedoCommand command)
        {
            UnDoReDoCommandCreated.Invoke(this, new UnDoReDoCommandCreateEventsArgs(command));
        }

        public void SaveChanges()
        {
            List<IUnDoRedoCommand> all_change_command = new List<IUnDoRedoCommand>(ChangesJornal);
            foreach (IUnDoRedoCommand unDoRedoCommand in all_change_command)
            {
                var ather_changed_objects = unDoRedoCommand.ChangedObjects.Where(ob => ob != this).ToList();
                foreach (IJornalable ather_object in ather_changed_objects)
                {
                    ather_object.ChangesJornal.Remove(unDoRedoCommand);
                }
                ChangesJornal.Remove(unDoRedoCommand);
            }
        }

        public void SaveChanges(IUnDoReDoSystem unDoReDo)
        {
            throw new NotImplementedException();
        }

        public void Save(IUnDoReDoSystem unDoReDo)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
