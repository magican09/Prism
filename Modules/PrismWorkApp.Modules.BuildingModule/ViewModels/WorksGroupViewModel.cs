using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class WorksGroupViewModel : BaseViewModel<bldWorksGroup>, INotifyPropertyChanged, INavigationAware
    {
        private string _title = "Ведомость работ";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private bldWork _selectedWork;
        public bldWork SelectedWork
        {
            get { return _selectedWork; }
            set { SetProperty(ref _selectedWork, value); }
        }
        private bldWorksGroup _selectedWorksGroup;
        public bldWorksGroup SelectedWorksGroup
        {
            get { return _selectedWorksGroup; }
            set { SetProperty(ref _selectedWorksGroup, value); }
        }

        private bldConstruction _selectedConstruction;
        public bldConstruction SelectedConstruction
        {
            get { return _selectedConstruction; }
            set { SetProperty(ref _selectedConstruction, value); }
        }
        public NotifyCommand<object> DataGridLostFocusCommand { get; private set; }
        public NotifyCommand UnDoCommand { get; protected set; }
        public NotifyCommand ReDoCommand { get; protected set; }
        public NotifyCommand SaveCommand { get; protected set; }
        public NotifyCommand<object> CloseCommand { get; protected set; }
        public NotifyCommand<FrameworkElement> AddElementCommand { get; protected set; }

        public IBuildingUnitsRepository _buildingUnitsRepository { get; }
        private IApplicationCommands _applicationCommands;
        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }
        public WorksGroupViewModel(IDialogService dialogService,
            IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository, IApplicationCommands applicationCommands)
        {
            UnDoReDo = new UnDoReDoSystem();
            DataGridLostFocusCommand = new NotifyCommand<object>(OnDataGridLostSocus);

            SaveCommand = new NotifyCommand(OnSave, CanSave)
                .ObservesProperty(() => SelectedWork);
            CloseCommand = new NotifyCommand<object>(OnClose);
            UnDoCommand = new NotifyCommand(() => { UnDoReDo.UnDo(1); },
                                     () => { return UnDoReDo.CanUnDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            ReDoCommand = new NotifyCommand(() => UnDoReDo.ReDo(1),
               () => { return UnDoReDo.CanReDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);

            AddElementCommand = new NotifyCommand<FrameworkElement>(OnAddElementCommand);
            _dialogService = dialogService;
            _buildingUnitsRepository = buildingUnitsRepository;
            _regionManager = regionManager;
            _applicationCommands = applicationCommands;
            _applicationCommands.SaveAllCommand.RegisterCommand(SaveCommand);
            _applicationCommands.ReDoCommand.RegisterCommand(ReDoCommand);
            _applicationCommands.UnDoCommand.RegisterCommand(UnDoCommand);
        }

        private void OnAddElementCommand(FrameworkElement obj)
        {
            throw new NotImplementedException();
        }

        private void OnDataGridLostSocus(object obj)
        {

            //if (obj == SelectedPreviousWork)
            //{
            //    SelectedNextWork = null;
            //    return;
            //}
            //if (obj == SelectedNextWork)
            //{

            //    SelectedPreviousWork = null;
            //    return;
            //}
        }
        public void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }
        private bool CanSave()
        {
            if (SelectedConstruction != null)
                return !SelectedConstruction.HasErrors;// && SelectedWork.UnDoReDoSystem.Count > 0;
            else
                return false;
        }
        public virtual void OnSave()
        {
            base.OnSave<bldWork>(SelectedWork);
        }
        public virtual void OnClose(object obj)
        {
            base.OnClose<bldWork>(obj, SelectedWork);
        }
        public override void OnWindowClose()
        {
            _applicationCommands.SaveAllCommand.UnregisterCommand(SaveCommand);
            _applicationCommands.ReDoCommand.UnregisterCommand(ReDoCommand);
            _applicationCommands.UnDoCommand.UnregisterCommand(UnDoCommand);
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
             ConveyanceObject navigane_message_works = (ConveyanceObject)navigationContext.Parameters["bld_works_group"];
           // ConveyanceObject navigane_message_construction = (ConveyanceObject)navigationContext.Parameters["bld_construction"];
            if (navigane_message_works != null)
            {
                SelectedWorksGroup = (bldWorksGroup)navigane_message_works.Object;
                EditMode = navigane_message_works.EditMode;
                if (SelectedConstruction != null) SelectedConstruction.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedConstruction = (bldConstruction) SelectedWorksGroup.ParentObject;
                SelectedConstruction.ErrorsChanged += RaiseCanExecuteChanged;
                UnDoReDo.Register(SelectedConstruction);
            }
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_works_group"];
            if (((bldWorksGroup)navigane_message.Object).Id != SelectedWorksGroup.Id)
                return false;
            else
                return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

    }
}
