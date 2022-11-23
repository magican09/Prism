using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
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
        public NotifyCommand SaveCommand { get; private set; }
        public NotifyCommand<object> CloseCommand { get; private set; }

        public NotifyCommand RemovePreviousWorkCommand { get; private set; }
        public NotifyCommand RemoveNextWorkCommand { get; private set; }

        public NotifyCommand AddPreviousWorkCommand { get; private set; }
        public NotifyCommand AddNextWorkCommand { get; private set; }

        public NotifyCommand EditPreviousWorkCommand { get; private set; }
        public NotifyCommand EditNextWorkCommand { get; private set; }

        public IBuildingUnitsRepository _buildingUnitsRepository { get; }
        private IApplicationCommands _applicationCommands;


        public WorkViewModel(IDialogService dialogService,
            IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository, IApplicationCommands applicationCommands
            , IPropertiesChangeJornal propertiesChangeJornal)
        {
            DataGridLostFocusCommand = new NotifyCommand<object>(OnDataGridLostSocus);
            SaveCommand = new NotifyCommand(OnSave, CanSave)
                .ObservesProperty(() => SelectedWork);
            CloseCommand = new NotifyCommand<object>(OnClose);

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
            _dialogService = dialogService;
            _buildingUnitsRepository = buildingUnitsRepository;
            _regionManager = regionManager;
            _applicationCommands = applicationCommands;
            _applicationCommands.SaveAllCommand.RegisterCommand(SaveCommand);
        }




        private void OnEditNextWork()
        {
            CoreFunctions.EditElementDialog<bldWork>(SelectedNextWork, "Последующая работа",
                 (result) => { }, _dialogService, typeof(WorkDialogView).Name, "Редактировать", Id);

        }
        private void OnAddNextWork()
        {
            bldWorksGroup AllWorks = new bldWorksGroup(SelectedWork.bldConstruction.Works.Where(wr=>wr.Id!=SelectedWork.Id).ToList());
            //new bldWorksGroup(_buildingUnitsRepository.Works.GetAllBldWorks());
            CoreFunctions.AddElementToCollectionWhithDialog<bldWorksGroup, bldWork>
                 (SelectedWork.NextWorks, AllWorks, _dialogService,
                 (result) =>
                 {
                     if (result.Result == ButtonResult.Yes)
                     {
                         bldWorksGroup new_nextWork_collection = (bldWorksGroup)
                                result.Parameters.GetValue<object>("current_collection");

                         bldWorksGroup add_works = new bldWorksGroup();

                         foreach (bldWork work in new_nextWork_collection)//Добавляем выбранные работы в писок
                         { 
                             if (SelectedWork.NextWorks.Where(wr => CoreFunctions.GetParsingId(wr) == CoreFunctions.GetParsingId(work)).FirstOrDefault() == null)//Если работы в списке нет...
                             {
                                 bldWork new_work = AllWorks.Where(wr => CoreFunctions.GetParsingId(wr) == CoreFunctions.GetParsingId(work)).FirstOrDefault();
                                 add_works.Add(new_work);
                                  SelectedWork.NextWorks.Add(new_work);
                             }
                         }
                         //CoreFunctions.CopyObjectReflectionNewInstances(new_nextWork_collection, SelectedWork.NextWorks);
                         foreach (bldWork work in add_works)
                               work?.PreviousWorks.Add(SelectedWork);
                     }
                 },
                 typeof(AddbldWorkToCollectionDialogView).Name,
                 typeof(WorkDialogView).Name,Id,
                  "Редактирование списка последующих работ",
                 "Форма для редактирования спика последующих работ.",
                 "Список всех работ текущей коснрукции", "Последущие работы");
        }
        private void OnRemoveNextWork()
        {
            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldWorksGroup, bldWork>
                (SelectedWork.NextWorks, SelectedNextWork, "Последующая работа",
                () => SelectedNextWork = null, _dialogService,Id);
        }

        private void OnAddPreviousWork()
        {
             bldWorksGroup AllWorks = new bldWorksGroup(SelectedWork.bldConstruction.Works.Where(wr=>wr.Id!=SelectedWork.Id).ToList());
            // new bldWorksGroup(_buildingUnitsRepository.Works.GetAllBldWorks());
            CoreFunctions.AddElementToCollectionWhithDialog<bldWorksGroup, bldWork>
                (SelectedWork.PreviousWorks, AllWorks, _dialogService,
                 (result) =>
                 {
                     if (result.Result == ButtonResult.Yes)

                     {
                         bldWorksGroup new_previousWork_collection = (bldWorksGroup)
                           result.Parameters.GetValue<object>("current_collection");

                         bldWorksGroup add_works = new bldWorksGroup();

                         foreach (bldWork work in new_previousWork_collection)
                         {
                             if (SelectedWork.PreviousWorks.Where(wr => CoreFunctions.GetParsingId(wr) == CoreFunctions.GetParsingId(work)).FirstOrDefault() == null)
                             {
                                 bldWork new_work = AllWorks.Where(wr => CoreFunctions.GetParsingId(wr) == CoreFunctions.GetParsingId(work)).FirstOrDefault();
                                 add_works.Add(new_work);
                                 SelectedWork.PreviousWorks.Add(new_work);
                            //     SelectedWork.PreviousWorks.Name = "1111111";
                             }
                         }
                         // RiseEvent  SelectedWork.PreviousWorks.CollectionChanged(N);
                         foreach (bldWork work in add_works)
                         {
                             work?.NextWorks.Add(SelectedWork);
                          //   work.NextWorks.Name = "222222";
                         }
                     }

                 },
                typeof(AddbldWorkToCollectionDialogView).Name,
                typeof(WorkDialogView).Name, Id,
                "Редактирование списка предыдущих работ",
                "Форма для редактирования.",
                "Список работ", "Все работы");
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
        private void OnEditPreviousWork()
        {
            CoreFunctions.EditElementDialog<bldWork>(SelectedPreviousWork, "Перыдыдущая работа",
                  (result) => { }, _dialogService, typeof(ConstructionDialogView).Name, "Редактировать", Id);
        }

        private bool CanSave()
        {
            if (SelectedWork != null)
                return !SelectedWork.HasErrors;// && SelectedWork.PropertiesChangeJornal.Count > 0;
            else
                return false;
        }
        public void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }
        
        public virtual void OnSave()
        {
            this.OnSave<bldWork>(SelectedWork);
        }
        public virtual void OnClose(object obj)
        {
            this.OnClose<bldWork>(obj, SelectedWork);
        }
        public override void OnWindowClose()
        {
            _applicationCommands.SaveAllCommand.UnregisterCommand(SaveCommand);
            base.OnWindowClose();
        }

        private void Save()
        {
            //CoreFunctions.CopyObjectReflectionNewInstances(SelectedConstruction, ResivedConstruction);
            CommonChangeJornal.SaveAll(Id);
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

