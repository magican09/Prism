using Prism.Mvvm;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using BindableBase = Prism.Mvvm.BindableBase;
namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class SelectElementFromCollectionTreeViewDialogViewModel<TConteiner, T>: LocalBindableBase, IDialogAware
         where TConteiner : ICollection<T>/*, INameable, INameableOservableCollection<T>*/
        where T : IEntityObject
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private INameable _selectedElement;
        public INameable SelectedElement
        {
            get { return _selectedElement; }
            set { SetProperty(ref _selectedElement, value); }
        }

        public Type ElementType { get; set; }
        private T _currentElement;
        public T CurrentElement
        {
            get { return _currentElement; }
            set { SetProperty(ref _currentElement, value); }
        }
        private TConteiner _commonCollection;
        public TConteiner CommonCollection
        {
            get { return _commonCollection; }
            set { SetProperty(ref _commonCollection, value); }
        }

        public event Action<IDialogResult> RequestClose;
        public NotifyCommand CloseCommand { get; private set; }
        public NotifyCommand ConfirmCommand { get; private set; }
        public SelectElementFromCollectionTreeViewDialogViewModel()
        {
            CloseCommand = new NotifyCommand(OnDialogClosed);
            ConfirmCommand = new NotifyCommand(OnConfirm);
        }

        private void OnConfirm()
        {
            DialogParameters param = new DialogParameters();
            param.Add("selected_element", SelectedElement);
            RequestClose.Invoke(new DialogResult(ButtonResult.Yes, param));
        }
        public void OnDialogClosed()
        {
            RequestClose.Invoke(new DialogResult(ButtonResult.No));
        }
        public bool CanCloseDialog()
        {
            return true;
        }

       

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("title");
            CommonCollection = (TConteiner)parameters.GetValue<object>("common_collection");
            CurrentElement = (T)parameters.GetValue<object>("current_element");
            ElementType = parameters.GetValue<System.Type>("element_type");
           
        }
    }
}
