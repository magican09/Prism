using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Modules.BuildingModule.ViewModels;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo;
using PrismWorkApp.Services.Repositories;
using System;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class ConstructionDialogViewModel : ConstructionViewModel, IDialogAware
    {


        public ConstructionDialogViewModel(IDialogService dialogService, IRegionManager regionManager
            , IBuildingUnitsRepository buildingUnitsRepository, IApplicationCommands applicationCommands,
             IUnDoReDoSystem unDoReDo)
            : base(dialogService, regionManager, buildingUnitsRepository, applicationCommands, unDoReDo)
        {

        }
        public event Action<IDialogResult> RequestClose;

        private Guid _currentContextId;

        public Guid CurrentContextId
        {
            get { return _currentContextId; }
            set { _currentContextId = value; }
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }
        override public void OnSave()
        {
            if (EditMode == ConveyanceObjectModes.EditMode.FOR_EDIT)
            {

                CoreFunctions.ConfirmActionOnElementDialog<bldConstruction>(SelectedConstruction,
                    "Сохранить", "строительная конструкция",
                    "Сохранить",
                     "Не сохранять",
                    "Отмена", (result) =>
                    {
                        if (result.Result == ButtonResult.Yes)
                        {
                            DialogParameters param = new DialogParameters();
                            param.Add("undo_redo",UnDoReDo);
                            RequestClose?.Invoke(new DialogResult(ButtonResult.Yes,param));
                        }
                        else
                        {
                            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
                        }

                    }, _dialogService);

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
            CommonUnDoReDo = (UnDoReDoSystem)parameters.GetValue<object>("undo_redo");
            if (navigane_message != null)
            {
                ResivedConstruction = (bldConstruction)navigane_message.Object;
                EditMode = navigane_message.EditMode;
                if (!EditMode)
                    Id = CurrentContextId;
                if (SelectedConstruction != null) SelectedConstruction.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedConstruction = ResivedConstruction;
                SelectedConstruction.ErrorsChanged += RaiseCanExecuteChanged;
                UnDoReDo = new UnDoReDoSystem();
                UnDoReDo.Register(SelectedConstruction);
            }

        }
    }
}
