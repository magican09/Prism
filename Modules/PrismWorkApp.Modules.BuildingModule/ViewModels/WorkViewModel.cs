using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Modules.BuildingModule.Core;
using PrismWorkApp.Modules.BuildingModule.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.ProjectModel.Data.Models;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using BindableBase = Prism.Mvvm.BindableBase;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class WorkViewModel : BaseViewModel<bldWork>, INotifyPropertyChanged, INavigationAware
    {
        private string _title = "Работа";
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
        private bldWork _resivedWork;
        public bldWork ResivedWork
        {
            get { return _resivedWork; }
            set { SetProperty(ref _resivedWork, value); }
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
        private bldObject _resivedObject;
        public bldObject ResivedObject
        {
            get { return _resivedObject; }
            set { SetProperty(ref _resivedObject, value); }
        }
        private bldObject _selectedObject;
        public bldObject SelectedObject
        {
            get { return _selectedObject; }
            set { SetProperty(ref _selectedObject, value); }
        }

        private bldWork _selectedPreviousWork;
        public bldWork SelectedPreviousWork
        {
            get { return _selectedPreviousWork; }
            set { SetProperty(ref _selectedPreviousWork, value); }
        }
        private bldWork _selectedNextWork;
        public bldWork SelectedNextWork
        {
            get { return _selectedNextWork; }
            set { SetProperty(ref _selectedNextWork, value); }
        }
        private bldMaterialsGroup _materials;
        public bldMaterialsGroup Materials
        {
            get { return _materials; }
            set { SetProperty(ref _materials, value); }
        }
        private bldMaterial _selectedMaterial;
        public bldMaterial SelectedMaterial
        {
            get { return _selectedMaterial; }
            set { SetProperty(ref _selectedMaterial, value); }
        }

        public Dictionary<Guid, object> _allDocuments = new Dictionary<Guid, object>();
        public Dictionary<Guid, object> AllDocuments
        {
            get { return _allDocuments; }
            set { SetProperty(ref _allDocuments, value); }
        }
        private object _selectedDocumentsList;
        public object SelectedDocumentsList
        {
            get { return _selectedDocumentsList; }
            set { SetProperty(ref _selectedDocumentsList, value); }
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



        public NotifyCommand<object> DataGridLostFocusCommand { get; private set; }
       public NotifyCommand RemovePreviousWorkCommand { get; private set; }
        public NotifyCommand RemoveNextWorkCommand { get; private set; }

        public NotifyCommand AddPreviousWorkCommand { get; private set; }
        public NotifyCommand AddNextWorkCommand { get; private set; }

        public NotifyCommand EditPreviousWorkCommand { get; private set; }
        public NotifyCommand EditNextWorkCommand { get; private set; }
        
        public NotifyCommand GenerateAxecDocsCommand { get; private set; }

        public IBuildingUnitsRepository _buildingUnitsRepository { get; }
     
        private IApplicationCommands _applicationCommands;
        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }


        public WorkViewModel(IDialogService dialogService,
            IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository, IApplicationCommands applicationCommands
            , IPropertiesChangeJornal propertiesChangeJornal)
        {
            CommonChangeJornal = propertiesChangeJornal as PropertiesChangeJornal;
              SaveCommand = new NotifyCommand(OnSave, CanSave).ObservesProperty(() => SelectedWork);
            CloseCommand = new NotifyCommand<object>(OnClose);
        
            UnDoLeftCommand = new NotifyCommand(() => base.OnUnDoLeft(Id),
                () => { return !CommonChangeJornal.IsOnFirstRecord(Id); })
               .ObservesPropertyChangedEvent(CommonChangeJornal);
         
            UnDoRightCommand = new NotifyCommand(() => base.OnUnDoRight(Id),
                () => { return !CommonChangeJornal.IsOnLastRecord(Id); })
                  .ObservesPropertyChangedEvent(CommonChangeJornal);

            RemovePreviousWorkCommand = new NotifyCommand(OnRemovePreviousWork,
                                        () => SelectedPreviousWork != null)
                    .ObservesProperty(() => SelectedPreviousWork);
            RemoveNextWorkCommand = new NotifyCommand(OnRemoveNextWork,
                                        () => SelectedNextWork != null)
                    .ObservesProperty(() => SelectedNextWork);

            AddPreviousWorkCommand = new NotifyCommand(OnAddPreviousWork);
            AddNextWorkCommand = new NotifyCommand(OnAddNextWork);

            EditPreviousWorkCommand = new NotifyCommand(OnEditPreviousWork,
                                        () => SelectedPreviousWork != null)
                    .ObservesProperty(() => SelectedPreviousWork);
            EditNextWorkCommand = new NotifyCommand(OnEditNextWork,
                                        () => SelectedNextWork != null)
                    .ObservesProperty(() => SelectedNextWork);

            DataGridLostFocusCommand = new NotifyCommand<object>(OnDataGridLostSocus);
            GenerateAxecDocsCommand = new NotifyCommand(OnGenerateAxecDocsCommand);


            _dialogService = dialogService;
            _buildingUnitsRepository = buildingUnitsRepository;
            _regionManager = regionManager;
            _applicationCommands = applicationCommands;
            _applicationCommands.SaveAllCommand.RegisterCommand(SaveCommand);
            _applicationCommands.UnDoRightCommand.RegisterCommand(UnDoRightCommand);
            _applicationCommands.UnDoLeftCommand.RegisterCommand(UnDoLeftCommand);
        }

        private void OnGenerateAxecDocsCommand()
        {
            SelectedWork.SaveAOSRToWord(ProjectService.SelectFileDirectory());
        }


        private void OnEditPreviousWork()
        {
            CoreFunctions.EditElementDialog<bldWork>(SelectedPreviousWork, "Перыдыдущая работа",
                  (result) => { }, _dialogService, typeof(WorkDialogView).Name, "Редактировать", Id);
        }
        private void OnEditNextWork()
        {
            CoreFunctions.EditElementDialog<bldWork>(SelectedNextWork, "Последующая работа",
                 (result) => { }, _dialogService, typeof(WorkDialogView).Name, "Редактировать", Id);

        }

        private void OnAddNextWork()
        {
            bldWorksGroup All_Works = new bldWorksGroup(_buildingUnitsRepository.Works.GetbldWorksAsync());

            NameablePredicate<bldWorksGroup, bldWork> predicate_1 = new NameablePredicate<bldWorksGroup, bldWork>();
            NameablePredicate<bldWorksGroup, bldWork> predicate_2 = new NameablePredicate<bldWorksGroup, bldWork>();
            NameablePredicate<bldWorksGroup, bldWork> predicate_3 = new NameablePredicate<bldWorksGroup, bldWork>();
            NameablePredicate<bldWorksGroup, bldWork> predicate_4 = new NameablePredicate<bldWorksGroup, bldWork>();

            predicate_1.Name = "Показать все из текущего проекта.";
            predicate_1.Predicate = cl => cl.Where(el => el.bldConstruction?.bldObject?.bldProject != null &&
                                    el.bldConstruction?.bldObject?.bldProject.Id == SelectedWork.bldConstruction?.bldObject?.bldProject.Id).ToList();
            predicate_2.Name = "Показать все из текущего объекта";
            predicate_2.Predicate = cl => cl.Where(el => el.bldConstruction?.bldObject != null &&
                                    el.bldConstruction?.bldObject?.Id == SelectedWork.bldConstruction?.bldObject?.Id).ToList();
            predicate_3.Name = "Показать все из текущеей конструкции";
            predicate_3.Predicate = cl => cl.Where(el => el.bldConstruction != null &&
                                 el.bldConstruction?.Id == SelectedWork.bldConstruction?.Id).ToList();
            predicate_4.Name = "Показать все";
            predicate_4.Predicate = cl => cl;

            NameablePredicateObservableCollection<bldWorksGroup, bldWork> nameablePredicatesCollection = new NameablePredicateObservableCollection<bldWorksGroup, bldWork>();
            nameablePredicatesCollection.Add(predicate_1);
            nameablePredicatesCollection.Add(predicate_2);
            nameablePredicatesCollection.Add(predicate_3);
            nameablePredicatesCollection.Add(predicate_4);
          
            CoreFunctions.AddElementToCollectionWhithDialog_Test<bldWorksGroup, bldWork>
              (SelectedWork.NextWorks, All_Works,
               nameablePredicatesCollection,
              _dialogService,
               (result) =>
               {
                   if (result.Result == ButtonResult.Yes)
                   {
                       SaveCommand.RaiseCanExecuteChanged();
                       foreach (bldWork bld_worck in SelectedWork.NextWorks)
                       {
                           bld_worck.bldConstruction = SelectedWork.bldConstruction;
                           if (!SelectedWork.bldConstruction.Works.Contains(bld_worck))
                               SelectedWork.bldConstruction.Works.Add(bld_worck);
                           bld_worck.PreviousWorks.Add(SelectedWork);
                       }

                   }
                   if (result.Result == ButtonResult.No)
                   {
                       CommonChangeJornal.UnDoAll(Id);
                   }
               },
              typeof(AddbldWorkToCollectionDialogView).Name,
              typeof(WorkDialogView).Name, Id,
              "Редактирование списка последущих работ",
              "Форма для редактирования состава последущих работ.",
              "Работы текущей конструкции", "Список работ");


            //new bldWorksGroup(_buildingUnitsRepository.Works.GetAllBldWorks());
          
        }

        private void OnRemoveNextWork()
        {
            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldWorksGroup, bldWork>
                (SelectedWork.NextWorks, SelectedNextWork, "Последующая работа",
                () => SelectedNextWork = null, _dialogService,Id);
        }

        private void OnAddPreviousWork()
        {
            bldWorksGroup All_Works = new bldWorksGroup(_buildingUnitsRepository.Works.GetbldWorksAsync());

            NameablePredicate<bldWorksGroup, bldWork> predicate_1 = new NameablePredicate<bldWorksGroup, bldWork>();
            NameablePredicate<bldWorksGroup, bldWork> predicate_2 = new NameablePredicate<bldWorksGroup, bldWork>();
            NameablePredicate<bldWorksGroup, bldWork> predicate_3 = new NameablePredicate<bldWorksGroup, bldWork>();
            NameablePredicate<bldWorksGroup, bldWork> predicate_4 = new NameablePredicate<bldWorksGroup, bldWork>();

            predicate_1.Name = "Показать все из текущего проекта.";
            predicate_1.Predicate = cl => cl.Where(el => el.bldConstruction?.bldObject?.bldProject != null &&
                                    el.bldConstruction?.bldObject?.bldProject.Id == SelectedWork.bldConstruction?.bldObject?.bldProject.Id).ToList();
            predicate_2.Name = "Показать все из текущего объекта";
            predicate_2.Predicate = cl => cl.Where(el => el.bldConstruction?.bldObject != null &&
                                    el.bldConstruction?.bldObject?.Id == SelectedWork.bldConstruction?.bldObject?.Id).ToList();
            predicate_3.Name = "Показать все из текущеей конструкции";
            predicate_3.Predicate = cl => cl.Where(el => el.bldConstruction != null &&
                                 el.bldConstruction?.Id == SelectedWork.bldConstruction?.Id).ToList();
            predicate_4.Name = "Показать все";
            predicate_4.Predicate = cl => cl;

            NameablePredicateObservableCollection<bldWorksGroup, bldWork> nameablePredicatesCollection = new NameablePredicateObservableCollection<bldWorksGroup, bldWork>();
            nameablePredicatesCollection.Add(predicate_1);
            nameablePredicatesCollection.Add(predicate_2);
            nameablePredicatesCollection.Add(predicate_3);
            nameablePredicatesCollection.Add(predicate_4);

            CoreFunctions.AddElementToCollectionWhithDialog_Test<bldWorksGroup, bldWork>
              (SelectedWork.PreviousWorks, All_Works,
               nameablePredicatesCollection,
              _dialogService,
               (result) =>
               {
                   if (result.Result == ButtonResult.Yes)
                   {
                       SaveCommand.RaiseCanExecuteChanged();
                       foreach (bldWork bld_worck in SelectedWork.PreviousWorks)
                       {
                           bld_worck.bldConstruction = SelectedWork.bldConstruction;
                           if (!SelectedWork.bldConstruction.Works.Contains(bld_worck))
                               SelectedWork.bldConstruction.Works.Add(bld_worck);
                           bld_worck.NextWorks.Add(SelectedWork);
                       }

                   }
                   if (result.Result == ButtonResult.No)
                   {
                       CommonChangeJornal.UnDoAll(Id);
                   }
               },
              typeof(AddbldWorkToCollectionDialogView).Name,
              typeof(WorkDialogView).Name, Id,
              "Редактирование списка предшествующих работ",
              "Форма для редактирования состава предшествующих работ.",
              "Работы текущей конструкции", "Список работ");


        }
        private void OnDataGridLostSocus(object obj)
        {

            if (obj == SelectedPreviousWork)
            {
                SelectedNextWork = null;
                return;
            }
            if (obj == SelectedNextWork)
            {

                SelectedPreviousWork = null;
                return;
            }
        }
        private void OnRemovePreviousWork()
        {

            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldWorksGroup, bldWork>
                 (SelectedWork.PreviousWorks, SelectedPreviousWork, "Предыдущая работа",
                 () => SelectedPreviousWork = null, _dialogService,Id);
        }
      

       
        public void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        private bool CanSave()
        {
            if (SelectedWork != null)
                return !SelectedWork.HasErrors;// && SelectedWork.PropertiesChangeJornal.Count > 0;
            else
                return false;
        }
        public override void OnSave()
        {
            base.OnSave<bldWork>(SelectedWork);
        }
        public override void OnClose(object obj)
        {
            base.OnClose<bldWork>(obj, SelectedWork);
        }
        public override void OnWindowClose()
        {
            _applicationCommands.SaveAllCommand.UnregisterCommand(SaveCommand);
            _applicationCommands.UnDoRightCommand.UnregisterCommand(UnDoRightCommand);
            _applicationCommands.UnDoLeftCommand.UnregisterCommand(UnDoLeftCommand);
            base.OnWindowClose();
        }

      
        public void OnNavigatedTo(NavigationContext navigationContext)
        {

            ConveyanceObject navigane_message_work = (ConveyanceObject)navigationContext.Parameters["bld_work"];
            ConveyanceObject navigane_message_construction = (ConveyanceObject)navigationContext.Parameters["bld_construction"];
             if (navigane_message_work != null)
            {
                ResivedWork = (bldWork)navigane_message_work.Object;
                ResivedConstruction = (bldConstruction)navigane_message_construction.Object;
                //  ResivedObject = (bldObject)navigane_message_object.Object;
                SelectedWork = ResivedWork;
                 EditMode = navigane_message_work.EditMode;
                if (SelectedWork != null) SelectedWork.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedWork = ResivedWork;
                SelectedWork.ErrorsChanged += RaiseCanExecuteChanged;
                SelectedConstruction = ResivedConstruction;
                SelectedObject = new bldObject();
             //   CoreFunctions.CopyObjectReflectionNewInstances(ResivedConstruction, SelectedConstruction);
              //  SelectedWork = SelectedConstruction.Works.Where(wr => wr.Id == ResivedWork.Id).FirstOrDefault();
                AllDocuments.Clear();
                if(SelectedWork.AOSRDocuments.Count>0) AllDocuments.Add(SelectedWork.AOSRDocuments.Id, SelectedWork.AOSRDocuments);
                if (SelectedWork.LaboratoryReports.Count > 0) AllDocuments.Add(SelectedWork.LaboratoryReports.Id, SelectedWork.LaboratoryReports);
                if (SelectedWork.ExecutiveSchemes.Count > 0) AllDocuments.Add(SelectedWork.ExecutiveSchemes.Id, SelectedWork.ExecutiveSchemes);
                 Title = ResivedWork.ShortName;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
          //  AllDocuments.Clear();
            ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_work"];
            if (((bldWork)navigane_message.Object).Id != SelectedWork.Id)
                return false;
            else
                return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }



    }
}

