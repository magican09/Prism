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
            if (obj is INotifyCollectionChanged)
            {
                (obj as INotifyCollectionChanged).CollectionChanged += OnCollectionObjectChanged;
                obj.PropertyBeforeChanged += OnPropertyBeforeChanged;

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
                this.SetCurrentPropertyStateRecord(stateRecord);
                this.Add(stateRecord);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(stateRecord.Name));

            }
        }
        private void OnPropertyObjectChanged(object sender, PropertyChangedEventArgs e)
        {

            var properties_list = sender.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);
            var property_current_value_prop_info = properties_list.Where(pri => pri.Name == e.PropertyName).FirstOrDefault();
            string prop_name = property_current_value_prop_info.Name;
            var property_current_value = property_current_value_prop_info.GetValue(sender);

            if (property_current_value is IJornalable current_value &&
                property_current_value != null && !RegistedObjects.Contains(current_value))
            {
                this.RegisterObject(current_value);
            }
        }
        private void OnCollectionObjectChanged(object sender, NotifyCollectionChangedEventArgs e)
        {



        }

        public virtual void UnDoLeft()
        {
            Guid currentContextId = CurrentContextId;
            PropertyStateRecord propertyState = null;
            var all_prop_states = this.Where(pr => pr.ContextId == currentContextId).OrderBy(pr => pr.Date);
            PropertyStateRecord current_prop_state = all_prop_states.Where(pr => pr.Status == JornalRecordStatus.CURRENT_VALUE).FirstOrDefault();

            if (current_prop_state != null)
            {
                //  current_prop_state.Status = JornalRecordStatus.MODIFIED;
                var previous_prop_states = all_prop_states.Where(pr => pr.Date < current_prop_state.Date).ToList();
                if (previous_prop_states.Count > 0)
                {
                    propertyState = previous_prop_states.OrderBy(pr => pr.Date).LastOrDefault();
                    current_prop_state.Status = JornalRecordStatus.MODIFIED;
                }
            }
            else
                propertyState = this.Where(pr => pr.ContextId == currentContextId).OrderBy(pr => pr.Date).LastOrDefault();

            if (propertyState != null)
            {
                var prop_info = propertyState.ParentObject.GetType().GetProperty(propertyState.Name); //Достаем с помощью рефлексии данные о свойстве из текущего объекта
                var prop_val = prop_info.GetValue(propertyState.ParentObject);

                if (prop_info.GetIndexParameters().Length == 0)//Если свойство не является индек..
                {
                    propertyState.ParentObject.JornalingOff();
                    prop_info.SetValue(propertyState.ParentObject, propertyState.Value); //Присваиваем свойству текущего объекта сохраненное в журнале значение
                    propertyState.ParentObject.JornalingOn();
                    propertyState.Status = JornalRecordStatus.CURRENT_VALUE;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyState.Name));
                }

            }
           
        }
        public virtual void UnDoRight()
        {
            Guid currentContextId = CurrentContextId;
            PropertyStateRecord propertyState = null;
            var all_prop_states = this.Where(pr => pr.ContextId == currentContextId).OrderBy(pr => pr.Date);
            PropertyStateRecord current_prop_state = all_prop_states.Where(pr => pr.Status == JornalRecordStatus.CURRENT_VALUE).FirstOrDefault();
            if (current_prop_state != null)
            {
                var previous_prop_states = all_prop_states.Where(pr => pr.Date > current_prop_state.Date).ToList();
                if (previous_prop_states.Count > 0)
                {
                    propertyState = previous_prop_states.OrderByDescending(pr => pr.Date).LastOrDefault();
                    current_prop_state.Status = JornalRecordStatus.MODIFIED;
                }
            }
            else
                propertyState = this.Where(pr => pr.ContextId == currentContextId).OrderBy(pr => pr.Date).LastOrDefault();
            if (propertyState != null)
            {
                var prop_info = propertyState.ParentObject.GetType().GetProperty(propertyState.Name); //Достаем с помощью рефлексии данные о свойстве из текущего объекта
                var prop_val = prop_info.GetValue(propertyState.ParentObject);

                if (prop_info.GetIndexParameters().Length == 0)//Если свойство не является индек..
                {
                    propertyState.ParentObject.JornalingOff();
                    prop_info.SetValue(propertyState.ParentObject, propertyState.Value); //Присваиваем свойству текущего объекта сохраненное в журнале значение
                    propertyState.ParentObject.JornalingOn();
                    propertyState.Status = JornalRecordStatus.CURRENT_VALUE;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyState.Name));
                }
                //  this.Remove(propertyState); //Удаляем из журнала сохраненное изменение
            }
           
        }

        public void Save()
        {
            Guid currentContextId = CurrentContextId;
            var records_fo_delete = this.Where(rc => rc.ContextId == currentContextId).ToList();
            foreach (PropertyStateRecord stateRecord in records_fo_delete)
                this.Remove(stateRecord);
        }
        private void SetCurrentPropertyStateRecord(PropertyStateRecord propertyState)
        {
            PropertyStateRecord current_prop_state = this.Where(ps => ps.Status == JornalRecordStatus.CURRENT_VALUE).FirstOrDefault();
            if (current_prop_state != null) current_prop_state.Status = JornalRecordStatus.MODIFIED;
            propertyState.Status = JornalRecordStatus.CURRENT_VALUE;
        }

        public bool IsOnLastRecord() //Если казатель CURRENT_VALUE на последней записи
        {
            Guid currentContextId = CurrentContextId;
            var records = this.Where(rc => rc.ContextId == currentContextId).OrderBy(rc => rc.Date);
            var current_last_record = records.LastOrDefault();
            if (current_last_record != null && current_last_record.Status == JornalRecordStatus.CURRENT_VALUE)
                return true;
            else
                return false;
        }
        public bool IsOnFirstRecord() //Если казатель CURRENT_VALUE на последней записи
        {
            Guid currentContextId = CurrentContextId;
            var records = this.Where(rc => rc.ContextId == currentContextId).OrderBy(rc => rc.Date);
            var current_first_record = records.FirstOrDefault();
            if (current_first_record != null && current_first_record.Status == JornalRecordStatus.CURRENT_VALUE)
                return true;
            else
                return false;
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




