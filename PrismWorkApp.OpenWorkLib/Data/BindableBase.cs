
using PrismWorkApp.OpenWorkLib.Core;
using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;


namespace PrismWorkApp.OpenWorkLib.Data
{
    public abstract class BindableBase : INotifyPropertyChanged, IJornalable, IValidateable, IBindableBase, ICopingEnableable
    {

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        protected virtual bool BaseSetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = "")
        {

            if (object.Equals(val, member)) return false;
            member = val;
            //Type tp = Children[Children.Count - 1].GetType();
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
        protected bool SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = null, bool jornal_mode = false)
        {
            ValidateProperty(propertyName, val);


            if (b_jornal_recording_flag) //Регистрация сделанных изменений в журнал изменений
            {
                if (CurrentContextId != Guid.Empty)
                {
                    if (val == null)
                        ;
                    List<Guid> AnatherWindowsIds = PropertiesChangeJornal.ContextIdHistory.Where(el => el != CurrentContextId).ToList(); //Ищем в журнале Id других окон в цепоче изменений
                    foreach (Guid prev_wnd_is in AnatherWindowsIds)
                    {
                        PropertyStateRecord propertyStateRecord_1 = new PropertyStateRecord(member, JornalRecordStatus.MODIFIED, propertyName, prev_wnd_is);
                        propertyStateRecord_1.ParentObject = (IEntityObject)this;
                        PropertiesChangeJornal.Add(propertyStateRecord_1);
                    }

                    PropertyStateRecord propertyStateRecord = new PropertyStateRecord(member, JornalRecordStatus.MODIFIED, propertyName, CurrentContextId);
                    propertyStateRecord.ParentObject = (IEntityObject)this;
                    PropertiesChangeJornal.Add(propertyStateRecord);


                    if (ObjectChangedNotify != null)
                        ObjectChangedNotify(this, new ObjectStateChangedEventArgs("", this, propertyStateRecord));
                }
            }

            return BaseSetProperty<T>(ref member, val, propertyName);
        }


        #region Validating
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate { };
        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
        public IEnumerable GetErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
                return _errors[propertyName];
            else
                return null;
        }
        public bool HasErrors
        {
            get { return _errors.Count > 0; }
        }
        private void ValidateProperty<T>(string propertyName, T value)
        {
            var results = new List<ValidationResult>();
            ValidationContext context = new ValidationContext(this);
            context.MemberName = propertyName;
            Validator.TryValidateProperty(value, context, results);

            if (results.Any())
            {

                _errors[propertyName] = results.Select(c => c.ErrorMessage).ToList();
            }
            else
            {
                _errors.Remove(propertyName);
            }
            ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }
        #endregion

        #region Changes Jornaling

        private bool b_jornal_recording_flag = true;
        private bool visible = true;
        private JornalRecordStatus status;
        private Guid _currentContextId;
        [NotMapped]
        public Guid CurrentContextId
        {
            get { return _currentContextId; }
            set
            {
                var prop_info_coll = this.GetType().GetProperties(); //Устанавливается при инициализации в LocalBindableBase  и его потомков
                b_jornal_recording_flag = false;
                foreach (PropertyInfo propertyInfo in prop_info_coll)
                {
                    var prop_val = propertyInfo.GetValue(this);
                    if (prop_val is IList && prop_val is IJornalable) //Если в свойствах есть коллекции, то устанавливаем их CurrentContextId 
                    {
                        (prop_val as IJornalable).CurrentContextId = CurrentContextId;
                    }
                }
                b_jornal_recording_flag = true;
                if (!PropertiesChangeJornal.ContextIdHistory.Contains(value))
                    PropertiesChangeJornal.ContextIdHistory.Add(value);
                _currentContextId = value;

            }
        }

        public PropertiesChangeJornal _propertiesChangeJornal;
        [NotMapped]
        public PropertiesChangeJornal PropertiesChangeJornal
        {
            get { return _propertiesChangeJornal; }
            set { _propertiesChangeJornal = value; }
        }

        [NotMapped]
        public bool IsVisible
        {
            get { return visible; }
            set
            {
                b_jornal_recording_flag = false;
                SetProperty(ref visible, value);
                b_jornal_recording_flag = true;
            }
        }
        [NotMapped]
        public ObservableCollection<IJornalable> ParentObjects { get; set; }
        [NotMapped]
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

        public virtual void AllPropertiesModifyedChangeNotificate()
        {
            var properties = this.GetType().GetProperties();
            foreach (PropertyInfo pr_info in properties)
            {
                OnPropertyChanged(pr_info.Name);
            }
        }
        public bool IsPropertiesChangeJornalIsEmpty(Guid currentContextId)
        {
            //   var dsf = PropertiesChangeJornal.Where(r => r.Id == currentContextId).FirstOrDefault();
            // if (PropertiesChangeJornal.Where(r => r.ContextId == currentContextId).FirstOrDefault() == null)
            if (PropertiesChangeJornal.Count == 0)
                return true;
            else
            {
                // if (jornal_empty) return true;
                //   else
                return false;
            }
        }

        public event ObjectStateChangeEventHandler ObjectChangedNotify;
        public void OnChildObjectChanges(object sender, ObjectStateChangedEventArgs e)
        {
            PropertiesChangeJornal.Add(e.PropertyStateRecord);
        }

        private void OnPropertyChanges(object sender, PropertyStateRecord _propertyStateRecord)
        {

            Status = JornalRecordStatus.MODIFIED;
            if (ObjectChangedNotify != null)
                ObjectChangedNotify(this, new ObjectStateChangedEventArgs("", this, _propertyStateRecord));
            //  return true;
        }



        public void SetParentObject(IEntityObject obj)
        {
        }
        public void JornalingOff()
        {
            b_jornal_recording_flag = false;

            var prop_info_coll = this.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in prop_info_coll)
            {
                var prop_val = propertyInfo.GetValue(this);
                if (prop_val is IList && prop_val is IJornalable)
                {
                    MethodInfo unDoAllMethod = propertyInfo.PropertyType.GetMethod("JornalingOff");
                    object res = unDoAllMethod.Invoke(prop_val, new object[] { });

                }
            }
        }
        public void JornalingOn()
        {
            b_jornal_recording_flag = true;
            var prop_info_coll = this.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in prop_info_coll)
            {
                var prop_val = propertyInfo.GetValue(this);
                if (prop_val is IList && prop_val is IJornalable)
                {
                    MethodInfo unDoAllMethod = propertyInfo.PropertyType.GetMethod("JornalingOn");
                    object res = unDoAllMethod.Invoke(prop_val, new object[] { });
                }
            }
        }
        public void ClearChangesJornal()
        {
            var prop_infos = this.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in prop_infos)
            {
                var prop_val = propertyInfo.GetValue(this);

                if (prop_val is IJornalable && (prop_val as IJornalable).PropertiesChangeJornal.Count!=0)
                {
                    MethodInfo unDoAllMethod = propertyInfo.PropertyType.GetMethod("ClearChangesJornal");
                    object res = unDoAllMethod.Invoke(prop_val, new object[] { });
                }
            }
            PropertiesChangeJornal.Clear();
            PropertiesChangeJornal.ContextIdHistory.Clear();
        }
        public virtual void UnDo(PropertyStateRecord propertyState)
        {
            if (propertyState != null)
            {
                if (propertyState.ParentObject.Id == (this as IEntityObject).Id)//Изменения были в дочернем объекте текущего отбъекта
                {
                    var prop_info = this.GetType().GetProperty(propertyState.Name); //Достаем с помощью рефлексии данные о свойстве из текущего объекта
                    b_jornal_recording_flag = false; //Отключаем ведение жрунала, так как собираемся переустановить текущее значение без журналирования
                    var prop_val = prop_info.GetValue(this);

                    if (prop_info.GetIndexParameters().Length == 0)//Если свойство не является индек..
                    {
                        prop_info.SetValue(this, propertyState.Value); //Присваиваем свойству текущего объекта сохраненное в журнале значение
                    }

                    PropertiesChangeJornal.Remove(propertyState); //Удаляем из журнала сохраненное изменение

                    b_jornal_recording_flag = true; //Включаем журналирование
                }
                else
                {
                    propertyState.ParentObject.UnDo(propertyState);
                    PropertiesChangeJornal.Remove(propertyState);
                }
            }
        }
        public virtual void UnDoAll(Guid currentContextId)
        {
            List<PropertyStateRecord> records =
              PropertiesChangeJornal.Where(pr => pr.ContextId == currentContextId).OrderByDescending(pr => pr.Date).ToList();
            foreach (PropertyStateRecord record in records)
                UnDo(record); //Отменяем измеенения всех переменных, которые не коллекции


        }
        public virtual void Save(object prop_id, Guid currentContextId)
        { //Сохранение текущего состояние свойства заключается в том, что мы просто удаляем все предыдущие изменнеия из журнала изменений
            if (prop_id != null)
            {
                PropertyStateRecord propertyState;
                if (prop_id is string)
                {
                    propertyState = PropertiesChangeJornal.Where(p => p.Name == prop_id.ToString() && p.ContextId == currentContextId).OrderBy(pr => pr.Date).LastOrDefault();
                    if (propertyState != null)
                    {
                        if (propertyState.Value is IEntityObject)
                        {
                            propertyState.ParentObject.SaveAll(currentContextId);
                            PropertiesChangeJornal.Remove(propertyState);
                        }
                        else
                            PropertiesChangeJornal.Remove(propertyState);
                    }
                }
                else
                {
                    propertyState = PropertiesChangeJornal.Where(p => p.Id == (Guid)prop_id).OrderBy(pr => pr.Date).LastOrDefault();

                    if (propertyState.ParentObject.Id == (this as IEntityObject).Id)//Изменения были в дочернем объекте текущего отбъекта
                    {
                        PropertiesChangeJornal.Remove(propertyState);
                    }
                    else
                    {
                        propertyState.ParentObject.SaveAll(currentContextId);
                        PropertiesChangeJornal.Remove(propertyState);
                    }

                    List<PropertyStateRecord> recordsFoDelete =
                        PropertiesChangeJornal.Where(p => p.Name == prop_id.ToString() && p.ContextId == currentContextId).ToList();
                    foreach (PropertyStateRecord record in recordsFoDelete)
                    {
                        PropertiesChangeJornal.Remove(record);
                    }
                }

                /*for (int ii = 0; ii < PropertiesChangeJornal.Count; ii++)  //Удаляем все записи с изменениями сохраняемого объекта
                    if (PropertiesChangeJornal[ii].Id == propertyState.Id)
                        PropertiesChangeJornal.Remove(PropertiesChangeJornal[ii]);*/
            }
        }
        public virtual void SaveAll(Guid currentContextId)
        {
            /* List<string> uniq_property_names = //Получаем имена свойство, которые подвергались изменениям
                     PropertiesChangeJornal.Select(k => k.Name)
                     .GroupBy(g => g)
                     .Where(c =>c.Count() == 1)
                     .Select(k => k.Key)
                     .ToList();*/
            List<string> uniq_property_names = //Получаем имена свойство, которые подвергались изменениям
                     PropertiesChangeJornal.GroupBy(g => g.Name).Select(x => x.First()).Select(jr => jr.Name).ToList();
            foreach (string prop_name in uniq_property_names)//Сохраняем последние изменения для каждого свойства
                Save(prop_name, currentContextId);



        }

        public void AdjustAllParentsObjects()
        {
            AdjustParentObjects(this);
        }
        private void AdjustParentObjects(IJornalable sourse)
        {
            var all_props_propinfos = sourse.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);
            foreach (PropertyInfo prop_info in all_props_propinfos)
            {
                var prop_value = prop_info.GetValue(sourse);
                if (prop_value is IJornalable && prop_value != null)
                {
                    if ((prop_value as IJornalable).ParentObjects == null)
                    {
                        (prop_value as IJornalable).ParentObjects = new ObservableCollection<IJornalable>();
                        (prop_value as IJornalable).ParentObjects.Add(sourse);
                        if ((prop_value is IList))
                        {
                            (prop_value as IJornalable).ObjectChangedNotify += OnChildObjectChanges;
                            (prop_value as IJornalable).AdjustAllParentsObjects();
                        }
                        else
                        {
                            (prop_value as IJornalable).ObjectChangedNotify += OnChildObjectChanges;
                            AdjustParentObjects((prop_value as IJornalable));

                        }

                    }
                }


            }


        }
        #endregion


        public BindableBase()
        {
            PropertiesChangeJornal = new PropertiesChangeJornal();
            PropertiesChangeJornal.ParentObject = this;
            PropertiesChangeJornal.JornalChangedNotify += OnPropertyChanges;
        }
        private string _code;
        public string Code
        {
            get
            {
                return _code;
                //return StructureLevel.Code;
            }
            set { SetProperty(ref _code, value); }
        }//Код

        [NotMapped]
        public virtual Func<IEntityObject, bool> RestrictionPredicate { get; set; } = x => true;//Предикат для ограничений при работе (наприме копирования рефлексией) с данныv объектом по умолчанию 
        [NotMapped]
        public bool CopingEnable { get; set; } = true;


        public virtual void SetCopy<TSourse>(object pointer, Func<TSourse, bool> predicate)
            where TSourse : IEntityObject
        {
            Functions.CopyObjectReflectionNewInstances(this, pointer, predicate);
            Functions.SetAllIdToZero(pointer);
        }
        public virtual object Clone<TSourse>(Func<TSourse, bool> predicate) where TSourse : IEntityObject
        {
            if (!CopingEnable)
                return null;
            object new_object = Activator.CreateInstance(this.GetType());
            Functions.GetCopyEntitityObject(this, new_object, predicate);
            Functions.SetAllIdToZero(new_object);
            return new_object;
        }



    }


}

