﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public class PropertyStateRecord : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected virtual bool  SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = "")
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
        public ContextIdStructure ContextIdStructure{ get; set; }
        public string Name { get; set; }
        public DateTime Date { get ; set ; }
        private object _value;
        public object Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        private JornalRecordStatus _status;
        public JornalRecordStatus Status
        {
            get { return _status; }
            set {  SetProperty(ref _status, value); }
        }

        public IJornalable ParentObject { get; set; }
        public PropertiesChangeJornal ParentJornal { get; set; }
        public PropertyStateRecord(object prop, JornalRecordStatus recordStatus, string name ="" )
        {
            Value = prop;
            Date = DateTime.Now;
            Status = recordStatus;
            Name = name;
        }
        public PropertyStateRecord(object prop, JornalRecordStatus recordStatus, string name, Guid currentContextId)
        {
            Value = prop;
            Date = DateTime.Now;
            Status = recordStatus;
            Name = name;
            ContextId = currentContextId;
        }
        public PropertyStateRecord(object prop, JornalRecordStatus recordStatus, string name, Guid currentContextId, IJornalable parentObject)
        {
            Value = prop;
            Date = DateTime.Now;
            Status = recordStatus;
            Name = name;
            ContextId = currentContextId;
            ParentObject = parentObject;
        }
    }
}
