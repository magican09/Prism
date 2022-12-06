using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Modules.BuildingModule.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

        public NotifyCommand<object> RemovePreviousWorkCommand { get; private set; }
        public NotifyCommand<object> RemoveNextWorkCommand { get; private set; }

        public NotifyCommand AddPreviousWorkCommand { get; private set; }
        public NotifyCommand<object> AddNextWorkCommand { get; private set; }
        public ObservableCollection<INotifyCommand> NextWorksContextMenuCommands { get; set; } = new ObservableCollection<INotifyCommand>();
      
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
                .ObservesProperty(() => SelectedConstruction);
            CloseCommand = new NotifyCommand<object>(OnClose);
            UnDoCommand = new NotifyCommand(() => { UnDoReDo.UnDo(1); },
                                     () => { return UnDoReDo.CanUnDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            ReDoCommand = new NotifyCommand(() => UnDoReDo.ReDo(1),
               () => { return UnDoReDo.CanReDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);

            RemoveNextWorkCommand = new NotifyCommand<object>(OnRemoveNextWork);
            RemoveNextWorkCommand.Name = "Удалить";
        
            AddNextWorkCommand = new NotifyCommand<object>(OnAddNextWork);
            AddNextWorkCommand.Name = "Добавить";
            NextWorksContextMenuCommands.Add(RemoveNextWorkCommand);
            NextWorksContextMenuCommands.Add(AddNextWorkCommand);
            _dialogService = dialogService;
            _buildingUnitsRepository = buildingUnitsRepository;
            _regionManager = regionManager;
            _applicationCommands = applicationCommands;
            _applicationCommands.SaveAllCommand.RegisterCommand(SaveCommand);
            _applicationCommands.ReDoCommand.RegisterCommand(ReDoCommand);
            _applicationCommands.UnDoCommand.RegisterCommand(UnDoCommand);
        }

        private void OnAddNextWork(object works)
        {
            bldWork removed_work = ((Tuple<object, object>)works).Item1 as bldWork;
            bldWork selected_work = ((Tuple<object, object>)works).Item2 as bldWork;


            bldWorksGroup All_Works = new bldWorksGroup(_buildingUnitsRepository.Works.GetbldWorksAsync().Where(wr => wr.Id != selected_work.Id &&
                                             !selected_work.PreviousWorks.Contains(wr) && !selected_work.NextWorks.Contains(wr)).ToList());

            ObservableCollection<bldWork> works_for_add_collection = new ObservableCollection<bldWork>();
            NameablePredicate<ObservableCollection<bldWork>, bldWork> predicate_1 = new NameablePredicate<ObservableCollection<bldWork>, bldWork>();
            predicate_1.Name = "Показать только из текущей конструкции.";
            predicate_1.Predicate = cl => cl.Where(el => el?.bldConstruction != null &&
                                                        el?.bldConstruction.Id == selected_work?.bldConstruction?.Id).ToList();
            NameablePredicate<ObservableCollection<bldWork>, bldWork> predicate_2 = new NameablePredicate<ObservableCollection<bldWork>, bldWork>();
            predicate_2.Name = "Показать на одну ступень выше, но без работ текущей кострукции";
            predicate_2.Predicate = cl => cl.Where(el => el?.bldConstruction?.Id != selected_work?.bldConstruction?.Id &&
                                                        (el.bldConstruction?.ParentConstruction?.Id == selected_work.bldConstruction?.ParentConstruction?.Id ||
                                                          el.bldConstruction?.bldObject?.Id == selected_work.bldConstruction?.bldObject?.Id)).ToList();
            NameablePredicateObservableCollection<ObservableCollection<bldWork>, bldWork> nameablePredicatesCollection = new NameablePredicateObservableCollection<ObservableCollection<bldWork>, bldWork>();
            nameablePredicatesCollection.Add(predicate_1);
            nameablePredicatesCollection.Add(predicate_2);
            CoreFunctions.AddElementsToCollectionWhithDialogList<ObservableCollection<bldWork>, bldWork>
              (works_for_add_collection, All_Works,
               nameablePredicatesCollection,
              _dialogService,
               (result) =>
               {
                   if (result.Result == ButtonResult.Yes)
                   {
                       UnDoReDoSystem localUnDoReDo = new UnDoReDoSystem();
                        localUnDoReDo.Register(selected_work);
                       foreach (bldWork bld_work in works_for_add_collection)
                       {

                           selected_work.AddNextWork(bld_work);
                       }
                       SaveCommand.RaiseCanExecuteChanged();
                       UnDoReDo.AddUnDoReDo(localUnDoReDo);
                   }
                   if (result.Result == ButtonResult.No)
                   {
                   }
               },
              typeof(AddWorksToCollectionFromListDialogView).Name,
              typeof(AddbldWorkToCollectionDialogView).Name, UnDoReDo,
               "Добавить работы как послудующие",
               "Форма добавления послудующих работ.",
               "Список работ", "");

        }
        private void OnRemoveNextWork(object works)
        {
            bldWork removed_work = ((Tuple<object,object>)works).Item1 as bldWork;
            bldWork selected_work = ((Tuple<object, object>)works).Item2 as bldWork;
            if (removed_work == null || selected_work == null) return;
            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldWorksGroup, bldWork>
                 (selected_work.NextWorks, removed_work, "Последующая работа",
                result => {
                    if (result.Result == ButtonResult.Yes)
                    {
                        UnDoReDo.Register(selected_work);
                        selected_work.RemoveNextWork(removed_work);
                    }
                }, _dialogService, Id);
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
