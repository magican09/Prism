﻿using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
//using System.Windows.Forms;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public abstract class AddElementsToCollectionDialogFromListViewModel<TConteiner, T> : LocalBindableBase, IDialogAware
        where TConteiner : ICollection<T>/*, INameable, INameableOservableCollection<T>*/, new()
        where T : class, IEntityObject, new()
    {
        private string _title = "Диалоговое окно сообщения";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }
        private string _commonCollectionName;
        public string CommonCollectionName
        {
            get { return _commonCollectionName; }
            set { SetProperty(ref _commonCollectionName, value); }
        }
        private string _currentCollectionName;
        public string CurrentCollectionName

        {

            get { return _currentCollectionName; }
            set { SetProperty(ref _currentCollectionName, value); }
        }

        private string _confirm;
        public string Confirm
        {
            get { return _confirm; }
            set { SetProperty(ref _confirm, value); }
        }
        private string _refuse;
        public string Refuse
        {
            get { return _refuse; }
            set { SetProperty(ref _refuse, value); }
        }

        private TConteiner _commonCollection;
        public TConteiner CommonCollection
        {
            get { return _commonCollection; }
            set { SetProperty(ref _commonCollection, value); }
        }
        private TConteiner _sortedCommonCollection = new TConteiner();
        public TConteiner SortedCommonCollection
        {
            get { return _sortedCommonCollection; }
            set { SetProperty(ref _sortedCommonCollection, value); }
        }
        private TConteiner _filteredCommonCollection = new TConteiner();
        public TConteiner FilteredCommonCollection
        {
            get { return _filteredCommonCollection; }
            set { SetProperty(ref _filteredCommonCollection, value); }
        }
        private TConteiner _currentCollection;
        public TConteiner CurrentCollection
        {
            get { return _currentCollection; }
            set { SetProperty(ref _currentCollection, value); }
        }
        private ObservableCollection<NameableObjectPointer> _filteredCommonPointersCollection = new ObservableCollection<NameableObjectPointer>();
        public ObservableCollection<NameableObjectPointer> FilteredCommonPointersCollection
        {
            get { return _filteredCommonPointersCollection; }
            set { SetProperty(ref _filteredCommonPointersCollection, value); }
        }
        private ObservableCollection<NameableObjectPointer> _sortedCommonPointersCollection = new ObservableCollection<NameableObjectPointer>();
        public ObservableCollection<NameableObjectPointer> SortedCommonPointersCollection
        {
            get { return _sortedCommonPointersCollection; }
            set { SetProperty(ref _sortedCommonPointersCollection, value); }
        }

        private T _selectedElement;


        public T SelectedElement
        {
            get { return _selectedElement; }
            set { SetProperty(ref _selectedElement, value); }
        }
        private T _newElement;


        public T NewElement
        {
            get { return _newElement; }
            set { SetProperty(ref _newElement, value); }
        }

        private Guid _currentContextId;


        public Guid CurrentContextId
        {
            get { return _currentContextId; }
            set { SetProperty(ref _currentContextId, value); }
        }

        private NameablePredicateObservableCollection<TConteiner, T> _predicateCollection;

        public NameablePredicateObservableCollection<TConteiner, T> PredicateCollection
        {
            get { return _predicateCollection; }
            set { SetProperty(ref _predicateCollection, value); }
        }

        private NameableObservabelObjectsCollection _treeViewCollection = new NameableObservabelObjectsCollection();

        public NameableObservabelObjectsCollection TreeViewCollection
        {
            get { return _treeViewCollection; }
            set { SetProperty(ref _treeViewCollection, value); }
        }


        private NameablePredicate<TConteiner, T> _selectedPredicate;


        public NameablePredicate<TConteiner, T> SelectedPredicate
        {
            get { return _selectedPredicate; }
            set { SetProperty(ref _selectedPredicate, value); }
        }


        public NotifyCommand<object> ConfirmDialogCommand { get; private set; }
        public NotifyCommand<object> TestConfirmDialogCommand { get; private set; }
        public NotifyCommand CloseDialogCommand { get; private set; }
        public NotifyCommand CreateNewElementCommand { get; private set; }
        public NotifyCommand CreateElementOnPatternInstanceCommand { get; private set; }
        //public NotifyCommand AddToCollectionCommand { get; private set; }
        public NotifyCommand RemoveFromCollectionCommand { get; private set; }
        public NotifyCommand SortingCommand { get; private set; }
        public NotifyCommand<object> FilteredElementCommand { get; private set; }
        public NotifyCommand<object> TreeViewSelectionChangeCommand { get; private set; }


        private bool _filterEnable;

        public bool FilterEnable
        {
            get { return _filterEnable; }
            set { SetProperty(ref _filterEnable, value); }
        }

        public event Action<IDialogResult> RequestClose;
        public event Action<IDialogResult> RequestNewObject;
        public string NewObjectDialogName { get; set; }
        public UnDoReDoSystem UnDoReDo { get; set; }
        private readonly IDialogService _dialogService;
        public AddElementsToCollectionDialogFromListViewModel(IDialogService dialogService)
        {
            UnDoReDo = new UnDoReDoSystem();
            CloseDialogCommand = new NotifyCommand(CloseDialog);
            ConfirmDialogCommand = new NotifyCommand<object>(ConfirmDialog);
            TestConfirmDialogCommand = new NotifyCommand<object>(OnTestConfirmDialog);
            CreateNewElementCommand = new NotifyCommand(OnCreateNewElement);
            CreateElementOnPatternInstanceCommand = new NotifyCommand(OnCreateElementOnPatternInstance, CanCreateElementOnPatternInstance)
                      .ObservesProperty(() => SelectedElement);
            TreeViewSelectionChangeCommand = new NotifyCommand<object>(OnTreeViewSelectionChange);

            SortingCommand = new NotifyCommand(OnSortingCommand, CanSorting)
           .ObservesProperty(() => SelectedPredicate);
            FilteredElementCommand = new NotifyCommand<object>(OnFilteredElement);
          
            _dialogService = dialogService;
        }

        private void OnTreeViewSelectionChange(object selected_collection)
        {
            if (selected_collection != null && selected_collection.GetType() == CommonCollection.GetType())
            {
                //FilteredCommonCollection.Clear();
                //SortedCommonCollection.Clear();
                //foreach (T element in selected_collection as ICollection<T>)
                //{
                //    SortedCommonCollection.Add(element);
                //    FilteredCommonCollection.Add(element);
                //}
                FilteredCommonPointersCollection.Clear();
                SortedCommonPointersCollection.Clear();
                foreach (NameableObjectPointer element in selected_collection as ICollection<NameableObjectPointer>)
                {
                    SortedCommonPointersCollection.Add(element);
                    FilteredCommonPointersCollection.Add(element);
                }

            }
        }

        private void OnFilteredElement(object obj)
        {
            //ComboBox comboBox = obj as ComboBox;
            //string combo_box_text = comboBox.Text;
            //comboBox.IsDropDownOpen = true;

            //ObservableCollection<T> finded_elements =
            //    new ObservableCollection<T>(CommonCollection.Where(el=>el.Name.Contains(combo_box_text)).ToList());
            //comboBox.ItemsSource = finded_elements;
            if (!FilterEnable) return;
            TextBox textBox = obj as TextBox;
            string text_box_text = textBox.Text;
            //FilteredCommonCollection.Clear();
            //foreach (T elemtnt in SortedCommonCollection.Where(el => el.Name.Contains(text_box_text)))
            //    FilteredCommonCollection.Add(elemtnt);

            FilteredCommonPointersCollection.Clear();
            foreach (NameableObjectPointer elemtnt in SortedCommonPointersCollection.Where(el => el.Name.Contains(text_box_text)))
                FilteredCommonPointersCollection.Add(elemtnt);


        }

        private bool CanSorting()
        {
            return SelectedPredicate != null; ;
        }

        private void OnSortingCommand()
        {
            //SortedCommonCollection.Clear();
            //FilteredCommonCollection.Clear();
            // foreach (T element in SelectedPredicate.Predicate.Invoke(CommonCollection))
            //{
            //    SortedCommonCollection.Add(element);
            //    FilteredCommonCollection.Add(element);
            // }
            SortedCommonPointersCollection.Clear();
            FilteredCommonPointersCollection.Clear();
            foreach (NameableObjectPointer element in SelectedPredicate.CollectionSelectPredicate.Invoke(CommonCollection))
            {
                SortedCommonPointersCollection.Add(element);
                FilteredCommonPointersCollection.Add(element);
            }
        }

        private bool CanRemoveFromCollection()
        {
            return CurrentCollection.Contains(SelectedElement);
        }



        private bool CanCreateElementOnPatternInstance()
        {
            return SelectedElement != null;
        }

        private void OnCreateElementOnPatternInstance()
        {
            var dialog_par = new DialogParameters();
            dialog_par.Add("title", Title);
            dialog_par.Add("message", Message);
            TConteiner current_collection = new TConteiner();
            TConteiner common_collection = new TConteiner();
            current_collection = CurrentCollection;
            common_collection = CoreFunctions.GetCollectionElementsList<TConteiner, T>(CommonCollection);
            dialog_par.Add("current_collection", current_collection);
            dialog_par.Add("common_collection", common_collection);
            dialog_par.Add("current_context_id", CurrentContextId);
            T new_element = new T();
            CoreFunctions.CopyObjectNewInstances<IEntityObject>(SelectedElement, new_element, new_element.RestrictionPredicate);
            CoreFunctions.SetAllIdToZero(new_element, false);
            CoreFunctions.SetAllIdToZero(new_element, true);
            new_element.Id = Guid.Empty;
            new_element.StoredId = Guid.NewGuid();
            //new_element.CurrentContextId = Id;
            ConveyanceObject conveyanceObject =
            new ConveyanceObject(new_element, ConveyanceObjectModes.EditMode.FOR_EDIT);
            dialog_par.Add("selected_element_conveyance_object", conveyanceObject);
            _dialogService.ShowDialog(NewObjectDialogName, dialog_par, (result) =>
            {
                if (result.Result == ButtonResult.Yes)
                {
                    CurrentCollection.Add(new_element);
                }
                if (result.Result == ButtonResult.No)
                {
                }
            }
            );
        }

        private void OnCreateNewElement()
        {
            var dialog_par = new DialogParameters();
            dialog_par.Add("title", Title);
            dialog_par.Add("message", Message);
            TConteiner current_collection = new TConteiner();
            TConteiner common_collection = new TConteiner();
            current_collection = CurrentCollection;

            common_collection = CoreFunctions.GetCollectionElementsList<TConteiner, T>(CommonCollection);

            dialog_par.Add("current_collection", current_collection);
            dialog_par.Add("common_collection", common_collection);
            T new_element = new T(); //Создаем новый элемент
            new_element.Id = Guid.Empty;
            new_element.StoredId = Guid.NewGuid();
            //new_element.CurrentContextId = Id;
            ConveyanceObject conveyanceObject =
                new ConveyanceObject(new_element, ConveyanceObjectModes.EditMode.FOR_EDIT);
            dialog_par.Add("selected_element_conveyance_object", conveyanceObject);
            dialog_par.Add("current_context_id", Id);

            _dialogService.ShowDialog(NewObjectDialogName, dialog_par, (result) =>
            {
                if (result.Result == ButtonResult.Yes)
                {
                    CurrentCollection.Add(new_element);
                }
                if (result.Result == ButtonResult.No)
                {
                }
            }
            );
        }

        private void ConfirmDialog(object elements)
        {

            //ObservableCollection<bldWork> selected_works = new ObservableCollection<bldWork>();
            foreach (DataGridCellInfo cell_info in (IList<DataGridCellInfo>)elements)
            {
                NameableObjectPointer element = cell_info.Item as NameableObjectPointer;
                if (!CurrentCollection.Contains(element.ObjectPointer)) CurrentCollection.Add(element.ObjectPointer as T);
                if (!CommonCollection.Contains(element.ObjectPointer)) CommonCollection.Remove(element.ObjectPointer as T);
            }
            var result = ButtonResult.Yes;
            var param = new DialogParameters();
            param.Add("confirm_dialog_param", "Подтверждено пользователем!");
            param.Add("refuse_dialog_param", "Отменено пользователем!");
            param.Add("common_collection", CommonCollection);
            param.Add("current_collection", CurrentCollection);
            RequestClose.Invoke(new DialogResult(result, param));

        }
        //private void OnTestConfirmDialog(object elements)
        //{

        //    //ObservableCollection<bldWork> selected_works = new ObservableCollection<bldWork>();
        //    foreach (DataGridCellInfo cell_info in (IList<DataGridCellInfo>)elements)
        //    {
        //        T element = cell_info.Item as T;
        //        if (!CurrentCollection.Contains(element)) CurrentCollection.Add(element);
        //        if (!CommonCollection.Contains(element)) CommonCollection.Remove(element);
        //    }
        //    var result = ButtonResult.Yes;
        //    var param = new DialogParameters();
        //    param.Add("confirm_dialog_param", "Подтверждено пользователем!");
        //    param.Add("refuse_dialog_param", "Отменено пользователем!");
        //    param.Add("common_collection", CommonCollection);
        //    param.Add("current_collection", CurrentCollection);
        //    RequestClose.Invoke(new DialogResult(result, param));

        //}
        private void CloseDialog()
        {
            var result = ButtonResult.No;
            var param = new DialogParameters();
            param.Add("refuse_dialog_param", "Отменено пользователем!");
            RequestClose.Invoke(new DialogResult(result, param));
        }
        public bool CanCloseDialog()
        {
            return true;
        }
        public void OnDialogClosed()
        {

        }
        public void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("title");
            Message = parameters.GetValue<string>("message");
            CurrentContextId = parameters.GetValue<Guid>("current_context_id");
            if (CurrentContextId != Guid.Empty)
                Id = CurrentContextId;
            else
                Id = Guid.NewGuid();
            CommonCollectionName = parameters.GetValue<string>("common_collection_name");
            CurrentCollectionName = parameters.GetValue<string>("current_collection_name");
            CommonCollection = (TConteiner)parameters.GetValue<object>("common_collection");
            CurrentCollection = (TConteiner)parameters.GetValue<object>("current_collection");
            Refuse = parameters.GetValue<string>("refuse_button_content"); 
             Confirm = parameters.GetValue<string>("confirm_button_content");
            NewObjectDialogName = parameters.GetValue<string>("new_object_dialog_name");
            PredicateCollection =
            parameters.GetValue<NameablePredicateObservableCollection<TConteiner, T>>("predicate_collection");

            SelectedPredicate = PredicateCollection[0];
            foreach (var predicate in PredicateCollection)
            {
                // var tree_view_root_item = new NameableObservableCollection<T>();  //Activator.CreateInstance(CommonCollection.GetType());
                var tree_view_root_item = new NameableObservabelObjectsCollection();  //Activator.CreateInstance(CommonCollection.GetType());

                foreach (T element in predicate.Predicate.Invoke(CommonCollection))
                    tree_view_root_item.Add(element);
                ((INameable)tree_view_root_item).Name = predicate.Name;
                TreeViewCollection.Add(tree_view_root_item);
            }

            //foreach (T element in SelectedPredicate.Predicate.Invoke(CommonCollection))
            //    FilteredCommonCollection.Add(element);
            if (SelectedPredicate.CollectionSelectPredicate == null)
            {
                SelectedPredicate.CollectionSelectPredicate  = (col) =>
                {
                    ObservableCollection<NameableObjectPointer> out_coll = new ObservableCollection<NameableObjectPointer>();
                    foreach (IEntityObject elm in col)
                    {
                        NameableObjectPointer objectPointer = new NameableObjectPointer();
                        objectPointer.Name = elm.Name;
                        objectPointer.Code = elm.Code;
                        objectPointer.ObjectPointer = elm;
                        out_coll.Add(objectPointer);
                    }
                    return out_coll;
                };
            }
              
            foreach (NameableObjectPointer elm in SelectedPredicate.CollectionSelectPredicate.Invoke(CommonCollection))
                    FilteredCommonPointersCollection.Add(elm);
          
               

            FilterEnable = false;
        }
        
    }
}
