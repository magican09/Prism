using PrismWorkApp.OpenWorkLib.Core;
using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    //public class NameableObservableCollection<TEntity>:ObservableCollection<TEntity>,IList<TEntity> where TEntity: class,IEntityObject
    public class NameableObservableCollection<TEntity> : ObservableCollection<TEntity>, IEntityObject, IJornalable, INameableOservableCollection<TEntity> where TEntity : class, IEntityObject, IJornalable
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event ObjectStateChangeEventHandler ObjectChangedNotify;//Событие вызывается при изменении в данном объекте 
        public event ObjectStateChangeEventHandler ObjectChangeSaved; //Событие вызывается при сохранении изменений в данном объекте
        public event ObjectStateChangeEventHandler ObjectChangeUndo; //Событие вызывается при отмете изменений в данном объекте

        public virtual Func<IEntityObject, bool> RestrictionPredicate { get; set; } = x => true;//Предикат для ограничений при работе с данных объектом по умолчанию

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("Id"); }
        }
        private Guid _storedId;
        public Guid StoredId
        {
            get { return _storedId; }
            set { _storedId = value; OnPropertyChanged("StoredId"); }
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }
        public NameableObservableCollection()
        {
            Id = Guid.NewGuid();
            CountNotificationSet();


        }
        public NameableObservableCollection(string name) : this()
        {
            Id = Guid.NewGuid();
            Name = name;
            CountNotificationSet();

        }
        public NameableObservableCollection(List<TEntity> list) : base(list)
        {
            Id = Guid.NewGuid();
            CountNotificationSet();

        }
        public NameableObservableCollection(ICollection<TEntity> collection) : base(collection)
        {
            Id = Guid.NewGuid();
            CountNotificationSet();

        }
        public NameableObservableCollection(IEnumerable<TEntity> entities) : base(entities)
        {
            Id = Guid.NewGuid();
            CountNotificationSet();

        }
        #region Validating
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate { };


        #endregion

        #region Changes Jornaling

        private bool b_jornal_recording_flag = true;
        private Guid _currentContextId;
        public Guid CurrentContextId
        {
            get { return _currentContextId; }
            set { _currentContextId = value; }
        }
        private JornalRecordStatus status;
        public JornalRecordStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                IsVisible = (status != JornalRecordStatus.REMOVED) ? true : false;
            }
        }
        public PropertiesChangeJornal PropertiesChangeJornal { get; set; } = new PropertiesChangeJornal();
        public ObservableCollection<IJornalable> ParentObjects { get; set; }
        public IJornalable ParentObject { get; set; }

        public void OnChildObjectChanges(object sender, ObjectStateChangedEventArgs e)//Вызывается дочерними объектами если в них были произведены изменения
        {
            if (e != null && !PropertiesChangeJornal.Contains(e.PropertyStateRecord))
            {
                PropertiesChangeJornal.Add(e.PropertyStateRecord);
            }
            ObjectChangedNotify?.Invoke(this, e);

        }
        public void OnChildObjectChangeSaved(object sender, ObjectStateChangedEventArgs e)
        {
            if (e != null && PropertiesChangeJornal.Contains(e.PropertyStateRecord))
            {
                PropertiesChangeJornal.Remove(e.PropertyStateRecord);
                ObjectChangeSaved?.Invoke(this, e);
            }
            ObjectChangedNotify?.Invoke(this, null);
        }
        public void OnChildObjectChangeUndo(object sender, ObjectStateChangedEventArgs e)
        {
            if (PropertiesChangeJornal.Contains(e.PropertyStateRecord))
            {
                PropertiesChangeJornal.Remove(e.PropertyStateRecord);
                ObjectChangeUndo?.Invoke(this, e);
            }
            ObjectChangedNotify?.Invoke(this, null);
        }

        public void UnDo(PropertyStateRecord propertyState)
        {
            // PropertyStateRecord lastPropState = PropertiesChangeJornal?.OrderBy(el => el.Date).LastOrDefault();
            //  List<PropertyStateRecord> Undo_Chain =
            //   PropertiesChangeJornal.Where(pr => pr.ContextId == currentContextId).OrderBy(pr => pr.Date).ToList();

            if (propertyState != null)
            {
                b_jornal_recording_flag = false; //Блокируем ведение журнала 
                if (propertyState.Status == JornalRecordStatus.ADDED)
                {
                    if (this.Contains((TEntity)propertyState.Value))
                    {
                        ((TEntity)propertyState.Value).Status = JornalRecordStatus.REMOVED;
                        PropertiesChangeJornal.Remove(propertyState);
                        ObjectChangeUndo?.Invoke(this, new ObjectStateChangedEventArgs("", this, propertyState));
                    }
                }
                else if (propertyState.Status == JornalRecordStatus.REMOVED)

                {
                    ((TEntity)propertyState.Value).Status = JornalRecordStatus.CREATED;
                    PropertiesChangeJornal.Remove(propertyState);
                    ObjectChangeUndo?.Invoke(this, new ObjectStateChangedEventArgs("", this, propertyState));
                }
                b_jornal_recording_flag = true;
            }


        }
        public void UnDoAll(Guid currentContextId)
        {
            List<PropertyStateRecord> records =
               PropertiesChangeJornal.Where(pr => pr.ContextId == currentContextId).OrderByDescending(pr => pr.Date).ToList();
            foreach (PropertyStateRecord record in records)
                UnDo(record); //Отменяем измеенения всех переменных, которые не коллекции

        }
        public virtual void Save(object prop_id, Guid currentContextId)
        {
            if ((Guid)prop_id != Guid.Empty)
            {
                List<PropertyStateRecord> propertyStateRecords =
                            PropertiesChangeJornal.Where(p => ((IKeyable)p.Value).Id == (Guid)prop_id && p.ContextId == currentContextId).ToList(); // Находим все записи о объекте в журнале

                PropertyStateRecord propertyState = propertyStateRecords.OrderBy(r => r.Date).LastOrDefault(); //Выбираем самую последнюю

                if (propertyState != null)
                {
                    if (propertyState.ParentObject.Id == (this as IEntityObject).Id)//Изменения были в дочернем объекте текущего отбъекта
                    {
                        if (((IJornalable)propertyState?.Value).Status == JornalRecordStatus.REMOVED) //Если удалит - удаляем
                            this.Remove((TEntity)propertyState.Value);

                        else if (((IJornalable)propertyState?.Value).Status == JornalRecordStatus.CREATED)
                            ((IJornalable)propertyState.Value).Status = JornalRecordStatus.CREATED;//Иначе назанчаем статус созданного 

                        PropertiesChangeJornal.Remove(propertyState); //Удаляем последнюю запись их журнала
                        ObjectChangeSaved?.Invoke(this, new ObjectStateChangedEventArgs("", this, propertyState));
                    }
                    else
                    {
                        propertyState.ParentObject.SaveAll(currentContextId);
                        PropertiesChangeJornal.Remove(propertyState);
                        ObjectChangeSaved?.Invoke(this, new ObjectStateChangedEventArgs("", this, propertyState));
                    }
                    /* List<PropertyStateRecord> recordsForDelete = //Очищаем журнал от записей
                                            PropertiesChangeJornal.Where(p => p.Id == (Guid)prop_id && p.ContextId == currentContextId).ToList();
                                       foreach (PropertyStateRecord record in recordsForDelete)
                                        {
                                            PropertiesChangeJornal.Remove(record);
                                        }*/
                    foreach (PropertyStateRecord record in propertyStateRecords)//Очищаем журнал от записей
                    {
                        PropertiesChangeJornal.Remove(record);
                        ObjectChangeSaved?.Invoke(this, new ObjectStateChangedEventArgs("", this, propertyState));
                    }
                }
            }
        }
        public void SaveAll(Guid currentContextId)
        {
            List<Guid> uniq_property_ids = //Получаем id элементов, которые подвергались изменениям
           PropertiesChangeJornal.Where(r => r.Value != null).GroupBy(g => g.ContextId).Select(x => x.First()).Select(jr => ((IEntityObject)jr.Value).Id).ToList();

            foreach (Guid prop_id in uniq_property_ids)
                Save(prop_id, currentContextId);
        }
        public void ClearChangesJornal()
        {
            PropertiesChangeJornal.Clear();
            PropertiesChangeJornal.ContextIdHistory.Clear();
            ParentObjects?.Clear();
            foreach (IEntityObject element in this)
            {
                if (element.PropertiesChangeJornal.Count != 0)
                    element.ClearChangesJornal();
            }
        }
        public bool IsPropertiesChangeJornalIsEmpty(Guid currentContextId)
        {
            //   var dsf = PropertiesChangeJornal.Where(r => r.Id == currentContextId).FirstOrDefault();
            if (PropertiesChangeJornal.Where(r => r.ContextId == currentContextId).FirstOrDefault() == null)
                return true;
            else
            {
                // if (jornal_empty) return true;
                //   else
                return false;
            }
        }

        public void AdjustAllParentsObjects()
        {

            AdjustParentObjects(this);
        }
        private void AdjustParentObjects(IJornalable sourse)
        {
            foreach (IEntityObject item in this)
            {
                if (Adjusted == false)
                {
                    if (item.ParentObjects == null)
                    {
                        item.ParentObjects = new ObservableCollection<IJornalable>();

                    }
                    item.ObjectChangedNotify += OnChildObjectChanges;
                    item.ObjectChangeSaved += OnChildObjectChangeSaved;
                    item.ObjectChangeUndo += OnChildObjectChangeUndo;
                    if (!item.ParentObjects.Contains(this))
                    {

                        item.ParentObjects.Add(this);
                    }

                   if(!item.Adjusted) item.AdjustAllParentsObjects();


                }
            }
        }
        public void ResetAllAdjustedParentObjects()
        {

        }
        #endregion

        public void SetAllValues(object in_object)
        {
            /*   var this_props = this.GetType().GetProperties();
               var in_props = in_object.GetType().GetProperties();

               foreach (PropertyInfo prop_info in this_props)
               {
                   var in_prop_value = prop_info.GetValue(in_object);
                   var this_prop_value = prop_info.GetValue(this);
                   var this_prop = this.GetType().GetProperty(prop_info.Name);

                   if (this_prop is BindableBase || this_prop is IEntityObject)
                         ((IEntityObject)this_prop_value).SetAllValues(in_prop_value);

                   if(this_prop.CanWrite) this_prop.SetValue(this, in_prop_value);
               }
               */
            Items.Clear();
            foreach (TEntity obj_item in ((NameableObservableCollection<TEntity>)in_object).Items)
                Items.Add(obj_item);
        }

        private void CountNotificationSet()
        {
            //   31.10.2022 PropertiesChangeJornal.JornalChangedNotify += OnPropertyChanges;
            CollectionChanged += (sender, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (IJornalable obj in e.NewItems)
                    {
                        obj.ObjectChangedNotify += OnChildObjectChanges;
                        obj.ObjectChangeSaved += OnChildObjectChangeSaved;
                        obj.ObjectChangeUndo += OnChildObjectChangeUndo;
                        if (obj.ParentObjects == null) obj.ParentObjects = new ObservableCollection<IJornalable>();
                        if (!obj.ParentObjects.Contains(this))
                            obj.ParentObjects.Add(this);
                        if (b_jornal_recording_flag && CurrentContextId != Guid.Empty)
                        {
                            List<Guid> AnatherWindowsIds = PropertiesChangeJornal.ContextIdHistory.Where(el => el != CurrentContextId).ToList();
                            foreach (Guid prev_wnd_is in AnatherWindowsIds)
                            {
                                PropertyStateRecord propertyStateRecord_1 = new PropertyStateRecord(obj, JornalRecordStatus.ADDED, ((IEntityObject)obj).Name, prev_wnd_is);
                                propertyStateRecord_1.ParentObject = (IEntityObject)this;
                                PropertiesChangeJornal.Add(propertyStateRecord_1);
                                ObjectChangedNotify?.Invoke(this, new ObjectStateChangedEventArgs("", this, propertyStateRecord_1));
                            }

                            PropertyStateRecord stateRecord = new PropertyStateRecord(obj, JornalRecordStatus.ADDED, ((IEntityObject)obj).Name, CurrentContextId);
                            stateRecord.ParentObject = this;
                            PropertiesChangeJornal.Add(stateRecord);
                            ObjectChangedNotify?.Invoke(this, new ObjectStateChangedEventArgs("", this, stateRecord));
                        }

                    }
                }
                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (IJornalable obj in e.OldItems)
                    {
                        obj.ObjectChangedNotify -= OnChildObjectChanges;
                        obj.ObjectChangeSaved -= OnChildObjectChangeSaved;
                        obj.ObjectChangeUndo -= OnChildObjectChangeUndo;
                        obj.ParentObjects.Remove(this);
                    }
                }


            };


        }

        public void SetParentObject(IJornalable obj)
        {
            ParentObject = obj;
        }


        public bool RemoveJournalable(TEntity item)
        {
            item.Status = JornalRecordStatus.REMOVED;
            List<Guid> AnatherWindowsIds = PropertiesChangeJornal.ContextIdHistory.Where(el => el != CurrentContextId).ToList();
            foreach (Guid prev_wnd_is in AnatherWindowsIds)
            {
                PropertyStateRecord propertyStateRecord_1 = new PropertyStateRecord(item, JornalRecordStatus.REMOVED, Name, prev_wnd_is);
                propertyStateRecord_1.ParentObject = (IEntityObject)this;
                PropertiesChangeJornal.Add(propertyStateRecord_1);
                PropertiesChangeJornal.Add(propertyStateRecord_1);
                ObjectChangedNotify?.Invoke(this, new ObjectStateChangedEventArgs("", this, propertyStateRecord_1));
            }
            PropertyStateRecord stateRecord = new PropertyStateRecord(item, JornalRecordStatus.REMOVED, Name, CurrentContextId);
            stateRecord.ParentObject = this;
            PropertiesChangeJornal.Add(stateRecord);
            ObjectChangedNotify?.Invoke(this, new ObjectStateChangedEventArgs("", this, stateRecord));
            return true;
        }
        public virtual object Clone<TSourse>(Func<TSourse, bool> predicate) where TSourse : IEntityObject
        {
            if (!CopingEnable)
                return null;

            object new_object_collection = Activator.CreateInstance(this.GetType());
            foreach (IEntityObject element in this)
            {
                var new_element = Activator.CreateInstance(element.GetType());
                new_element = element.Clone<TSourse>(predicate);
                ((IList)new_object_collection).Add(new_element);
            }
            //   Functions.SetAllIdToZero(new_object_collection);
            return new_object_collection;
        }




        public bool IsVisible { get; set; } = true;

        private string _code;
        public string Code
        {
            get
            {
                return _code;
            }
            set { _code = value; OnPropertyChanged("Code"); }
        }//Код
        public bool Adjusted { get; set; } = false;

        private bool _isPinterConteiner = false;

        public bool IsPointerContainer
        {
            get { return _isPinterConteiner; }
            set { _isPinterConteiner = value; }
        }

        public bool CopingEnable { get; set; } = true;




    }
}
