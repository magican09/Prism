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
using System.Text;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class MaterialViewModel : BaseViewModel<bldMaterial>, INotifyPropertyChanged, INavigationAware
    {
        private string _title = "Работа";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private bldMaterial _selectedMaterial;
        public bldMaterial SelectedMaterial
        {
            get { return _selectedMaterial; }
            set { SetProperty(ref _selectedMaterial, value); }
        }
        //private bldWork _selectedWork;
        //public bldWork SelectedWork
        //{
        //    get { return _selectedWork; }
        //    set { SetProperty(ref _selectedWork, value); }
        //}
        public NotifyCommand<object> DataGridLostFocusCommand { get; private set; }
        public NotifyCommand UnDoCommand { get; protected set; }
        public NotifyCommand ReDoCommand { get; protected set; }
        public NotifyCommand SaveCommand { get; protected set; }
        public NotifyCommand<object> CloseCommand { get; protected set; }

        public IBuildingUnitsRepository _buildingUnitsRepository { get; }
        public IbldMaterialsUnitsRepository _bldMaterialsUnitsRepository { get; }

        public MaterialViewModel(IDialogService dialogService,
            IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository, IApplicationCommands applicationCommands)
        {
            UnDoReDo = new UnDoReDoSystem();
            _dialogService = dialogService;
            _buildingUnitsRepository = buildingUnitsRepository;
            _regionManager = regionManager;
            DataGridLostFocusCommand = new NotifyCommand<object>(OnDataGridLostSocus);
            ApplicationCommands = applicationCommands;

            SaveCommand = new NotifyCommand(OnSave, CanSave)
                 .ObservesProperty(() => SelectedMaterial);
            CloseCommand = new NotifyCommand<object>(OnClose);
            UnDoCommand = new NotifyCommand(() => { UnDoReDo.UnDo(1); },
                                     () => { return UnDoReDo.CanUnDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            ReDoCommand = new NotifyCommand(() => UnDoReDo.ReDo(1),
               () => { return UnDoReDo.CanReDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);


            ApplicationCommands.SaveAllCommand.RegisterCommand(SaveCommand);
            ApplicationCommands.ReDoCommand.RegisterCommand(ReDoCommand);
            ApplicationCommands.UnDoCommand.RegisterCommand(UnDoCommand);

        }

        private void OnDataGridLostSocus(object obj)
        {
            throw new NotImplementedException();
        }

        private bool CanSave()
        {
            if (SelectedMaterial != null)
                return !SelectedMaterial.HasErrors;// && SelectedWork.UnDoReDoSystem.Count > 0;
            else
                return false;
        }
        public void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        public virtual void OnSave()
        {
            base.OnSave<bldMaterial>(SelectedMaterial);
        }
        public virtual void OnClose(object obj)
        {
            base.OnClose<bldMaterial>(obj, SelectedMaterial);
        }
        public override void OnWindowClose()
        {
            ApplicationCommands.SaveAllCommand.UnregisterCommand(SaveCommand);
            ApplicationCommands.ReDoCommand.UnregisterCommand(ReDoCommand);
            ApplicationCommands.UnDoCommand.UnregisterCommand(UnDoCommand);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

            ConveyanceObject navigane_message_material = (ConveyanceObject)navigationContext.Parameters["bld_material"];
            if (navigane_message_material != null)
            {
                bldMaterial ResivedMaterial = (bldMaterial)navigane_message_material.Object;
             
                EditMode = navigane_message_material.EditMode;
                if (SelectedMaterial != null) SelectedMaterial.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedMaterial = ResivedMaterial;
                SelectedMaterial.ErrorsChanged += RaiseCanExecuteChanged;
                UnDoReDo.Register(SelectedMaterial);
                Title = $"{SelectedMaterial.Code} {SelectedMaterial.ShortName}";
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            //  AllDocuments.Clear();
            ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_material"];
            if (((bldMaterial)navigane_message.Object).Id != SelectedMaterial.Id)
                return false;
            else
                return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}
