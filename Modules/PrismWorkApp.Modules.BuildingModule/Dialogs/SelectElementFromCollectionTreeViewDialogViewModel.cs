using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class SelectElementFromCollectionTreeViewDialogViewModel<TConteiner, T> : LocalBindableBase, IDialogAware
         where TConteiner : ICollection<T>/*, INameable, INameableOservableCollection<T>*/
        where T : IEntityObject
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private IEntityObject _selectedElement;
        public IEntityObject SelectedElement
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
        private TConteiner _currentElementsCollection;
        public TConteiner CurrentElementsCollection
        {
            get { return _currentElementsCollection; }
            set { SetProperty(ref _currentElementsCollection, value); }
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
        public NotifyCommand<object> SelectionChangeCommand { get; private set; }
        public SelectElementFromCollectionTreeViewDialogViewModel()
        {
            CloseCommand = new NotifyCommand(DialogClosed);
            ConfirmCommand = new NotifyCommand(OnConfirm, () => SelectedElement != null &&
            SelectedElement.GetType() == ElementType &&
            CurrentElementsCollection.Where(el => el.Id == SelectedElement.Id).FirstOrDefault() == null
            ).ObservesProperty(() => SelectedElement);
            SelectionChangeCommand = new NotifyCommand<object>(OnSelectionChange);
        }

        private void OnSelectionChange(object selected_object)
        {
            if (selected_object.GetType() == ElementType)
                SelectedElement = (IEntityObject)selected_object;
            else
                SelectedElement = null;

        }

        private void OnConfirm()
        {
            DialogParameters param = new DialogParameters();
            param.Add("selected_element", SelectedElement);
            RequestClose.Invoke(new DialogResult(ButtonResult.Yes, param));
        }
        public void DialogClosed()
        {
            RequestClose.Invoke(new DialogResult(ButtonResult.No));
        }
        public void OnDialogClosed()
        {

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
            CurrentElementsCollection = (TConteiner)parameters.GetValue<object>("current_element_collection");
            ElementType = parameters.GetValue<System.Type>("element_type");

        }
    }
}
