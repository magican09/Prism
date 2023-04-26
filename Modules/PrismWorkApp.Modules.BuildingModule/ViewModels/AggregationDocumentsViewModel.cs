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
        private bldDocument _selectedAggregationDocument;

        public bldDocument SelectedAggregationDocument
        {
            get { return _selectedAggregationDocument; }
            set { SetProperty(ref _selectedAggregationDocument, value); }
        }

        public NotifyCommand<object> ContextMenuOpenedCommand { get; private set; }

        public NotifyCommand<object> DataGridSelectionChangedCommand { get; private set; }

        public NotifyCommand UnDoCommand { get; protected set; }
        public NotifyCommand ReDoCommand { get; protected set; }
        public NotifyCommand SaveCommand { get; protected set; }
        public NotifyCommand<object> CloseCommand { get; protected set; }

        public NotifyCommand<object> CloseAggregationDocumentCommand { get; private set; }

        public NotifyCommand<object> CreateNewDocumentCommand { get; private set; }
        public NotifyCommand<object> CreatedBasedOnCommand { get; private set; }
        public NotifyCommand<object> RemoveDocumentsCommand { get; private set; }
        public NotifyCommand<object> CopyDocumentsCommand { get; private set; }
        public NotifyCommand<object> PasteDocumentsCommand { get; private set; }
        public ObservableCollection<bldDocument> DocumentsBuffer = new ObservableCollection<bldDocument>();
        public NotifyCommand<object> AddDocumentsToCommand { get; private set; }
        public NotifyCommand<object> MoveDocumentsToCommand { get; private set; }

        public NotifyCommand<object> AddNewAggregationDocumentCommand { get; private set; }
        public NotifyCommand<object> AddNewMaterialCertificateCommand { get; private set; }
        public NotifyCommand<object> AddNewLaboratoryReportCommand { get; private set; }
        public NotifyCommand<object> AddNewExecutiveSchemeCommand { get; private set; }

        public NotifyCommand<object> OpenImageFileCommand { get; private set; }
        public NotifyCommand<object> SaveImageFileToDiskCommand { get; private set; }
        public NotifyCommand<object> LoadImageFileFromDiskCommand { get; private set; }

        public ObservableCollection<MenuItem> CommonContextMenuItems { get; set; }


        AppObjectsModel AppObjectsModel { get; set; }

        public IBuildingUnitsRepository _buildingUnitsRepository { get; }
        AppObjectsModel _appObjectsModel;
        IUnDoReDoSystem _parentUnDoReDoSystem;
        public AggregationDocumentsViewModel(IDialogService dialogService,
           IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository, IApplicationCommands applicationCommands, IAppObjectsModel appObjectsModel)
        {

            UnDoReDo = new UnDoReDoSystem(this);
            ApplicationCommands = applicationCommands;
            AppObjectsModel = appObjectsModel as AppObjectsModel;
            _dialogService = dialogService;
            _buildingUnitsRepository = buildingUnitsRepository;
            _regionManager = regionManager;

           
            ContextMenuOpenedCommand = new NotifyCommand<object>(OnContextMenuOpened);
            DataGridSelectionChangedCommand = new NotifyCommand<object>(OnDataGridSelectionChanged);

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

            CreateNewDocumentCommand = new NotifyCommand<object>(OnCreateNewDocument, (ob) =>
            { return SelectedDocument != null && SelectedAggregationDocument != null; })
                .ObservesProperty(() => SelectedDocument).ObservesProperty(() => SelectedAggregationDocument);
            CreateNewDocumentCommand.Name = "Создать новый ...";

            CreatedBasedOnCommand = new NotifyCommand<object>(OnCreatedBasedOn, (ob) =>
             { return SelectedDocument != null && SelectedAggregationDocument != null; })
                .ObservesProperty(() => SelectedDocument).ObservesProperty(() => SelectedAggregationDocument);
            CreatedBasedOnCommand.Name = "Создать новый на основании..";

            RemoveDocumentsCommand = new NotifyCommand<object>(OnRemoveDocuments, (ob) =>
            SelectedDocument != null && SelectedAggregationDocument != null)
                .ObservesProperty(() => SelectedDocument).ObservesProperty(() => SelectedAggregationDocument); ;
            RemoveDocumentsCommand.Name = "Удалить";

            CopyDocumentsCommand = new NotifyCommand<object>(OnCopyDocuments, (ob) =>
                 SelectedDocument != null && SelectedAggregationDocument != null).ObservesProperty(() => SelectedDocument);
            CopyDocumentsCommand.Name = "Копировать";

            PasteDocumentsCommand = new NotifyCommand<object>(OnPasteDocuments, (ob) =>
                 SelectedDocument != null && SelectedAggregationDocument != null).ObservesProperty(() => SelectedDocument);
            PasteDocumentsCommand.Name = "Вставить";

            AddDocumentsToCommand = new NotifyCommand<object>(OnAddDocumentTo, (ob) => SelectedDocuments.Count > 0).ObservesProperty(() => SelectedDocument);
            AddDocumentsToCommand.Name = "Добавить в... ";

            MoveDocumentsToCommand = new NotifyCommand<object>(OnMoveDocumentTo, (ob) => SelectedDocuments.Count > 0).ObservesProperty(() => SelectedDocument);
            MoveDocumentsToCommand.Name = "Переместить в... ";

            AddNewAggregationDocumentCommand = new NotifyCommand<object>(OnCreateNewAggregationDocument, (ob) =>
             SelectedAggregationDocument != null).ObservesProperty(() => SelectedAggregationDocument);

            AddNewMaterialCertificateCommand = new NotifyCommand<object>(OnAddNewMaterialCertificate, (ob) =>
             SelectedAggregationDocument != null).ObservesProperty(() => SelectedAggregationDocument); ;

            AddNewLaboratoryReportCommand = new NotifyCommand<object>(OnAddNewLaboratoryReport, (ob) =>
           SelectedAggregationDocument != null).ObservesProperty(() => SelectedAggregationDocument); ;

            AddNewExecutiveSchemeCommand = new NotifyCommand<object>(OnAddNewExecutiveScheme, (ob) =>
           SelectedAggregationDocument != null).ObservesProperty(() => SelectedAggregationDocument); ;


            OpenImageFileCommand = new NotifyCommand<object>(OnOpenImageFile);
            SaveImageFileToDiskCommand = new NotifyCommand<object>(OnSaveImageFileToDisk);
            LoadImageFileFromDiskCommand = new NotifyCommand<object>(OnLoadImageFileFromDisk);

            #endregion
            ApplicationCommands.SaveAllCommand.RegisterCommand(SaveCommand);
            ApplicationCommands.ReDoCommand.RegisterCommand(ReDoCommand);
            ApplicationCommands.UnDoCommand.RegisterCommand(UnDoCommand);

            ApplicationCommands.CreateNewCommand.RegisterCommand(CreateNewDocumentCommand);
            ApplicationCommands.CreateBasedOnCommand.RegisterCommand(CreatedBasedOnCommand);
            ApplicationCommands.DeleteCommand.RegisterCommand(RemoveDocumentsCommand);

            ApplicationCommands.AddNewAggregationDocumentCommand.RegisterCommand(AddNewAggregationDocumentCommand);
            ApplicationCommands.AddNewMaterialCertificateCommand.RegisterCommand(AddNewMaterialCertificateCommand);
            ApplicationCommands.AddNewLaboratoryReportCommand.RegisterCommand(AddNewLaboratoryReportCommand);
            ApplicationCommands.AddNewExecutiveSchemeCommand.RegisterCommand(AddNewExecutiveSchemeCommand);

            AllUnitsOfMeasurements = new ObservableCollection<bldUnitOfMeasurement>(_buildingUnitsRepository.UnitOfMeasurementRepository.GetAllAsync());

        }

       

        private void OnAddNewExecutiveScheme(object obj)
        {
            AppObjectsModel.AddNewExecutiveSchemeCommand.Execute(SelectedAggregationDocument.AttachedDocuments);
        }

        private void OnAddNewLaboratoryReport(object obj)
        {
            AppObjectsModel.AddNewLaboratoryReportCommand.Execute(SelectedAggregationDocument.AttachedDocuments);
        }

        private void OnAddNewMaterialCertificate(object obj)
        {
            AppObjectsModel.AddNewMaterialCertificateCommand.Execute(SelectedAggregationDocument.AttachedDocuments);
        }

        private void OnCreateNewAggregationDocument(object obj)
        {
            AppObjectsModel.AddNewAggregationDocumentCommand.Execute(SelectedAggregationDocument.AttachedDocuments);
        }

        private void OnMoveDocumentTo(object obj)
        {
            bldDocument selected_aggr_document = SelectedAggregationDocument;

            bldAggregationDocumentsGroup All_AggregationDocuments = new bldAggregationDocumentsGroup(
                AggregationDocuments.Where(ad => ad.Id != SelectedAggregationDocument.Id).ToList());

            CoreFunctions.SelectElementFromCollectionWhithDialog<bldAggregationDocumentsGroup, bldAggregationDocument>
                      ((bldAggregationDocumentsGroup)All_AggregationDocuments, _dialogService, (result) =>
                      {
                          if (result.Result == ButtonResult.Yes)
                          {
                              bldAggregationDocument for_added_aggr_doc = result.Parameters.GetValue<bldAggregationDocument>("element");
                              UnDoReDoSystem loacl_unDoReDoSystem = new UnDoReDoSystem();
                              loacl_unDoReDoSystem.Register(for_added_aggr_doc);
                              loacl_unDoReDoSystem.Register(selected_aggr_document);

                              ObservableCollection<bldDocument> selected_docs = new ObservableCollection<bldDocument>(SelectedDocuments);
                              foreach (bldDocument document in selected_docs)
                                  if (!for_added_aggr_doc.AttachedDocuments.Contains(document))
                                  {
                                      for_added_aggr_doc.AttachedDocuments.Add(document);
                                      selected_aggr_document.AttachedDocuments.Remove(document);
                                  }
                              loacl_unDoReDoSystem.UnRegister(for_added_aggr_doc);
                              loacl_unDoReDoSystem.UnRegister(selected_aggr_document);
                              UnDoReDo.AddUnDoReDoSysAsCommand(loacl_unDoReDoSystem);
                              UnDoReDo.Register(for_added_aggr_doc);
                              UnDoReDo.Register(selected_aggr_document);

                          }
                      }, typeof(SelectAggregationDocumentFromCollectionDialogView).Name,
                      "Выберете документ для добавления в качестве приложения",
                         "Форма для добавления",
                         "Перечень каталогов");

        }

        private void OnAddDocumentTo(object obj)
        {
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

        private void OnPasteDocuments(object obj)
        {
            int insert_index = SelectedAggregationDocument.AttachedDocuments.IndexOf(SelectedDocument) + 1;
            UnDoReDoSystem loacl_unDoReDoSystem = new UnDoReDoSystem();
            loacl_unDoReDoSystem.Register(SelectedAggregationDocument);
            if (insert_index > SelectedAggregationDocument.AttachedDocuments.Count)
                foreach (bldDocument document in DocumentsBuffer)
                    SelectedAggregationDocument.AttachedDocuments.Add(document);
            else
                foreach (bldDocument document in DocumentsBuffer)
                    SelectedAggregationDocument.AttachedDocuments.Insert(insert_index, document);
            loacl_unDoReDoSystem.UnRegister(SelectedAggregationDocument);
            UnDoReDo.AddUnDoReDoSysAsCommand(loacl_unDoReDoSystem);
            UnDoReDo.Register(SelectedAggregationDocument);
        }

        private void OnCopyDocuments(object obj)
        {
            DocumentsBuffer.Clear();
            foreach (bldDocument document in SelectedDocuments)
            {
                bldDocument new_certificate = document.Clone() as bldDocument;
                new_certificate.IsHaveImageFile = false;
                DocumentsBuffer.Add(new_certificate);
            }
        }

        private void OnCloseAggregationDocument(object obj)
        {
            bldDocument selected_AGDocument = SelectedAggregationDocument;

            int changes_number = selected_AGDocument.UnDoReDoSystem.GetChangesNamber(selected_AGDocument);
            int un_changed_to_DB_namber = selected_AGDocument.UnDoReDoSystem.GetUnChangesObjectsNamber(selected_AGDocument);
            if (changes_number > 0)
            {
                CoreFunctions.ConfirmActionOnElementDialog<bldDocument>(SelectedAggregationDocument, "Сохранить", SelectedAggregationDocument.Name, "Сохранить", "Не сохранять", "Отмена",
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
            IEnumerable items_sourse = contextMenu.ItemsSource;
            switch (clicked_document.GetType().Name)
            {
                case (nameof(bldMaterialCertificate)):
                case (nameof(bldLaboratoryReport)):
                case (nameof(bldExecutiveScheme)):
                    {
                        if (context_menu_item_commands?.Name != nameof(bldDocument))
                        {
                            context_menu_item_commands = new NotifyMenuCommands()
                            {
                             CreateNewDocumentCommand,
                             CreatedBasedOnCommand,
                             CopyDocumentsCommand,
                             PasteDocumentsCommand,
                             AddDocumentsToCommand,
                             MoveDocumentsToCommand,
                             RemoveDocumentsCommand
                            };
                            context_menu_item_commands.Name = nameof(bldDocument);
                            contextMenu.ItemsSource = context_menu_item_commands;
                        }
                        break;
                    }
                case (nameof(bldAggregationDocument)):
                    {
                        if (context_menu_item_commands?.Name != nameof(bldAggregationDocument))
                        {
                            context_menu_item_commands = new NotifyMenuCommands()
                            {
                             CreateNewDocumentCommand,
                             CreatedBasedOnCommand,
                             CopyDocumentsCommand,
                             PasteDocumentsCommand,
                             AddDocumentsToCommand,
                             MoveDocumentsToCommand,
                             RemoveDocumentsCommand
                            };
                            context_menu_item_commands.Name = nameof(bldAggregationDocument);
                            contextMenu.ItemsSource = context_menu_item_commands;
                        }
                        break;
                    }
            }
        }
        #endregion
        private void OnDataGridSelectionChanged(object obj)
        {
            bldDocument selected_certificate = ((IList)obj)[0] as bldDocument;
            bldDocument selected_aggr_document = ((IList)obj)[1] as bldDocument;

            if (_selectedAggregationDocument != selected_aggr_document) SelectedAggregationDocument = selected_aggr_document;

            SelectedDocuments.Clear();
            foreach (object elm in ((IList)obj)[2] as IList)
                SelectedDocuments.Add(elm as bldDocument);
        }

        private void OnPastingCellClipboardContent(object obj)
        {
            GridViewCellClipboardEventArgs e = obj as GridViewCellClipboardEventArgs;
            string data = Clipboard.GetData(DataFormats.Text).ToString();
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

        private void OnRemoveDocuments(object obj)
        {

            foreach (bldDocument document in new ObservableCollection<bldDocument>(SelectedDocuments))
                SelectedAggregationDocument.AttachedDocuments.Remove(document);
        }

        private void OnCreatedBasedOn(object obj)
        {
            bldDocument new_document = SelectedDocument.Clone() as bldDocument;
            new_document.IsHaveImageFile = false;
            SelectedAggregationDocument.AttachedDocuments.Add(new_document);
            //   SelectedAggregationDocument.AddCreatedBasedOnNewDocument(SelectedDocument);
        }

        private void OnCreateNewDocument(object obj)
        {
            bldDocument new_document = (bldDocument)Activator.CreateInstance(SelectedDocument.GetType());
            SelectedAggregationDocument.AttachedDocuments.Add(new_document);
        }

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
         if(UnDoReDo.ChangedObjects.Count>0)
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
                        UnDoReDo.ClearStacks();
                    }
                });

        }
        public override void OnWindowClose()
        {
            ApplicationCommands.SaveAllCommand.UnregisterCommand(SaveCommand);
            ApplicationCommands.ReDoCommand.UnregisterCommand(ReDoCommand);
            ApplicationCommands.UnDoCommand.UnregisterCommand(UnDoCommand);
            ApplicationCommands.CreateBasedOnCommand.UnregisterCommand(CreateNewDocumentCommand);
            ApplicationCommands.CreateBasedOnCommand.UnregisterCommand(CreatedBasedOnCommand);
            ApplicationCommands.DeleteCommand.UnregisterCommand(RemoveDocumentsCommand);
            ApplicationCommands.AddNewAggregationDocumentCommand.UnregisterCommand(AddNewAggregationDocumentCommand);
            ApplicationCommands.AddNewMaterialCertificateCommand.UnregisterCommand(AddNewMaterialCertificateCommand);
            ApplicationCommands.AddNewLaboratoryReportCommand.UnregisterCommand(AddNewLaboratoryReportCommand);
            ApplicationCommands.AddNewExecutiveSchemeCommand.UnregisterCommand(AddNewExecutiveSchemeCommand);

            UnDoReDo.ParentUnDoReDo?.UnSetUnDoReDoSystemAsChildren(UnDoReDo);
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_agrregation_document"];
            if (navigationContext.Parameters["title"] != null) Title = navigationContext.Parameters["title"].ToString();
            if (navigane_message != null)
            {
                bldAggregationDocument arg_document = (bldAggregationDocument)navigane_message.Object;

                EditMode = navigane_message.EditMode;
                if (AggregationDocuments.Where(ad => ad.StoredId == arg_document.StoredId).FirstOrDefault() == null)
                {

                    AggregationDocuments.Add(arg_document);
                    //  if(UnDoReDo.ParentUnDoReDo==null)
                    arg_document.UnDoReDoSystem.SetUnDoReDoSystemAsChildren(UnDoReDo);
                    UnDoReDo.Register(arg_document);
                    if (SelectedAggregationDocument == null) SelectedAggregationDocument = arg_document;
                }
                if (AggregationDocuments != null) AggregationDocuments.ErrorsChanged -= RaiseCanExecuteChanged;
                AggregationDocuments.ErrorsChanged += RaiseCanExecuteChanged;
               
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

         //   this.OnSave();
            
        }
    }
}
