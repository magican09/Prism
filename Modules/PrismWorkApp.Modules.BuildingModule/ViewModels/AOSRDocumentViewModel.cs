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
    public class AOSRDocumentViewModel : BaseViewModel<AOSRDocument>, INotifyPropertyChanged, INavigationAware
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

        public AOSRDocumentViewModel(IDialogService dialogService,
            IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository)
        {
            DataGridLostFocusCommand = new DelegateCommand<object>(OnDataGridLostSocus);
            SaveCommand = new DelegateCommand(OnSave, CanSave)
                .ObservesProperty(() => SelectedAOSRDocument);
            CloseCommand = new DelegateCommand<object>(OnClose);

            GenerateWordDocumentCommand = new DelegateCommand(OnGenerateWordDocumentCommand);
            _dialogService = dialogService;
            _buildingUnitsRepository = buildingUnitsRepository;
            _regionManager = regionManager;
        }

        private void OnGenerateWordDocumentCommand()
        {
            ProjectService.SaveAOSRToWord(SelectedAOSRDocument);
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




        private bool CanSave()
        {
            if (SelectedAOSRDocument != null)
                return !SelectedAOSRDocument.HasErrors ;
            else
                return false;
        }
        public void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }
        public virtual void OnSave()
        {
            this.OnSave<bldAOSRDocument>(SelectedAOSRDocument);
        }
        public virtual void OnClose(object obj)
        {
            this.OnClose<bldAOSRDocument>(obj, SelectedAOSRDocument);
        }


        public void OnNavigatedTo(NavigationContext navigationContext)
        {

            ConveyanceObject navigane_message_aosr_document = (ConveyanceObject)navigationContext.Parameters["bld_aosr_document"];
            if (navigane_message_aosr_document != null)
            {
                ResivedAOSRDocument = (bldAOSRDocument)navigane_message_aosr_document.Object;
                EditMode = navigane_message_aosr_document.EditMode;
                if (SelectedAOSRDocument != null) SelectedAOSRDocument.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedAOSRDocument = ResivedAOSRDocument;
                SelectedAOSRDocument.ErrorsChanged += RaiseCanExecuteChanged;

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

