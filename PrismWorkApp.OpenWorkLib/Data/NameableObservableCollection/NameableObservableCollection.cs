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
    public class NameableObservableCollection<TEntity> : ObservableCollection<TEntity>, IEntityObject,IJornalable, ILevelable, INameableOservableCollection<TEntity> where TEntity : class, IEntityObject
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
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

        private bool b_jornal_recording_flag = true;
        private void CountNotificationSet()
        {
            CollectionChanged += (sender, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    //  int last_struyctere_number = StructureLevel.StructureLevels.Count>0 ? StructureLevel.StructureLevels[StructureLevel.StructureLevels.Count-1].Number :0 ;

                    foreach (object obj in e.NewItems)
                    {
                        if (b_jornal_recording_flag && CurrentContextId != Guid.Empty)
                            PropertiesChangeJornal.Add(new PropertyStateRecord(obj, JornalRecordStatus.ADDED, Name, CurrentContextId));


                        /*    if (obj is ILevelable )
                            {
                                if (((ILevelable)obj).StructureLevel == null)  
                                    ((ILevelable)obj).StructureLevel = new StructureLevel();

                                ((ILevelable)obj).StructureLevel.Level = StructureLevel.Number;
                                ((ILevelable)obj).StructureLevel.Number = ++last_struyctere_number;
                            }*/
                    }
                }
                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (object obj in e.OldItems)
                    {
                        //if (b_jornal_recording_flag)
                        //{
                        //    PropertiesChangeJornal.Add(new PropertyStateRecord(obj, JornalRecordStatus.REMOVED, Name));
                        //}

                    }
                }
            };

            PropertyChanged += (sende, e) =>
              {

              };
        }

        public bool RemoveJournalable(TEntity item)
        {
            item.Status = JornalRecordStatus.REMOVED;
            PropertiesChangeJornal.Add(new PropertyStateRecord(item, JornalRecordStatus.REMOVED, Name, CurrentContextId));
            return true;
        }

        public void UnDo(Guid currentContextId)
        {
            PropertyStateRecord lastPropState = PropertiesChangeJornal.Where(r => r.ContextId == currentContextId)
                .OrderBy(el => el.Date).Last();
            if (lastPropState != null)
            {
                b_jornal_recording_flag = false; //Блокируем ведение журнала 
                if (lastPropState.Status == JornalRecordStatus.ADDED)
                {
                    // this.Remove((TEntity)lastPropState.Value);
                    if (this.Contains((TEntity)lastPropState.Value))
                    {
                        ((TEntity)lastPropState.Value).Status = JornalRecordStatus.REMOVED;
                        PropertiesChangeJornal.Remove(lastPropState);
                    }
                }
                else if (lastPropState.Status == JornalRecordStatus.REMOVED)

                {
                    //this.Add((TEntity)lastPropState.Value);
                    ((TEntity)lastPropState.Value).Status = JornalRecordStatus.CREATED;
                    PropertiesChangeJornal.Remove(lastPropState);
                }
                b_jornal_recording_flag = true;
            }


        }
        public void UnDoAll(Guid currentContextId)
        {
            while (PropertiesChangeJornal.Count > 0) UnDo(currentContextId); //Отменяем   все изменения
        }
        public virtual void Save(object prop_id, Guid currentContextId)
        {
            if ((Guid)prop_id != Guid.Empty)
            {
                List<PropertyStateRecord> propertyStateRecords =
                        PropertiesChangeJornal.Where(p => ((IKeyable)p.Value).Id == (Guid)prop_id && p.ContextId == currentContextId).ToList(); // Находим все записи о объекте в журнале
                PropertyStateRecord last_record = propertyStateRecords.OrderBy(r => r.Date).LastOrDefault(); //Выбираем самую последнюю
                if (last_record != null)
                {
                    if (((IJornalable)last_record?.Value).Status == JornalRecordStatus.REMOVED) //Если удалит - удаляем
                        this.Remove((TEntity)last_record.Value);
                    //   ((IJornalable)last_record.Value).Status = JornalRecordStatus.REMOVED;// 

                    else if (((IJornalable)last_record?.Value).Status == JornalRecordStatus.CREATED)
                        ((IJornalable)last_record.Value).Status = JornalRecordStatus.CREATED;//Иначе назанчаем статус созданного 

                    // foreach (PropertyStateRecord record in propertyStateRecords) //Очищаем журнал от записаей данного объекта
                    PropertiesChangeJornal.Remove(last_record); //Удаляем последнюю запись их журнала
                }
            }
        }

        public void SaveAll(Guid currentContextId)
        {
            List<Guid> uniq_property_id =
                    PropertiesChangeJornal.Select(k => (k.Value as IKeyable).Id)
                    .GroupBy(g => g)
                    .Where(c => c.Count() == 1)
                    .Select(k => k.Key)
                    .ToList();
            foreach (Guid prop_id in uniq_property_id)
                Save(prop_id, currentContextId);

            /* foreach (TEntity entity in this)
             {
                 entity.SaveAll();
             }
             */
        }

        public void JornalingOff()
        {
            b_jornal_recording_flag = false;
            foreach (TEntity entity in this)
            {
                entity.JornalingOff();
            }
        }

        public void JornalingOn()
        {
            b_jornal_recording_flag = true;
            foreach (TEntity entity in this)
            {
                entity.JornalingOn();
            }
        }

        public void UpdateStructure()
        {
            if (StructureLevel.Status == StructureLevelStatus.UN_DEFINED)
            {
                if (StructureLevel.ParentStructureLevel != null)
                {
                    StructureLevel.Level = StructureLevel.ParentStructureLevel.Number;
                    StructureLevel.DeptIndex = StructureLevel.ParentStructureLevel.DeptIndex+1;
                }
                else
                    StructureLevel.ParentStructureLevel = new StructureLevel(0);

                StructureLevel.Status = StructureLevelStatus.IN_PROCESS;
                int level_strutures_count = 0;
                if (this is bldObjectsGroup)
                    ;
                foreach (ILevelable elm in this)
                {
                  if (elm.StructureLevel.Status == StructureLevelStatus.UN_DEFINED)
                    {
                         
                        elm.StructureLevel.ParentStructureLevel = StructureLevel;
                        elm.StructureLevel.Level = StructureLevel.Number;
                        elm.StructureLevel.Number = level_strutures_count++;
                        elm.StructureLevel.Value = elm;
                        elm.StructureLevel.Status = StructureLevelStatus.IN_PROCESS;
                        StructureLevel.StructureLevels.Add(elm.StructureLevel);
                        if (elm is bldWork)
                        {
                            if ((elm as bldWork).bldConstruction != null)
                            {
                                var wrks = (elm as bldWork).bldConstruction.Works;
                            }
                         }

                    }
                }
                foreach (ILevelable elm in this)
                {

                   if (elm.StructureLevel.Status != StructureLevelStatus.DEFINED)
                    {
                        elm.StructureLevel.Status = StructureLevelStatus.IN_PROCESS;
                        elm.UpdateStructure();
                        elm.StructureLevel.Status = StructureLevelStatus.DEFINED;
                    }
                }
                StructureLevel.Status = StructureLevelStatus.DEFINED;
            }
            OnPropertyChanged("Code");
        }
        public void ClearStructureLevel()
        {
            StructureLevel.StructureLevels.Clear();
            StructureLevel.Status = StructureLevelStatus.IN_PROCESS;
            foreach (ILevelable elm in this)
            {

                if (elm.StructureLevel.Status != StructureLevelStatus.UN_DEFINED)
                {
                    elm.ClearStructureLevel();
                }
            }
            StructureLevel.Status = StructureLevelStatus.UN_DEFINED;
        }
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate { };
        private PropertiesChangeJornal PropertiesChangeJornal { get; set; } = new PropertiesChangeJornal();
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
                IsVisible = (status == JornalRecordStatus.CREATED) ? true : false;
            }
        }

        public bool IsVisible { get; set; } = true;
        private Guid _currentContextId;

        public Guid CurrentContextId
        {
            get { return _currentContextId; }
            set { _currentContextId = value; }
        }

        public StructureLevel StructureLevel { get; set; } = new StructureLevel();
        private string _code;
        public string Code
        {
            get {
                // return  StructureLevel.Code;
                return _code;
            }
            set { _code = value; OnPropertyChanged("Code"); }
        }//Код

        private bool _isPinterConteiner =false;

        public bool IsPointerContainer
        {
            get { return _isPinterConteiner; }
            set { _isPinterConteiner = value; }
        }

        public bool CopingEnable { get; set; } = true;

        public object ParentObject { get; set; }
    }
}
