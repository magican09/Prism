using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public class PropertyStateRecord : ObservableCollection<PropertyStateRecord>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected virtual bool SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = "")
        {
            if (object.Equals(val, member)) return false;
            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid _сontextId;
        public Guid ContextId
        {
            get
            {
                if (ContextIdStructure != null)
                    return ContextIdStructure.ContextId;
                else
                    return _сontextId;
            }
            set
            {


                if (ContextIdStructure != null)
                {
                    ContextIdStructure.ContextId = value;
                }
                else
                    _сontextId = value;
            }
        }
        public Guid ChildWindowContextId { get; set; }
        public ContextIdStructure ContextIdStructure { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        private object _value;
        public object Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }
        private int _index;
        public int Index
        {
            get { return _index; }
            set { SetProperty(ref _index, value); }
        }
        private JornalRecordType _status;
        public JornalRecordType Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        private JornalRecordState _state = JornalRecordState.NONE;
        public JornalRecordState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        public IJornalable ParentObject { get; set; }
        public PropertiesChangeJornal ParentJornal { get; set; }
        public PropertyStateRecord(object prop, JornalRecordType recordStatus, string name = "")
        {
            Value = prop;
            Date = DateTime.Now;
            Status = recordStatus;
            Name = name;
        }
        public PropertyStateRecord(object prop, JornalRecordType recordStatus, string name, Guid currentContextId)
        {
            Value = prop;
            Date = DateTime.Now;
            Status = recordStatus;
            Name = name;
            ContextId = currentContextId;
        }
        public PropertyStateRecord(object prop, JornalRecordType recordStatus, string name, Guid currentContextId, IJornalable parentObject)
        {
            Value = prop;
            Date = DateTime.Now;
            Status = recordStatus;
            Name = name;
            ContextId = currentContextId;
            ParentObject = parentObject;
        }
        public PropertyStateRecord(object prop, JornalRecordType recordStatus, string name, Guid currentContextId, IJornalable parentObject, int index)
        {
            Value = prop;
            Date = DateTime.Now;
            Status = recordStatus;
            Name = name;
            ContextId = currentContextId;
            ParentObject = parentObject;
            Index = index;
        }
        public PropertyStateRecord(PropertyStateRecord stateRecord) :
            this(stateRecord.Value, stateRecord.Status, stateRecord.Name, stateRecord.ContextId, stateRecord.ParentObject)
        {
            Date = DateTime.Now;
        }
        public PropertyStateRecord()
        {

        }
    }


}
