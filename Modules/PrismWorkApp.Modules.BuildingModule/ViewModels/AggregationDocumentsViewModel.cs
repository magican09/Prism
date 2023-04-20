using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Modules.BuildingModule.Core;
using PrismWorkApp.Modules.BuildingModule.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class AggregationDocumentsViewModel : BaseViewModel<bldMaterialCertificate>, INotifyPropertyChanged, INavigationAware
    {


        private bldDocument _selectedDocument;
        public bldDocument SelectedDocument
        {
            get { return _selectedDocument; }
            set { SetProperty(ref _selectedDocument, value); }
        }
        private ObservableCollection<bldDocument> _selectedDocuments = new ObservableCollection<bldDocument>();
        public ObservableCollection<bldDocument> SelectedDocuments
        {
            get { return _selectedDocuments; }
            set { SetProperty(ref _selectedDocuments, value); }
        }

        private ObservableCollection<bldUnitOfMeasurement> _allUnitsOfMeasurements;
        public ObservableCollection<bldUnitOfMeasurement> AllUnitsOfMeasurements
        {
            get { return _allUnitsOfMeasurements; }
            set { SetProperty(ref _allUnitsOfMeasurements, value); }
        }
        private bldAggregationDocumentsGroup _aggregationDocuments = new bldAggregationDocumentsGroup();
        public bldAggregationDocumentsGroup AggregationDocuments
        {
            get { return _aggregationDocuments; }
            set { SetProperty(ref _aggregationDocuments, value); }
        }
        private bldAggregationDocument _selectedAggregationDocument;

        public bldAggregationDocument SelectedAggregationDocument
        {
            get { return _selectedAggregationDocument; }
            set { SetProperty(ref _selectedAggregationDocument, value); }
        }

        public NotifyCommand<object> ContextMenuOpenedCommand { get; private set; }

        public NotifyCommand<object> DataGridLostFocusCommand { get; private set; }
        public NotifyCommand<object> DataGridSelectionChangedCommand { get; private set; }
        //public NotifyCommand<object> DataGridCopyingCommand { get; private set; }
        //public NotifyCommand<object> DataGridCopyingCellClipboardContentCommand { get; private set; }
        //public NotifyCommand<object> DataGridPastingCommand { get; private set; }
        //public NotifyCommand<object> DataGridPastingCellClipboardContentCommand { get; private set; }

        public NotifyCommand UnDoCommand { get; protected set; }
        public NotifyCommand ReDoCommand { get; protected set; }
        public NotifyCommand SaveCommand { get; protected set; }
        public NotifyCommand<object> CloseCommand { get; protected set; }

        public NotifyCommand<object> CloseAggregationDocumentCommand { get; private set; }

        public NotifyCommand<object> CreateNewMaterialCertificateCommand { get; private set; }
        public NotifyCommand<object> CreatedBasedOnMaterialCertificateCommand { get; private set; }
        public NotifyCommand<object> RemoveMaterialCertificateCommand { get; private set; }
        public NotifyCommand<object> CopyMaterialCertificateCommand { get; private set; }
        public NotifyCommand<object> PasteMaterialCertificateCommand { get; private set; }
        public ObservableCollection<bldMaterialCertificate> MaterialCertificatesBuffer = new ObservableCollection<bldMaterialCertificate>();
        public NotifyCommand<object> AddMaterialCertificateToCommand { get; private set; }
        public NotifyCommand<object> MoveMaterialCertificateToCommand { get; private set; }

        public NotifyCommand<object> OpenImageFileCommand { get; private set; }
        public NotifyCommand<object> SaveImageFileToDiskCommand { get; private set; }
        public NotifyCommand<object> LoadImageFileFromDiskCommand { get; private set; }

        public ObservableCollection<MenuItem> CommonContextMenuItems { get; set; }



        public IBuildingUnitsRepository _buildingUnitsRepository { get; }
        AppObjectsModel _appObjectsModel;
        public AggregationDocumentsViewModel(IDialogService dialogService,
           IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository, IApplicationCommands applicationCommands, IAppObjectsModel appObjectsModel)
        {

            UnDoReDo = new UnDoReDoSystem();
            ApplicationCommands = applicationCommands;

            _dialogService = dialogService;
            _buildingUnitsRepository = buildingUnitsRepository;
            _regionManager = regionManager;
            ContextMenuOpenedCommand = new NotifyCommand<object>(OnContextMenuOpened);

            DataGridSelectionChangedCommand = new NotifyCommand<object>(OnDataGridSelectionChanged);
            DataGridLostFocusCommand = new NotifyCommand<object>(OnDataGridLostFocus);

            //DataGridCopyingCommand = new NotifyCommand<object>(OnDataGridCopying);
            //DataGridCopyingCellClipboardContentCommand = new NotifyCommand<object>(OnDataGridCopyingCellClipboardContent);
            //DataGridPastingCommand= new NotifyCommand<object>(OnDataGridPastin);
            //DataGridPastingCellClipboardContentCommand = new NotifyCommand<object>(OnDataGridPastingCellClipboardContent);


            SaveCommand = new NotifyCommand(OnSave, CanSave)
                .ObservesProperty(() => SelectedDocument);
            CloseCommand = new NotifyCommand<object>(OnClose);
            UnDoCommand = new NotifyCommand(() => { UnDoReDo.UnDo(1); },
                                     () => { return UnDoReDo.CanUnDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            UnDoCommand.Name = "UnDoCommand";
            ReDoCommand = new NotifyCommand(() => UnDoReDo.ReDo(1),
               () => { return UnDoReDo.CanReDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            ReDoCommand.Name = "ReDoCommand";
            #region ContextMenu Commands

            CloseAggregationDocumentCommand = new NotifyCommand<object>(OnCloseAggregationDocument,
                                    (ob) => { return SelectedAggregationDocument != null; }).ObservesProperty(() => SelectedAggregationDocument);
            CloseAggregationDocumentCommand.Name = "Удалить";

            CreateNewMaterialCertificateCommand = new NotifyCommand<object>(OnCreateNewMaterialCertificate, (ob) =>
            SelectedAggregationDocument != null).ObservesProperty(() => SelectedAggregationDocument);

            CreateNewMaterialCertificateCommand.Name = "Создать новый документ";
            CreatedBasedOnMaterialCertificateCommand = new NotifyCommand<object>(OnCreatedBasedOnMaterialCertificate, (ob) =>
             { return SelectedDocument is bldMaterialCertificate && SelectedAggregationDocument != null; })
                .ObservesProperty(() => SelectedDocument).ObservesProperty(() => SelectedAggregationDocument);
            CreatedBasedOnMaterialCertificateCommand.Name = "Создать новый на основании..";

            RemoveMaterialCertificateCommand = new NotifyCommand<object>(OnRemoveMaterialCertificate, (ob) =>
            SelectedDocument is bldMaterialCertificate && SelectedAggregationDocument != null)
                .ObservesProperty(() => SelectedDocument).ObservesProperty(() => SelectedAggregationDocument); ;
            RemoveMaterialCertificateCommand.Name = "Удалить";

            CopyMaterialCertificateCommand = new NotifyCommand<object>(OnCopyMaterialCertificate, (ob) =>
                 SelectedDocument is bldMaterialCertificate && SelectedAggregationDocument != null).ObservesProperty(() => SelectedDocument);
            CopyMaterialCertificateCommand.Name = "Копировать";
            PasteMaterialCertificateCommand = new NotifyCommand<object>(OnPasteMaterialCertificate, (ob) =>
                 SelectedDocument is bldMaterialCertificate && SelectedAggregationDocument != null).ObservesProperty(() => SelectedDocument);
            PasteMaterialCertificateCommand.Name = "Вставить";

            AddMaterialCertificateToCommand = new NotifyCommand<object>(OnAddMaterialCertificateTo, (ob) => SelectedDocuments.Count > 0).ObservesProperty(() => SelectedDocument);
            AddMaterialCertificateToCommand.Name = "Добавить в... ";

            MoveMaterialCertificateToCommand = new NotifyCommand<object>(OnMoveMaterialCertificateTo, (ob) => SelectedDocuments.Count > 0).ObservesProperty(() => SelectedDocument);
            MoveMaterialCertificateToCommand.Name = "Переместить в... ";

            OpenImageFileCommand = new NotifyCommand<object>(OnOpenImageFile);
            SaveImageFileToDiskCommand = new NotifyCommand<object>(OnSaveImageFileToDisk);
            LoadImageFileFromDiskCommand = new NotifyCommand<object>(OnLoadImageFileFromDisk);

            #endregion

            ApplicationCommands.SaveAllCommand.RegisterCommand(SaveCommand);
            ApplicationCommands.ReDoCommand.RegisterCommand(ReDoCommand);
            ApplicationCommands.UnDoCommand.RegisterCommand(UnDoCommand);

            ApplicationCommands.CreateNewCommand.RegisterCommand(CreateNewMaterialCertificateCommand);
            ApplicationCommands.CreateBasedOnCommand.RegisterCommand(CreatedBasedOnMaterialCertificateCommand);
            ApplicationCommands.DeleteCommand.RegisterCommand(RemoveMaterialCertificateCommand);

            AllUnitsOfMeasurements = new ObservableCollection<bldUnitOfMeasurement>(_buildingUnitsRepository.UnitOfMeasurementRepository.GetAllAsync());

        }

        private void OnMoveMaterialCertificateTo(object obj)
        {
            bldDocument selected_certificate = SelectedDocument;
            bldAggregationDocument selected_aggr_document = SelectedAggregationDocument;

            bldAggregationDocumentsGroup All_AggregationDocuments = new bldAggregationDocumentsGroup(
                AggregationDocuments.Where(ad => ad.Id != SelectedAggregationDocument.Id).ToList());

            CoreFunctions.SelectElementFromCollectionWhithDialog<bldAggregationDocumentsGroup, bldAggregationDocument>
                      ((bldAggregationDocumentsGroup)All_AggregationDocuments, _dialogService, (result) =>
                      {
                          if (result.Result == ButtonResult.Yes)
                          {
                              bldAggregationDocument selected_aggregation_doc = result.Parameters.GetValue<bldAggregationDocument>("element");
                              UnDoReDoSystem loacl_unDoReDoSystem = new UnDoReDoSystem();
                              loacl_unDoReDoSystem.Register(selected_aggregation_doc);
                              loacl_unDoReDoSystem.Register(selected_aggr_document);

                              ObservableCollection<bldDocument> selected_docs = new ObservableCollection<bldDocument>(SelectedDocuments);
                              foreach (bldDocument document in selected_docs)
                                  if (!selected_aggregation_doc.AttachedDocuments.Contains(document))
                                  {
                                      selected_aggregation_doc.AttachedDocuments.Add(document);
                                      selected_aggr_document.AttachedDocuments.Remove(document);
                                  }
                              loacl_unDoReDoSystem.UnRegister(selected_aggregation_doc);
                              loacl_unDoReDoSystem.UnRegister(selected_aggr_document);
                              UnDoReDo.AddUnDoReDoSysAsCommand(loacl_unDoReDoSystem);
                              UnDoReDo.Register(selected_aggregation_doc);
                              UnDoReDo.Register(selected_aggr_document);

                          }
                      }, typeof(SelectAggregationDocumentFromCollectionDialogView).Name,
                      "Выберете документ для добавления в качестве приложения",
                         "Форма для добавления",
                         "Перечень каталогов");

        }

        private void OnAddMaterialCertificateTo(object obj)
        {
            bldDocument selected_certificate = SelectedDocument;
            bldAggregationDocument selected_aggr_document = SelectedAggregationDocument;

            bldAggregationDocumentsGroup All_AggregationDocuments = new bldAggregationDocumentsGroup(
                AggregationDocuments.Where(ad => ad.Id != SelectedAggregationDocument.Id).ToList());

            CoreFunctions.SelectElementFromCollectionWhithDialog<bldAggregationDocumentsGroup, bldAggregationDocument>
                      ((bldAggregationDocumentsGroup)All_AggregationDocuments, _dialogService, (result) =>
                      {
                          if (result.Result == ButtonResult.Yes)
                          {
                              bldAggregationDocument selected_aggregation_doc = result.Parameters.GetValue<bldAggregationDocument>("element");
                              UnDoReDoSystem loacl_unDoReDoSystem = new UnDoReDoSystem();
                              loacl_unDoReDoSystem.Register(selected_aggregation_doc);
                              ObservableCollection<bldDocument> selected_docs = new ObservableCollection<bldDocument>(SelectedDocuments);
                              foreach (bldDocument document in selected_docs)
                                  if (!selected_aggregation_doc.AttachedDocuments.Contains(document))
                                      selected_aggregation_doc.AttachedDocuments.Add(document);

                              loacl_unDoReDoSystem.UnRegister(selected_aggregation_doc);
                              UnDoReDo.AddUnDoReDoSysAsCommand(loacl_unDoReDoSystem);
                              UnDoReDo.Register(selected_aggregation_doc);


                          }
                      }, typeof(SelectAggregationDocumentFromCollectionDialogView).Name,
                      "Выберете документ для добавления в качестве приложения",
                         "Форма для добавления",
                         "Перечень каталогов");





        }

        private void OnPasteMaterialCertificate(object obj)
        {
            // bldMaterialCertificate selected_certificate = ((IList)obj)[0] as bldMaterialCertificate;
            // bldAggregationDocument selected_aggr_document = ((IList)obj)[1] as bldAggregationDocument;
            bldDocument selected_certificate = SelectedDocument;
            bldAggregationDocument selected_aggr_document = SelectedAggregationDocument;

            int insert_index = selected_aggr_document.AttachedDocuments.IndexOf(SelectedDocument) + 1;
            UnDoReDoSystem loacl_unDoReDoSystem = new UnDoReDoSystem();
            loacl_unDoReDoSystem.Register(selected_aggr_document);
            if (insert_index > selected_aggr_document.AttachedDocuments.Count)
                foreach (bldDocument document in MaterialCertificatesBuffer)
                    selected_aggr_document.AttachedDocuments.Add(document);
            else
                foreach (bldDocument document in MaterialCertificatesBuffer)
                    selected_aggr_document.AttachedDocuments.Insert(insert_index, document);
            loacl_unDoReDoSystem.UnRegister(selected_aggr_document);
            UnDoReDo.AddUnDoReDoSysAsCommand(loacl_unDoReDoSystem);
            UnDoReDo.Register(selected_aggr_document);
        }

        private void OnCopyMaterialCertificate(object obj)
        {
            // bldMaterialCertificate selected_certificate = ((IList)obj)[0] as bldMaterialCertificate;
            // bldAggregationDocument selected_aggr_document = ((IList)obj)[1] as bldAggregationDocument;
            bldDocument selected_certificate = SelectedDocument;
            bldAggregationDocument selected_aggr_document = SelectedAggregationDocument;

            //List<bldMaterialCertificate> selected_documents = new List<bldMaterialCertificate>();
            //foreach (bldMaterialCertificate certificae in ((IList)obj)[2] as IList)
            //    selected_documents.Add(certificae);

            ObservableCollection<bldDocument> selected_documents = SelectedDocuments;
            MaterialCertificatesBuffer.Clear();
            foreach (bldDocument document in selected_documents)
            {
                bldMaterialCertificate new_certificate = document.Clone() as bldMaterialCertificate;
                new_certificate.IsHaveImageFile = false;

                MaterialCertificatesBuffer.Add(new_certificate);
            }
        }

        private void OnCloseAggregationDocument(object obj)
        {
            bldAggregationDocument selected_AGDocument = SelectedAggregationDocument;

            int changes_number = selected_AGDocument.UnDoReDoSystem.GetChangesNamber(selected_AGDocument);
            int un_changed_to_DB_namber = selected_AGDocument.UnDoReDoSystem.GetUnChangesObjectsNamber(selected_AGDocument);
            if (changes_number > 0)
            {
                CoreFunctions.ConfirmActionOnElementDialog<bldAggregationDocument>(SelectedAggregationDocument, "Сохранить", SelectedAggregationDocument.Name, "Сохранить", "Не сохранять", "Отмена",
                    (result) =>
                {
                    if (result.Result == ButtonResult.Yes)
                    {
                        UnDoReDo.Save(selected_AGDocument);
                        AggregationDocuments.Remove(selected_AGDocument);
                        UnDoReDo.ParentUnDoReDo.Register(selected_AGDocument);
                    }
                    if (result.Result == ButtonResult.No)
                    {
                        UnDoReDo.UnDoAll(selected_AGDocument);
                        AggregationDocuments.Remove(selected_AGDocument);
                        UnDoReDo.ParentUnDoReDo.Register(selected_AGDocument);
                    }
                }
                 , _dialogService);
            }

            else
                AggregationDocuments.Remove(SelectedAggregationDocument);
            if (AggregationDocuments.Count > 0)
                SelectedAggregationDocument = AggregationDocuments[AggregationDocuments.Count - 1] as bldAggregationDocument;
        }


        #region Mouse methods
        NotifyMenuCommands context_menu_item_commands = null;

        private void OnContextMenuOpened(object obj)
        {

            ContextMenu contextMenu = ((IList)obj)[0] as ContextMenu;
            object clicked_document = ((IList)obj)[1];
            //   GridViewCell grid_cell = (GridViewCell)((IList)obj)[2];
            var selected_doc = (bldMaterialCertificate)clicked_document;
            //  SelectedDocument = selected_doc;
            IEnumerable items_sourse = contextMenu.ItemsSource;


            switch (clicked_document.GetType().Name)
            {
                case (nameof(bldMaterialCertificate)):
                    {

                        if (context_menu_item_commands?.Name != nameof(bldMaterialCertificate))
                        {
                            context_menu_item_commands = new NotifyMenuCommands()
                            {
                             CreateNewMaterialCertificateCommand,
                             CreatedBasedOnMaterialCertificateCommand,
                             CopyMaterialCertificateCommand,
                             PasteMaterialCertificateCommand,
                              AddMaterialCertificateToCommand,
                             MoveMaterialCertificateToCommand,
                               RemoveMaterialCertificateCommand,

                        };
                            context_menu_item_commands.Name = nameof(bldMaterialCertificate);
                            contextMenu.ItemsSource = context_menu_item_commands;
                        }
                        break;
                    }
                case (nameof(bldDocument)):
                case (nameof(bldAggregationDocument)):

                    {

                        break;

                    }
            }


        }
        #endregion


        private void OnDataGridSelectionChanged(object obj)
        {
            bldMaterialCertificate selected_certificate = ((IList)obj)[0] as bldMaterialCertificate;
            bldAggregationDocument selected_aggr_document = ((IList)obj)[1] as bldAggregationDocument;

            // if (_selectedDocument != selected_certificate) SelectedDocument = selected_certificate;
            if (_selectedAggregationDocument != selected_aggr_document) SelectedAggregationDocument = selected_aggr_document;

            SelectedDocuments.Clear();
            foreach (object elm in ((IList)obj)[2] as IList)
                SelectedDocuments.Add(elm as bldMaterialCertificate);


        }

        private void OnPastingCellClipboardContent(object obj)
        {
            GridViewCellClipboardEventArgs e = obj as GridViewCellClipboardEventArgs;
            string data = Clipboard.GetData(DataFormats.Text).ToString();
        }

        private void OnCopyingCellClipboardContent(object obj)
        {
            GridViewCellClipboardEventArgs e = obj as GridViewCellClipboardEventArgs;
            var val = e.Value;
            var itm = e.Cell.Item;
            var cl = e.Cell.Column;

        }

        private void OnCopyedCommand(object obj)
        {

        }

        private void OnCopying(object obj)
        {
            GridViewClipboardEventArgs e = obj as GridViewClipboardEventArgs;

        }

        private void OnLoadImageFileFromDisk(object document)
        {
            bldDocument selected_document = document as bldDocument;
            string image_file_name = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                image_file_name = openFileDialog.FileName;
            if (image_file_name != "")
                try
                {
                    using (FileStream fs = File.OpenRead(image_file_name))
                    {
                        byte[] buffer = new byte[fs.Length];
                        fs.ReadAsync(buffer, 0, buffer.Length);
                        selected_document.ImageFile = new Picture();
                        selected_document.ImageFile.Data = buffer;
                        selected_document.ImageFile.FileName = openFileDialog.SafeFileName;
                        selected_document.IsHaveImageFile = true;
                    }
                }
                catch
                {
                    throw new Exception("Не удается считать файл!");
                }

        }

        private void OnSaveImageFileToDisk(object document)
        {
            bldDocument selected_document = document as bldDocument;
            if (selected_document.ImageFile == null)
                selected_document = _buildingUnitsRepository.DocumentsRepository.MaterialCertificates.LoadPropertyObjects(selected_document.Id);

            CommonOpenFileDialog dialog = new CommonOpenFileDialog();

            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {

                string BD_FilesDir = dialog.FileName; ;

                if (!Directory.Exists(BD_FilesDir))
                    Directory.CreateDirectory(BD_FilesDir);
                string s = Path.Combine(BD_FilesDir, selected_document.ImageFile.FileName);

                using (System.IO.FileStream fs = new System.IO.FileStream(s, FileMode.OpenOrCreate))
                {
                    fs.Write(Functions.FormatPDFFromAccess(selected_document.ImageFile.Data));
                }
            }

        }

        private void OnOpenImageFile(object document)
        {
            bldDocument selected_document = document as bldDocument;
            string BD_FilesDir = Path.GetTempPath();

            if (!Directory.Exists(BD_FilesDir))
                Directory.CreateDirectory(BD_FilesDir);
            if (selected_document.ImageFile == null)
                selected_document = _buildingUnitsRepository.DocumentsRepository.MaterialCertificates.LoadPropertyObjects(selected_document.Id);
            string s = Path.Combine(BD_FilesDir, selected_document.ImageFile.FileName);

            using (System.IO.FileStream fs = new System.IO.FileStream(s, FileMode.OpenOrCreate))
            {
                fs.Write(Functions.FormatPDFFromAccess(selected_document.ImageFile.Data));
            }
            ProcessStartInfo info = new ProcessStartInfo(s);
            info.UseShellExecute = true;
            using (var proc = Process.Start(info)) { }
        }

        private void OnRemoveMaterialCertificate(object obj)
        {// bldMaterialCertificate selected_certificate = ((IList)obj)[0] as bldMaterialCertificate;
         // bldAggregationDocument selected_aggr_document = ((IList)obj)[1] as bldAggregationDocument;
            bldDocument selected_certificate = SelectedDocument;
            bldAggregationDocument selected_aggr_document = SelectedAggregationDocument;
            if (selected_certificate != null && selected_aggr_document != null)
                selected_aggr_document.AttachedDocuments.Remove(selected_certificate);
        }

        private void OnCreatedBasedOnMaterialCertificate(object obj)
        {
            // bldMaterialCertificate selected_certificate = ((IList)obj)[0] as bldMaterialCertificate;
            // bldAggregationDocument selected_aggr_document = ((IList)obj)[1] as bldAggregationDocument;
            bldDocument selected_certificate = SelectedDocument;
            bldAggregationDocument selected_aggr_document = SelectedAggregationDocument;

            bldMaterialCertificate new_certificate = SelectedDocument.Clone() as bldMaterialCertificate;
            new_certificate.IsHaveImageFile = false;
            if (!selected_aggr_document.IsDbBranch)
            {
                _buildingUnitsRepository.DocumentsRepository.MaterialCertificates.Add(new_certificate);
                new_certificate.IsDbBranch = true;
            }
            if (selected_certificate != null && selected_aggr_document != null)
                selected_aggr_document.AttachedDocuments.Add(new_certificate);
        }


        private void OnCreateNewMaterialCertificate(object obj)
        {
            // bldMaterialCertificate selected_certificate = ((IList)obj)[0] as bldMaterialCertificate;
            // bldAggregationDocument selected_aggr_document = ((IList)obj)[1] as bldAggregationDocument;
            bldDocument selected_certificate = SelectedDocument;
            bldAggregationDocument selected_aggr_document = SelectedAggregationDocument;

            bldMaterialCertificate new_certificate = new bldMaterialCertificate();
            if (!selected_aggr_document.IsDbBranch)
            {
                _buildingUnitsRepository.DocumentsRepository.MaterialCertificates.Add(new_certificate);
                new_certificate.IsDbBranch = true;
            }
            if (selected_aggr_document != null)
                selected_aggr_document.AttachedDocuments.Add(new_certificate);

        }


        #region  Commmands Methods
        private void OnRemoveUnitOfMeasurement(object obj)
        {

            bldMaterialCertificate selected_certificate = SelectedDocument as bldMaterialCertificate;
            // UnDoReDo.Register(selected_certificate);
            //selected_certificate.UnitOfMeasurement = new bldUnitOfMeasurement("-");

        }

        private void OnSelectUnitOfMeasurement(object obj)
        {
            bldMaterialCertificate selected_certificate = obj as bldMaterialCertificate;
            ObservableCollection<bldUnitOfMeasurement> All_UnitOfMeasurements =
                 new ObservableCollection<bldUnitOfMeasurement>(_buildingUnitsRepository.UnitOfMeasurementRepository.GetAllUnits());
            NameablePredicate<ObservableCollection<bldUnitOfMeasurement>, bldUnitOfMeasurement> predicate_1 = new NameablePredicate<ObservableCollection<bldUnitOfMeasurement>, bldUnitOfMeasurement>();
            predicate_1.Name = "Все единицы измеререния";
            predicate_1.Predicate = (col) => col;
            NameablePredicateObservableCollection<ObservableCollection<bldUnitOfMeasurement>, bldUnitOfMeasurement> predicatesCollection = new NameablePredicateObservableCollection<ObservableCollection<bldUnitOfMeasurement>, bldUnitOfMeasurement>();
            predicatesCollection.Add(predicate_1);
            ObservableCollection<bldUnitOfMeasurement> units_for_add_collection = new ObservableCollection<bldUnitOfMeasurement>();
            CoreFunctions.AddElementsToCollectionWhithDialogList<ObservableCollection<bldUnitOfMeasurement>, bldUnitOfMeasurement>
             (units_for_add_collection, All_UnitOfMeasurements,
              predicatesCollection,
             _dialogService,
              (result) =>
              {
                  if (result.Result == ButtonResult.Yes)
                  {
                      foreach (bldUnitOfMeasurement unit_of_measurement in units_for_add_collection)
                      {
                          selected_certificate.UnitOfMeasurement = unit_of_measurement;
                          break;
                      }
                      SaveCommand.RaiseCanExecuteChanged();
                  }
                  if (result.Result == ButtonResult.No)
                  {
                  }
              },
             typeof(AddUnitOfMeasurementToCollectionFromListDialogView).Name,
              "Выбрать единицу измерения",
              "Форма для выбора единицы измерения.",
              "Список един измерения", "");
        }
        #endregion
        private void OnDataGridLostFocus(object obj)
        {
            //SelectedDocuments.Clear();
            //SelectedDocument = null;
            //SelectedAggregationDocument = null;

        }

        public void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }
        private bool CanSave()
        {
            if (AggregationDocuments != null)
                return !AggregationDocuments.HasErrors;
            else
                return false;
        }
        public virtual void OnSave()
        {
            base.OnSave<bldDocumentsGroup>(AggregationDocuments,
                 (result) =>
                 {
                     if (result.Result == ButtonResult.Yes)
                     {
                         UnDoReDo.ParentUnDoReDo.Save(AggregationDocuments);
                     }

                 });
        }
        public virtual void OnClose(object obj)
        {
            base.OnClose<bldDocumentsGroup>(obj, AggregationDocuments,
                (result) =>
                {
                    if (result.Result == ButtonResult.Yes)
                    {
                        UnDoReDo.Save(AggregationDocuments);
                        //    UnDoReDo.ParentUnDoReDo?.UnSetUnDoReDoSystemAsChildren(UnDoReDo);
                    }
                    if (result.Result == ButtonResult.No)
                    {
                        UnDoReDo.UnDoAll(AggregationDocuments);
                    }
                });

        }
        public override void OnWindowClose()
        {
            ApplicationCommands.SaveAllCommand.UnregisterCommand(SaveCommand);
            ApplicationCommands.ReDoCommand.UnregisterCommand(ReDoCommand);
            ApplicationCommands.UnDoCommand.UnregisterCommand(UnDoCommand);
            ApplicationCommands.CreateNewCommand.RegisterCommand(CreatedBasedOnMaterialCertificateCommand);
            ApplicationCommands.CreateBasedOnCommand.RegisterCommand(CreatedBasedOnMaterialCertificateCommand);
            ApplicationCommands.DeleteCommand.RegisterCommand(RemoveMaterialCertificateCommand);
            UnDoReDo.ParentUnDoReDo?.UnSetUnDoReDoSystemAsChildren(UnDoReDo);
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_agrregation_document"];
            if (navigane_message != null)
            {
                bldAggregationDocument arg_document = (bldAggregationDocument)navigane_message.Object;

                EditMode = navigane_message.EditMode;
                if (AggregationDocuments.Where(ad => ad.Id == arg_document.Id).FirstOrDefault() == null)
                {

                    AggregationDocuments.Add(arg_document);
                    arg_document.UnDoReDoSystem.SetUnDoReDoSystemAsChildren(UnDoReDo);
                    UnDoReDo.Register(arg_document);
                    if (SelectedAggregationDocument == null) SelectedAggregationDocument = arg_document;
                }
                if (AggregationDocuments != null) AggregationDocuments.ErrorsChanged -= RaiseCanExecuteChanged;
                AggregationDocuments.ErrorsChanged += RaiseCanExecuteChanged;
                Title = $"Сертификаты/Паспорта";
            }
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            //ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_agrregation_document"];
            //bldAggregationDocument arg_document = (bldAggregationDocument)navigane_message.Object;
            //if (AggregationDocuments.Where(ad => ad.Id == arg_document.Id).FirstOrDefault() == null)
            //{
            //    return false;
            //}
            //else
            //    return true;

            return true;
        }
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}
