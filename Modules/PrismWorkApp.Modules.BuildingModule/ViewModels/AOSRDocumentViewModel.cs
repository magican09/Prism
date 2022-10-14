using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Modules.BuildingModule.Core;
using PrismWorkApp.Modules.BuildingModule.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.ProjectModel.Data.Models;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
//using BindableBase = Prism.Mvvm.BindableBase;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class AOSRDocumentViewModel : LocalBindableBase, INotifyPropertyChanged, INavigationAware
    {
        private string _title = "АОСР";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private SimpleEditableBldProject _selectedProject;
        public SimpleEditableBldProject SelectedProject
        {
            get { return _selectedProject; }
            set { SetProperty(ref _selectedProject, value); }
        }
        private bldProject _resivedProject;
        public bldProject ResivedProject
        {
            get { return _resivedProject; }
            set { SetProperty(ref _resivedProject, value); }

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
        private bldAOSRDocument _selectedAOSRDocument;
        public bldAOSRDocument SelectedAOSRDocument
        {
            get { return _selectedAOSRDocument; }
            set { SetProperty(ref _selectedAOSRDocument, value); }
        }
        private bldAOSRDocument _resivedAOSRDocument;
        public bldAOSRDocument ResivedAOSRDocument
        {
            get { return _resivedAOSRDocument; }
            set { SetProperty(ref _resivedAOSRDocument, value); }
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

        private bldAOSRDocument _selectedPreviousWork;
        public bldAOSRDocument SelectedPreviousWork
        {
            get { return _selectedPreviousWork; }
            set { SetProperty(ref _selectedPreviousWork, value); }
        }
        private bldAOSRDocument _selectedNextWork;
        public bldAOSRDocument SelectedNextWork
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



        public DelegateCommand<object> DataGridLostFocusCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand<object> CloseCommand { get; private set; }

        public DelegateCommand RemovePreviousWorkCommand { get; private set; }
        public DelegateCommand RemoveNextWorkCommand { get; private set; }

        public DelegateCommand AddPreviousWorkCommand { get; private set; }
        public DelegateCommand AddNextWorkCommand { get; private set; }

        public DelegateCommand EditPreviousWorkCommand { get; private set; }
        public DelegateCommand EditNextWorkCommand { get; private set; }
        
        public DelegateCommand GenerateWordDocumentCommand { get; private set; }

        public IBuildingUnitsRepository _buildingUnitsRepository { get; }



        protected readonly IDialogService _dialogService;
        private readonly IRegionManager _regionManager;

        public AOSRDocumentViewModel(IDialogService dialogService,
            IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository)
        {
            DataGridLostFocusCommand = new DelegateCommand<object>(OnDataGridLostSocus);
            SaveCommand = new DelegateCommand(OnSave, CanSave)
                .ObservesProperty(() => SelectedAOSRDocument);
            CloseCommand = new DelegateCommand<object>(OnClose);

            RemovePreviousWorkCommand = new DelegateCommand(OnRemovePreviousWork,
                                        () => SelectedPreviousWork != null)
                    .ObservesProperty(() => SelectedPreviousWork);
            RemoveNextWorkCommand = new DelegateCommand(OnRemoveNextWork,
                                        () => SelectedNextWork != null)
                    .ObservesProperty(() => SelectedNextWork);

            AddPreviousWorkCommand = new DelegateCommand(OnAddPreviousWork);
            AddNextWorkCommand = new DelegateCommand(OnAddNextWork);

            EditPreviousWorkCommand = new DelegateCommand(OnEditPreviousWork,
                                        () => SelectedPreviousWork != null)
                    .ObservesProperty(() => SelectedPreviousWork);
            EditNextWorkCommand = new DelegateCommand(OnEditNextWork,
                                        () => SelectedNextWork != null)
                    .ObservesProperty(() => SelectedNextWork);
            GenerateWordDocumentCommand = new DelegateCommand(OnGenerateWordDocumentCommand);
            _dialogService = dialogService;
            _buildingUnitsRepository = buildingUnitsRepository;
            _regionManager = regionManager;
        }

        private void OnGenerateWordDocumentCommand()
        {
            ProjectService.SaveAOSRToWord(SelectedAOSRDocument);
        }

        private void OnClose(object obj)
        {
            CoreFunctions.ConfirmActionOnElementDialog<bldAOSRDocument>(SelectedAOSRDocument, "Сохранить", "работу", "Сохранить", "Не сохранять", "Отмена", (result) =>
            {
                if (obj != null)
                {
                    if (result.Result == ButtonResult.Yes)
                    {
                        Save();
                        // KeepAlive = false;
                        if (_regionManager.Regions[RegionNames.ContentRegion].Views.Contains(obj))
                            _regionManager.Regions[RegionNames.ContentRegion].Remove(obj);
                    }
                    else if (result.Result == ButtonResult.No)
                    {
                        //KeepAlive = false;
                        if (_regionManager.Regions[RegionNames.ContentRegion].Views.Contains(obj))
                            _regionManager.Regions[RegionNames.ContentRegion].Remove(obj);
                    }
                    else if (result.Result == ButtonResult.Cancel)
                    {

                    }
                }
            }, _dialogService);
        }

        private void OnEditNextWork()
        {
            CoreFunctions.EditElementDialog<bldAOSRDocument>(SelectedNextWork, "Последующая работа",
                 (result) => { }, _dialogService, typeof(WorkDialogView).Name, "Редактировать", Id);

        }

        private void OnEditPreviousWork()
        {
            CoreFunctions.EditElementDialog<bldAOSRDocument>(SelectedPreviousWork, "Перыдыдущая работа",
                  (result) => { }, _dialogService, typeof(ConstructionDialogView).Name, "Редактировать", Id);
        }



        private void OnAddNextWork()
        {
         /*   bldWorksGroup AllWorks = new bldWorksGroup(SelectedAOSRDocument.bldConstruction.Works.Where(wr=>wr.Id!=SelectedAOSRDocument.Id).ToList());
            //new bldWorksGroup(_buildingUnitsRepository.Works.GetAllBldWorks());
            CoreFunctions.AddElementToCollectionWhithDialog<bldWorksGroup, bldAOSRDocument>
                 (SelectedAOSRDocument.NextWorks, AllWorks, _dialogService,
                 (result) =>
                 {
                     if (result.Result == ButtonResult.Yes)
                     {
                         bldWorksGroup new_nextWork_collection = (bldWorksGroup)
                                result.Parameters.GetValue<object>("current_collection");

                         bldWorksGroup add_works = new bldWorksGroup();

                         foreach (bldAOSRDocument work in new_nextWork_collection)//Добавляем выбранные работы в писок
                         { 
                             if (SelectedAOSRDocument.NextWorks.Where(wr => CoreFunctions.GetParsingId(wr) == CoreFunctions.GetParsingId(work)).FirstOrDefault() == null)//Если работы в списке нет...
                             {
                                 bldAOSRDocument new_work = AllWorks.Where(wr => CoreFunctions.GetParsingId(wr) == CoreFunctions.GetParsingId(work)).FirstOrDefault();
                                 add_works.Add(new_work);
                                  SelectedAOSRDocument.NextWorks.Add(new_work);
                             }
                         }
                         //CoreFunctions.CopyObjectReflectionNewInstances(new_nextWork_collection, SelectedAOSRDocument.NextWorks);
                         foreach (bldAOSRDocument work in add_works)
                               work?.PreviousWorks.Add(SelectedAOSRDocument);
                     }
                 },
                 typeof(AddbldWorkToCollectionDialogView).Name,
                 typeof(WorkDialogView).Name,
                  "Редактирование списка последующих работ",
                 "Форма для редактирования спика последующих работ.",
                 "Список всех работ текущей коснрукции", "Последущие работы");*/
        }

        private void OnAddPreviousWork()
        {
          /*   bldWorksGroup AllWorks = new bldWorksGroup(SelectedAOSRDocument.bldConstruction.Works.Where(wr=>wr.Id!=SelectedAOSRDocument.Id).ToList());
            // new bldWorksGroup(_buildingUnitsRepository.Works.GetAllBldWorks());
            CoreFunctions.AddElementToCollectionWhithDialog<bldWorksGroup, bldAOSRDocument>
                (SelectedAOSRDocument.PreviousWorks, AllWorks, _dialogService,
                 (result) =>
                 {
                     if (result.Result == ButtonResult.Yes)

                     {
                         bldWorksGroup new_previousWork_collection = (bldWorksGroup)
                           result.Parameters.GetValue<object>("current_collection");

                         bldWorksGroup add_works = new bldWorksGroup();

                         foreach (bldAOSRDocument work in new_previousWork_collection)
                         {
                             if (SelectedAOSRDocument.PreviousWorks.Where(wr => CoreFunctions.GetParsingId(wr) == CoreFunctions.GetParsingId(work)).FirstOrDefault() == null)
                             {
                                 bldAOSRDocument new_work = AllWorks.Where(wr => CoreFunctions.GetParsingId(wr) == CoreFunctions.GetParsingId(work)).FirstOrDefault();
                                 add_works.Add(new_work);
                                 SelectedAOSRDocument.PreviousWorks.Add(new_work);
                            //     SelectedAOSRDocument.PreviousWorks.Name = "1111111";
                             }
                         }
                         // RiseEvent  SelectedAOSRDocument.PreviousWorks.CollectionChanged(N);
                         foreach (bldAOSRDocument work in add_works)
                         {
                             work?.NextWorks.Add(SelectedAOSRDocument);
                          //   work.NextWorks.Name = "222222";
                         }
                     }

                 },
                typeof(AddbldWorkToCollectionDialogView).Name,
                typeof(WorkDialogView).Name,
                "Редактирование списка предыдущих работ",
                "Форма для редактирования.",
                "Список работ", "Все работы");*/
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

        private void OnRemoveNextWork()
        {
           /* CoreFunctions.RemoveElementFromCollectionWhithDialog<bldWorksGroup, bldAOSRDocument>
                (SelectedAOSRDocument.NextWorks, SelectedNextWork, "Последующая работа",
                () => SelectedNextWork = null, _dialogService);*/
        }

        private void OnRemovePreviousWork()
        {

           /* CoreFunctions.RemoveElementFromCollectionWhithDialog<bldWorksGroup, bldAOSRDocument>
                 (SelectedAOSRDocument.PreviousWorks, SelectedPreviousWork, "Предыдущая работа",
                 () => SelectedPreviousWork = null, _dialogService);*/
        }


        private bool CanSave()
        {
            if (SelectedAOSRDocument != null)
                return !SelectedAOSRDocument.HasErrors;
            else
                return false;
        }
        public void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }
        public virtual void OnSave()
        {
            CoreFunctions.ConfirmActionOnElementDialog<bldAOSRDocument>(SelectedAOSRDocument, "Сохранить", "работу", "Сохранить", "Не сохранять", "Отмена", (result) =>
            {
                if (result.Result == ButtonResult.Yes)
                {
                    Save();
                }
            }, _dialogService);
        }

        private void Save()
        {
         //    CoreFunctions.CopyObjectReflectionNewInstances(SelectedConstruction, ResivedConstruction);
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {

            ConveyanceObject navigane_message_aosr_document = (ConveyanceObject)navigationContext.Parameters["bld_aosr_document"];
            ConveyanceObject navigane_message_work = (ConveyanceObject)navigationContext.Parameters["bld_work"];
            ConveyanceObject navigane_message_project = (ConveyanceObject)navigationContext.Parameters["bld_project"];

            //   ConveyanceObject navigane_message_construction = (ConveyanceObject)navigationContext.Parameters["bld_construction"];
            if (navigane_message_aosr_document != null)
            {
                ResivedProject =(bldProject) navigane_message_project.Object;
                ResivedWork = (bldWork)navigane_message_work.Object;
                ResivedAOSRDocument =(bldAOSRDocument)navigane_message_aosr_document.Object;
               // ResivedConstruction = (bldConstruction)navigane_message_construction.Object;
               //  ResivedObject = (bldObject)navigane_message_object.Object;

                EditMode = navigane_message_aosr_document.EditMode;
                if (SelectedAOSRDocument != null) SelectedAOSRDocument.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedAOSRDocument = new bldAOSRDocument();
                SelectedAOSRDocument.ErrorsChanged += RaiseCanExecuteChanged;
                SelectedConstruction = new bldConstruction();
                SelectedProject = new SimpleEditableBldProject();
             //   CoreFunctions.CopyObjectReflectionNewInstances(ResivedProject, SelectedProject);
                SelectedObject = SelectedProject.BuildingObjects.Where(ob => ob.Id == ResivedAOSRDocument.bldWork.bldConstruction.bldObject.Id).FirstOrDefault();
                SelectedConstruction = SelectedObject.Constructions.Where(cn => cn.Id == ResivedAOSRDocument.bldWork.bldConstruction.Id).FirstOrDefault();
                SelectedWork = SelectedConstruction.Works.Where(wr => wr.Id == ResivedAOSRDocument.bldWork.Id).FirstOrDefault();
                SelectedAOSRDocument = SelectedWork.AOSRDocuments.Where(dc => dc.Id == ResivedAOSRDocument.Id).FirstOrDefault();
             /*   SelectedAOSRDocument = SelectedConstruction.Works.Where(wr => wr.Id == ResivedAOSRDocument.Id).FirstOrDefault();
                AllDocuments.Clear();
                AllDocuments.Add(SelectedAOSRDocument.AOSRDocuments.Id, SelectedAOSRDocument.AOSRDocuments);
                AllDocuments.Add(SelectedAOSRDocument.LaboratoryReports.Id, SelectedAOSRDocument.LaboratoryReports);
                AllDocuments.Add(SelectedAOSRDocument.ExecutiveSchemes.Id, SelectedAOSRDocument.ExecutiveSchemes);
             */
                Title = ResivedAOSRDocument.ShortName;
               
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
          //  AllDocuments.Clear();
            ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_aosr_document"];
            if (((bldAOSRDocument)navigane_message.Object).Id != SelectedAOSRDocument.Id)
                return false;
            else
                return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }



    }
}

