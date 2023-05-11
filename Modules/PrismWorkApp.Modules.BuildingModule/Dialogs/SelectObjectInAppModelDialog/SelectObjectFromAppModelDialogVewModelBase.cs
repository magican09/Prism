﻿using Prism.Mvvm;
using Prism.Services.Dialogs;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Modules.BuildingModule.Core;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BindableBase = Prism.Mvvm.BindableBase;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class SelectObjectFromAppModelDialogVewModelBase<T> : BindableBase, IDialogAware
    //  where TContainer : ICollection, IList, new()
      where T : OpenWorkLib.Data.IEntityObject
    {
        private string _title = "Выбрать";
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
        
        private DataItemCollection _items;
        public DataItemCollection Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }
        private DataItem _selectedElement;
        public DataItem SelectedElement
        {
            get { return _selectedElement; }
            set { SetProperty(ref _selectedElement, value); }
        }
        private T _resivedElement;
        public T ResivedElement
        {
            get { return _resivedElement; }
            set { SetProperty(ref _resivedElement, value); }
        }
        public event Action<IDialogResult> RequestClose;
        public NotifyCommand CloseDialogCommand { get; private set; }
        public NotifyCommand ConfirmDialogCommand { get; private set; }
        public NotifyCommand CreateNewCommand { get; private set; }
        public NotifyCommand SelectionChangedCommnad { get; private set; }
        
        private AppObjectsModel _appObjectsModel;
        public SelectObjectFromAppModelDialogVewModelBase(IAppObjectsModel appObjectsModel)
        {
            _appObjectsModel = appObjectsModel as AppObjectsModel;
            DataItem Root = new DataItem();
            Root.AttachedObject = _appObjectsModel.AllModels;
            Items = new DataItemCollection(null);
            Items.Add(Root);

            CloseDialogCommand = new NotifyCommand(CloseDialog);
            ConfirmDialogCommand = new NotifyCommand(ConfirmDialog, ()=> SelectedElement != null).
                ObservesProperty(() => SelectedElement);
            CreateNewCommand = new NotifyCommand(OnCreateNew);
            SelectionChangedCommnad = new NotifyCommand(OnSelectionChanged);
        }

        private void OnSelectionChanged()
        {
            
        }

        private void OnCreateNew()
        {
            // SelectedElement = new T();
           // CurrentCollection.Add(SelectedElement);
            //var result = ButtonResult.Yes;
            //var param = new DialogParameters();
            //param.Add("confirm_dialog_param", "Создан новый элемент");
            //param.Add("element", SelectedElement);
            //RequestClose.Invoke(new DialogResult(result, param));
        }


        private void ConfirmDialog()
        {
            var result = ButtonResult.Yes;
            var param = new DialogParameters();
            param.Add("confirm_dialog_param", "Подтверждено пользователем!");
            param.Add("element", SelectedElement.AttachedObject);
            RequestClose.Invoke(new DialogResult(result, param));
        }

        private void CloseDialog()
        {
            var result = ButtonResult.No;
            var param = new DialogParameters();
            param.Add("confirm_dialog_param", "Отменено пользователем!");
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
            //Title = parameters.GetValue<string>("title");
            //Message = parameters.GetValue<string>("message");
            ////CurrentCollection = parameters.GetValue<TContainer>("current_collection");
            //Refuse = parameters.GetValue<string>("confirm_button_content");
            //Confirm = parameters.GetValue<string>("refuse_button_content");
        }
    }
}