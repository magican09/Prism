using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public delegate void PropertiesChangeJornalChangedEventHandler(object sender, PropertyStateRecord _propertyStateRecord);
    public delegate void ObjectStateChangeEventHandler(object sender, ObjectStateChangedEventArgs e);

    public class PropertiesChangeJornal : PropertyStateRecord, IPropertiesChangeJornal, INotifyPropertyChanged
    {

        public event PropertiesChangeJornalChangedEventHandler JornalChangedNotify;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected virtual bool SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = "")
        {
            if (object.Equals(val, member)) return false;
            member = val;
            //Type tp = Children[Children.Count - 1].GetType();
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        private Guid _currentContextId;
        public Guid CurrentContextId
        {
            get { return _currentContextId; }
            set { SetProperty(ref _currentContextId, value); }
        }

        public ObservableCollection<Guid> ContextIdHistory { get; set; } = new ObservableCollection<Guid>();
        public ObservableCollection<IJornalable> RegistedObjects { get; set; } = new ObservableCollection<IJornalable>();
        public PropertiesChangeJornal()
        {

            CollectionChanged += PropertiesChangeJornal_CollectionChanged;
        }

        private void PropertiesChangeJornal_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (PropertyStateRecord stateRecord in e.NewItems)
                {
                    stateRecord.ParentJornal = this;
                    if (JornalChangedNotify != null)
                    {
                    }
                    else
                        ;
                }
            }
        }
        public IJornalable ParentObject { get; set; }
        private bool IsContainsRecord(Guid currentContextId)
        {
            return this.Count > 0;
        }
        public void RegisterObject(IJornalable obj) //Регистрирем объет в журнале для наблюдения за имземениями
        {
            if (obj is INotifyJornalableCollectionChanged collection)
            {
                collection.CollectionChangedBeforAdd += OnCollectionChangedBeforAdd;
                collection.CollectionChangedBeforeRemove += OnCollectionChangedBeforeRemove;
                collection.PropertyBeforeChanged += OnCollectionPropertyChanged;
                collection.PropertyChanged += OnPropertyObjectChanged;
            }
            else if (obj is INotifyPropertyChanged)
            {
                (obj as INotifyPropertyChanged).PropertyChanged += OnPropertyObjectChanged;
                obj.PropertyBeforeChanged += OnPropertyBeforeChanged;
            }
            if (!RegistedObjects.Contains(obj))
                RegistedObjects.Add(obj);

            var properties_list = obj.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);
            foreach (PropertyInfo propertyInfo in properties_list)
            {
                var prop_value = propertyInfo.GetValue(obj);

                if (prop_value != null && prop_value is IJornalable && !RegistedObjects.Contains(prop_value))
                {
                    RegisterObject(prop_value as IJornalable);
                }
            }
            if (obj is IList)
            {

                foreach (IJornalable element in obj as IList)
                {
                    if (!RegistedObjects.Contains(element)) RegisterObject(element);
                }
            }

        }

        private void OnCollectionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        private void OnCollectionChangedBeforAdd(object sender, CollectionChangedEventArgs e)
        {

        }

        private void OnCollectionChangedBeforeRemove(object sender, CollectionChangedEventArgs e)
        {
            PropertyStateRecord stateRecord;

            PropertyStateRecord last_state_record = this.Where(r => r.ContextId == (sender as IJornalable).CurrentContextId).OrderBy(r => r.Index).LastOrDefault();
            int last_index = 0;
            if (last_state_record != null)
                last_index = ++last_state_record.Index;


            if (e.CurrentContextId != Guid.Empty)
                stateRecord = new PropertyStateRecord(
                       e.Item, JornalRecordType.REMOVED, e.Item.Id.ToString(),
                       e.CurrentContextId, sender as IJornalable, last_index);
            else
                stateRecord = new PropertyStateRecord(
                   e.Item, JornalRecordType.REMOVED, e.Item.Id.ToString(),
                 CurrentContextId, sender as IJornalable, last_index);

            (e.Item as IJornalable).IsVisible = false;
            if (this.SetRecordIndex(stateRecord))
            {
                this.Add(stateRecord);
                stateRecord.State = JornalRecordState.SAVED;
            }
        }

        private void OnPropertyBeforeChanged(object sender, PropertyChangedEventArgs e)
        {
            var properties_list = sender.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);
            var property_last_value_prop_info = properties_list.Where(pri => pri.Name == e.PropertyName).FirstOrDefault();
            var results = new List<ValidationResult>();
            var context_obj = new ValidationContext(sender);
            bool obj_validate_result = false;
            if (!Validator.TryValidateObject(sender, context_obj, results, true))
                obj_validate_result = results?[0]?.MemberNames?.FirstOrDefault() == property_last_value_prop_info?.Name;
            if (obj_validate_result == false)
            {
                var property_last_value = property_last_value_prop_info.GetValue(sender);

                PropertyStateRecord stateRecord = new PropertyStateRecord(property_last_value,
                                                        JornalRecordType.MODIFIED,
                                                         e.PropertyName, (sender as IJornalable).CurrentContextId,
                                                         sender as IJornalable, 0);
                if (SetRecordIndex(stateRecord))
                {
                    this.Add(stateRecord);
                    stateRecord.State = JornalRecordState.SAVED;
                    foreach (Guid currentId in ContextIdHistory)
                    {
                        PropertyStateRecord prop_state_record = this.Where(r => r.Name == currentId.ToString()).FirstOrDefault();
                        if (prop_state_record == null)
                            prop_state_record = new PropertyStateRecord(sender, JornalRecordType.MODIFIED, currentId.ToString(), currentId, null);
                        if (SetRecordIndex(prop_state_record))
                        {
                            if (!this.Contains(prop_state_record))
                            {
                                this.Add(prop_state_record);
                                prop_state_record.State = JornalRecordState.SAVED;
                                prop_state_record.Status = JornalRecordType.COMPLEX_RECORD;
                            }
                            PropertyStateRecord record = new PropertyStateRecord(stateRecord);
                            record.ContextId = currentId;
                            if (SetRecordIndex(record, prop_state_record))
                            {
                                prop_state_record.Add(record);
                                record.State = JornalRecordState.SAVED;
                                record.Status = JornalRecordType.COMPLEX_RECORD;
                            }
                        }
                    }
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(stateRecord.Name));
            }
        }
        private void OnPropertyObjectChanged(object sender, PropertyChangedEventArgs e)
        {

            var properties_list = sender.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);
            var property_current_value_prop_info = properties_list.Where(pri => pri.Name == e.PropertyName).FirstOrDefault();
            string prop_name = property_current_value_prop_info.Name;
            (sender as IJornalable).JornalingOff();
            var property_current_value = property_current_value_prop_info.GetValue(sender);
            (sender as IJornalable).JornalingOn();
            if (property_current_value is IJornalable current_value &&
                property_current_value != null && !RegistedObjects.Contains(current_value))
            {
                this.RegisterObject(current_value);
            }
        }
        public virtual void UnDoLeft(Guid currentContextId)
        {
            var all_prop_states = this.Where(pr => pr.ContextId == currentContextId).OrderBy(pr => pr.Index);
            PropertyStateRecord last_saved_state = all_prop_states.Where(pr => pr.State == JornalRecordState.SAVED)
                                                                         .OrderBy(r => r.Index).LastOrDefault();
            PropertyStateRecord previous_picked_state = null;
            if (last_saved_state != null) // Если не было выбранной сохраненой записи 
            {
                this.UnDo(last_saved_state);
                last_saved_state.State = JornalRecordState.UNDO_COMPLETE;
            }
        }
        public virtual void UnDoRight(Guid currentContextId)
        {
            var all_prop_states = this.Where(pr => pr.ContextId == currentContextId).OrderBy(pr => pr.Index);
            PropertyStateRecord first_undo_comlete_state = all_prop_states.Where(pr => pr.State == JornalRecordState.UNDO_COMPLETE)
                                                                         .OrderBy(r => r.Index).FirstOrDefault();
            if (first_undo_comlete_state != null) // Если не было выбранной сохраненой записи 
            {
                this.UnDo(first_undo_comlete_state,false);
                first_undo_comlete_state.State = JornalRecordState.SAVED;
            }
        }
        //cxcxcxcdsdsdsxc
        public virtual void UnDo(PropertyStateRecord propertyState,bool _undo_left_direction = true)
        {
            if (propertyState.ParentObject != null)
            {
                var prop_info = propertyState.ParentObject.GetType().GetProperty(propertyState.Name); //Достаем с помощью рефлексии данные о свойстве из текущего объекта
                if (prop_info != null && prop_info.GetIndexParameters().Length == 0)//Если свойство не является индек..
                {
                    var prop_val = prop_info.GetValue(propertyState.ParentObject);
                    propertyState.ParentObject.JornalingOff();
                    prop_info.SetValue(propertyState.ParentObject, propertyState.Value); //Присваиваем свойству текущего объекта сохраненное в журнале значение
                    propertyState.ParentObject.JornalingOn();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyState.Name));
                }
                if (propertyState.ParentObject is IList)
                {
                    switch (propertyState.Status)
                    {
                        case JornalRecordType.REMOVED:
                            propertyState.Status = JornalRecordType.ADDED;
                            (propertyState.Value as IJornalable).IsVisible = true;
                            break;
                        case JornalRecordType.ADDED:
                            propertyState.Status = JornalRecordType.REMOVED;
                            (propertyState.Value as IJornalable).IsVisible = false;
                            break;
                        default:
                            break;
                    }
                }
            }
            else if(propertyState.Count>0 && propertyState.Status == JornalRecordType.COMPLEX_RECORD)//Еасли  изменене не одно свойсто 
            {
                
                List<PropertyStateRecord> records = null;
                if (_undo_left_direction)
                    records = propertyState.OrderByDescending(r => r.Index).ToList();
                else
                    records = propertyState.OrderBy(r => r.Index).ToList();

                foreach (PropertyStateRecord record in records)
                {
                    UnDo(record);
                }
            }
        }
        public void UnDoAll(Guid currentContextId)
        {
            var records_for_undo = this.Where(r => r.ContextId == currentContextId).OrderByDescending(r => r.Date).ToList();
            foreach (PropertyStateRecord record in records_for_undo)
            {
                this.UnDo(record);
                this.Remove(record);
            }
        }
        public void Save(PropertyStateRecord propertyState)
        {
            if (propertyState.ParentObject is INotifyJornalableCollectionChanged)
            {
                if (propertyState.Status == JornalRecordType.REMOVED)
                    if (((IList)propertyState.ParentObject).Contains(propertyState.Value as IJornalable))
                        ((INotifyJornalableCollectionChanged)propertyState.ParentObject).RemoveItem(propertyState.Value as IJornalable);
            }
            this.Remove(propertyState);
        }
        public void SaveAll(Guid currentContextId)
        {
            var records_for_delete = this.Where(r => r.ContextId == currentContextId).ToList();
            foreach (PropertyStateRecord record in records_for_delete)
                this.Save(record);
        }
        private bool SetRecordIndex(PropertyStateRecord prop_last_state_record, PropertyStateRecord records_coll = null)
        {
            if (records_coll == null) records_coll = this;
            var saved_records = records_coll.Where(r => r.State == JornalRecordState.SAVED).ToList();
            var undo_comlete_records = records_coll.Where(r => r.State == JornalRecordState.UNDO_COMPLETE).ToList();
            var last_saved_record = saved_records.OrderBy(r => r.Index).LastOrDefault();
            int save_recorsd_last_index = 0;
            int undo_complete_first_index = 0;
            if (saved_records.Count > 0)
                save_recorsd_last_index = saved_records.OrderBy(r => r.Index).LastOrDefault().Index;
            if (undo_comlete_records.Count > 0)
                undo_complete_first_index = undo_comlete_records.OrderBy(r => r.Index).FirstOrDefault().Index;
            prop_last_state_record.Index = save_recorsd_last_index + 1;
            if (last_saved_record != null && last_saved_record.Name == prop_last_state_record.Name &&
                                           last_saved_record.Value == prop_last_state_record.Value &&
                                             last_saved_record.ContextId == prop_last_state_record.ContextId)
            {
                return false;
            }
            foreach (PropertyStateRecord _record in undo_comlete_records)
            {
                _record.Index = ++save_recorsd_last_index + 1;
            }
            return true;
        }

        public bool IsOnLastRecord(Guid currentContextId) //Если казатель PICKED на последней записи
        {
            var saved_records = this.Where(r => r.State == JornalRecordState.SAVED).ToList();
            var undo_comlete_records = this.Where(r => r.State == JornalRecordState.UNDO_COMPLETE).ToList();
            if (saved_records.Count > 0 &&
               undo_comlete_records.Count == 0)
                return true;
            else
                return false;
        }
        public bool IsOnFirstRecord(Guid currentContextId) //Если казатель PICKED на последней записи
        {
            var saved_records = this.Where(r => r.State == JornalRecordState.SAVED).ToList();
            var undo_comlete_records = this.Where(r => r.State == JornalRecordState.UNDO_COMPLETE).ToList();

            if (saved_records.Count == 0 &&
               undo_comlete_records.Count > 0)
                return true;
            else
                return false;
        }

        private ObservableCollection<PropertyStateRecord> ChangesDetect(IJornalable obj)// Определяет не зажурналированные изменения в объетке и если они были  - возращает запись журанл
        {
            ObservableCollection<PropertyStateRecord> propertyStateRecords = new ObservableCollection<PropertyStateRecord>();

            var properties_list = obj.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);
            foreach (PropertyInfo propertyInfo in properties_list)
            {
                var prop_val = propertyInfo.GetValue(obj);
                string prop_name = propertyInfo.Name;
                if (!(prop_val is IList))
                {
                    PropertyStateRecord last_state_record = this.Where(r => r.Name == prop_name).OrderBy(r => r.Date).LastOrDefault();

                }
                else
                {

                }
            }


            return propertyStateRecords;

        }
    }
}

public class ObjectStateChangedEventArgs
{
    public string ObjectName { get; set; }
    public IJornalable Object { get; set; }
    public PropertyStateRecord PropertyStateRecord { get; set; }
    public ObjectStateChangedEventArgs()
    {

    }
    public ObjectStateChangedEventArgs(string name, IJornalable obj, PropertyStateRecord propertyStateRecord)
    {
        ObjectName = name;
        Object = obj;
        PropertyStateRecord = propertyStateRecord;
    }
}




