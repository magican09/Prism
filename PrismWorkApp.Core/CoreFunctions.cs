using Prism.Services.Dialogs;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace PrismWorkApp.Core
{
    public static class CoreFunctions
    {


        public static T GetChildOfType<T>(this DependencyObject depObj)
    where T : DependencyObject
        {
            if (depObj == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = (child as T) ?? GetChildOfType<T>(child);
                if (result != null) return result;
            }
            return null;
        }
      

        public static void RemoveElementFromCollectionWhithDialog<TContainer, T>
               (T element, string element_type_name,
            Action<IDialogResult> elm_erase_action, IDialogService dialogService, Guid current_context_id)
           where TContainer : ICollection<T>, INameableOservableCollection<T>
           where T : class, INameable, IRegisterable
        {
            var dialog_par = new DialogParameters();
            dialog_par.Add("massege",
               $"Вы действительно хотите удалить {element_type_name} \"{element.Name }\" ?!");
            dialog_par.Add("confirm_button_content", "Удалить");
            dialog_par.Add("refuse_button_content", "Отмена");
            dialog_par.Add("current_context_id", current_context_id);
            string element_name = element.Name;

            dialogService.Show(typeof(ConfirmActionWhithoutCancelDialog).Name, dialog_par, result =>
           {
               if (result.Result == ButtonResult.Yes)
               {
                   var res_massage = result.Parameters.GetValue<string>("confirm_dialog_param");
                   var p = new DialogParameters();
                   p.Add("message", $"{element_type_name.ToUpper()} " +
                       $"\"{element.Name}\" удален!");
                   elm_erase_action.Invoke(new DialogResult(ButtonResult.Yes));
                   dialogService.Show(typeof(MessageDialog).Name, p, (r) => { });
                 }
               if (result.Result == ButtonResult.No)
               {
                   elm_erase_action.Invoke(new DialogResult(ButtonResult.No));
               }
           });

        }


        public static void RemoveElementWhithDialog<TContainer, T>
             (T element, string element_type_name,
          Action elm_erase_action, IDialogService dialogService)
          where T : class, INameable, IRegisterable
        {
            var dialog_par = new DialogParameters();
            dialog_par.Add("massege",
               $"Вы действительно хотите удалить {element_type_name} \"{element.Name }\" ?!");
            dialog_par.Add("confirm_button_content", "Удалить");
            dialog_par.Add("refuse_button_content", "Отмена");
            string element_name = element.Name;

            dialogService.Show(typeof(ConfirmActionWhithoutCancelDialog).Name, dialog_par, result =>
            {
                if (result.Result == ButtonResult.Yes)
                {
                    elm_erase_action.Invoke();
                    var res_massage = result.Parameters.GetValue<string>("confirm_dialog_param");
                    var p = new DialogParameters();
                    p.Add("message", $"{element_type_name.ToUpper()} " +
                        $"\"{element.Name}\" удален!");
                    dialogService.Show(typeof(MessageDialog).Name, p, (r) => { });
                }
                if (result.Result == ButtonResult.No)
                {
                    elm_erase_action.Invoke();
                }
            });

        }

        /* public static void AddElementToCollectionWhithDialog<TContainer, T>
                (TContainer collection, T element, Action elm_add_action, IDialogService dialogService,
             string element_type_name = "")
            where TContainer : ICollection<T>
            where T : INameable, IRegisterable
         {
             var dialog_par = new DialogParameters();
             dialog_par.Add("massege",
                $"Вы действительно хотите добавить {element_type_name} \"{element.Name }\" ?!");
             dialog_par.Add("confirm_button_content", "Удалить");
             dialog_par.Add("refuse_button_content", "Закрыть");
             string element_name = element.Name;

             dialogService.Show(typeof(ConfirmActionDialog).Name, dialog_par, result =>
             {
                 if (result.Result == ButtonResult.Yes)
                 {
                     collection.Add(element);
                     var res_massage = result.Parameters.GetValue<string>("confirm_dialog_param");
                     var p = new DialogParameters();
                     p.Add("message", $"{element_type_name.ToUpper()} " +
                         $"\"{element.Name}\" добавлен!");
                     dialogService.Show(typeof(MessageDialog).Name, p, (r) => { });
                     elm_add_action.Invoke();
                 }
                 if (result.Result == ButtonResult.No)
                 {
                     elm_add_action.Invoke();
                 }
             });

         }
         */
        public static void ConfirmActionOnElementDialog<T>
              (T element, string action_name, string element_type_name,
            string confirm_action_name,
            string refuse_action_name,
            string cencel_action_name,
            Action<IDialogResult> elm_save_action, IDialogService dialogService)
          where T : INameable, IRegisterable
        {
            var dialog_par = new DialogParameters();
            dialog_par.Add("massege",
               $"Вы действительно хотите {action_name} {element_type_name} \"{element.Name }\" ?!");
            dialog_par.Add("confirm_button_content", confirm_action_name);
            dialog_par.Add("refuse_button_content", refuse_action_name);
            dialog_par.Add("cancel_button_content", cencel_action_name);

            string element_name = element.Name;

            dialogService.ShowDialog(typeof(ConfirmActionDialog).Name, dialog_par, result =>
            {
                if (result.Result == ButtonResult.Yes)
                {
                    var res_massage = result.Parameters.GetValue<string>("confirm_dialog_param");
                    var p = new DialogParameters();
                    /* p.Add("message", $"{element_type_name.ToUpper()} " +
                         $"\"{element.Name}\" создан!");*/
                    p.Add("message", $"Готово!");
                    elm_save_action.Invoke(new DialogResult(ButtonResult.Yes));
                    dialogService.ShowDialog(typeof(MessageDialog).Name, p, (r) => { });

                }
                else if (result.Result == ButtonResult.No)
                {
                    elm_save_action.Invoke(new DialogResult(ButtonResult.No));
                }
                else if (result.Result == ButtonResult.Cancel)
                {
                    elm_save_action.Invoke(new DialogResult(ButtonResult.Cancel));
                }
            });

        }

        public static void ConfirmActionDialog
             (string action_name, string element_type_name, string confirm_action_name, string cencel_action_name,
           Action<IDialogResult> elm_save_action, IDialogService dialogService)
        {
            var dialog_par = new DialogParameters();
            dialog_par.Add("massege",
               $"Вы действительно хотите {action_name} {element_type_name} ?!");
            dialog_par.Add("confirm_button_content", confirm_action_name);
            dialog_par.Add("refuse_button_content", cencel_action_name);
            dialogService.ShowDialog(typeof(ConfirmActionWhithoutCancelDialog).Name, dialog_par, result =>
           {
               if (result.Result == ButtonResult.Yes)
               {
                   var res_massage = result.Parameters.GetValue<string>("confirm_dialog_param");
                   var p = new DialogParameters();
                   p.Add("message", $"Готово!");
                   elm_save_action.Invoke(new DialogResult(ButtonResult.Yes));
                   dialogService.ShowDialog(typeof(MessageDialog).Name, p, (r) => { });
               }
               if (result.Result == ButtonResult.No)
               {
                   elm_save_action.Invoke(new DialogResult(ButtonResult.No));
               }
           });

        }

        public static void ConfirmActionDialog
            (string action_name, string element_type_name, string confirm_action_name,
            string cencel_action_name, string confirm_Yes_action_massage,
          Action<IDialogResult> elm_save_action, IDialogService dialogService)
        {
            var dialog_par = new DialogParameters();
            dialog_par.Add("massege",
               $"Вы действительно хотите {action_name} {element_type_name} ?!");
            dialog_par.Add("confirm_button_content", confirm_action_name);
            dialog_par.Add("refuse_button_content", cencel_action_name);
            dialogService.ShowDialog(typeof(ConfirmActionWhithoutCancelDialog).Name, dialog_par, result =>
            {
                if (result.Result == ButtonResult.Yes)
                {
                    var res_massage = result.Parameters.GetValue<string>("confirm_dialog_param");
                    var p = new DialogParameters();
                    p.Add("message", confirm_Yes_action_massage);
                    elm_save_action.Invoke(new DialogResult(ButtonResult.Yes));
                    dialogService.ShowDialog(typeof(MessageDialog).Name, p, (r) => { });
                }
                if (result.Result == ButtonResult.No)
                {
                    elm_save_action.Invoke(new DialogResult(ButtonResult.No));
                }
            });

        }

        public static void EditElementDialog<T>
                   (T element, string element_type_name,
                 Action<IDialogResult> elm_save_action, IDialogService dialogService,
                   string dialogViewName,
                   string newObjectDialogName,
                   IUnDoReDoSystem undo_redo,
                   string title = "",
                   string message = "")
               where T : INameable, IRegisterable, new()
        {
            var dialog_par = new DialogParameters();
            dialog_par.Add("title", title);
            dialog_par.Add("message", message);
            dialog_par.Add("confirm_button_content", "Сохранить");
            dialog_par.Add("refuse_button_content", "Закрыть");
            dialog_par.Add("new_object_dialog_name", newObjectDialogName);
            dialog_par.Add("undo_redo", undo_redo);
            T element_for_edit = element;
            ConveyanceObject send_obj = new ConveyanceObject(element_for_edit, ConveyanceObjectModes.EditMode.FOR_EDIT);
            dialog_par.Add("selected_element_conveyance_object", send_obj);
            dialogService.ShowDialog(dialogViewName, dialog_par, (result) =>
            {
                if (result.Result == ButtonResult.Yes)
                {
                    DialogParameters param = new DialogParameters();
                    param.Add("undo_redo", result.Parameters.GetValue<UnDoReDoSystem>("undo_redo"));
                    elm_save_action.Invoke(new DialogResult(ButtonResult.Yes, param));
                }


            });

        }
        /// <summary>
        /// Функция для добавления в текущую коллекцию нового объекта или объекта из другой 
        /// коллекции с помощью диалогового окна.
        /// </summary>
        /// <typeparam name="TContainer"></typeparam> тип коллекций
        /// <typeparam name="T"></typeparam>// тип объектов коллекций
        /// <param name="currentCollection"></param> текущая коллекция 
        /// <param name="commonCollection"></param> общая коллекция
        /// <param name="dialogService"></param> //  инетерфейс сервиса диалоговых окон от Prism
        /// <param name="message"></param> сообщение в заголовке диалогового окна
        /// <param name="currentCollectionName"></param> Название тукущей коллекции
        /// <param name="commonCollectionName"></param>Название общей коллекции
        public static void AddElementToCollectionWhithDialog<TContainer, T>
            (TContainer currentCollection, TContainer commonCollection,
            IDialogService dialogService, Action<IDialogResult> action,
                string dialogViewName,
                string newObjectDialogName,
                Guid current_context_id,
                string title = "",
                string message = "",
                string currentCollectionName = "",
                string commonCollectionName = ""
                )
            where TContainer : ICollection<T>, INameableOservableCollection<T>, new()
            where T : INameable
        {
            TContainer current_collection = new TContainer();
            TContainer common_collection = new TContainer();
            //CopyObjectReflectionNewInstances(currentCollection, current_collection);
            //CopyObjectReflectionNewInstances(commonCollection, common_collection);
            current_collection = currentCollection;

            common_collection = GetCollectionElementsList<TContainer, T>(commonCollection);


            foreach (T elm in current_collection) //Удаляем извременной общей коллекции объекты которые уже есть  редактируемой коллекции
            {
                var obj = common_collection.Where(el => ((IEntityObject)el).Id == ((IEntityObject)elm).Id).FirstOrDefault();
                if (obj == null)
                    common_collection.Where(el => ((IEntityObject)el).StoredId == ((IEntityObject)elm).StoredId).FirstOrDefault();
                if (obj != null)
                    common_collection.Remove(obj);
            }

            var dialog_par = new DialogParameters();
            dialog_par.Add("title", title);
            dialog_par.Add("message", message);
            dialog_par.Add("current_collection_name", currentCollectionName);
            dialog_par.Add("common_collection_name", commonCollectionName);
            dialog_par.Add("common_collection", common_collection);
            dialog_par.Add("current_collection", current_collection);
            dialog_par.Add("confirm_button_content", "Сохранить");
            dialog_par.Add("refuse_button_content", "Закрыть");
            dialog_par.Add("new_object_dialog_name", newObjectDialogName);
            dialog_par.Add("current_context_id", current_context_id);


            dialogService.ShowDialog(dialogViewName, dialog_par, action);
        }
        public static void AddElementsToCollectionWhithDialogList<TContainer, T>
       (TContainer currentCollection,
       TContainer commonCollection_all,
        NameablePredicateObservableCollection<TContainer, T> predicate_collection,
        IDialogService dialogService, Action<IDialogResult> action,
           string dialogViewName,          
           string title = "",
           string message = "",
           string currentCollectionName = "",
           string commonCollectionName = ""
           )
       where TContainer : ICollection<T>, new()
       where T : class
        {
            TContainer current_collection = new TContainer();
            TContainer common_collection = new TContainer();
            current_collection = currentCollection;
            common_collection = commonCollection_all;
            var dialog_par = new DialogParameters();
            dialog_par.Add("title", title);
            dialog_par.Add("message", message);
            dialog_par.Add("current_collection_name", currentCollectionName);
            dialog_par.Add("common_collection_name", commonCollectionName);
            dialog_par.Add("common_collection", common_collection);
            dialog_par.Add("current_collection", current_collection);
            dialog_par.Add("confirm_button_content", "Сохранить");
            dialog_par.Add("refuse_button_content", "Закрыть");
            dialog_par.Add("predicate_collection", predicate_collection);

            dialogService.ShowDialog(dialogViewName, dialog_par, action);
        }
     
        public static void AddElementToCollectionWhithDialog_Test<TContainer, T>
           (TContainer currentCollection,
           TContainer commonCollection_all,
            NameablePredicateObservableCollection<TContainer, T> predicate_collection,// commonCollection_restricted,
            IDialogService dialogService, Action<IDialogResult> action,
               string dialogViewName,
               string newObjectDialogName,
               Guid current_context_id,
               string title = "",
               string message = "",
               string currentCollectionName = "",
               string commonCollectionName = ""
               )
           where TContainer : ICollection<T>/*, INameableOservableCollection<T>*/, new()
           where T : class
        {
            TContainer current_collection = new TContainer();
            TContainer common_collection = new TContainer();
            current_collection = currentCollection;
            common_collection = commonCollection_all;
            var dialog_par = new DialogParameters();
            dialog_par.Add("title", title);
            dialog_par.Add("message", message);
            dialog_par.Add("current_collection_name", currentCollectionName);
            dialog_par.Add("common_collection_name", commonCollectionName);
            dialog_par.Add("common_collection", common_collection);
            dialog_par.Add("current_collection", current_collection);
            dialog_par.Add("confirm_button_content", "Сохранить");
            dialog_par.Add("refuse_button_content", "Закрыть");
            dialog_par.Add("new_object_dialog_name", newObjectDialogName);
            dialog_par.Add("current_context_id", current_context_id);
            dialog_par.Add("predicate_collection", predicate_collection);

            dialogService.ShowDialog(dialogViewName, dialog_par, action);
        }
        public static TContainer GetCollectionElementsList<TContainer, T>(TContainer input_collection)
            where TContainer : ICollection<T>, new()
            where T : INameable
        {
            TContainer output_collection = new TContainer();
            foreach (T elm in input_collection) //Создаем временную коллекцию для хранения списка элементов общей коллекции 
                output_collection.Add(elm);
            return output_collection;
        }
        /*  public static void CopyObjectReflectionNewInstances_backup(object sourse, object target, bool objectsTreeCatalogReset = true)
         {

             if (sourse == null) { target = null; return; } //Если источник не инециализирован - выходим

             if (target == null) //Если  цель не инициализирована - создаем в соотвесвии с иточником
                 target = Activator.CreateInstance(sourse.GetType());

             Guid sourse_parsing_id = Guid.Empty;
             if (GetParsingId(sourse) != Guid.Empty) //Определяем Id - сперва ищем в источнике, потом в приемнике ( последовательно)
                 sourse_parsing_id = GetParsingId(sourse);
             else if (GetParsingId(target) != Guid.Empty)
                 sourse_parsing_id = GetParsingId(target);

             if (objectsTreeCatalogReset) //Сбрасываем таблица с данными уже скопированных объектов, счетчик глубины рекурсии и навигациооного объекта, если уставновлен флаг
             {
                 ObjectsTreeMetaData.Clear();
                 Recursive_depth = 0;
                 NavigateParametrDepth = 0;
             }
             Recursive_depth++;
             if (sourse is IEntityObject)
                 if (!ObjectsTreeMetaData.ContainsKey(sourse_parsing_id))
                     ObjectsTreeMetaData.Add(sourse_parsing_id, target);

             var target_props = target.GetType().GetProperties() //Выбираем все не идексные свойства
                     .Where(p => p.GetIndexParameters().Length == 0);
             var sourse_props = sourse.GetType().GetProperties()
                     .Where(p => p.GetIndexParameters().Length == 0);
             foreach (PropertyInfo target_prop_info in target_props)
             {
                 var sourse_prop_info =
                     sourse_props.FirstOrDefault(p => p.Name == target_prop_info.Name);
                 if (sourse_prop_info != null && sourse_prop_info.CanWrite == true)
                 {
                     var sourse_prop_value = sourse_prop_info.GetValue(sourse);
                     var target_prop_value = target_prop_info.GetValue(target);
                     var new_target_prop_value = target_prop_info.GetValue(target);

                     var target_prop = target.GetType().GetProperty(target_prop_info.Name);
                     var sourse_prop = sourse.GetType().GetProperty(sourse_prop_info.Name);

                     if (sourse_prop_value is IEntityObject || target_prop_value is IEntityObject) //Если свойство является объектом..
                     {
                         Guid ParsingId = Guid.Empty;
                         if (GetParsingId(sourse_prop_value) != Guid.Empty)
                             ParsingId = GetParsingId(sourse_prop_value);
                         else if (GetParsingId(target_prop_value) != Guid.Empty)
                             ParsingId = GetParsingId(target_prop_value);

                         if (new_target_prop_value == null)
                             new_target_prop_value = Activator.CreateInstance(sourse_prop_value.GetType());

                         if (!ObjectsTreeMetaData.ContainsKey(ParsingId)) //Если не встречали этот объект, глубоко его копируем ..
                         {
                             var results = new List<ValidationResult>();
                             var results_t = new List<ValidationResult>();
                             var context_sourse = new ValidationContext(sourse);
                             var context_target = new ValidationContext(target);
                             bool taget_contain_validate_prop = false;
                             bool sourse_contain_validate_prop = false;

                             if (!Validator.TryValidateObject(sourse, context_sourse, results, true))
                                 sourse_contain_validate_prop = results?[0]?.MemberNames?.FirstOrDefault() == sourse_prop_info?.Name;
                             if (!Validator.TryValidateObject(sourse, context_sourse, results_t, true))
                                 taget_contain_validate_prop = results_t?[0]?.MemberNames?.FirstOrDefault() == target_prop_info?.Name;

                             if (sourse_contain_validate_prop || taget_contain_validate_prop)
                             {
                                 NavigateParametrDepth++;
                                 if (sourse_prop_value != null && NavigateParametrDepth == 1)
                                     new_target_prop_value = null;
                                 //     else if (sourse_prop_value == null && NavigateParametrDepth == 1 && target_prop_value != null);
                             }
                             else
                                 CopyObjectReflectionNewInstances(sourse_prop_value, new_target_prop_value, false);

                         }
                         else if (ObjectsTreeMetaData.ContainsKey(ParsingId))//Если объект уже был ...
                             new_target_prop_value = ObjectsTreeMetaData[ParsingId];
                         else
                             ;
                     }
                     else
                         new_target_prop_value = sourse_prop_value;

                     if (target_prop.CanWrite) target_prop.SetValue(target, new_target_prop_value);
                 }
             }

             if ((sourse is IList)) //if input parameters is Tlist 
             {
                 foreach (IEntityObject obj in (IEnumerable<IEntityObject>)sourse)
                 {

                     var find_obj = ((IEnumerable<IEntityObject>)target).
                         Where(ob => (ob.Id == obj.Id && ob.StoredId == Guid.Empty) ||
                             (ob.Id == Guid.Empty && ob.StoredId == obj.StoredId) ||
                              (ob.Id == obj.Id && ob.StoredId == obj.StoredId)).FirstOrDefault();
                     // CopyObjectReflectionNewInstances(obj, new_obj, false);
                     if (target is bldObjectsGroup)
                         ;
                     if (find_obj == null)
                     {
                         var new_obj = Activator.CreateInstance(obj.GetType());
                         CopyObjectReflectionNewInstances(obj, new_obj, false);
                         ((IList)target).Add(new_obj);
                     }
                     else
                         CopyObjectReflectionNewInstances(obj, find_obj, false);
                 }
                 if (target is bldObjectsGroup)
                     ;

                 for (int ii = ((IList)target).Count - 1; ii >= 0; ii--)
                 {
                     var obj = (IEntityObject)((IList)target)[ii];
                     if (obj is bldObject)
                         ;
                     var find_obj = ((IEnumerable<IEntityObject>)sourse).Where(ob => ob.Id == obj.Id && ob.StoredId == obj.StoredId).FirstOrDefault();
                     if (find_obj == null)
                     {
                         ((IList)target).Remove(obj);
                         //ii--;
                     }

                 }
             }
             Recursive_depth--;
             if (Recursive_depth == 0)
             {
                 ObjectsTreeMetaData.Clear();
                 NavigateParametrDepth = 0;
             }
         }
        public static void DeepCopyWhithReflection(object sourse, object target, bool objectsTreeCatalogReset = true)
         {

             if (sourse == null) { target = null; return; } //Если источник не инециализирован - выходим

             if (target == null) //Если  цель не инициализирована - создаем в соотвесвии с иточником
                 target = Activator.CreateInstance(sourse.GetType());

             Guid sourse_parsing_id = Guid.Empty;
             if (GetParsingId(sourse) != Guid.Empty) //Определяем Id - сперва ищем в источнике, потом в приемнике ( последовательно)
                 sourse_parsing_id = GetParsingId(sourse);
             else if (GetParsingId(target) != Guid.Empty)
                 sourse_parsing_id = GetParsingId(target);

             if (objectsTreeCatalogReset) //Сбрасываем таблица с данными уже скопированных объектов, счетчик глубины рекурсии и навигациооного объекта, если уставновлен флаг
             {
                 ObjectsTreeMetaData.Clear();
                 Recursive_depth = 0;
                 NavigateParametrDepth = 0;
             }
             Recursive_depth++;
             if (sourse is IEntityObject)
                 if (!ObjectsTreeMetaData.ContainsKey(sourse_parsing_id))
                     ObjectsTreeMetaData.Add(sourse_parsing_id, target);

             var target_props = target.GetType().GetProperties() //Выбираем все не идексные свойства
                     .Where(p => p.GetIndexParameters().Length == 0);
             var sourse_props = sourse.GetType().GetProperties()
                     .Where(p => p.GetIndexParameters().Length == 0);
             foreach (PropertyInfo target_prop_info in target_props)
             {
                 var sourse_prop_info =
                     sourse_props.FirstOrDefault(p => p.Name == target_prop_info.Name);
                 if (sourse_prop_info != null && sourse_prop_info.CanWrite == true)
                 {
                     var sourse_prop_value = sourse_prop_info.GetValue(sourse);
                     var target_prop_value = target_prop_info.GetValue(target);
                     var new_target_prop_value = target_prop_info.GetValue(target);

                     var target_prop = target.GetType().GetProperty(target_prop_info.Name);
                     var sourse_prop = sourse.GetType().GetProperty(sourse_prop_info.Name);

                     if (sourse_prop_value is IEntityObject || target_prop_value is IEntityObject) //Если свойство является объектом..
                     {
                         Guid ParsingId = Guid.Empty;
                         if (GetParsingId(sourse_prop_value) != Guid.Empty)
                             ParsingId = GetParsingId(sourse_prop_value);
                         else if (GetParsingId(target_prop_value) != Guid.Empty)
                             ParsingId = GetParsingId(target_prop_value);

                         if (new_target_prop_value == null)
                             new_target_prop_value = Activator.CreateInstance(sourse_prop_value.GetType());

                         if (!ObjectsTreeMetaData.ContainsKey(ParsingId)) //Если не встречали этот объект, глубоко его копируем ..
                         {
                             var results = new List<ValidationResult>();
                             var results_t = new List<ValidationResult>();
                             var context_sourse = new ValidationContext(sourse);
                             var context_target = new ValidationContext(target);
                             bool taget_contain_validate_prop = false;
                             bool sourse_contain_validate_prop = false;

                             if (!Validator.TryValidateObject(sourse, context_sourse, results, true))
                                 sourse_contain_validate_prop = results?[0]?.MemberNames?.FirstOrDefault() == sourse_prop_info?.Name;
                             if (!Validator.TryValidateObject(sourse, context_sourse, results_t, true))
                                 taget_contain_validate_prop = results_t?[0]?.MemberNames?.FirstOrDefault() == target_prop_info?.Name;

                             if (sourse_contain_validate_prop || taget_contain_validate_prop)
                             {
                                 NavigateParametrDepth++;
                                 if (sourse_prop_value != null && NavigateParametrDepth == 1)
                                     new_target_prop_value = null;
                                 //     else if (sourse_prop_value == null && NavigateParametrDepth == 1 && target_prop_value != null);
                             }
                             else
                                 DeepCopyWhithReflection(sourse_prop_value, new_target_prop_value, false);

                         }
                         else if (ObjectsTreeMetaData.ContainsKey(ParsingId))//Если объект уже был ...
                             new_target_prop_value = ObjectsTreeMetaData[ParsingId];
                         else
                             ;
                     }
                     else
                         new_target_prop_value = sourse_prop_value;

                     if (target_prop.CanWrite) target_prop.SetValue(target, new_target_prop_value);
                 }
             }

             if ((sourse is IList)) //if input parameters is Tlist 
             {
                 foreach (IEntityObject obj in (IEnumerable<IEntityObject>)target)
                 {
                     if (!ObjectsTreeMetaData.ContainsKey(CoreFunctions.GetParsingId(obj)))
                         ObjectsTreeMetaData.Add(CoreFunctions.GetParsingId(obj), obj);
                 }

                 foreach (IEntityObject obj in (IEnumerable<IEntityObject>)sourse)
                 {

                     var find_obj = ((IEnumerable<IEntityObject>)target).
                         Where(ob => CoreFunctions.GetParsingId(ob) == CoreFunctions.GetParsingId(obj)).FirstOrDefault();
                     if (target is bldWork)
                         ;
                     if (find_obj == null && !ObjectsTreeMetaData.ContainsKey(CoreFunctions.GetParsingId(obj))) //Если в целевом списке такой объект существует то меняем его содержимое в соотвествии с источником
                     {
                         var new_obj = Activator.CreateInstance(obj.GetType());
                         DeepCopyWhithReflection(obj, new_obj, false);
                         ((IList)target).Add(new_obj);
                     }
                     else if (ObjectsTreeMetaData.ContainsKey(CoreFunctions.GetParsingId(obj)) && find_obj == null)
                     {
                         ((IList)target).Add(ObjectsTreeMetaData[CoreFunctions.GetParsingId(obj)]);
                     }
                     else
                         DeepCopyWhithReflection(obj, find_obj, false);
                 }
                 if (target is bldObjectsGroup)
                     ;

                 for (int ii = ((IList)target).Count - 1; ii >= 0; ii--)
                 {
                     var obj = (IEntityObject)((IList)target)[ii];
                     if (obj is bldObject)
                         ;
                     var find_obj = ((IEnumerable<IEntityObject>)sourse).Where(ob => ob.Id == obj.Id && ob.StoredId == obj.StoredId).FirstOrDefault();
                     if (find_obj == null)
                     {
                         ((IList)target).Remove(obj);
                         //ii--;
                     }

                 }
             }
             Recursive_depth--;
             if (Recursive_depth == 0)
             {
                 ObjectsTreeMetaData.Clear();
                 NavigateParametrDepth = 0;
             }
         }
         */
        public static Dictionary<Guid, TreeObjectInfo> ObjectsTreeMetaData { get; set; } = new Dictionary<Guid, TreeObjectInfo>();//Каталог всех объектов внутри объекта
        public static Dictionary<Guid, TreeObjectInfo> AddObjectsTreeMetaData { get; set; } = new Dictionary<Guid, TreeObjectInfo>();
        public static Dictionary<Guid, TreeObjectInfo> CiclingObjectsCatalog { get; set; } = new Dictionary<Guid, TreeObjectInfo>();//Каталог всех объектов внутри объекта
        public static int Recursive_depth { get; set; } = 0;//Глубина рекурсии
        public static int NavigateParametrDepth { get; set; } = 0;//Глубина навигационного свойства
        public static int InitialRecursive_depth { get; set; } = 0;
        public static int Cicling_recursive_depth { get; set; } = 0;//Глубина рекурсии

        public static void SetCiclingObjectsCatalog(object sourse, bool objectsTreeCatalogReset = true)
        {
            if (sourse == null) { return; } //Если источник не инециализирован - выходим
            Guid sourse_parsing_id = Guid.Empty;
            if (GetParsingId(sourse) != Guid.Empty) //Определяем Id - сперва ищем в источнике, потом в приемнике (последовательно)
                sourse_parsing_id = GetParsingId(sourse);
            if (objectsTreeCatalogReset) //Сбрасываем таблица с данными уже скопированных объектов, счетчик глубины рекурсии и навигациооного объекта, если уставновлен флаг
            {
                ObjectsTreeMetaData.Clear();
                AddObjectsTreeMetaData.Clear();
                Cicling_recursive_depth = 0;
                NavigateParametrDepth = 0;
            }
            Cicling_recursive_depth++;
            if (!AddObjectsTreeMetaData.ContainsKey(sourse_parsing_id))
            {
                AddObjectsTreeMetaData.Add(sourse_parsing_id,
                  new TreeObjectInfo(Recursive_depth, sourse, null, ((IEntityObject)sourse).Name, false, StatusObjectsInTree.LINK_PROCESSED));

                var sourse_props_infoes = sourse.GetType().GetProperties().Where(p => p.GetIndexParameters().Length == 0);
                foreach (PropertyInfo sourse_prop_info in sourse_props_infoes) //Записываем в каталог все объекты внутри объекта источника
                {
                    var sourse_prop_value = sourse_prop_info.GetValue(sourse);
                    if (sourse_prop_value is IEntityObject)
                    {
                        Guid sourse_prop_Id = ((IEntityObject)sourse_prop_value).Id;
                        bool sourse_prop_is_navigate_prop = GetNavigateProperties(sourse).Where(pr => pr.Name == sourse_prop_info?.Name).FirstOrDefault() != null;

                        if (!AddObjectsTreeMetaData.ContainsKey(sourse_prop_Id))//Если мы не встречали данный объект ранее
                        {
                            AddObjectsTreeMetaData.Add(sourse_prop_Id,
                             new TreeObjectInfo(Recursive_depth, sourse_prop_value, null, sourse_prop_info.Name,
                             sourse_prop_is_navigate_prop, StatusObjectsInTree.LINK_PROCESSED));
                            SetCiclingObjectsCatalog(sourse, false);
                        }

                    }

                }
                if (sourse is IList)
                {
                    foreach (IEntityObject element in (sourse as IList))
                    {
                        SetCiclingObjectsCatalog(element, false);
                    }
                }

            }
            else if (AddObjectsTreeMetaData[sourse_parsing_id].Status == StatusObjectsInTree.LINK_PROCESSED)
            {
                AddObjectsTreeMetaData[sourse_parsing_id].Status = StatusObjectsInTree.CICLING_OBJ;
            }
            else
            {

            }

            Cicling_recursive_depth--;

        }
        static int tree_level = 0;
        static int initial_tree_level = 0;
        static bool sourse_prop_is_navigate_prop_enable = false;
        #region скрыть CopyObjectReflectionNewInstances
        public static void CopyObjectNewInstances<TSourse>(object sourse, object target, Func<TSourse, bool> predicate, bool objectsTreeCatalogReset = true)
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
            //else
            //    AddObjectsTreeMetaData[sourse_parsing_id].Status = StatusObjectsInTree.COPY_DONE;
            //if (sourse is IEntityObject)
            // {
            //     ObjectsTreeMetaData[sourse_parsing_id].TargetValue = target;
            //     ObjectsTreeMetaData[sourse_parsing_id].SourseValue = sourse;
            // }

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

                    if ((sourse_prop_value is IEntityObject || target_prop_value is IEntityObject)) //Если свойство является объектом..&&!(sourse is IList)
                    {
                        Guid ParsingId = Guid.Empty;
                        if (GetParsingId(sourse_prop_value) != Guid.Empty)
                            ParsingId = GetParsingId(sourse_prop_value);
                        else if (GetParsingId(target_prop_value) != Guid.Empty)
                            ParsingId = GetParsingId(target_prop_value);

                        if (!ObjectsTreeMetaData[ParsingId].IsFirstNavigate) //Если свойтва не навигационное ссылающеся на верние уровни ...
                        {
                            if (!AddObjectsTreeMetaData.ContainsKey(ParsingId)) //если  не встречали   и копировали ранее
                            {
                                if (predicate.Invoke((TSourse)sourse_prop_value))
                                {
                                    if (sourse_prop_value != null && target_prop_value == null) //Еслим целевой объект нулл содаем новый 
                                    {
                                        new_target_prop_value = Activator.CreateInstance(sourse_prop_value.GetType());
                                        CopyObjectNewInstances(sourse_prop_value, new_target_prop_value, predicate, false);

                                    }
                                    else if (sourse_prop_value != null && target_prop_value != null)//Если объект  не встречалься и  не является навигационныс свойством ...
                                    {
                                        CopyObjectNewInstances(sourse_prop_value, new_target_prop_value, predicate, false);
                                    }
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
                    else
                        new_target_prop_value = sourse_prop_value;

                    if (target_prop.CanWrite && !set_value_skip_flag)
                    {
                        (target as IJornalable).JornalingOff();
                        target_prop.SetValue(target, new_target_prop_value);
                        (target as IJornalable).JornalingOn();
                    }
                    set_value_skip_flag = false;
                }
            }

            if ((sourse is IList) && !((IContainerFunctionabl)sourse).IsPointerContainer) //if input parameters is Tlist 
            {
                (target as IJornalable).JornalingOff();

                foreach (IEntityObject sourse_element in (IEnumerable<IEntityObject>)sourse)  //Регистрируем все объекты списка...
                {
                    var find_obj = ((IEnumerable<IEntityObject>)target).
                          Where(ob => CoreFunctions.GetParsingId(ob) == CoreFunctions.GetParsingId(sourse_element)).FirstOrDefault();

                    if (!AddObjectsTreeMetaData.ContainsKey(GetParsingId(sourse_element)))
                        AddObjectsTreeMetaData.Add(GetParsingId(sourse_element), new TreeObjectInfo(Recursive_depth, sourse_element, find_obj, sourse_element.Name, false, StatusObjectsInTree.LINK_PROCESSED));
                }

                foreach (IEntityObject sourse_element in (IEnumerable<IEntityObject>)sourse)//Копируем ...
                {
                    var find_obj = ((IEnumerable<IEntityObject>)target).
                        Where(ob => CoreFunctions.GetParsingId(ob) == CoreFunctions.GetParsingId(sourse_element)).FirstOrDefault();

                    if (find_obj == null)
                    {
                        if (AddObjectsTreeMetaData[GetParsingId(sourse_element)].Status == StatusObjectsInTree.LINK_PROCESSED
                            && AddObjectsTreeMetaData[GetParsingId(sourse_element)].TargetValue == null)
                        {
                            var new_obj = Activator.CreateInstance(sourse_element.GetType());
                            AddObjectsTreeMetaData[GetParsingId(sourse_element)].TargetValue = new_obj;
                            CopyObjectNewInstances(sourse_element, new_obj, predicate, false);
                            ((IList)target).Add(new_obj);
                        }
                        else if (AddObjectsTreeMetaData[GetParsingId(sourse_element)].Status == StatusObjectsInTree.LINK_PROCESSED
                            && AddObjectsTreeMetaData[GetParsingId(sourse_element)].TargetValue != null)
                        {
                            CopyObjectNewInstances(sourse_element, AddObjectsTreeMetaData[GetParsingId(sourse_element)].TargetValue, predicate, false);
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

                            CopyObjectNewInstances(sourse_element, find_obj, predicate, false);
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
            (target as IJornalable).JornalingOn();

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



        public static void CopyObjectReflectionNewInstances_Test(object sourse, object target, bool objectsTreeCatalogReset = true)
        {



            if (sourse == null) { target = null; return; } //Если источник не инециализирован - выходим
            if (target == null) //Если  цель не инициализирована - создаем в соотвесвии с иточником
                target = Activator.CreateInstance(sourse.GetType());
            if (sourse.GetType() != target.GetType()) { return; } //Если источник не инециализирован - выходим

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
            }


            SetCiclingObjectsCatalog(sourse);
            Recursive_depth++;

            if (!AddObjectsTreeMetaData.ContainsKey(sourse_parsing_id))
                AddObjectsTreeMetaData.Add(sourse_parsing_id,
                    new TreeObjectInfo(Recursive_depth, sourse, target, ((IEntityObject)target).Name, false, StatusObjectsInTree.NONE));

            var target_props_infoes = target.GetType().GetProperties().Where(p => p.GetIndexParameters().Length == 0);
            var sourse_props_infoes = sourse.GetType().GetProperties().Where(p => p.GetIndexParameters().Length == 0);
            var target_props_1 = target.GetType().GetProperties().Where(pr => pr.GetType() is IList);

            foreach (PropertyInfo sourse_prop_info in sourse_props_infoes) //Записываем в каталог все объекты внутри объекта источника
            {
                var target_prop_info = target_props_infoes.Where(pr => pr.Name == sourse_prop_info.Name).FirstOrDefault();
                var sourse_prop_value = sourse_prop_info.GetValue(sourse);
                var target_prop_value = target_prop_info.GetValue(target);

                if (sourse_prop_value is IEntityObject)
                {
                    Guid sourse_prop_Id = ((IEntityObject)sourse_prop_value).Id;
                    bool sourse_prop_is_navigate_prop = GetNavigateProperties(sourse).Where(pr => pr.Name == sourse_prop_info?.Name).FirstOrDefault() != null;

                    if (!AddObjectsTreeMetaData.ContainsKey(sourse_prop_Id))//Если мы не встречали данный объект ранее
                    {
                        AddObjectsTreeMetaData.Add(sourse_prop_Id,
                         new TreeObjectInfo(Recursive_depth, sourse_prop_value, target_prop_value, sourse_prop_info.Name,
                         sourse_prop_is_navigate_prop, StatusObjectsInTree.NONE));
                    }
                }

            }

            foreach (PropertyInfo sourse_prop_info in sourse_props_infoes)//Копируем объекты из источника в целевой объект
            {
                var target_prop_info = target_props_infoes.Where(pr => pr.Name == sourse_prop_info.Name).FirstOrDefault();
                var sourse_prop_value = sourse_prop_info.GetValue(sourse);
                var target_prop_value = target_prop_info.GetValue(target);

                if (sourse_prop_value is IEntityObject)
                {
                    Guid sourse_prop_Id = ((IEntityObject)sourse_prop_value).Id;

                    if (target_prop_value == null)
                        Activator.CreateInstance(target_prop_info.PropertyType);
                    if (sourse_prop_value is IEntityObject && AddObjectsTreeMetaData[sourse_prop_Id].Status != StatusObjectsInTree.COPY_DONE)//
                    {

                        if (!AddObjectsTreeMetaData[sourse_prop_Id].IsFirstNavigate)
                        {
                            //   AddObjectsTreeMetaData[sourse_prop_Id].Status = StatusObjectsInTree.LINK_PROCESSED;
                            CopyObjectReflectionNewInstances_Test(sourse_prop_value, target_prop_value, false);
                            //   AddObjectsTreeMetaData[sourse_prop_Id].Status = StatusObjectsInTree.COPY_DONE;
                        }

                    }
                    //  b_value_skip_flag = true;
                }
                else
                if (target_prop_info.CanWrite)
                    target_prop_info.SetValue(target, sourse_prop_value);
                //   b_value_skip_flag = false;
            }

            var va = AddObjectsTreeMetaData.Where(r => r.Value.IsFirstNavigate).ToList();
            var initial_object = AddObjectsTreeMetaData.OrderBy(el => el.Value.RecursiveLevel).First().Value.SourseValue;

            if ((sourse is IList)) //if input parameters is Tlist 
            {
                foreach (IEntityObject element in (sourse as IList))
                {
                    if (!AddObjectsTreeMetaData.ContainsKey(element.Id))//Если мы не встречали данный объект ранее
                    {
                        AddObjectsTreeMetaData.Add(element.Id,
                         new TreeObjectInfo(Recursive_depth, element, null, element.Name, false, StatusObjectsInTree.NONE));
                    }
                }
                foreach (IEntityObject element in (sourse as IList))
                {
                    if (!AddObjectsTreeMetaData.ContainsKey(element.Id) && !AddObjectsTreeMetaData[element.Id].IsFirstNavigate)
                    {

                    }
                    var new_element = Activator.CreateInstance(element.GetType());

                    if (AddObjectsTreeMetaData[element.Id].Status != StatusObjectsInTree.COPY_DONE &&
                         !AddObjectsTreeMetaData[element.Id].IsFirstNavigate)
                    {
                        var target_list = (target as IList);
                        if (!target_list.Contains(element)) target_list.Add(new_element);
                        CopyObjectReflectionNewInstances_Test(element, new_element, false);
                        if (!target_list.Contains(element)) target_list.Add(new_element);

                    }
                }

            }


            AddObjectsTreeMetaData[sourse_parsing_id].Status = StatusObjectsInTree.COPY_DONE;
            Recursive_depth--;
            if (Recursive_depth == InitialRecursive_depth)
            {
                AddObjectsTreeMetaData.Clear();
                ObjectsTreeMetaData.Clear();
                NavigateParametrDepth = 0;
            }
        }
        public static void GetObjectsFullCatalog_Test(object sourse, object target, Dictionary<Guid, TreeObjectInfo> catalog, bool reset = true)
        {
            if (reset)
            {
                tree_level = 0;
            }
            tree_level++;
            if (!catalog.ContainsKey(((IEntityObject)sourse).Id))
                catalog.Add(((IEntityObject)sourse).Id, new TreeObjectInfo(tree_level, sourse, target));

            // if (target == null) target = Activator.CreateInstance(sourse.GetType());
            //   if (sourse == null) sourse = Activator.CreateInstance(target.GetType());

            if (sourse.GetType() != target?.GetType())
                target = null;


            var all_sourse_props = sourse?.GetType().GetProperties() //Выбираем все не идексные свойства
                  .Where(p => p.GetIndexParameters().Length == 0);

            var all_target_props = target?.GetType().GetProperties() //Выбираем все не идексные свойства
               .Where(p => p.GetIndexParameters().Length == 0);

            var all_props = all_sourse_props;

            //if (all_sourse_props == null) all_props = all_target_props;
            var common_prop = sourse;
            // if (sourse == null) common_prop = target;

            foreach (PropertyInfo sourse_prop_info in all_props)
            {
                var sourse_prop_value = sourse_prop_info.GetValue(common_prop);

                if (sourse != null) sourse_prop_value = sourse_prop_info.GetValue(sourse);
                else { sourse = null; sourse_prop_value = null; }

                var target_prop_value = sourse_prop_info.GetValue(common_prop);

                if (target != null) target_prop_value = sourse_prop_info.GetValue(target);
                else { target = null; target_prop_value = null; }

                bool sourse_prop_is_navigate_prop = GetNavigateProperties(common_prop).Where(pr => pr.Name == sourse_prop_info?.Name).FirstOrDefault() != null;
                //    if (!sourse_prop_is_navigate_prop || sourse_prop_is_navigate_prop_enable)
                {
                    if ((sourse_prop_value is IEntityObject || target_prop_value is IEntityObject))
                    {
                        if (sourse_prop_value is bldWork)
                            ;
                        Guid prop_id = Guid.Empty;
                        if (sourse_prop_value != null) prop_id = CoreFunctions.GetParsingId(sourse_prop_value);
                        else if (target_prop_value != null) prop_id = CoreFunctions.GetParsingId(target_prop_value);

                        if (!catalog.ContainsKey(prop_id))
                        {
                            catalog.Add(prop_id, new TreeObjectInfo(tree_level, sourse_prop_value,
                            target_prop_value, sourse_prop_info?.Name, false));
                            GetObjectsFullCatalog_Test(sourse_prop_value, target_prop_value, catalog, false);
                        }
                        else if (catalog[prop_id].RecursiveLevel > tree_level)
                        {
                            catalog[prop_id].RecursiveLevel = tree_level;
                        }
                    }

                    if (tree_level == initial_tree_level && (!sourse_prop_is_navigate_prop || sourse_prop_is_navigate_prop_enable))
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


            }
            if (sourse is IList)
            {
                foreach (IEntityObject soures_element in (IEnumerable<IEntityObject>)sourse)
                {
                    var find_target_obj = target;
                    if (target != null)
                        find_target_obj = ((IEnumerable<IEntityObject>)target).
                       Where(ob => CoreFunctions.GetParsingId(ob) == CoreFunctions.GetParsingId(soures_element)).FirstOrDefault();

                    if (!catalog.ContainsKey(((IEntityObject)soures_element).Id))
                        catalog.Add(((IEntityObject)soures_element).Id, new TreeObjectInfo(tree_level, soures_element, find_target_obj));

                    GetObjectsFullCatalog_Test(soures_element, find_target_obj, catalog, false);
                }
            }
            tree_level--;
            if (tree_level == initial_tree_level && sourse_prop_is_navigate_prop_enable == false)
            {
                //   sourse_prop_is_navigate_prop_enable = true;
                //   GetObjectsFullCatalog_Test(sourse, target, catalog, false);

            }
        }

        public static void GetObjectsFullCatalog_Test_buckaup(object sourse, object target, Dictionary<Guid, TreeObjectInfo> catalog, bool reset = true)
        {
            if (reset)
            {
                tree_level = catalog.OrderBy(el => el.Value.RecursiveLevel).Last().Value.RecursiveLevel;
                initial_tree_level = tree_level;
            }
            tree_level++;
            if (!catalog.ContainsKey(((IEntityObject)sourse).Id))
                catalog.Add(((IEntityObject)sourse).Id, new TreeObjectInfo(tree_level, sourse, target));

            // if (target == null) target = Activator.CreateInstance(sourse.GetType());
            //   if (sourse == null) sourse = Activator.CreateInstance(target.GetType());

            if (sourse.GetType() != target?.GetType())
                target = null;


            var all_sourse_props = sourse?.GetType().GetProperties() //Выбираем все не идексные свойства
                  .Where(p => p.GetIndexParameters().Length == 0);

            var all_target_props = target?.GetType().GetProperties() //Выбираем все не идексные свойства
               .Where(p => p.GetIndexParameters().Length == 0);

            var all_props = all_sourse_props;

            //if (all_sourse_props == null) all_props = all_target_props;
            var common_prop = sourse;
            // if (sourse == null) common_prop = target;

            foreach (PropertyInfo sourse_prop_info in all_props)
            {
                var sourse_prop_value = sourse_prop_info.GetValue(common_prop);

                if (sourse != null) sourse_prop_value = sourse_prop_info.GetValue(sourse);
                else { sourse = null; sourse_prop_value = null; }

                var target_prop_value = sourse_prop_info.GetValue(common_prop);

                if (target != null) target_prop_value = sourse_prop_info.GetValue(target);
                else { target = null; target_prop_value = null; }

                bool sourse_prop_is_navigate_prop = GetNavigateProperties(common_prop).Where(pr => pr.Name == sourse_prop_info?.Name).FirstOrDefault() != null;
                //    if (!sourse_prop_is_navigate_prop || sourse_prop_is_navigate_prop_enable)
                {
                    if ((sourse_prop_value is IEntityObject || target_prop_value is IEntityObject))
                    {
                        if (sourse_prop_value is bldWork)
                            ;
                        Guid prop_id = Guid.Empty;
                        if (sourse_prop_value != null) prop_id = CoreFunctions.GetParsingId(sourse_prop_value);
                        else if (target_prop_value != null) prop_id = CoreFunctions.GetParsingId(target_prop_value);

                        if (!catalog.ContainsKey(prop_id))
                        {
                            catalog.Add(prop_id, new TreeObjectInfo(tree_level, sourse_prop_value,
                            target_prop_value, sourse_prop_info?.Name, false));
                            GetObjectsFullCatalog_Test(sourse_prop_value, target_prop_value, catalog, false);
                        }
                        else if (catalog[prop_id].RecursiveLevel > tree_level)
                        {
                            catalog[prop_id].RecursiveLevel = tree_level;
                        }
                    }

                    if (tree_level == initial_tree_level && (!sourse_prop_is_navigate_prop || sourse_prop_is_navigate_prop_enable))
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


            }
            if (sourse is IList)
            {
                foreach (IEntityObject soures_element in (IEnumerable<IEntityObject>)sourse)
                {
                    var find_target_obj = target;
                    if (target != null)
                        find_target_obj = ((IEnumerable<IEntityObject>)target).
                       Where(ob => CoreFunctions.GetParsingId(ob) == CoreFunctions.GetParsingId(soures_element)).FirstOrDefault();

                    if (!catalog.ContainsKey(((IEntityObject)soures_element).Id))
                        catalog.Add(((IEntityObject)soures_element).Id, new TreeObjectInfo(tree_level, soures_element, find_target_obj));

                    GetObjectsFullCatalog_Test(soures_element, find_target_obj, catalog, false);
                }
            }
            tree_level--;
            if (tree_level == initial_tree_level && sourse_prop_is_navigate_prop_enable == false)
            {
                //   sourse_prop_is_navigate_prop_enable = true;
                //   GetObjectsFullCatalog_Test(sourse, target, catalog, false);

            }
        }

        public static void CopyObjectReflectionNewInstances_Test_1(object sourse, object target, bool objectsTreeCatalogReset = true)
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
                Dictionary<Guid, TreeObjectInfo> up_nav_tree = new Dictionary<Guid, TreeObjectInfo>(); //Каталог навигациорных свойств 
                if (GetNavigateProperties(sourse).Count > 0)
                    GetNavigatePropertyUpTree(sourse, up_nav_tree);
                else if (GetNavigateProperties(target).Count > 0)
                    GetNavigatePropertyUpTree(target, up_nav_tree);

                foreach (var up_nav_prop in up_nav_tree)
                    ObjectsTreeMetaData.Add(up_nav_prop.Key, new TreeObjectInfo(up_nav_prop.Value));

                var obj_tree_root_object = up_nav_tree.OrderBy(ob => ob.Value.RecursiveLevel).Last().Value.SourseValue;

                //   GetNavigatePropertyDownTree(sourse, ObjectsTreeMetaData, true);

                GetObjectsFullCatalog_Test(obj_tree_root_object, target, ObjectsTreeMetaData);//Создать каталог всех объектов входящих в объект и( объектов на которые есть ссылки ??)
                var obb = ObjectsTreeMetaData.Where(el => el.Value.RecursiveLevel <= 1).ToList();
                var fn = ObjectsTreeMetaData.Where(ob => ob.Value.IsFirstNavigate == true).ToList();

                foreach (var up_nav_prop in up_nav_tree)
                    foreach (var tr_obj in ObjectsTreeMetaData.Where(val => val.Key == up_nav_prop.Key))
                    {
                        tr_obj.Value.IsFirstNavigate = true;
                        tr_obj.Value.RecursiveLevel = up_nav_prop.Value.RecursiveLevel;
                    }

                var ii = ObjectsTreeMetaData.Where(r => r.Value.RecursiveLevel <= 0).ToList();



            }
            Recursive_depth++;

            if (!AddObjectsTreeMetaData.ContainsKey(sourse_parsing_id))
                AddObjectsTreeMetaData.Add(sourse_parsing_id,
                    new TreeObjectInfo(Recursive_depth, sourse, target, ((IEntityObject)target).Name, false, StatusObjectsInTree.LINK_PROCESSED));

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

                    if ((sourse_prop_value is IEntityObject || target_prop_value is IEntityObject)) //Если свойство является объектом..&&!(sourse is IList)
                    {
                        Guid ParsingId = Guid.Empty;
                        if (GetParsingId(sourse_prop_value) != Guid.Empty)
                            ParsingId = GetParsingId(sourse_prop_value);
                        else if (GetParsingId(target_prop_value) != Guid.Empty)
                            ParsingId = GetParsingId(target_prop_value);

                        if (!ObjectsTreeMetaData[ParsingId].IsFirstNavigate) //Если свойтва не навигационное ссылающеся на верние уровни ...
                        {
                            if (!AddObjectsTreeMetaData.ContainsKey(ParsingId)) //если не встречали  и копировали ранее
                            {
                                if (sourse_prop_value != null && target_prop_value == null) //Еслим целевой объект нулл содаем новый 
                                {
                                    new_target_prop_value = Activator.CreateInstance(sourse_prop_value.GetType());

                                    var sr = ObjectsTreeMetaData[ParsingId].SourseValue;
                                    if (sourse_prop_value is bldWork)
                                        ;
                                    if (ObjectsTreeMetaData[ParsingId].RecursiveLevel <= 1)
                                        ;
                                    CopyObjectReflectionNewInstances_Test_1(sourse_prop_value, new_target_prop_value, false);

                                }
                                else if (sourse_prop_value != null && target_prop_value != null)//Если объект  не встречалься и  не является навигационныс свойством ...
                                {
                                    if (sourse_prop_value is bldWork)
                                        ;
                                    if (ObjectsTreeMetaData[ParsingId].RecursiveLevel <= 0)
                                        ;
                                    CopyObjectReflectionNewInstances_Test_1(sourse_prop_value, new_target_prop_value, false);
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
                                if (Recursive_depth == 1) //Если мы находися в самом корне копируемого объекта
                                {
                                    if (sourse_prop_value == null && target_prop_value != null) // если навигационное в таргете , то нетрогаем его... 
                                    {
                                        new_target_prop_value = target_prop_value;
                                        set_value_skip_flag = true;//устанавливаем флаг чтобы ниже пропустить присваивание свойства..
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
                    else
                        new_target_prop_value = sourse_prop_value;

                    if (target_prop.CanWrite && !set_value_skip_flag) target_prop.SetValue(target, new_target_prop_value);

                    set_value_skip_flag = false;
                }
            }

            if ((sourse is IList) && !((IContainerFunctionabl)sourse).IsPointerContainer) //if input parameters is Tlist 
            {

                foreach (IEntityObject sourse_element in (IEnumerable<IEntityObject>)sourse)  //Регистрируем все объекты списка...
                {
                    var find_obj = ((IEnumerable<IEntityObject>)target).
                          Where(ob => CoreFunctions.GetParsingId(ob) == CoreFunctions.GetParsingId(sourse_element)).FirstOrDefault();

                    if (!AddObjectsTreeMetaData.ContainsKey(GetParsingId(sourse_element)))
                        AddObjectsTreeMetaData.Add(GetParsingId(sourse_element), new TreeObjectInfo(Recursive_depth, sourse_element, find_obj, sourse_element.Name, false, StatusObjectsInTree.LINK_PROCESSED));
                }
                if (sourse is bldWorksGroup)
                    ;
                foreach (IEntityObject sourse_element in (IEnumerable<IEntityObject>)sourse)//Копируем ...
                {
                    var find_obj = ((IEnumerable<IEntityObject>)target).
                        Where(ob => CoreFunctions.GetParsingId(ob) == CoreFunctions.GetParsingId(sourse_element)).FirstOrDefault();

                    Guid sourse_element_id = GetParsingId(sourse_element);
                    if (find_obj == null)//Если в целевом объекте нет элемента с таким Id
                    {
                        if (AddObjectsTreeMetaData[sourse_element_id].Status == StatusObjectsInTree.LINK_PROCESSED
                            && AddObjectsTreeMetaData[sourse_element_id].TargetValue == null)
                        {
                            if (sourse_element is bldWork)
                                ;
                            var el = ObjectsTreeMetaData[sourse_element_id];
                            if (ObjectsTreeMetaData[sourse_element_id].RecursiveLevel <= 1)
                                ;
                            var new_obj = Activator.CreateInstance(sourse_element.GetType());
                            AddObjectsTreeMetaData[sourse_element_id].TargetValue = new_obj;
                            CopyObjectReflectionNewInstances_Test_1(sourse_element, new_obj, false);
                            ((IList)target).Add(new_obj);
                        }
                        else if (AddObjectsTreeMetaData[sourse_element_id].Status == StatusObjectsInTree.LINK_PROCESSED
                            && AddObjectsTreeMetaData[sourse_element_id].TargetValue != null)
                        {
                            CopyObjectReflectionNewInstances_Test_1(sourse_element, AddObjectsTreeMetaData[GetParsingId(sourse_element)].TargetValue, false);
                            ((IList)target).Add(AddObjectsTreeMetaData[sourse_element_id].TargetValue);
                        }
                        else
                        {
                            ((IList)target).Add(AddObjectsTreeMetaData[sourse_element_id].TargetValue);
                        }
                    }
                    else if (find_obj != null)
                    {
                        if (AddObjectsTreeMetaData[sourse_element_id].Status == StatusObjectsInTree.LINK_PROCESSED)
                        {

                            CopyObjectReflectionNewInstances_Test_1(sourse_element, find_obj, false);
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

            if (sourse is bldWorksGroup)
                ;
            AddObjectsTreeMetaData[sourse_parsing_id].Status = StatusObjectsInTree.COPY_DONE;
            Recursive_depth--;
            if (Recursive_depth == InitialRecursive_depth)
            {
                AddObjectsTreeMetaData.Clear();
                ObjectsTreeMetaData.Clear();
                NavigateParametrDepth = 0;
            }
        }

        public static void CopyObjectReflectionNewInstances(object sourse, object target, bool objectsTreeCatalogReset = true)
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
            //else
            //    AddObjectsTreeMetaData[sourse_parsing_id].Status = StatusObjectsInTree.COPY_DONE;
            //if (sourse is IEntityObject)
            // {
            //     ObjectsTreeMetaData[sourse_parsing_id].TargetValue = target;
            //     ObjectsTreeMetaData[sourse_parsing_id].SourseValue = sourse;
            // }

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

                    if ((sourse_prop_value is IEntityObject || target_prop_value is IEntityObject)) //Если свойство является объектом..&&!(sourse is IList)
                    {
                        Guid ParsingId = Guid.Empty;
                        if (GetParsingId(sourse_prop_value) != Guid.Empty)
                            ParsingId = GetParsingId(sourse_prop_value);
                        else if (GetParsingId(target_prop_value) != Guid.Empty)
                            ParsingId = GetParsingId(target_prop_value);

                        if (!ObjectsTreeMetaData[ParsingId].IsFirstNavigate) //Если свойтва не навигационное ссылающеся на верние уровни ...
                        {
                            if (!AddObjectsTreeMetaData.ContainsKey(ParsingId)) //если  не встречали   и копировали ранее
                            {
                                if (sourse_prop_value != null && target_prop_value == null) //Еслим целевой объект нулл содаем новый 
                                {
                                    new_target_prop_value = Activator.CreateInstance(sourse_prop_value.GetType());
                                    CopyObjectReflectionNewInstances(sourse_prop_value, new_target_prop_value, false);

                                }
                                else if (sourse_prop_value != null && target_prop_value != null)//Если объект  не встречалься и  не является навигационныс свойством ...
                                {
                                    CopyObjectReflectionNewInstances(sourse_prop_value, new_target_prop_value, false);
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
                    else
                        new_target_prop_value = sourse_prop_value;

                    if (target_prop.CanWrite && !set_value_skip_flag) target_prop.SetValue(target, new_target_prop_value);

                    set_value_skip_flag = false;
                }
            }

            if ((sourse is IList) && !((IContainerFunctionabl)sourse).IsPointerContainer) //if input parameters is Tlist 
            {

                foreach (IEntityObject sourse_element in (IEnumerable<IEntityObject>)sourse)  //Регистрируем все объекты списка...
                {
                    var find_obj = ((IEnumerable<IEntityObject>)target).
                          Where(ob => CoreFunctions.GetParsingId(ob) == CoreFunctions.GetParsingId(sourse_element)).FirstOrDefault();

                    if (!AddObjectsTreeMetaData.ContainsKey(GetParsingId(sourse_element)))
                        AddObjectsTreeMetaData.Add(GetParsingId(sourse_element), new TreeObjectInfo(Recursive_depth, sourse_element, find_obj, sourse_element.Name, false, StatusObjectsInTree.LINK_PROCESSED));
                }

                foreach (IEntityObject sourse_element in (IEnumerable<IEntityObject>)sourse)//Копируем ...
                {
                    var find_obj = ((IEnumerable<IEntityObject>)target).
                        Where(ob => CoreFunctions.GetParsingId(ob) == CoreFunctions.GetParsingId(sourse_element)).FirstOrDefault();

                    if (find_obj == null)
                    {
                        if (AddObjectsTreeMetaData[GetParsingId(sourse_element)].Status == StatusObjectsInTree.LINK_PROCESSED
                            && AddObjectsTreeMetaData[GetParsingId(sourse_element)].TargetValue == null)
                        {
                            var new_obj = Activator.CreateInstance(sourse_element.GetType());
                            AddObjectsTreeMetaData[GetParsingId(sourse_element)].TargetValue = new_obj;
                            CopyObjectReflectionNewInstances(sourse_element, new_obj, false);
                            ((IList)target).Add(new_obj);
                        }
                        else if (AddObjectsTreeMetaData[GetParsingId(sourse_element)].Status == StatusObjectsInTree.LINK_PROCESSED
                            && AddObjectsTreeMetaData[GetParsingId(sourse_element)].TargetValue != null)
                        {
                            CopyObjectReflectionNewInstances(sourse_element, AddObjectsTreeMetaData[GetParsingId(sourse_element)].TargetValue, false);
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

                            CopyObjectReflectionNewInstances(sourse_element, find_obj, false);
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

        #endregion
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

        public static void GetObjectsCatalog(object obj, Dictionary<Guid, object> catalog) //Добаляет в  каталог дерево всех объектов  внутри объекта 
        {
            var all_props = obj.GetType().GetProperties() //Выбираем все не идексные свойства
                  .Where(p => p.GetIndexParameters().Length == 0);

            catalog.Add(((IEntityObject)obj).Id, obj);//Добавляем в катлог сам объект

            foreach (PropertyInfo prop_info in all_props) //Проходим по его свойствам
            {
                var prop_value = prop_info.GetValue(obj);
                if ((prop_value is IEntityObject) && !catalog.ContainsKey(((IEntityObject)prop_value).Id))//Если свойство являеся объектом и его не было в каталоге
                    GetObjectsCatalog(prop_value, catalog); //добавляем его в каталог
            }
            if (obj is IList)//Если свойство представляет собой список
            {
                foreach (IEntityObject element in (IEnumerable<IEntityObject>)obj) ///добавляем все его элементы в каталог рекрсивоно
                    GetObjectsCatalog(element, catalog);
            }

            // return result;
        }
        static int recursive_level = 0;

        //public static void SetStructureLevels(object obj,ref  StructureLevel up_structure_level , bool reset = true) //Устанваливает все свойства LevelInStructure объектов 
        //{
        //    var all_props = obj.GetType().GetProperties() //Выбираем все не идексные свойства
        //          .Where(p => p.GetIndexParameters().Length == 0);

        //    if (reset) { recursive_level = 0; }
        //    recursive_level++;
        //    int structure_number = 0;
        //    foreach (PropertyInfo prop_info in all_props) //Проходим по его свойствам
        //    {
        //        var prop_value = prop_info.GetValue(obj);
        //        bool is_nvigate_prop = GetNavigateProperties(obj).Where(pr => pr.Name == prop_info?.Name).FirstOrDefault() != null;
        //        if ((prop_value is IEntityObject) && !is_nvigate_prop)//Если свойство являеся объектом и его не было в каталоге
        //        {
        //            if ((prop_value is ILevelable) && !((ILevelable)prop_value).StructureLevel.IsDefined)
        //            {
        //                StructureLevel new_structure_level = new StructureLevel(up_structure_level.StructureLevels.Count);
        //                up_structure_level.StructureLevels.Add(new_structure_level);
        //                new_structure_level.ParentStructureLevel = up_structure_level;
        //              //  new_structure_level.Code = ((IEntityObject)prop_value).Name;
        //                ((ILevelable)prop_value).StructureLevel = new_structure_level;
        //                ((ILevelable)prop_value).StructureLevel.IsDefined = true;

        //                // SetStructureLevels(prop_value, ref new_structure_level, false);
        //                up_structure_level = new_structure_level;
        //            }
        //            //    else
        //            SetStructureLevels(prop_value, ref up_structure_level, false);
        //            //     prop_info.SetValue(obj, prop_value);
        //        }
        //        else 
        //        {
        //        }
        //    }

        //    if (obj is IList)//Если свойство представляет собой список
        //    {
        //        structure_number = 0;
        //        foreach (IEntityObject element in (IEnumerable<IEntityObject>)obj) ///добавляем все его элементы в каталог рекрсивоно
        //        {
        //            if ((element is ILevelable) && !((ILevelable)element).StructureLevel.IsDefined)
        //            {

        //                //   up_structure_level.MaxNumber++;
        //                StructureLevel new_structure_level = new StructureLevel(up_structure_level.Number);
        //                new_structure_level.Number = up_structure_level.StructureLevels.Count;
        //                up_structure_level.StructureLevels.Add(new_structure_level);
        //            //    new_structure_level.Code = element.Name;
        //                new_structure_level.ParentStructureLevel = up_structure_level;

        //                ((ILevelable)element).StructureLevel = new_structure_level;
        //                ((ILevelable)element).StructureLevel.IsDefined = true;
        //             //   new_structure_level = ((ILevelable)element).StructureLevel;
        //                SetStructureLevels(element, ref new_structure_level,false);
        //                new_structure_level = null;
        //            }
        //         //   up_structure_level.StructureLevels.Add(new StructureLevel(1));
        //        }
        //    }
        //    recursive_level--;

        //}
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

        private static Dictionary<Guid, object> SetNavigatePropsToNullInTreeObjectsTreeMetaData { get; set; } = new Dictionary<Guid, object>();
        public static void SetNavigatePropsToNullInTree(object obj, string navigatePropName, bool catalogTreeReset = true)
        {
            if (catalogTreeReset)
            {
                SetNavigatePropsToNullInTreeObjectsTreeMetaData.Clear();
            }
            if (!SetNavigatePropsToNullInTreeObjectsTreeMetaData.ContainsKey(((IEntityObject)obj).Id))
                SetNavigatePropsToNullInTreeObjectsTreeMetaData.Add(((IEntityObject)obj).Id, obj);
            var all_props = obj.GetType().GetProperties() //Выбираем все не идексные свойства
                  .Where(p => p.GetIndexParameters().Length == 0);

            foreach (PropertyInfo propertyInfo in GetNavigateProperties(obj))//Есом свойство навигационное...
            {
                var nav_prop_val = propertyInfo.GetValue(obj);
                if (navigatePropName == propertyInfo.Name)
                    if (propertyInfo.CanWrite) propertyInfo.SetValue(obj, null);
            }
            foreach (PropertyInfo prop_info in all_props)
            {
                var prop_value = prop_info.GetValue(obj);
                if ((prop_value is IEntityObject) && prop_value != null)
                {
                    if (!SetNavigatePropsToNullInTreeObjectsTreeMetaData.ContainsKey(((IEntityObject)prop_value).Id))
                        SetNavigatePropsToNullInTree(prop_value, navigatePropName, false);
                    if (prop_info.CanWrite) prop_info.SetValue(obj, prop_value);
                }

            }
            if (obj is IList)
            {
                foreach (IEntityObject element in (IEnumerable<IEntityObject>)obj)
                {
                    SetNavigatePropsToNullInTree(element, navigatePropName, false);
                }
            }

            // return result;
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

        public static void CopyObjectReflection(object sourse, object target)
        {

            if (sourse == null) { target = null; return; } //Если источник не инециализирован - выходим

            if (target == null) //Если  цель не инициализирована - создаем в соотвесвии с иточником
                target = Activator.CreateInstance(sourse.GetType());

            Guid sourse_parsing_id = Guid.Empty;
            if (GetParsingId(sourse) != Guid.Empty) //Определяем Id - сперва ищем в источнике, потом в приемнике ( последовательно)
                sourse_parsing_id = GetParsingId(sourse);
            else if (GetParsingId(target) != Guid.Empty)
                sourse_parsing_id = GetParsingId(target);

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



                    if ((sourse_prop_value is IEntityObject || target_prop_value is IEntityObject)) //Если свойство является объектом..&&!(sourse is IList)
                    {
                        if (sourse_prop_value != null && target_prop_value == null) //Еслим целевой объект нулл содаем новый 
                        {
                            new_target_prop_value = Activator.CreateInstance(sourse_prop_value.GetType());
                            CopyObjectReflection(sourse_prop_value, new_target_prop_value);

                        }
                        else if (sourse_prop_value != null && target_prop_value != null)//Если объект  не встречалься и  не является навигационныс свойством ...
                        {
                            CopyObjectReflection(sourse_prop_value, new_target_prop_value);
                        }
                    }
                    else
                        new_target_prop_value = sourse_prop_value;

                    if (target_prop.CanWrite) target_prop.SetValue(target, new_target_prop_value);

                }
            }

            if ((sourse is IList)) //if input parameters is Tlist 
            {

                foreach (IEntityObject sourse_element in (IEnumerable<IEntityObject>)sourse)//Копируем ...
                {
                    var find_obj = ((IEnumerable<IEntityObject>)target).
                        Where(ob => CoreFunctions.GetParsingId(ob) == CoreFunctions.GetParsingId(sourse_element)).FirstOrDefault();

                    if (find_obj == null)
                    {
                        if (AddObjectsTreeMetaData[GetParsingId(sourse_element)].Status == StatusObjectsInTree.LINK_PROCESSED
                            && AddObjectsTreeMetaData[GetParsingId(sourse_element)].TargetValue == null)
                        {
                            var new_obj = Activator.CreateInstance(sourse_element.GetType());
                            AddObjectsTreeMetaData[GetParsingId(sourse_element)].TargetValue = new_obj;
                            //  CopyObjectReflection(sourse_element, new_obj, false);
                            ((IList)target).Add(new_obj);
                        }
                        else if (AddObjectsTreeMetaData[GetParsingId(sourse_element)].Status == StatusObjectsInTree.LINK_PROCESSED
                            && AddObjectsTreeMetaData[GetParsingId(sourse_element)].TargetValue != null)
                        {
                            //    CopyObjectReflection(sourse_element, AddObjectsTreeMetaData[GetParsingId(sourse_element)].TargetValue, false);
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

                            //     CopyObjectReflection(sourse_element, find_obj, false);
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

        public static void SelectElementFromCollectionWhithDialog<TContainer, T>(TContainer collection,
                IDialogService dialogService, Action<IDialogResult> action,
                string dialogViewName,
                string title = "",
                string message = "",
                string collectionName = "")
            where TContainer : ICollection<T>
            where T : class, new()
        {

            var dialog_par = new DialogParameters();
            dialog_par.Add("title", title);
            dialog_par.Add("message", message);
            dialog_par.Add("current_collection", collection);
            dialog_par.Add("current_collection_name", collectionName);
            dialog_par.Add("confirm_button_content", "Добавить");
            dialog_par.Add("refuse_button_content", "Закрыть");
            dialogService.ShowDialog(dialogViewName, dialog_par, action);

        }


        /// <summary>
        /// 
        /// </summary>

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
                    if (sourse_prop_value != null) prop_id = CoreFunctions.GetParsingId(sourse_prop_value);
                    else if (target_prop_value != null) prop_id = CoreFunctions.GetParsingId(target_prop_value);

                    if (!catalog.ContainsKey(prop_id))
                    {
                        catalog.Add(prop_id, new TreeObjectInfo(tree_level, sourse_prop_value,
                        target_prop_value, sourse_prop_info?.Name, false));
                        GetObjectsFullCatalog(sourse_prop_value, target_prop_value, catalog, false);
                    }
                }

                //if (sourse_prop_info.CanWrite) sourse_prop_info.SetValue(sourse, sourse_prop_value);
                //if (sourse_prop_info.CanWrite) sourse_prop_info.SetValue(target, target_prop_value);
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
                       Where(ob => CoreFunctions.GetParsingId(ob) == CoreFunctions.GetParsingId(soures_element)).FirstOrDefault();

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
        public static void GetNavigatePropertyDownTree(object obj, Dictionary<Guid, TreeObjectInfo> result_ctalog, bool reset = true) //Возращает дерево навигационных свойсв объекта
        {
            // int tree_level = 0;
            if (reset)
            {
                //  result_ctalog.Clear();
                tree_level = 0;
            }
            tree_level++;
            if (obj != null)
            {
                var obj_all_props = obj.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);
                //    var obj_all_navigate_props = GetNavigateProperties(obj); //Получаем список навигационных свойств объекта

                foreach (PropertyInfo prop_info in obj_all_props)
                {
                    var prop_val = prop_info.GetValue(obj);
                    if (prop_val != null && prop_val is IEntityObject)
                    {
                        bool prop_is_navigate_prop = GetNavigateProperties(obj).Where(pr => pr.Name == prop_info?.Name).FirstOrDefault() != null;
                        Guid obj_id = ((IEntityObject)prop_val).Id;
                        if (!result_ctalog.ContainsKey(obj_id) && prop_is_navigate_prop) //Если в каталоге нет 
                            result_ctalog.Add(obj_id, new TreeObjectInfo(tree_level, prop_val, null, prop_info.Name, false));
                        else
                            GetNavigatePropertyDownTree(prop_val, result_ctalog, false);
                    }
                }
                if (obj is IList)
                {
                    foreach (IEntityObject soures_element in (IEnumerable<IEntityObject>)obj)
                    {
                        if (!result_ctalog.ContainsKey(soures_element.Id))
                            GetNavigatePropertyDownTree(soures_element, result_ctalog, false);
                    }
                }

            }
            tree_level--;
            if (tree_level == 0)
            {

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

        public static void GetObjectsCatalog(object obj, Dictionary<Guid, object> catalog)
        {

            var all_props = obj.GetType().GetProperties() //Выбираем все не идексные свойства
                  .Where(p => p.GetIndexParameters().Length == 0);
            catalog.Add(((IEntityObject)obj).Id, obj);

            foreach (PropertyInfo prop_info in all_props)
            {
                var prop_value = prop_info.GetValue(obj);
                if ((prop_value is IEntityObject) && !catalog.ContainsKey(((IEntityObject)prop_value).Id))
                    GetObjectsCatalog(prop_value, catalog);
            }
            if (obj is IList)
            {
                foreach (IEntityObject element in (IEnumerable<IEntityObject>)obj)
                    GetObjectsCatalog(element, catalog);
            }

            // return result;
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
