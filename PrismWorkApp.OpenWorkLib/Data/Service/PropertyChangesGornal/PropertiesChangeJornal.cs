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

    public class PropertiesChangeJornal : ObservableCollection<PropertyStateRecord>, IPropertiesChangeJornal, INotifyPropertyChanged
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
           
            
            PropertyStateRecord stateRecord = new PropertyStateRecord(
                    e.Item, JornalRecordStatus.REMOVED, e.Item.Id.ToString(),
                    CurrentContextId, sender as IJornalable);
            (e.Item as IJornalable).IsVisible = false;

            this.UnPickRecords(); 
            this.Add(stateRecord);
           

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
                PropertyStateRecord stateRecord = new PropertyStateRecord(property_last_value, JornalRecordStatus.MODIFIED, e.PropertyName,
                    (sender as IJornalable).CurrentContextId, sender as IJornalable);

                this.UnPickRecords();
                this.Add(stateRecord);
              
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(stateRecord.Name));

            }
        }
        private void OnPropertyObjectChanged(object sender, PropertyChangedEventArgs e)
        {

            var properties_list = sender.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);
            var property_current_value_prop_info = properties_list.Where(pri => pri.Name == e.PropertyName).FirstOrDefault();
            string prop_name = property_current_value_prop_info.Name;
            if (sender is IList)
                ;
            (sender as IJornalable).JornalingOff();
            var property_current_value = property_current_value_prop_info.GetValue(sender);
            (sender as IJornalable).JornalingOn();
            if (property_current_value is IJornalable current_value &&
                property_current_value != null && !RegistedObjects.Contains(current_value))
            {
                this.RegisterObject(current_value);
            }
        }


        public virtual void UnDoLeft()
        {
            Guid currentContextId = CurrentContextId;
            var all_prop_states = this.Where(pr => pr.ContextId == currentContextId).OrderBy(pr => pr.Date);
            PropertyStateRecord picked_state = all_prop_states.Where(pr => pr.PointerStatus == JornalRecordPointerStatus.PICKED).FirstOrDefault();
            PropertyStateRecord previous_picked_state = null;
            if (picked_state != null) // Если не было выбранной сохраненой записи 
            {
                previous_picked_state = all_prop_states.Where(r => r.Date < picked_state.Date)
                 .OrderBy(r => r.Date).LastOrDefault();
                picked_state.PointerStatus = JornalRecordPointerStatus.UNPICKED;
            }
            else
            {
                previous_picked_state = all_prop_states.OrderBy(r => r.Date).LastOrDefault();
            }

            if (previous_picked_state != null) previous_picked_state.PointerStatus = JornalRecordPointerStatus.PICKED;

            this.UnDo(previous_picked_state);


        }
        public virtual void UnDoRight()
        {
            Guid currentContextId = CurrentContextId;
            var all_prop_states = this.Where(pr => pr.ContextId == currentContextId).OrderBy(pr => pr.Date);
            PropertyStateRecord picked_state = all_prop_states.Where(pr => pr.PointerStatus == JornalRecordPointerStatus.PICKED).FirstOrDefault();
            PropertyStateRecord next_picked_state = null;
            if (picked_state != null) // Если не было выбранной сохраненой записи 
            {
                next_picked_state = all_prop_states.Where(r => r.Date > picked_state.Date)
                 .OrderBy(r => r.Date).FirstOrDefault();
                picked_state.PointerStatus = JornalRecordPointerStatus.UNPICKED;
            }
            else
            {
                next_picked_state = all_prop_states.OrderBy(r => r.Date).LastOrDefault();
            }

            if (next_picked_state != null) next_picked_state.PointerStatus = JornalRecordPointerStatus.PICKED;

            this.UnDo(next_picked_state);

        }
        //cxcxcxcxc
        public virtual void UnDo(PropertyStateRecord propertyState)
        {
            var prop_info = propertyState.ParentObject.GetType().GetProperty(propertyState.Name); //Достаем с помощью рефлексии данные о свойстве из текущего объекта
            if (prop_info != null && prop_info.GetIndexParameters().Length == 0)//Если свойство не является индек..
            {

                var prop_val = prop_info.GetValue(propertyState.ParentObject);
                propertyState.ParentObject.JornalingOff();
                prop_info.SetValue(propertyState.ParentObject, propertyState.Value); //Присваиваем свойству текущего объекта сохраненное в журнале значение
                propertyState.ParentObject.JornalingOn();
                propertyState.PointerStatus = JornalRecordPointerStatus.PICKED;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyState.Name));
            }
            if (propertyState.ParentObject is IList)
            {
                switch (propertyState.Status)
                {
                    case JornalRecordStatus.REMOVED:
                        propertyState.Status = JornalRecordStatus.ADDED;
                        (propertyState.Value as IJornalable).IsVisible = true;
                        break;
                    case JornalRecordStatus.ADDED:
                        propertyState.Status = JornalRecordStatus.REMOVED;
                        (propertyState.Value as IJornalable).IsVisible = false;
                        break;
                    default:
                        break;

                }
            }
        }
        public void Save()
        {
            Guid currentContextId = CurrentContextId;
            var records_fo_delete = this.Where(rc => rc.ContextId == currentContextId).ToList();
            foreach (PropertyStateRecord stateRecord in records_fo_delete)
                this.Remove(stateRecord);
        }
        private void UnPickRecords()
        {
            PropertyStateRecord current_prop_state = this.Where(ps => ps.PointerStatus == JornalRecordPointerStatus.PICKED).FirstOrDefault();
             if (current_prop_state != null)
            {
                 var next_prop_state_records = this.Where(ps => ps.Name == current_prop_state.Name && ps.Date > current_prop_state.Date).ToList();
                foreach (PropertyStateRecord record in next_prop_state_records)
                    this.Remove(record);
                current_prop_state.PointerStatus = JornalRecordPointerStatus.UNPICKED;
                current_prop_state.Date = DateTime.Now;
           //     this.Add(new PropertyStateRecord(current_prop_state));
            }
            //    propertyState.PointerStatus = JornalRecordPointerStatus.PICKED;
        }

        public bool IsOnLastRecord() //Если казатель PICKED на последней записи
        {
            Guid currentContextId = CurrentContextId;
            PropertyStateRecord picked_record = this.Where(r => r.ContextId == currentContextId &&
                                            r.PointerStatus == JornalRecordPointerStatus.PICKED).FirstOrDefault();
            PropertyStateRecord next_record = null;
            if (picked_record != null)
                next_record = this.Where(rc => rc.Date > picked_record.Date).OrderBy(rc => rc.Date).FirstOrDefault();
            if (this.Count == 1) return true;
            if (picked_record == null && next_record == null) return false;//В журнале ничего нет
            if (picked_record != null && next_record == null) return true; //12.11.22
            if (picked_record == null && next_record != null) return false;
            if (picked_record != null && next_record != null) return false;

            return true;


        }
        public bool IsOnFirstRecord() //Если казатель PICKED на последней записи
        {
            Guid currentContextId = CurrentContextId;
            PropertyStateRecord picked_record = this.Where(r => r.ContextId == currentContextId &&
                                            r.PointerStatus == JornalRecordPointerStatus.PICKED).FirstOrDefault();
            PropertyStateRecord privious_record = null;
            if (picked_record != null)
                privious_record = this.Where(rc => rc.Date < picked_record.Date).OrderBy(rc => rc.Date).LastOrDefault();

            if (picked_record == null && privious_record == null) return false;//В журнале ничего нет
            if (picked_record != null && privious_record == null) return true;
            if (picked_record == null && privious_record != null) return false;
            if (picked_record != null && privious_record != null) return false;

            return false;
        }

        private ObservableCollection<PropertyStateRecord> ChangesDetect(IJornalable obj)// Определяет не зажурналированные изменения в объетке и если они были  - возращает запись журанл
        {
            ObservableCollection<PropertyStateRecord> propertyStateRecords = new ObservableCollection<PropertyStateRecord>();
        
            var properties_list = obj.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);
             foreach(PropertyInfo propertyInfo in properties_list)
            {
                var prop_val = propertyInfo.GetValue(obj);
                string prop_name = propertyInfo.Name;
                if(!(prop_val is IList))
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




