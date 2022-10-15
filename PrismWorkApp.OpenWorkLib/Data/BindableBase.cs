
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
    public abstract class BindableBase : INotifyPropertyChanged, IJornalable, IValidateable, IBindableBase, IAddable, ILevelable,ICopingEnableable
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

        public virtual void AllPropertiesModifyedChangeNotificate()
        {
            var properties = this.GetType().GetProperties();
            foreach (PropertyInfo pr_info in properties)
            {
                OnPropertyChanged(pr_info.Name);
            }
        }
        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate { };

        public System.Collections.IEnumerable GetErrors(string propertyName)
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

        protected bool SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = null, bool jornal_mode = false)
        {
            ValidateProperty(propertyName, val);


            if (b_jornal_recording_flag) //Регистрация сделанных изменений в журнал изменений
            {
                if (CurrentContextId != Guid.Empty) PropertiesChangeJornal.Add(new PropertyStateRecord(member, JornalRecordStatus.MODIFIED, propertyName, CurrentContextId));
            }

            return BaseSetProperty<T>(ref member, val, propertyName);
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

        private PropertiesChangeJornal PropertiesChangeJornal { get; set; } = new PropertiesChangeJornal();
        public bool IsPropertiesChangeJornalIsEmpty(Guid currentContextId)
        {
            //   var dsf = PropertiesChangeJornal.Where(r => r.Id == currentContextId).FirstOrDefault();
            if (PropertiesChangeJornal.Where(r => r.ContextId == currentContextId).FirstOrDefault() == null)
                return true;
            else
                return false;
        }
        private Guid _currentContextId;

        [NotMapped]
        public Guid CurrentContextId
        {
            get { return _currentContextId; }
            set
            {
                var prop_info_coll = this.GetType().GetProperties();
                b_jornal_recording_flag = false;
                foreach (PropertyInfo propertyInfo in prop_info_coll)
                {
                    var prop_val = propertyInfo.GetValue(this);
                    if (prop_val is IList && prop_val is IJornalable)
                    {
                        (prop_val as IJornalable).CurrentContextId = CurrentContextId;
                    }
                }
                b_jornal_recording_flag = true;
                _currentContextId = value;

            }
        }

        private JornalRecordStatus status;
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
                IsVisible = (status == JornalRecordStatus.CREATED) ? true : false;
            }
        }
        private bool visible = true;
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
        public virtual void Save(object prop_id, Guid currentContextId)
        { //Сохранение текущего состояние свойства заключается в том, что мы просто удаляем все предыдущие изменнеия из журнала изменений
            if (prop_id != null)
            {
                List<PropertyStateRecord> propertyStateRecords =
                        PropertiesChangeJornal.Where(p => p.Name == prop_id.ToString() && p.ContextId == currentContextId).ToList();
                foreach (PropertyStateRecord record in propertyStateRecords)
                    PropertiesChangeJornal.Remove(record);
            }
        }

        public virtual void SaveAll(Guid currentContextId)
        {
            List<string> uniq_property_names = //Получаем имена свойство, которые подвергались изменениям
                    PropertiesChangeJornal.Select(k => k.Name)
                    .GroupBy(g => g)
                    .Where(c => c.Count() == 1)
                    .Select(k => k.Key)
                    .ToList();
            foreach (string prop_name in uniq_property_names)//Сохраняем последние изменения для каждого свойства
                Save(prop_name, currentContextId);
            /// Если свойства представляет собой список, то побегаем по всем элементам списка и сохраняем изменения в каждом эелемента
            var prop_info_coll = this.GetType().GetProperties();
            b_jornal_recording_flag = false;
            foreach (PropertyInfo propertyInfo in prop_info_coll)
            {
                var prop_val = propertyInfo.GetValue(this);
                if (prop_val is IList && prop_val is IJornalable)
                {
                    MethodInfo saveAllMethod = propertyInfo.PropertyType.GetMethod("SaveAll");//Вызываем методо SaveAll каждого элмената
                    object res = saveAllMethod.Invoke(prop_val, new object[] { currentContextId });
                }
            }
            b_jornal_recording_flag = true;
        }

        private bool b_jornal_recording_flag = true;

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
        public virtual void UnDo(Guid currentContextId)
        {
            PropertyStateRecord propertyState = PropertiesChangeJornal.Where(r => r.ContextId == currentContextId)?
                .OrderBy(r => r.Date).LastOrDefault();//Берем последене измение в свойствах объекта
            if (propertyState != null)
            {
                var prop_info = this.GetType().GetProperty(propertyState.Name); //Достаем с помощью рефлексии данные о свойстве из текущего объекта
                b_jornal_recording_flag = false; //Отключаем ведение жрунала, так как собираемся переустановить текущее значение без журналирования

                if (prop_info.GetIndexParameters().Length == 0)//Если свойство не является индек..
                {
                    var prop = prop_info.GetValue(this);
                    prop_info.SetValue(this, propertyState.Value); //Присваиваем свойству текущего объекта сохраненное в журнале значение
                }

                PropertiesChangeJornal.Remove(propertyState); //Удаляем из журнала сохраненное изменение
                b_jornal_recording_flag = true; //Включаем журналирование
            }
        }

        public virtual void UnDoAll(Guid currentContextId)
        {
            while (PropertiesChangeJornal.Where(r => r.ContextId == currentContextId).FirstOrDefault() != null) UnDo(currentContextId); //Отменяем измеенения всех переменных, которые не коллекции
            b_jornal_recording_flag = false;
            var prop_info_coll = this.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in prop_info_coll)
            {
                var prop_val = propertyInfo.GetValue(this);
                if (prop_val is IList && prop_val is IJornalable)
                {
                    MethodInfo unDoAllMethod = propertyInfo.PropertyType.GetMethod("UnDoAll");
                    object res = unDoAllMethod.Invoke(prop_val, new object[] { currentContextId });
                }
            }
            b_jornal_recording_flag = true;

        }


        public void Add(object obj)
        {
            var prop_infoes = this.GetType().GetRuntimeProperties().Where(pr => pr.GetIndexParameters().Length == 0);
            Type obj_type = obj.GetType();
            int level_strutures_count = 0;

            foreach (PropertyInfo prop_info in prop_infoes)
            {
                var prop_value = prop_info.GetValue(this);
                if (prop_value is ILevelable)
                    level_strutures_count++;

                if (prop_value is IList)
                {
                    var genericArgumentsType = prop_value.GetType().GetInterface("IEnumerable`1").GetGenericArguments()[0];
                    if (genericArgumentsType == obj_type)
                    {
                        ((IList)prop_value).Add(obj);

                        if (prop_value is ILevelable)
                        {
                            ((ILevelable)prop_value).StructureLevel.ParentStructureLevel = StructureLevel;
                        }
                    }
                ((ILevelable)prop_value).UpdateStructure();
                }
            }

        }
        [NotMapped]
        public bool CopingEnable { get; set; } = true;
        public virtual void SetCopy<TSourse>(object pointer, Func<TSourse, bool> predicate)
            where TSourse:IEntityObject
        {
            Functions.CopyObjectReflectionNewInstances(this, pointer, predicate);
            Functions.SetAllIdToZero(pointer);
        }
        
        public void UpdateStructure()
        {
            if (StructureLevel.Status != StructureLevelStatus.DEFINED)
            {
                StructureLevel.Level = StructureLevel.ParentStructureLevel.Number;
                StructureLevel.Value = this;
                StructureLevel.DeptIndex = StructureLevel.ParentStructureLevel.DeptIndex+1;
                StructureLevel.Status = StructureLevelStatus.IN_PROCESS;
                var prop_infoes = this.GetType().GetRuntimeProperties().Where(pr => pr.GetIndexParameters().Length == 0);
                int level_strutures_count = 0;

                foreach (PropertyInfo prop_info in prop_infoes)
                {
                    var prop_value = prop_info.GetValue(this);
                    if (prop_value is ILevelable)
                        level_strutures_count++;

                    if (prop_value is ILevelable && ((ILevelable)prop_value).StructureLevel.Status != StructureLevelStatus.DEFINED)
                    {
                        ((ILevelable)prop_value).StructureLevel.ParentStructureLevel = StructureLevel;
                        ((ILevelable)prop_value).StructureLevel.Number = level_strutures_count++;
                        //   ((ILevelable)prop_value).Code = $"{((ILevelable)prop_value).StructureLevel.Level.ToString()}.{ ((ILevelable)prop_value).StructureLevel.Number.ToString()}";
                        ((ILevelable)prop_value).StructureLevel.Value = prop_value;
                      //  ((ILevelable)prop_value).StructureLevel.Status = StructureLevelStatus.IN_PROCESS;
                        ((ILevelable)prop_value).UpdateStructure();
                        //((ILevelable)prop_value).StructureLevel.Status = StructureLevelStatus.DEFINED;

                    }
                    StructureLevel.Status = StructureLevelStatus.DEFINED;
                }
              
            }
            OnPropertyChanged("Code"); 
        }

        public void ClearStructureLevel()
        {
            var prop_infoes = this.GetType().GetRuntimeProperties().Where(pr => pr.GetIndexParameters().Length == 0);
            StructureLevel.Status = StructureLevelStatus.IN_PROCESS;
            foreach (PropertyInfo prop_info in prop_infoes)
            {
                var prop_value = prop_info.GetValue(this);
                if (prop_value is ILevelable && ((ILevelable)prop_value).StructureLevel.Status!= StructureLevelStatus.UN_DEFINED)
                {
                    ((ILevelable)prop_value).StructureLevel.Status  =  StructureLevelStatus.IN_PROCESS;
                    ((ILevelable)prop_value).StructureLevel.StructureLevels.Clear();
                    ((ILevelable)prop_value).ClearStructureLevel();
                }

            }
            StructureLevel.Status = StructureLevelStatus.UN_DEFINED;
        }

        private StructureLevel _structureLevel = new StructureLevel();
        [NotMapped]
        public StructureLevel StructureLevel
        {
            get { return _structureLevel; }
            set { SetProperty(ref _structureLevel, value); }
        }
        private string _code;
        public string Code
        {
            get {

                // return $"{StructureLevel.ParentStructureLevel.Level.ToString()}.{StructureLevel.Level.ToString()}.{StructureLevel.Number.ToString()}";
                return StructureLevel.Code;
            }
            set { SetProperty(ref _code, value); }
        }//Код

   
    }


}

