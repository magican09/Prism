using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Core
{
    public static class Functions
    {
        public static Dictionary<Guid, TreeObjectInfo> ObjectsTreeMetaData { get; set; } = new Dictionary<Guid, TreeObjectInfo>();//Каталог всех объектов внутри объекта
        public static Dictionary<Guid, TreeObjectInfo> AddObjectsTreeMetaData { get; set; } = new Dictionary<Guid, TreeObjectInfo>();
        public static Dictionary<Guid, TreeObjectInfo> CiclingObjectsCatalog { get; set; } = new Dictionary<Guid, TreeObjectInfo>();//Каталог всех объектов внутри объекта
        public static int Recursive_depth { get; set; } = 0;//Глубина рекурсии
        public static int NavigateParametrDepth { get; set; } = 0;//Глубина навигационного свойства
        public static int InitialRecursive_depth { get; set; } = 0;
        public static int Cicling_recursive_depth { get; set; } = 0;//Глубина рекурсии

        public static void CopyObjectReflectionNewInstances<TSourse>(object sourse, object target, Func<TSourse, bool> predicate, bool objectsTreeCatalogReset = true)
        where TSourse : IEntityObject
        {

            if (sourse == null) { target = null; return; } //Если источник не инециализирован - выходим

            if (target == null) //Если  цель не инициализирована - создаем в соотвесвии с иточником
                target = Activator.CreateInstance(sourse.GetType());

            Guid sourse_parsing_id = Guid.Empty;
            if (GetParsingId(sourse) != Guid.Empty) //Определяем Id - сперва ищем в источнике, потом в приемнике (последовательно)
                sourse_parsing_id = GetParsingId(sourse);
            else if (GetParsingId(target) != Guid.Empty)
                sourse_parsing_id = GetParsingId(target);

            if (objectsTreeCatalogReset) //Сбрасываем таблица с данными уже скопированных объектов, счетчик глубины рекурсии и навигациооного объекта, если уставновлен флаг
            {
                ObjectsTreeMetaData.Clear();
                AddObjectsTreeMetaData.Clear();
                Recursive_depth = 0;
                NavigateParametrDepth = 0;
                GetObjectsFullCatalog(sourse, target, ObjectsTreeMetaData);//Создать каталог всех объектов входящих в объект и( объектов на которые есть ссылки ??)
                Dictionary<Guid, TreeObjectInfo> up_nav_tree = new Dictionary<Guid, TreeObjectInfo>(); //Каталог навигациорных свойств 
                if (GetNavigateProperties(sourse).Count > 0)
                    GetNavigatePropertyUpTree(sourse, up_nav_tree);
                else if (GetNavigateProperties(target).Count > 0)
                    GetNavigatePropertyUpTree(target, up_nav_tree);

                foreach (var up_nav_prop in up_nav_tree)
                    foreach (var tr_obj in ObjectsTreeMetaData.Where(val => val.Key == up_nav_prop.Key))
                        tr_obj.Value.IsFirstNavigate = true;

            }
            Recursive_depth++;

            if (!AddObjectsTreeMetaData.ContainsKey(sourse_parsing_id))
                AddObjectsTreeMetaData.Add(sourse_parsing_id, new TreeObjectInfo(Recursive_depth, sourse, target, ((IEntityObject)target).Name, false, StatusObjectsInTree.LINK_PROCESSED));

            var target_props = target.GetType().GetProperties() //Выбираем все не идексные свойства
                    .Where(p => p.GetIndexParameters().Length == 0);
            var sourse_props = sourse.GetType().GetProperties()
                    .Where(p => p.GetIndexParameters().Length == 0);

            var target_props_1 = target.GetType().GetProperties().Where(pr => pr.GetType() is IList);
            foreach (PropertyInfo target_prop_info in target_props)
            {
                var sourse_prop_info = sourse_props.FirstOrDefault(p => p.Name == target_prop_info.Name);

                if (sourse_prop_info != null && sourse_prop_info.CanWrite == true)
                {
                    var sourse_prop_value = sourse_prop_info.GetValue(sourse);
                    var target_prop_value = target_prop_info.GetValue(target);
                    var new_target_prop_value = target_prop_info.GetValue(target);

                    var target_prop = target.GetType().GetProperty(target_prop_info.Name);
                    var sourse_prop = sourse.GetType().GetProperty(sourse_prop_info.Name);

                    bool sourse_prop_is_navigate_prop = GetNavigateProperties(sourse).Where(pr => pr.Name == sourse_prop_info?.Name).FirstOrDefault() != null;
                    bool taget_prop_is_navigate_prop = GetNavigateProperties(target).Where(pr => pr.Name == sourse_prop_info?.Name).FirstOrDefault() != null;

                    bool set_value_skip_flag = false;
                    if (!predicate.Invoke((TSourse)sourse))
                        ;
                    if ((sourse_prop_value is IEntityObject || target_prop_value is IEntityObject)) //Если свойство является объектом..&&!(sourse is IList)
                    {
                        Guid ParsingId = Guid.Empty;
                        if (GetParsingId(sourse_prop_value) != Guid.Empty)
                            ParsingId = GetParsingId(sourse_prop_value);
                        else if (GetParsingId(target_prop_value) != Guid.Empty)
                            ParsingId = GetParsingId(target_prop_value);
                        if (predicate.Invoke((TSourse)sourse))
                        {
                            if (!ObjectsTreeMetaData[ParsingId].IsFirstNavigate) //Если свойтва не навигационное ссылающеся на верние уровни ...
                            {
                                if (!AddObjectsTreeMetaData.ContainsKey(ParsingId)) //если  не встречали   и копировали ранее
                                {
                                    if (sourse_prop_value != null && target_prop_value == null) //Еслим целевой объект нулл содаем новый 
                                    {
                                        new_target_prop_value = Activator.CreateInstance(sourse_prop_value.GetType());
                                        CopyObjectReflectionNewInstances(sourse_prop_value, new_target_prop_value, predicate, false);

                                    }
                                    else if (sourse_prop_value != null && target_prop_value != null)//Если объект  не встречалься и  не является навигационныс свойством ...
                                    {
                                        CopyObjectReflectionNewInstances(sourse_prop_value, new_target_prop_value, predicate, false);
                                    }
                                }
                                else //Если уде встречали и копировали...
                                {
                                    new_target_prop_value = AddObjectsTreeMetaData[ParsingId].TargetValue;
                                }

                            }
                            else//Если свойство относится к навигационным верхних уровней..
                            {
                                if (!AddObjectsTreeMetaData.ContainsKey(ParsingId)) //Если встретили навигационное свойтво и оно первое..
                                {
                                    if (Recursive_depth == 1)
                                    {
                                        if (sourse_prop_value == null && target_prop_value != null) // если навигационное в таргете , то нетрогаем его... 
                                        {
                                            new_target_prop_value = target_prop_value;
                                            set_value_skip_flag = true;
                                            AddObjectsTreeMetaData.Add(ParsingId, new TreeObjectInfo(Recursive_depth, sourse_prop_value, target_prop_value,
                                                    sourse_prop_info.Name, true, StatusObjectsInTree.LINK_PROCESSED));
                                        }
                                        else if (sourse_prop_value != null) //Если оисточныих не равен нулю и навигационный
                                        {
                                            new_target_prop_value = null;
                                        }

                                    }
                                }
                                else
                                if (AddObjectsTreeMetaData.ContainsKey(ParsingId))
                                    new_target_prop_value = AddObjectsTreeMetaData[ParsingId].TargetValue;
                            }
                        }
                    }
                    else
                        new_target_prop_value = sourse_prop_value;

                    if (target_prop.CanWrite && !set_value_skip_flag) target_prop.SetValue(target, new_target_prop_value);

                    set_value_skip_flag = false;
                }
            }

            if ((sourse is IList)&& predicate.Invoke((TSourse)sourse)) //if input parameters is Tlist 
            {

                foreach (IEntityObject sourse_element in (IEnumerable<IEntityObject>)sourse)  //Регистрируем все объекты списка...
                {
                    var find_obj = ((IEnumerable<IEntityObject>)target).
                          Where(ob => GetParsingId(ob) == GetParsingId(sourse_element)).FirstOrDefault();

                    if (!AddObjectsTreeMetaData.ContainsKey(GetParsingId(sourse_element)))
                        AddObjectsTreeMetaData.Add(GetParsingId(sourse_element), new TreeObjectInfo(Recursive_depth, sourse_element, find_obj, sourse_element.Name, false, StatusObjectsInTree.LINK_PROCESSED));
                }

                foreach (IEntityObject sourse_element in (IEnumerable<IEntityObject>)sourse)//Копируем ...
                {
                    var find_obj = ((IEnumerable<IEntityObject>)target).
                        Where(ob => GetParsingId(ob) == GetParsingId(sourse_element)).FirstOrDefault();

                    if (find_obj == null)
                    {
                        if (AddObjectsTreeMetaData[GetParsingId(sourse_element)].Status == StatusObjectsInTree.LINK_PROCESSED
                            && AddObjectsTreeMetaData[GetParsingId(sourse_element)].TargetValue == null)
                        {
                            var new_obj = Activator.CreateInstance(sourse_element.GetType());
                            AddObjectsTreeMetaData[GetParsingId(sourse_element)].TargetValue = new_obj;
                            CopyObjectReflectionNewInstances(sourse_element, new_obj, predicate, false);
                            ((IList)target).Add(new_obj);
                        }
                        else if (AddObjectsTreeMetaData[GetParsingId(sourse_element)].Status == StatusObjectsInTree.LINK_PROCESSED
                            && AddObjectsTreeMetaData[GetParsingId(sourse_element)].TargetValue != null)
                        {
                            CopyObjectReflectionNewInstances(sourse_element, AddObjectsTreeMetaData[GetParsingId(sourse_element)].TargetValue, predicate, false);
                            ((IList)target).Add(AddObjectsTreeMetaData[GetParsingId(sourse_element)].TargetValue);
                        }
                        else
                        {
                            ((IList)target).Add(AddObjectsTreeMetaData[GetParsingId(sourse_element)].TargetValue);
                        }
                    }
                    else if (find_obj != null)
                    {
                        if (AddObjectsTreeMetaData[GetParsingId(sourse_element)].Status == StatusObjectsInTree.LINK_PROCESSED)
                        {

                            CopyObjectReflectionNewInstances(sourse_element, find_obj, predicate, false);
                        }

                    }
                }

                for (int ii = ((IList)target).Count - 1; ii >= 0; ii--)
                {
                    var obj = (IEntityObject)((IList)target)[ii];
                    var find_obj = ((IEnumerable<IEntityObject>)sourse).Where(ob => ob.Id == obj.Id).FirstOrDefault();
                    if (find_obj == null)
                    {
                        ((IList)target).Remove(obj);
                    }
                }

            }

            if (sourse is bldObject)
                ;
            AddObjectsTreeMetaData[sourse_parsing_id].Status = StatusObjectsInTree.COPY_DONE;
            Recursive_depth--;
            if (Recursive_depth == 0)
            {
                AddObjectsTreeMetaData.Clear();
                ObjectsTreeMetaData.Clear();
                NavigateParametrDepth = 0;
            }
        }

        public static Guid GetParsingId(object obj)///Возвращает свойство Id если оно не пустое, иначе возращает StoredId 
        {
            Guid ParsingId = Guid.Empty;
            if ((obj is IEntityObject))
            {
                if (((IEntityObject)obj).Id != Guid.Empty)
                    ParsingId = ((IEntityObject)obj).Id;
                else
                    ParsingId = ((IEntityObject)obj).StoredId;
                if (ParsingId == Guid.Empty)
                {
                    ((IEntityObject)obj).Id = Guid.NewGuid();
                    ParsingId = ((IEntityObject)obj).Id;
                }
            }

            return ParsingId;
        }

        public static ObservableCollection<PropertyInfo> GetNavigateProperties(object obj)//Возращает всспиок навигационных свойств объекта 
        {
            var target_props = obj.GetType().GetProperties() //Выбираем все не идексные свойства
                   .Where(p => p.GetIndexParameters().Length == 0);
            ObservableCollection<PropertyInfo> result = new ObservableCollection<PropertyInfo>();
            foreach (PropertyInfo prop_info in target_props)
            {
                var results = new List<ValidationResult>();
                var context_obj = new ValidationContext(obj);
                bool obj_validate_result = false;
                if (!Validator.TryValidateObject(obj, context_obj, results, true))
                    obj_validate_result = results?[0]?.MemberNames?.FirstOrDefault() == prop_info?.Name;

                if (obj_validate_result) result.Add(prop_info);
            }
            return result;
        }

        public static void SetAllIdToZero(object sourse, bool set_new_id = false)
        {
            if (sourse is IEntityObject)
                ((IEntityObject)sourse).Id = Guid.Empty;
            var sourse_props = sourse.GetType().GetProperties()
                    .Where(p => p.GetIndexParameters().Length == 0);
            foreach (PropertyInfo prop_info in sourse_props)
            {
                var sourse_prop_value = prop_info.GetValue(sourse);
                if (sourse_prop_value is IEntityObject && ((IEntityObject)sourse_prop_value).Id != Guid.Empty) //Если свойство является объектом..
                {
                    SetAllIdToZero(sourse_prop_value, set_new_id);
                }
            }
            if ((sourse is IList)) //if input parameters is Tlist 
            {
                foreach (IEntityObject obj in (IEnumerable<IEntityObject>)sourse)
                {

                    if (obj is IEntityObject)
                        ((IEntityObject)obj).Id = Guid.Empty;
                    SetAllIdToZero(obj, set_new_id);
                }
            }

            if (set_new_id)
                ((IEntityObject)sourse).Id = Guid.NewGuid();
            else
                ((IEntityObject)sourse).Id = Guid.Empty;
            ((IEntityObject)sourse).StoredId = Guid.NewGuid();
        }
        static int tree_level = 0;
        static int initial_tree_level = 0;
        static bool sourse_prop_is_navigate_prop_enable = false;
        public static void GetNavigatePropertyUpTree(object obj, Dictionary<Guid, TreeObjectInfo> result_ctalog, bool reset = true) //Возращает дерево навигационных свойсв объекта
        {
            // int tree_level = 0;
            if (reset)
            {
                result_ctalog.Clear();
                tree_level = 0;
            }
            if (obj != null)
            {
                tree_level--;
                var obj_all_props = obj.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);
                var obj_all_navigate_props = GetNavigateProperties(obj); //Получаем список навигационных свойств объекта

                foreach (PropertyInfo prop_info in obj_all_navigate_props)
                {
                    var prop_val = prop_info.GetValue(obj);
                    if (prop_val != null)
                    {
                        Guid obj_id = ((IEntityObject)prop_val).Id;
                        if (!result_ctalog.ContainsKey(obj_id)) //Если в каталоге нет 
                            result_ctalog.Add(obj_id, new TreeObjectInfo(tree_level, prop_val, null, prop_info.Name, false));
                        GetNavigatePropertyUpTree(prop_val, result_ctalog, false);
                    }
                }
                tree_level++;
            }
            if (tree_level == 0)
            {

            }
        }
        public static void GetObjectsFullCatalog(object sourse, object target, Dictionary<Guid, TreeObjectInfo> catalog, bool reset = true)
        {
            if (reset) { catalog.Clear(); tree_level = 0; }
            // if (target == null) target = Activator.CreateInstance(sourse.GetType());
            //   if (sourse == null) sourse = Activator.CreateInstance(target.GetType());

            tree_level++;
            if (tree_level == 1)
                catalog.Add(((IEntityObject)sourse).Id, new TreeObjectInfo(tree_level, sourse, target));

            var all_sourse_props = sourse?.GetType().GetProperties() //Выбираем все не идексные свойства
                  .Where(p => p.GetIndexParameters().Length == 0);
            var all_target_props = target?.GetType().GetProperties() //Выбираем все не идексные свойства
               .Where(p => p.GetIndexParameters().Length == 0);

            var all_props = all_sourse_props;

            if (all_sourse_props == null) all_props = all_target_props;
            var common_prop = sourse;
            if (sourse == null) common_prop = target;

            foreach (PropertyInfo sourse_prop_info in all_props)
            {
                var sourse_prop_value = sourse_prop_info.GetValue(common_prop);

                if (sourse != null) sourse_prop_value = sourse_prop_info.GetValue(sourse);
                else { sourse = null; sourse_prop_value = null; }

                var target_prop_value = sourse_prop_info.GetValue(common_prop);

                if (target != null) target_prop_value = sourse_prop_info.GetValue(target);
                else { target = null; target_prop_value = null; }


                if ((sourse_prop_value is IEntityObject || target_prop_value is IEntityObject))
                {
                    Guid prop_id = Guid.Empty;
                    if (sourse_prop_value != null) prop_id = GetParsingId(sourse_prop_value);
                    else if (target_prop_value != null) prop_id = GetParsingId(target_prop_value);

                    if (!catalog.ContainsKey(prop_id))
                    {
                        catalog.Add(prop_id, new TreeObjectInfo(tree_level, sourse_prop_value,
                        target_prop_value, sourse_prop_info?.Name, false));
                        GetObjectsFullCatalog(sourse_prop_value, target_prop_value, catalog, false);
                    }
                }

                  bool sourse_prop_is_navigate_prop = GetNavigateProperties(common_prop).Where(pr => pr.Name == sourse_prop_info?.Name).FirstOrDefault() != null;

                if (tree_level == 1 && sourse_prop_is_navigate_prop)
                {
                    Guid first_nav_prop_id = Guid.Empty;
                    if (sourse_prop_value != null && sourse_prop_value is IEntityObject)
                    {
                        first_nav_prop_id = catalog.Where(el => el.Key == ((IEntityObject)sourse_prop_value).Id).FirstOrDefault().Key;
                        catalog[first_nav_prop_id].IsFirstNavigate = true;
                    }
                    if (target_prop_value != null && target_prop_value is IEntityObject)
                    {
                        first_nav_prop_id = catalog.Where(el => el.Key == ((IEntityObject)target_prop_value).Id).FirstOrDefault().Key;
                        catalog[first_nav_prop_id].IsFirstNavigate = true;
                    }
                }
            }
            if (sourse is IList)
            {
                foreach (IEntityObject soures_element in (IEnumerable<IEntityObject>)sourse)
                {
                    var find_target_obj = target;
                    if (target != null)
                        find_target_obj = ((IEnumerable<IEntityObject>)target).
                       Where(ob => GetParsingId(ob) == GetParsingId(soures_element)).FirstOrDefault();

                    if (!catalog.ContainsKey(((IEntityObject)soures_element).Id))
                        catalog.Add(((IEntityObject)soures_element).Id, new TreeObjectInfo(tree_level, soures_element, find_target_obj));

                    GetObjectsFullCatalog(soures_element, find_target_obj, catalog, false);
                }
            }
            tree_level--;
            if (reset)
            {
                tree_level = 0;
                //  catalog.Clear();
            }
        }

    }


    public class TreeObjectInfo
    {
        public int RecursiveLevel { get; set; }//Глубина рекурсии на которой встретился объект
        private object _sourseValue;
        public object SourseValue
        {
            get { return _sourseValue; }
            set { _sourseValue = value; if (_sourseValue != null) Type = _sourseValue.GetType(); }
        }
        private object _targetValue;
        public object TargetValue
        {
            get { return _targetValue; }
            set { _targetValue = value; }
        }

        public Type Type { get; set; }

        public StatusObjectsInTree Status { get; set; } = StatusObjectsInTree.NONE;
        private string Name { get; set; }

        public bool IsFirstNavigate { get; set; } = false;
        public Guid BranchID { get; set; } = Guid.Empty;//Уроверь объекта в дерере объектов
        public TreeObjectInfo(int rec_level, object sourse, object target,
            string name = "", bool is_first_navigate_prop = false, StatusObjectsInTree status = StatusObjectsInTree.NONE)
        {
            RecursiveLevel = rec_level;
            SourseValue = (IEntityObject)sourse;
            TargetValue = target;
            Name = name;
            IsFirstNavigate = is_first_navigate_prop;
            Status = status;
            //  BranchID = branchId;
        }
        public TreeObjectInfo(TreeObjectInfo treeObjectInfo)
        {
            RecursiveLevel = treeObjectInfo.RecursiveLevel;
            SourseValue = treeObjectInfo.SourseValue;
            TargetValue = treeObjectInfo.TargetValue;
            Name = treeObjectInfo.Name;
            IsFirstNavigate = treeObjectInfo.IsFirstNavigate;
            Status = treeObjectInfo.Status;

        }


    }
    public enum StatusObjectsInTree
    {
        COPY_DONE,
        LINK_PROCESSED,
        NONE,
        CICLING_OBJ,
        NAVIGATE_OBJ
    }
    public class ObjectsTree
    {
        public static Dictionary<Guid, object> ObjectsReestr { get; set; }
        public TreeBranchNode RootTreeBranch { get; set; }
        public ObjectsTree(object sourse_object)
        {

        }
    }
    public class TreeBranchNode
    {
        public object Entity { get; set; }
        public ObservableCollection<TreeBranchNode> TreeBranchNodes { get; set; }
        public TreeBranchNode(object sourse_object)
        {
            TreeBranchNodes = new ObservableCollection<TreeBranchNode>();
            Entity = sourse_object;

            var sourse_props = sourse_object.GetType().GetProperties();
            if (sourse_object is IList) //if input parameters is Tlist 
            {
                foreach (IEntityObject obj in (IEnumerable<IEntityObject>)sourse_object)
                {
                    TreeBranchNodes.Add(new TreeBranchNode(obj));
                }
            }

        }
    }
}
