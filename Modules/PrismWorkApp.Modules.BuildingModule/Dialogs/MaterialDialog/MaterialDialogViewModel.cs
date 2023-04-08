using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Modules.BuildingModule.ViewModels;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.Services.Repositories;
using System;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class MaterialDialogViewModel : MaterialViewModel, IDialogAware
    {
        private Guid _currentContextId;

        public Guid CurrentContextId
        {
            get { return _currentContextId; }
            set { _currentContextId = value; }
        }
        public MaterialDialogViewModel(IDialogService dialogService, IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository, IApplicationCommands applicationCommands)
            : base(dialogService, regionManager, buildingUnitsRepository, applicationCommands)
        {

        }
        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            //throw new NotImplementedException();
        }
        override public void OnSave()
        {
            if (EditMode == ConveyanceObjectModes.EditMode.FOR_EDIT)
            {
                CoreFunctions.ConfirmActionOnElementDialog<bldMaterial>(SelectedMaterial,
                    "Сохранить", "материал",
                    "Сохранить",
                     "Не сохранять",
                    "Отмена", (result) =>
               {
                   if (result.Result == ButtonResult.Yes)
                   {
                       DialogParameters param = new DialogParameters();
                       param.Add("undo_redo", UnDoReDo);
                       RequestClose?.Invoke(new DialogResult(ButtonResult.Yes, param));
                   }
                   else
                   {
                       RequestClose?.Invoke(new DialogResult(ButtonResult.No));
                   }
               }, _dialogService);
            }
            else
            {
            }
        }
        override public void OnClose(object obj)
        {
            UnDoReDo.UnDoAll();
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));

        }
        public void OnDialogOpened(IDialogParameters parameters)
        {
            ConveyanceObject navigane_message = (ConveyanceObject)parameters.GetValue<object>("selected_element_conveyance_object");
            if (navigane_message != null)
            {
                bldMaterial ResivedMaterial = (bldMaterial)navigane_message.Object;
                EditMode = navigane_message.EditMode;
                if (!EditMode)
                    Id = CurrentContextId;
                if (SelectedMaterial != null) SelectedMaterial.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedMaterial = ResivedMaterial;
                SelectedMaterial.ErrorsChanged += RaiseCanExecuteChanged;
                // UnDoReDo.Register(SelectedMaterial);
            }
        }
    }
}
