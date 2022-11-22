using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Modules.BuildingModule.ViewModels;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class ObjectDialogViewModel : ObjectViewModel,IDialogAware
    {

        public ObjectDialogViewModel(IDialogService dialogService, IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository,
            IApplicationCommands applicationCommands, IPropertiesChangeJornal propertiesChangeJornal)
            :base(dialogService, regionManager, buildingUnitsRepository, applicationCommands, propertiesChangeJornal)
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
         override  public void OnSave()
        {
            if (EditMode == ConveyanceObjectModes.EditMode.FOR_EDIT)
            {
                      CoreFunctions.ConfirmActionOnElementDialog<bldObject>(SelectedBuildingObject, 
                         "Сохранить", "строительный объект",
                         "Сохранить",
                          "Не сохранять",
                         "Отмена",  (result) =>
                     {
                         if (result.Result == ButtonResult.Yes)
                         {
                             //CommonChangeJornal.SaveAll(Id);
                             base.OnSave<bldObject>(SelectedBuildingObject);
                             RequestClose?.Invoke(new DialogResult(ButtonResult.Yes));
                         }
                         else
                         {
                           // CommonChangeJornal.UnDoAll(Id);
                           RequestClose?.Invoke(new DialogResult(ButtonResult.No));
                         }

                     }, _dialogService);
            }
        }
        override public  void OnClose(object obj)
        {
        //    if (EditMode) SelectedBuildingObject.UnDoAll(Id);
          base.OnClose<bldObject>(obj, SelectedBuildingObject);
          RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
           
        }


        public void OnDialogOpened(IDialogParameters parameters)
        {
            ConveyanceObject navigane_message = (ConveyanceObject)parameters.GetValue<object>("selected_element_conveyance_object");
            CurrentContextId = (Guid)parameters.GetValue<object>("current_context_id");
            if (navigane_message != null)
            {
                ResivedObject = (bldObject)navigane_message.Object;
                EditMode = navigane_message.EditMode;
                Id = CurrentContextId;
                if (SelectedBuildingObject != null) SelectedBuildingObject.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedBuildingObject = ResivedObject;
                SelectedBuildingObject.ErrorsChanged += RaiseCanExecuteChanged;
            }

        }
    }
}
