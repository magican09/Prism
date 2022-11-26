using PrismWorkApp.OpenWorkLib.Core;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo;
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
        public event PropertyChangedEventHandler PropertyBeforeChanged = delegate { };
        public event PropertyBeforeChangeEventHandler _PropertyBeforeChanged = delegate { };

        public event CollectionChangedEventHandler CollectionChangedBeforeRemove = delegate { };
        public event CollectionChangedEventHandler CollectionChangedBeforAdd = delegate { };


        public event ObjectStateChangeEventHandler ObjectChangedNotify;//Событие вызывается при изменении в данном объекте 
        public event ObjectStateChangeEventHandler ObjectChangeSaved; //Событие вызывается при сохранении изменений в данном объекте
        public event ObjectStateChangeEventHandler ObjectChangeUndo; //Событие вызывается при отмете изменений в данном объекте

        public virtual Func<IEntityObject, bool> RestrictionPredicate { get; set; } = x => true;//Предикат для ограничений при работе с данных объектом по умолчанию

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (b_jornal_recording_flag) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public void OnPropertyBeforChanged([CallerMemberName] string prop = "")
        {
            if (b_jornal_recording_flag) PropertyBeforeChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            set { OnPropertyBeforChanged("Id"); _id = value; OnPropertyChanged("Id"); }
        }
        private Guid _storedId;
        public Guid StoredId
        {
            get { return _storedId; }
            set { OnPropertyBeforChanged("Id"); _storedId = value; OnPropertyChanged("StoredId"); }
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { OnPropertyBeforChanged("Id"); _name = value; OnPropertyChanged("Name"); }
        }
        public NameableObservableCollection()
        {
            Id = Guid.NewGuid();

        }
        public NameableObservableCollection(string name) : this()
        {
            Id = Guid.NewGuid();
            Name = name;
        }
        public NameableObservableCollection(List<TEntity> list) : base(list)
        {
            Id = Guid.NewGuid();
        }
        public NameableObservableCollection(ICollection<TEntity> collection) : base(collection)
        {
            Id = Guid.NewGuid();

        }
        public NameableObservableCollection(IEnumerable<TEntity> entities) : base(entities)
        {
            Id = Guid.NewGuid();
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
        private JornalRecordType status;
        public JornalRecordType Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                IsVisible = (status != JornalRecordType.REMOVED) ? true : false;
            }
        }
        public PropertiesChangeJornal PropertiesChangeJornal { get; set; }
        public ObservableCollection<IJornalable> ParentObjects { get; set; }
        public ObservableCollection<IJornalable> ChildObjects { get; set; }
        public AdjustStatus AdjustedStatus { get; set; } = AdjustStatus.UNADJUSTED;

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
        #endregion

        //public bool RemoveItem(IJornalable item)
        //{
        //    base.Remove(item as TEntity);
        //    return true;
        //}
        //public bool Remove(IJornalable item, Guid currentContextId)
        //{
        //    this.Remove(item as TEntity, currentContextId);
        //    return true;
        //}

        //public bool Remove(TEntity item, Guid currentContextId)
        //{
        //    if (b_jornal_recording_flag) CollectionChangedBeforeRemove(this, new CollectionChangedEventArgs(item, currentContextId));
        //    return true;
        //}
        //public bool Remove(TEntity item)
        //{
        //    if (b_jornal_recording_flag) CollectionChangedBeforeRemove(this, new CollectionChangedEventArgs(item, CurrentContextId));
        //    return true;
        //}
        //public void Add(TEntity item, Guid currentContextId)//Если используется интерфейс iCollection(T)
        //{
        //    if (b_jornal_recording_flag) CollectionChangedBeforAdd(this, new CollectionChangedEventArgs(item, currentContextId));
        //    base.Add(item);
        //}
        //public void Add(TEntity item)//Если используется интерфейс iCollection(T)
        //{
        //    if (b_jornal_recording_flag) CollectionChangedBeforAdd(this, new CollectionChangedEventArgs(item, CurrentContextId));
        //    base.Add(item);

        //}
        //public int Add(object? value, Guid currentContextId) //Если используется интрефейс IList
        //{
        //    if (b_jornal_recording_flag) CollectionChangedBeforAdd(this, new CollectionChangedEventArgs(value as IJornalable, currentContextId));
        //    base.Add(value as TEntity);
        //    return this.IndexOf(value as TEntity);
        //}

        //public int Add(object? value) //Если используется интрефейс IList
        //{

        //    if (b_jornal_recording_flag) CollectionChangedBeforAdd(this, new CollectionChangedEventArgs(value as IJornalable, CurrentContextId));
        //    base.Add(value as TEntity);

        //    return this.IndexOf(value as TEntity);

        //}
        
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


        public bool IsPointerContainer { get; set; }
        public bool CopingEnable { get; set; } = true;




    }
}
