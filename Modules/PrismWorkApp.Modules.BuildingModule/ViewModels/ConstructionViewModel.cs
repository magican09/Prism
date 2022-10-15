using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.Modules.BuildingModule.Dialogs;
using PrismWorkApp.Modules.BuildingModule.Views;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.ProjectModel.Data.Models;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using BindableBase = Prism.Mvvm.BindableBase;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class ConstructionViewModel : BaseViewModel<bldConstruction>, INotifyPropertyChanged,
        INavigationAware//, IRegionMemberLifetime

    {
        private string _title = "Проект";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private bldConstruction _selectedConstruction;
        public bldConstruction SelectedConstruction
        {
            get { return _selectedConstruction; }
            set { SetProperty(ref _selectedConstruction, value); }
        }
        private bldConstruction _resivedConstruction;
        public bldConstruction ResivedConstruction
        {
            get { return _resivedConstruction; }
            set { SetProperty(ref _resivedConstruction, value); }
        }
        private bldConstruction _selectedChildConstruction;
        public bldConstruction SelectedChildConstruction
        {
            get { return _selectedChildConstruction; }
            set { SetProperty(ref _selectedChildConstruction, value); }
        }
      
        private bldWork _selectedWork;
        public bldWork SelectedWork
        {
            get { return _selectedWork; }
            set { SetProperty(ref _selectedWork, value); }
        }
       
        private bldConstructionsGroup _Constructions;
        public bldConstructionsGroup Constructions
        {
            get { return _Constructions; }
            set { SetProperty(ref _Constructions, value); }
        }
        private bldWorksGroup _Works;
        public bldWorksGroup Works
        {
            get { return _Works; }
            set { SetProperty(ref _Works, value); }
        }
        private bool _editMode;
        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }

        private bool _keepAlive = true;

        public bool KeepAlive
        {
            get { return _keepAlive; }
            set { _keepAlive = value; }
        }



        public DelegateCommand<object> DataGridLostFocusCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand<object> CloseCommand { get; private set; }

        public DelegateCommand RemoveConstructionCommand { get; private set; }
        public DelegateCommand RemoveWorkCommand { get; private set; }
        
        public DelegateCommand AddConstructionCommand { get; private set; }
        public DelegateCommand AddWorkCommand { get; private set; }

        public DelegateCommand EditConstructionCommand { get; private set; }
        public DelegateCommand EditWorkCommand { get; private set; }

        public IBuildingUnitsRepository _buildingUnitsRepository { get; }


        public ConstructionViewModel(IDialogService dialogService, 
            IRegionManager regionManager, IBuildingUnitsRepository  buildingUnitsRepository)
        {
            DataGridLostFocusCommand = new DelegateCommand<object>(OnDataGridLostSocus);
            SaveCommand = new DelegateCommand(OnSave, CanSave);
            CloseCommand = new DelegateCommand<object>(OnClose);
         
            RemoveConstructionCommand = new DelegateCommand(OnRemoveConstruction,
                                        () => SelectedChildConstruction != null)
                    .ObservesProperty(() => SelectedChildConstruction);
            RemoveWorkCommand = new DelegateCommand(OnRemoveWork,
                                        () => SelectedWork != null)
                    .ObservesProperty(() => SelectedWork);
         
            AddConstructionCommand = new DelegateCommand(OnAddConstruction);
            AddWorkCommand = new DelegateCommand(OnAddWork);

            EditConstructionCommand = new DelegateCommand(OnEditConstruction,
                                        () => SelectedChildConstruction != null)
                    .ObservesProperty(() => SelectedChildConstruction);
            EditWorkCommand = new DelegateCommand(OnEditWork,
                                        () => SelectedWork != null)
                    .ObservesProperty(() => SelectedWork);
            _dialogService = dialogService;
             _buildingUnitsRepository = buildingUnitsRepository;
            _regionManager = regionManager;
        }

      

        private void OnDataGridLostSocus(object obj)
        {

            if (obj == SelectedChildConstruction)
            {
                 SelectedWork = null;
                return;
            }
            if (obj == SelectedWork)
            {
             
                SelectedChildConstruction = null;
                  return;
            }
        }

        private void OnEditWork()
        {
            CoreFunctions.EditElementDialog<bldWork>(SelectedWork, "Работа",
                  (result) => { SaveCommand.RaiseCanExecuteChanged(); }, _dialogService, typeof(WorkDialogView).Name, "Редактировать", Id);

        }
        private void OnAddWork()
        {
            bldWorksGroup Works =
                new bldWorksGroup(_buildingUnitsRepository.Works.GetbldWorksAsync());
            // Works.ClearStructureLevel();
            //  Works.UpdateStructure();
            NameablePredicate<bldWorksGroup, bldWork> predicate_1 = new NameablePredicate<bldWorksGroup, bldWork>();
            predicate_1.Name = "Показать только из текущего объекта.";
            predicate_1.Predicate = cl => cl.Where(el => el.bldConstruction.bldObject.Id == SelectedConstruction.bldObject.Id).ToList();
            NameablePredicate<bldWorksGroup, bldWork> predicate_2 = new NameablePredicate<bldWorksGroup, bldWork>();
            predicate_2.Name = "Показать все кроме текущего объекта";
            predicate_2.Predicate = cl => cl.Where(el => el.bldConstruction.bldObject.Id != SelectedConstruction.bldObject.Id).ToList();
            NameablePredicate<bldWorksGroup, bldWork> predicate_3 = new NameablePredicate<bldWorksGroup, bldWork>();
            predicate_3.Name = "Показать все";
            predicate_3.Predicate = cl => cl;

            NameablePredicateObservableCollection<bldWorksGroup, bldWork> nameablePredicatesCollection = new NameablePredicateObservableCollection<bldWorksGroup, bldWork>();
            nameablePredicatesCollection.Add(predicate_1);
            nameablePredicatesCollection.Add(predicate_2);
            nameablePredicatesCollection.Add(predicate_3);

            CoreFunctions.AddElementToCollectionWhithDialog_Test<bldWorksGroup, bldWork>
                (SelectedConstruction.Works, Works,
                 nameablePredicatesCollection,
                _dialogService,
                 (result) =>
                 {
                     if (result.Result == ButtonResult.Yes)
                     {
                         SaveCommand.RaiseCanExecuteChanged();
                         foreach (bldWork work in SelectedConstruction.Works)
                             work.bldConstruction = SelectedConstruction;
                     }
                     if (result.Result == ButtonResult.No)
                     {
                         SelectedConstruction.Works.UnDoAll(Id);
                     }
                 },
                typeof(AddbldWorkToCollectionDialogView).Name,
                typeof(WorkDialogView).Name, Id,
                "Редактирование списка работ",
                "Форма для редактирования состава работ текушей коснтрукции.",
                "Работы текущей конструкции", "Все работы");
          
            /*CoreFunctions.AddElementToCollectionWhithDialog<bldWorksGroup, bldWork>
               (SelectedConstruction.Works, Works, _dialogService,
                (result) =>
                {
                    if (result.Result == ButtonResult.Yes)
                    {
                        SaveCommand.RaiseCanExecuteChanged();
                    }
                    if (result.Result == ButtonResult.No)
                    {
                        SelectedConstruction.Works.UnDoAll(Id);
                    }
                },
               typeof(AddbldWorkToCollectionDialogView).Name,
               typeof(WorkDialogView).Name, Id,
               "Редактирование списка работ",
               "Форма для редактирования состава работ текушей коснтрукции.",
               "Работы текущей конструкции", "Все работы");*/
        }
        private void OnRemoveWork()
        {

            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldWorksGroup, bldWork>
                 (SelectedConstruction.Works, SelectedWork, "Строительная конструкция",
                 () => { SelectedWork = null; SaveCommand.RaiseCanExecuteChanged(); }, _dialogService);
        }


        private void OnEditConstruction()
        {
            CoreFunctions.EditElementDialog<bldConstruction>(SelectedChildConstruction, "Учасник строительства",
                  (result) => { SaveCommand.RaiseCanExecuteChanged(); }, _dialogService, typeof(ConstructionDialogView).Name, "Редактировать", Id);

        }
        private void OnAddConstruction()
        {
            bldConstructionsGroup Constructions =
                new bldConstructionsGroup(_buildingUnitsRepository.Constructions.GetbldConstructionsAsync());
            if (SelectedConstruction.Constructions == null)
                SelectedConstruction.Constructions = new bldConstructionsGroup();
            CoreFunctions.AddElementToCollectionWhithDialog<bldConstructionsGroup, bldConstruction>
                (SelectedConstruction.Constructions, Constructions, _dialogService,
                 (result) =>
                 {
                     if (result.Result == ButtonResult.Yes)
                     {
                         SaveCommand.RaiseCanExecuteChanged();
                     }
                     if (result.Result == ButtonResult.No)
                     {
                         SelectedConstruction.Constructions.UnDoAll(Id);
                     }
                 },
                typeof(AddbldConstructionToCollectionDialogView).Name,
                typeof(ConstructionDialogView).Name, Id,
                "Редактирование списка конструкций",
                "Форма для редактирования состава коснструций текушей коснтрукции.",
                "Конструкции текущей конструкции", "Все конструкции");
        }
        private void OnRemoveConstruction()
        {

            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldConstructionsGroup, bldConstruction>
                 (SelectedChildConstruction.Constructions, SelectedChildConstruction, "Строительная конструкция",
                 () => { SelectedChildConstruction = null; SaveCommand.RaiseCanExecuteChanged(); }, _dialogService);
        }

        private bool CanSave()
        {
            if (SelectedConstruction != null)
                return !SelectedConstruction.HasErrors;
            else
                return false;
        }
        public void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }
       
        public virtual void OnSave()
        {
            this.OnSave<bldConstruction>(SelectedConstruction);
        }
        public virtual void OnClose(object obj)
        {
            this.OnClose<bldConstruction>(obj, SelectedConstruction);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

            ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_construction"];
            if (navigane_message != null)
            {
                ResivedConstruction = (bldConstruction)navigane_message.Object;
                SelectedConstruction = ResivedConstruction;
                EditMode = navigane_message.EditMode;
                if (SelectedConstruction != null) SelectedConstruction.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedConstruction = ResivedConstruction;
                SelectedConstruction.ErrorsChanged += RaiseCanExecuteChanged;
                Title = ResivedConstruction.Name;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_construction"];
            if (((bldConstruction)navigane_message.Object).Id != SelectedConstruction.Id)
                return false;
            else
                return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }



    }

}
