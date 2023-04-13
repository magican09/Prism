using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class UnitsOfMeasurementsViewModel:BaseViewModel<bldUnitOfMeasurement>, INotifyPropertyChanged, INavigationAware
    {
        public IBuildingUnitsRepository _buildingUnitsRepository { get; }
        private NameableObservableCollection<bldUnitOfMeasurement> _unitOfMeasurements;

        public NameableObservableCollection<bldUnitOfMeasurement> UnitOfMeasurements
        {
            get { return _unitOfMeasurements; }
            set { _unitOfMeasurements = value; }
        }
        private bldUnitOfMeasurement  _selectedUnitOfMeasurement;

        public bldUnitOfMeasurement SelectedUnitOfMeasurement
        {
            get { return _selectedUnitOfMeasurement; }
            set { _selectedUnitOfMeasurement = value; }
        }


        public NotifyCommand UnDoCommand { get; protected set; }
        public NotifyCommand ReDoCommand { get; protected set; }
        public NotifyCommand SaveCommand { get; protected set; }
        public NotifyCommand<object> CloseCommand { get; protected set; }


        public UnitsOfMeasurementsViewModel(IDialogService dialogService,
           IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository, IApplicationCommands applicationCommands, IAppObjectsModel appObjectsModel)
        {
            UnDoReDo = new UnDoReDoSystem();
            ApplicationCommands = applicationCommands;
          
            _dialogService = dialogService;
            _buildingUnitsRepository = buildingUnitsRepository;
            _regionManager = regionManager;
            SaveCommand = new NotifyCommand(OnSave, CanSave)
              .ObservesProperty(() => SelectedUnitOfMeasurement);
            CloseCommand = new NotifyCommand<object>(OnClose);
            UnDoCommand = new NotifyCommand(() => { UnDoReDo.UnDo(1); },
                                     () => { return UnDoReDo.CanUnDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            UnDoCommand.Name = "UnDoCommand";
            ReDoCommand = new NotifyCommand(() => UnDoReDo.ReDo(1),
               () => { return UnDoReDo.CanReDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            ReDoCommand.Name = "ReDoCommand";


            ApplicationCommands.SaveAllCommand.RegisterCommand(SaveCommand);
            ApplicationCommands.ReDoCommand.RegisterCommand(ReDoCommand);
            ApplicationCommands.UnDoCommand.RegisterCommand(UnDoCommand);
        }

        public override void OnWindowClose()
        {
            ApplicationCommands.SaveAllCommand.UnregisterCommand(SaveCommand);
            ApplicationCommands.ReDoCommand.UnregisterCommand(ReDoCommand);
            ApplicationCommands.UnDoCommand.UnregisterCommand(UnDoCommand);
            UnDoReDo.ParentUnDoReDo?.UnSetUnDoReDoSystemAsChildren(UnDoReDo);
        }
        public void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }
        private bool CanSave()
        {
            if (UnitOfMeasurements != null)
                return !UnitOfMeasurements.HasErrors;
            else
                return false;
        }
        public virtual void OnSave()
        {
            base.OnSave<NameableObservableCollection<bldUnitOfMeasurement>>(UnitOfMeasurements,
                 (result) =>
                 {
                     if (result.Result == ButtonResult.Yes)
                         UnDoReDo.ParentUnDoReDo.Save(UnitOfMeasurements);
                 });
        }
        public virtual void OnClose(object obj)
        {
            base.OnClose<NameableObservableCollection<bldUnitOfMeasurement>>(obj, UnitOfMeasurements,
                (result) =>
                {
                    if (result.Result == ButtonResult.Yes)
                        UnDoReDo.Save(UnitOfMeasurements);
                    if (result.Result == ButtonResult.No)
                        UnDoReDo.UnDoAll(UnitOfMeasurements);
                });
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_units_of_measurements"];
            if (navigane_message != null)
            {
                NameableObservableCollection<bldUnitOfMeasurement> unit_of_measurements = (NameableObservableCollection<bldUnitOfMeasurement>)navigane_message.Object;
                EditMode = navigane_message.EditMode;

                unit_of_measurements.UnDoReDoSystem.SetUnDoReDoSystemAsChildren(UnDoReDo);
                UnitOfMeasurements = unit_of_measurements;
                UnDoReDo.Register(UnitOfMeasurements);
       
                if (UnitOfMeasurements != null) UnitOfMeasurements.ErrorsChanged -= RaiseCanExecuteChanged;
                UnitOfMeasurements.ErrorsChanged += RaiseCanExecuteChanged;
                Title = $"Ед. изм.";
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
           
        }
    }
}
