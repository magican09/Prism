﻿using Microsoft.Win32;
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
 
 
        private bldMaterialCertificate _selectedDocument;
        public bldMaterialCertificate SelectedDocument
        {
            get { return _selectedDocument; }
            set { SetProperty(ref _selectedDocument, value); }
        }
        private ObservableCollection<bldMaterialCertificate> _selectedDocuments = new ObservableCollection<bldMaterialCertificate>();
        public ObservableCollection<bldMaterialCertificate> SelectedDocuments
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
        private bldDocumentsGroup _aggregationDocuments = new bldDocumentsGroup();
        public bldDocumentsGroup AggregationDocuments
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

        public NotifyCommand<object> RemoveAggregationDocumentCommand { get; private set; }

        public NotifyCommand<object> CreateNewMaterialCertificateCommand { get; private set; }
        public NotifyCommand<object> CreatedBasedOnMaterialCertificateCommand { get; private set; }
        public NotifyCommand<object> RemoveMaterialCertificateCommand { get; private set; }


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
            RemoveAggregationDocumentCommand = new NotifyCommand<object>(OnRemoveAggregationDocument,
                                    (ob) => { return SelectedAggregationDocument != null; } ).ObservesProperty(() => SelectedAggregationDocument);
            RemoveAggregationDocumentCommand.Name = "Удалить";

            CreateNewMaterialCertificateCommand = new NotifyCommand<object>(OnCreateNewMaterialCertificate, (ob) => SelectedAggregationDocument != null).ObservesProperty(() => SelectedAggregationDocument);
            CreateNewMaterialCertificateCommand.Name = "Создать новый документ";
            CreatedBasedOnMaterialCertificateCommand = new NotifyCommand<object>(OnCreatedBasedOnMaterialCertificate,
                                    (ob) => { return SelectedDocument is bldMaterialCertificate; }).ObservesProperty(() => SelectedDocument);
            CreatedBasedOnMaterialCertificateCommand.Name = "Создать новый на основании..";
            RemoveMaterialCertificateCommand = new NotifyCommand<object>(OnRemoveMaterialCertificate, CanRemoveMaterialCertificate).ObservesProperty(() => SelectedDocument);
            RemoveMaterialCertificateCommand.Name = "Удалить";

            
            OpenImageFileCommand = new NotifyCommand<object>(OnOpenImageFile);
            SaveImageFileToDiskCommand = new NotifyCommand<object>(OnSaveImageFileToDisk);
            LoadImageFileFromDiskCommand = new NotifyCommand<object>(OnLoadImageFileFromDisk);

            #endregion

            ApplicationCommands.SaveAllCommand.RegisterCommand(SaveCommand);
            ApplicationCommands.ReDoCommand.RegisterCommand(ReDoCommand);
            ApplicationCommands.UnDoCommand.RegisterCommand(UnDoCommand);

            ApplicationCommands.CreateNewCommand.RegisterCommand(CreateNewMaterialCertificateCommand);
            ApplicationCommands.CreateBasedOnCommand.RegisterCommand(CreatedBasedOnMaterialCertificateCommand);
            AllUnitsOfMeasurements = new ObservableCollection<bldUnitOfMeasurement>(_buildingUnitsRepository.UnitOfMeasurementRepository.GetAllAsync());

        }

        private void OnRemoveAggregationDocument(object obj)
        {
            bldAggregationDocument selected_AGDocument = SelectedAggregationDocument;

            int changes_number = selected_AGDocument.UnDoReDoSystem.GetChangesNamber(selected_AGDocument);
            if (changes_number > 0)
            {
                CoreFunctions.ConfirmActionOnElementDialog<bldAggregationDocument>(SelectedAggregationDocument, "Сохранить", SelectedAggregationDocument.Name, "Сохранить", "Не сохранять", "Отмена",
                    (result) =>
                {
                    if(result.Result== ButtonResult.Yes)
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
           
        }



        //private void OnDataGridPastingCellClipboardContent(object obj)
        //{

        //}

        //private void OnDataGridPastin(object obj)
        //{

        //}

        //private void OnDataGridCopyingCellClipboardContent(object obj)
        //{

        //}

        //private void OnDataGridCopying(object obj)
        //{

        //}
        #region Mouse methods

        private void OnContextMenuOpened(object obj)
        {

            ContextMenu contextMenu = ((IList)obj)[0] as ContextMenu;
            object clicked_document = ((IList)obj)[1];
            GridViewCell grid_cell = (GridViewCell)((IList)obj)[2];
            var selected_doc = (bldMaterialCertificate)clicked_document;
            SelectedDocument = selected_doc;
            NotifyMenuCommands context_menu_item_commands = null;
            switch (clicked_document.GetType().Name)
            {
                case (nameof(bldMaterialCertificate)):
                    {
                        context_menu_item_commands = new NotifyMenuCommands()
                            {
                             CreateNewMaterialCertificateCommand,
                             CreatedBasedOnMaterialCertificateCommand,
                             RemoveMaterialCertificateCommand
                        };
                        
                        break;
                    }
                case (nameof(bldDocument)):
                case (nameof(bldAggregationDocument)):

                    {
                        context_menu_item_commands = new NotifyMenuCommands()
                            {
                             
                            };
                        break;

                    }
            }
            contextMenu.ItemsSource = context_menu_item_commands;

        }
        #endregion


        private void OnDataGridSelectionChanged(object obj)
        {
            List<object> grid_state_objects = obj as List<object>;
          //  SelectedDocument = grid_state_objects[0] as bldMaterialCertificate;
            //   SelectedDocuments = (ObservableCollection<bldMaterialCertificate>) grid_state_objects[1];
            SelectedDocuments.Clear();
            var selected_items = (ObservableCollection<object>)grid_state_objects[1];
            foreach (object elm in selected_items)
                SelectedDocuments.Add(obj as bldMaterialCertificate);
            SelectedAggregationDocument = (bldAggregationDocument)grid_state_objects[2];
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
                        selected_certificate.ImageFile.Data = buffer;
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
            if (selected_certificate.ImageFile == null)
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
                    fs.Write(Functions.FormatPDFFromAccess(selected_certificate.ImageFile.Data));
                }
            }

        }

        private void OnOpenImageFile(object document)
        {
            bldMaterialCertificate selected_certificate = document as bldMaterialCertificate;
            string BD_FilesDir = Path.GetTempPath();

            if (!Directory.Exists(BD_FilesDir))
                Directory.CreateDirectory(BD_FilesDir);
            if (selected_certificate.ImageFile == null)
                selected_certificate = _buildingUnitsRepository.DocumentsRepository.MaterialCertificates.LoadPropertyObjects(selected_certificate.Id);
            string s = Path.Combine(BD_FilesDir, selected_certificate.ImageFile.FileName);

            using (System.IO.FileStream fs = new System.IO.FileStream(s, FileMode.OpenOrCreate))
            {
                fs.Write(Functions.FormatPDFFromAccess(selected_certificate.ImageFile.Data));
            }
            ProcessStartInfo info = new ProcessStartInfo(s);
            info.UseShellExecute = true;
            using (var proc = Process.Start(info)) { }
        }
        private bool CanRemoveMaterialCertificate(object obj)
        { 

            return SelectedDocument!=null;
        }
        private void OnRemoveMaterialCertificate(object obj)
        {
            bldMaterialCertificate selected_certificate =((IList)obj)[0] as bldMaterialCertificate;
            bldAggregationDocument selected_aggr_documtnt = ((IList)obj)[1] as bldAggregationDocument; ;
          if(selected_aggr_documtnt != null && selected_aggr_documtnt != null)
            selected_aggr_documtnt.AttachedDocuments.Remove(selected_certificate);
        }
        private void OnCreatedBasedOnMaterialCertificate(object obj)
        {

            bldMaterialCertificate new_certificate = SelectedDocument.Clone() as bldMaterialCertificate;
            new_certificate.IsHaveImageFile = false;
            SelectedAggregationDocument.AttachedDocuments.Add(new_certificate);
        }

        private void OnCreateNewMaterialCertificate(object obj)
        {
            bldMaterialCertificate new_certificate = new bldMaterialCertificate();
            SelectedAggregationDocument.AttachedDocuments.Add(new_certificate);
            //UnDoReDo.Register(new_certificate);
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
            SelectedDocuments.Clear();
            SelectedDocument = null;
            SelectedAggregationDocument = null;

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
                (result)=>
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
