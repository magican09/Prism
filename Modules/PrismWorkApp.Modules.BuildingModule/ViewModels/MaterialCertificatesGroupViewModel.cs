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
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class MaterialCertificatesGroupViewModel : BaseViewModel<bldMaterialCertificate>, INotifyPropertyChanged, INavigationAware
    {
        private string _title = "Список сертификатов";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private DataGridColumn _selectedGridColumn;
        public DataGridColumn SelectedGridColumn
        {
            get { return _selectedGridColumn; }
            set { SetProperty(ref _selectedGridColumn, value); }
        }

        private bldMaterialCertificate _selectedDocument;
        public bldMaterialCertificate SelectedDocument
        {
            get { return _selectedDocument; }
            set { SetProperty(ref _selectedDocument, value); }
        }
        private bldDocumentsGroup _selectedDocumentsGroup;
        public bldDocumentsGroup SelectedDocumentsGroup
        {
            get { return _selectedDocumentsGroup; }
            set { SetProperty(ref _selectedDocumentsGroup, value); }
        }
        private ObservableCollection<bldMaterialCertificate> _selectedDocuments = new ObservableCollection<bldMaterialCertificate>();
        public ObservableCollection<bldMaterialCertificate> SelectedDocuments
        {
            get { return _selectedDocuments; }
            set { SetProperty(ref _selectedDocuments, value); }
        }
        private bool _filterEnable = false;
        public bool FilterEnable
        {
            get { return _filterEnable; }
            set { SetProperty(ref _filterEnable, value); }
        }
        private ObservableCollection<bldUnitOfMeasurement> _allUnitsOfMeasurements;
        public ObservableCollection<bldUnitOfMeasurement> AllUnitsOfMeasurements
        {
            get { return _allUnitsOfMeasurements; }
            set { SetProperty(ref _allUnitsOfMeasurements, value); }
        }
        //private NameableObservableCollection<bldDocument> _filteredCommonPointersCollection = new NameableObservableCollection<bldDocument>();
        //public NameableObservableCollection<bldDocument> FilteredCommonPointersCollection
        //{
        //    get { return _filteredCommonPointersCollection; }
        //    set { SetProperty(ref _filteredCommonPointersCollection, value); }
        //}
        //private NameableObservableCollection<bldDocument> _sortedCommonPointersCollection = new NameableObservableCollection<bldDocument>();
        //public NameableObservableCollection<bldDocument> SortedCommonPointersCollection
        //{
        //    get { return _sortedCommonPointersCollection; }
        //    set { SetProperty(ref _sortedCommonPointersCollection, value); }
        //}

        public NotifyCommand<object> DataGridLostFocusCommand { get; private set; }
        public NotifyCommand<object> DataGridSelectionChangedCommand { get; private set; }
        public NotifyCommand UnDoCommand { get; protected set; }
        public NotifyCommand ReDoCommand { get; protected set; }
        public NotifyCommand SaveCommand { get; protected set; }
        public NotifyCommand<object> CloseCommand { get; protected set; }

        public ObservableCollection<INotifyCommand> CommonCommands { get; set; } = new ObservableCollection<INotifyCommand>();
        public NotifyCommand CreateNewCommand { get; private set; }
        public NotifyCommand<object> CreatedBasedOnCommand { get; private set; }

        public ObservableCollection<INotifyCommand> UnitsOfMeasurementContextMenuCommands { get; set; } = new ObservableCollection<INotifyCommand>();
        public NotifyCommand<object> SelectUnitOfMeasurementCommand { get; private set; }
        public NotifyCommand<object> RemoveUnitOfMeasurementCommand { get; private set; }


        public NotifyCommand<object> OpenImageFileCommand { get; private set; }
        public NotifyCommand<object> SaveImageFileToDiskCommand { get; private set; }
        public NotifyCommand<object> LoadImageFileFromDiskCommand { get; private set; }

        public ObservableCollection<MenuItem> CommonContextMenuItems { get; set; }

        public NotifyCommand<object> CopyingCommand { get; private set; }
        public NotifyCommand<object> CopyingCellClipboardContentCommand { get; private set; }
        public NotifyCommand<object> CopyedCommand { get; private set; }

        public NotifyCommand<object> PastingCellClipboardContentCommand { get; private set; }
        public NotifyCommand<object> ContextMenuOpeningCommand { get; private set; }

        public IBuildingUnitsRepository _buildingUnitsRepository { get; }

        public MaterialCertificatesGroupViewModel(IDialogService dialogService,
           IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository, IApplicationCommands applicationCommands)
        {

            UnDoReDo = new UnDoReDoSystem();
            ApplicationCommands = applicationCommands;
            _dialogService = dialogService;
            _buildingUnitsRepository = buildingUnitsRepository;
            _regionManager = regionManager;
            DataGridSelectionChangedCommand = new NotifyCommand<object>(OnDataGridSelectionChanged);
            DataGridLostFocusCommand = new NotifyCommand<object>(OnDataGridLostFocus);

            SaveCommand = new NotifyCommand(OnSave, CanSave)
                .ObservesProperty(() => SelectedDocument);
            CloseCommand = new NotifyCommand<object>(OnClose);
            UnDoCommand = new NotifyCommand(() => { UnDoReDo.UnDo(1); },
                                     () => { return UnDoReDo.CanUnDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            ReDoCommand = new NotifyCommand(() => UnDoReDo.ReDo(1),
               () => { return UnDoReDo.CanReDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);

            #region ContextMenu Commands

            CreateNewCommand = new NotifyCommand(OnCreateNewMaterialCertificate);
            CreateNewCommand.Name = "Создать новый документ";
            CreatedBasedOnCommand = new NotifyCommand<object>(OnCreatedBasedOn,
                                    (ob) => { return SelectedDocument != null; }).ObservesProperty(() => SelectedDocument);
            CreatedBasedOnCommand.Name = "Создать новый на основании..";
            CommonCommands.Add(CreateNewCommand);
            CommonCommands.Add(CreatedBasedOnCommand);

            SelectUnitOfMeasurementCommand = new NotifyCommand<object>(OnSelectUnitOfMeasurement);
            SelectUnitOfMeasurementCommand.Name = "Установить";
            RemoveUnitOfMeasurementCommand = new NotifyCommand<object>(OnRemoveUnitOfMeasurement);
            RemoveUnitOfMeasurementCommand.Name = "Удалить";
            UnitsOfMeasurementContextMenuCommands.Add(SelectUnitOfMeasurementCommand);
            UnitsOfMeasurementContextMenuCommands.Add(RemoveUnitOfMeasurementCommand);

            OpenImageFileCommand = new NotifyCommand<object>(OnOpenImageFile);
            SaveImageFileToDiskCommand = new NotifyCommand<object>(OnSaveImageFileToDisk);
            LoadImageFileFromDiskCommand = new NotifyCommand<object>(OnLoadImageFileFromDisk);

            CopyingCommand = new NotifyCommand<object>(OnCopying);
            CopyingCellClipboardContentCommand = new NotifyCommand<object>(OnCopyingCellClipboardContent);
            PastingCellClipboardContentCommand = new NotifyCommand<object>(OnPastingCellClipboardContent);
            CopyedCommand = new NotifyCommand<object>(OnCopyedCommand);
            ContextMenuOpeningCommand = new NotifyCommand<object>(OnContextMenuOpening);

            CommonContextMenuItems = new ObservableCollection<MenuItem>();
            MenuItem addItem = new MenuItem();
            addItem.Text = "Add";
            CommonContextMenuItems.Add(addItem);
            MenuItem editItem = new MenuItem();
            editItem.Text = "Edit";
            CommonContextMenuItems.Add(editItem);
            MenuItem deleteItem = new MenuItem();
            deleteItem.Text = "Delete";
            CommonContextMenuItems.Add(deleteItem);

            #endregion
            ApplicationCommands.SaveAllCommand.RegisterCommand(SaveCommand);
            ApplicationCommands.ReDoCommand.RegisterCommand(ReDoCommand);
            ApplicationCommands.UnDoCommand.RegisterCommand(UnDoCommand);

            ApplicationCommands.CreateNewCommand.RegisterCommand(CreateNewCommand);
            ApplicationCommands.CreateBasedOnCommand.RegisterCommand(CreatedBasedOnCommand);

            AllUnitsOfMeasurements = new ObservableCollection<bldUnitOfMeasurement>(_buildingUnitsRepository.UnitOfMeasurementRepository.GetAllAsync());


        }

        private void OnContextMenuOpening(object obj)
        {
            List<object> grid_state_objects = obj as List<object>;
            SelectedDocument = grid_state_objects[0] as bldMaterialCertificate;
            //   SelectedDocuments = (ObservableCollection<bldMaterialCertificate>) grid_state_objects[1];
            SelectedDocuments.Clear();
            var selected_items = (ObservableCollection<object>)grid_state_objects[1];
            foreach (object elm in selected_items)
                SelectedDocuments.Add(obj as bldMaterialCertificate);
            //    ContextMenu contextMenu   = obj as ContextMenu;
            //GridViewCell clicked_cell = contextMenu.GetClickedElement<GridViewCell>();
            //GridViewRow clicked_row = contextMenu.GetClickedElement<GridViewRow>();

            //if (clicked_cell != null)
            //{
            //    bldMaterialCertificate clicked_document = clicked_cell.DataContext as bldMaterialCertificate;
            //    var d = clicked_cell.Value;
            //}
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
            bldMaterialCertificate selected_certificate = document as bldMaterialCertificate;
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
                        selected_certificate.ImageFile = new Picture();
                        selected_certificate.ImageFile.FileData.Data = buffer;
                        selected_certificate.ImageFile.FileName = openFileDialog.SafeFileName;
                    }
                }
                catch
                {
                    throw new Exception("Не удается считать файл!");
                }
        }

        private void OnSaveImageFileToDisk(object document)
        {
            bldMaterialCertificate selected_certificate = document as bldMaterialCertificate;
            selected_certificate = _buildingUnitsRepository.DocumentsRepository.MaterialCertificates.LoadPropertyObjects(selected_certificate.Id);

            CommonOpenFileDialog dialog = new CommonOpenFileDialog();

            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {

                string BD_FilesDir = dialog.FileName; ;

                if (!Directory.Exists(BD_FilesDir))
                    Directory.CreateDirectory(BD_FilesDir);
                string s = Path.Combine(BD_FilesDir, selected_certificate.ImageFile.FileName);

                using (System.IO.FileStream fs = new System.IO.FileStream(s, FileMode.OpenOrCreate))
                {
                    fs.Write(Functions.FormatPDFFromAccess(selected_certificate.ImageFile.FileData.Data));
                }
            }

        }

        private void OnOpenImageFile(object document)
        {
            bldMaterialCertificate selected_certificate = document as bldMaterialCertificate;
            string BD_FilesDir = Path.GetTempPath();

            if (!Directory.Exists(BD_FilesDir))
                Directory.CreateDirectory(BD_FilesDir);
            selected_certificate = _buildingUnitsRepository.DocumentsRepository.MaterialCertificates.LoadPropertyObjects(selected_certificate.Id);
            string s = Path.Combine(BD_FilesDir, selected_certificate.ImageFile.FileName);

            using (System.IO.FileStream fs = new System.IO.FileStream(s, FileMode.OpenOrCreate))
            {
                fs.Write(Functions.FormatPDFFromAccess(selected_certificate.ImageFile.FileData.Data));
            }
            ProcessStartInfo info = new ProcessStartInfo(s);
            info.UseShellExecute = true;
            using (var proc = Process.Start(info)) { }
        }

        private void OnCreatedBasedOn(object obj)
        {
            //bldMaterialCertificate new_certificate = SelectedDocument.Clone() as bldMaterialCertificate;
            //new_certificate.IsHaveImageFile = false;
            //new_certificate.ImageFile = null;
            //UnDoReDoSystem localUnDoReDoSystem = new UnDoReDoSystem();
            //UnDoReDo.SetChildrenUnDoReDoSystem(localUnDoReDoSystem);

            //localUnDoReDoSystem.Register(SelectedDocumentsGroup);
            //// localUnDoReDoSystem.Register(FilteredCommonPointersCollection);

            //SelectedDocumentsGroup.Add(new_certificate);
            //// FilteredCommonPointersCollection.Add(new_certificate);

            //localUnDoReDoSystem.UnRegister(SelectedDocumentsGroup);
            ////   localUnDoReDoSystem.UnRegister(FilteredCommonPointersCollection);

            //UnDoReDo.UnSetUnDoReDoSystemAsChildren(localUnDoReDoSystem);
            //UnDoReDo.AddUnDoReDoSysAsCommand(localUnDoReDoSystem);
            //UnDoReDo.Register(new_certificate);

        }

        private void OnCreateNewMaterialCertificate()
        {
            //bldMaterialCertificate new_certificate = new bldMaterialCertificate();
            //UnDoReDoSystem localUnDoReDoSystem = new UnDoReDoSystem();
            //UnDoReDo.SetChildrenUnDoReDoSystem(localUnDoReDoSystem);

            //localUnDoReDoSystem.Register(SelectedDocumentsGroup);
            ////    localUnDoReDoSystem.Register(FilteredCommonPointersCollection);

            //SelectedDocumentsGroup.Add(new_certificate);
            ////   FilteredCommonPointersCollection.Add(new_certificate);

            //localUnDoReDoSystem.UnRegister(SelectedDocumentsGroup);
            ////  localUnDoReDoSystem.UnRegister(FilteredCommonPointersCollection);

            //UnDoReDo.UnSetUnDoReDoSystemAsChildren(localUnDoReDoSystem);
            //UnDoReDo.AddUnDoReDoSysAsCommand(localUnDoReDoSystem);
            //UnDoReDo.Register(new_certificate);
        }

        private void OnFilterDisable()
        {
            //          FilteredCommonPointersCollection = new NameableObservableCollection<bldDocument>(SortedCommonPointersCollection);
        }


        #region  Commmands Methods
        private void OnRemoveUnitOfMeasurement(object obj)
        {

            bldMaterialCertificate selected_certificate = SelectedDocument as bldMaterialCertificate;
            UnDoReDo.Register(selected_certificate);
            selected_certificate.UnitOfMeasurement = new bldUnitOfMeasurement("-");

        }

        private void OnSelectUnitOfMeasurement(object obj)
        {
            bldMaterialCertificate selected_certificate = obj as bldMaterialCertificate;
            ObservableCollection<bldUnitOfMeasurement> All_UnitOfMeasurements =
                 new ObservableCollection<bldUnitOfMeasurement>(_buildingUnitsRepository.UnitOfMeasurementRepository.GetAllAsync());
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
                          UnDoReDo.Register(selected_certificate);
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
            SelectedDocuments.Clear();

        }
        private void OnDataGridSelectionChanged(object certificates)
        {
            SelectedDocuments.Clear();
            foreach (bldMaterialCertificate certificate in (IList)certificates)
                SelectedDocuments.Add(certificate);
        }

        public void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }
        private bool CanSave()
        {
            if (SelectedDocument != null)
                return !SelectedDocument.HasErrors;// && SelectedWork.UnDoReDoSystem.Count > 0;
            else
                return false;
        }
        public virtual void OnSave()
        {
            base.OnSave<bldDocumentsGroup>(SelectedDocumentsGroup);
        }
        public virtual void OnClose(object obj)
        {
            base.OnClose<bldDocumentsGroup>(obj, SelectedDocumentsGroup);
        }
        public override void OnWindowClose()
        {
            ApplicationCommands.SaveAllCommand.UnregisterCommand(SaveCommand);
            ApplicationCommands.ReDoCommand.UnregisterCommand(ReDoCommand);
            ApplicationCommands.UnDoCommand.UnregisterCommand(UnDoCommand);

        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            ConveyanceObject navigane_message_works = (ConveyanceObject)navigationContext.Parameters["bld_material_certificates"];
            if (navigane_message_works != null)
            {
                EditMode = navigane_message_works.EditMode;
                bldDocumentsGroup documents = (bldDocumentsGroup)navigane_message_works.Object;
                if (SelectedDocumentsGroup != null) SelectedDocumentsGroup.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedDocumentsGroup = (bldDocumentsGroup)navigane_message_works.Object;
                SelectedDocumentsGroup.ErrorsChanged += RaiseCanExecuteChanged;
                Title = $"{SelectedDocumentsGroup.Code} {SelectedDocumentsGroup.Name}";
                //SortedCommonPointersCollection = new NameableObservableCollection<bldDocument>(SelectedDocumentsGroup.Where(el => el != null).ToList());
                //FilteredCommonPointersCollection = new NameableObservableCollection<bldDocument>(SortedCommonPointersCollection);

                UnDoReDo.Register(SelectedDocumentsGroup);
                foreach (bldMaterialCertificate document in SelectedDocumentsGroup)
                {
                    UnDoReDo.Register(document);
                    foreach (bldMaterialCertificate attach_document in document.AttachedDocuments)
                        UnDoReDo.Register(attach_document);
                }
            }
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_material_certificates"];

            if (((bldDocumentsGroup)navigane_message.Object).Id != SelectedDocumentsGroup.Id)
            {

                return false;
            }
            else
                return true;
        }
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}
