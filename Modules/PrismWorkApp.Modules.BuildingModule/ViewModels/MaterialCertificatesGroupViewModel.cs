using Prism;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Core.Dialogs;
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
using System.Threading;
using System.Windows.Controls;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class MaterialCertificatesGroupViewModel : BaseViewModel<bldMaterialCertificate>, INotifyPropertyChanged, INavigationAware, IActiveAware
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
        private bldMaterialCertificatesGroup _selectedDocumentsGroup;
        public bldMaterialCertificatesGroup SelectedDocumentsGroup
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
        private ObservableCollection<bldMaterialCertificate> _filteredCommonPointersCollection = new ObservableCollection<bldMaterialCertificate>();
        public ObservableCollection<bldMaterialCertificate> FilteredCommonPointersCollection
        {
            get { return _filteredCommonPointersCollection; }
            set { SetProperty(ref _filteredCommonPointersCollection, value); }
        }
        private ObservableCollection<bldMaterialCertificate> _sortedCommonPointersCollection = new ObservableCollection<bldMaterialCertificate>();
        public ObservableCollection<bldMaterialCertificate> SortedCommonPointersCollection
        {
            get { return _sortedCommonPointersCollection; }
            set { SetProperty(ref _sortedCommonPointersCollection, value); }
        }

        public NotifyCommand<object> DataGridLostFocusCommand { get; private set; }
        public NotifyCommand<object> DataGridSelectionChangedCommand { get; private set; }
        public NotifyCommand UnDoCommand { get; protected set; }
        public NotifyCommand ReDoCommand { get; protected set; }
        public NotifyCommand SaveCommand { get; protected set; }
        public NotifyCommand<object> CloseCommand { get; protected set; }
        public NotifyCommand<object> FindElementCommand { get; private set; }
        public NotifyCommand FilterDisableCommand { get; private set; }
        public NotifyCommand<object> FilteredElementCommand { get; private set; }
        public NotifyCommand<object> SortedElementCommand { get; private set; }

        public ObservableCollection<INotifyCommand>  MaterialCertificatesContextMenuCommands { get; set; } = new ObservableCollection<INotifyCommand>();
        public NotifyCommand CreateNewMaterialCertificateCommand { get; private set; }
        public NotifyCommand<object> AddCreatedFromTemplateMaterialCertificateCommand { get; private set; }

        public ObservableCollection<INotifyCommand> UnitsOfMeasurementContextMenuCommands { get; set; } = new ObservableCollection<INotifyCommand>();
        public NotifyCommand<object> SelectUnitOfMeasurementCommand { get; private set; }
        public NotifyCommand<object> RemoveUnitOfMeasurementCommand { get; private set; }

        public NotifyCommand OpenImageFileCommand { get; private set;}
        public NotifyCommand SaveImageFileToDiskCommand { get; private set;}

        public IBuildingUnitsRepository _buildingUnitsRepository { get; }
 
        public MaterialCertificatesGroupViewModel(IDialogService dialogService,
           IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository, IApplicationCommands applicationCommands)
        {
           
            UnDoReDo = new UnDoReDoSystem();
            ApplicationCommands = applicationCommands;
            DataGridSelectionChangedCommand = new NotifyCommand<object>(OnDataGridSelectionChanged);
            DataGridLostFocusCommand = new NotifyCommand<object>(OnDataGridLostFocus);

            FindElementCommand = new NotifyCommand<object>(OnFindElement);
            FilteredElementCommand = new NotifyCommand<object>(OnFilteredElement);
            FilterDisableCommand = new NotifyCommand(OnFilterDisable);
            SortedElementCommand = new NotifyCommand<object>(OnSortingElement);

            _buildingUnitsRepository = buildingUnitsRepository;
        
            SaveCommand = new NotifyCommand(OnSave, CanSave)
                .ObservesProperty(() => SelectedDocument);
            CloseCommand = new NotifyCommand<object>(OnClose);
            UnDoCommand = new NotifyCommand(() => { UnDoReDo.UnDo(1); },
                                     () => { return UnDoReDo.CanUnDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            ReDoCommand = new NotifyCommand(() => UnDoReDo.ReDo(1),
               () => { return UnDoReDo.CanReDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);

            #region ContextMenu Commands

            CreateNewMaterialCertificateCommand = new NotifyCommand(OnCreateNewMaterialCertificate);
            CreateNewMaterialCertificateCommand.Name = "Создать новый документ";
            AddCreatedFromTemplateMaterialCertificateCommand = new NotifyCommand<object>(OnAddCreatedFromTemplateMaterialCertificate,
                                    (ob) => { return SelectedDocuments.Count == 1;}).ObservesPropertyChangedEvent(SelectedDocuments);
            AddCreatedFromTemplateMaterialCertificateCommand.Name = "Создать новый на основании..";
            MaterialCertificatesContextMenuCommands.Add(CreateNewMaterialCertificateCommand);
            MaterialCertificatesContextMenuCommands.Add(AddCreatedFromTemplateMaterialCertificateCommand);
        
            SelectUnitOfMeasurementCommand = new NotifyCommand<object>(OnSelectUnitOfMeasurement);
            SelectUnitOfMeasurementCommand.Name = "Установить";
            RemoveUnitOfMeasurementCommand = new NotifyCommand<object>(OnUnitOfMeasurement);
            RemoveUnitOfMeasurementCommand.Name = "Удалить";
            UnitsOfMeasurementContextMenuCommands.Add(SelectUnitOfMeasurementCommand);
            UnitsOfMeasurementContextMenuCommands.Add(RemoveUnitOfMeasurementCommand);

            OpenImageFileCommand = new NotifyCommand(OnOpenImageFile);
            SaveImageFileToDiskCommand = new NotifyCommand(OnSaveImageFileToDisk);

            #endregion
            ApplicationCommands.SaveAllCommand.RegisterCommand(SaveCommand);
            ApplicationCommands.ReDoCommand.RegisterCommand(ReDoCommand);
            ApplicationCommands.UnDoCommand.RegisterCommand(UnDoCommand);
        }

        private void OnSaveImageFileToDisk()
        {
           
           
        }

        private void OnOpenImageFile()
        {
            string BD_FilesDir = Directory.GetCurrentDirectory();
            BD_FilesDir = Path.Combine(BD_FilesDir, "Temp");
            if (!Directory.Exists(BD_FilesDir))
                Directory.CreateDirectory(BD_FilesDir);
            string s = Path.Combine(BD_FilesDir,SelectedDocument.ImageFile.FileName);
           
            using (System.IO.FileStream fs = new System.IO.FileStream(s, FileMode.OpenOrCreate))
            {
                     fs.Write(SelectedDocument.ImageFile.Data);
            }
            ProcessStartInfo info = new ProcessStartInfo(s);
            info.UseShellExecute = true;
            using (var proc = Process.Start(info)) { }
            Thread.Sleep(500);
            File.Delete(s);                                                                      //Process.Start(s);
        }

        private void OnAddCreatedFromTemplateMaterialCertificate(object obj)
        {
            bldMaterialCertificate new_certificate =SelectedDocument.Clone()as bldMaterialCertificate;
            SelectedDocumentsGroup.Add(new_certificate);
            FilteredCommonPointersCollection.Add(new_certificate);
            SortedCommonPointersCollection.Add(new_certificate);

        }

        private void OnCreateNewMaterialCertificate()
        {
            bldMaterialCertificate  new_certificate = new  bldMaterialCertificate();
            SelectedDocumentsGroup.Add(new_certificate);
            FilteredCommonPointersCollection.Add(new_certificate);
            SortedCommonPointersCollection.Add(new_certificate);
        }

        private void OnFilterDisable()
        {
            FilteredCommonPointersCollection = new ObservableCollection<bldMaterialCertificate>(SortedCommonPointersCollection);
        }
        private void OnFindElement(object obj)
        {
            string find_string = ((Tuple<object, object>) obj).Item2 as string;
            DataGrid data_grid = ((Tuple<object, object>)obj).Item1 as DataGrid;


               var find_materials = CoreFunctions.FindElementInCollection<bldMaterialCertificate, ObservableCollection<bldMaterialCertificate>>(
                SelectedDocumentsGroup, SelectedGridColumn.SortMemberPath, find_string);
            if (find_materials.Count > 0)
            {
                data_grid.SelectedIndex = data_grid.Items.IndexOf(find_materials[0]);
            }
        }
        private void OnSortingElement(object obj)
        {
                //SortedCommonCollection.Clear();
                //FilteredCommonCollection.Clear();
                // foreach (T element in SelectedPredicate.Predicate.Invoke(CommonCollection))
                //{
                //    SortedCommonCollection.Add(element);
                //    FilteredCommonCollection.Add(element);
                // }
                SortedCommonPointersCollection.Clear();
                FilteredCommonPointersCollection.Clear();
                foreach (bldMaterialCertificate material in SelectedDocumentsGroup)
                {
                    SortedCommonPointersCollection.Add(material);
                    FilteredCommonPointersCollection.Add(material);
                }
            
        }
        private void OnFilteredElement(object obj)
        {
            //ComboBox comboBox = obj as ComboBox;
            //string combo_box_text = comboBox.Text;
            //comboBox.IsDropDownOpen = true;

            //ObservableCollection<T> finded_elements =
            //    new ObservableCollection<T>(CommonCollection.Where(el=>el.Name.Contains(combo_box_text)).ToList());
            //comboBox.ItemsSource = finded_elements;
            if (!FilterEnable) return;
            if (SelectedGridColumn == null) return;
                TextBox textBox = obj as TextBox;
            string text_box_text = textBox.Text;
            //FilteredCommonCollection.Clear();
            //foreach (T elemtnt in SortedCommonCollection.Where(el => el.Name.Contains(text_box_text)))
            //    FilteredCommonCollection.Add(elemtnt);

            FilteredCommonPointersCollection.Clear();
            //foreach (bldMaterialCertificate elemtnt in SortedCommonPointersCollection.Where(el => el.Name.Contains(text_box_text)))
            //    FilteredCommonPointersCollection.Add(elemtnt);
           
            var find_materials = CoreFunctions.FindElementInCollection<bldMaterialCertificate, ObservableCollection<bldMaterialCertificate>>(
                SortedCommonPointersCollection, SelectedGridColumn.SortMemberPath, text_box_text);
            foreach (bldMaterialCertificate elemtnt in find_materials)
                FilteredCommonPointersCollection.Add(elemtnt);


        }
        #region  Commmands Methods
        private void OnUnitOfMeasurement(object obj)
        {
            bldWork selected_work = obj as bldWork;
            UnDoReDo.Register(selected_work);
            selected_work.UnitOfMeasurement = null;

        }

        private void OnSelectUnitOfMeasurement(object obj)
        {
            bldWork selected_work = obj as bldWork;
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
                          UnDoReDo.Register(selected_work);
                          selected_work.UnitOfMeasurement = unit_of_measurement;
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
            foreach (bldMaterialCertificate  certificate in (IList)certificates)
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
        public override void OnSave()
        {
            base.OnSave();
            foreach (bldMaterialCertificate certificate in SelectedDocumentsGroup)
            _buildingUnitsRepository.MaterialCertificates.Add(certificate);
            _buildingUnitsRepository.Complete();
        }
        public override void OnWindowClose()
        {
            ApplicationCommands.SaveAllCommand.UnregisterCommand(SaveCommand);
            ApplicationCommands.ReDoCommand.UnregisterCommand(ReDoCommand);
            ApplicationCommands.UnDoCommand.UnregisterCommand(UnDoCommand);

        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            ConveyanceObject navigane_message_works = (ConveyanceObject)navigationContext.Parameters["bld_documents"];
            if (navigane_message_works != null)
            {
                EditMode = navigane_message_works.EditMode;
                bldMaterialCertificatesGroup documents = (bldMaterialCertificatesGroup)navigane_message_works.Object;
                if (SelectedDocumentsGroup != null) SelectedDocumentsGroup.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedDocumentsGroup = (bldMaterialCertificatesGroup)navigane_message_works.Object;
                SelectedDocumentsGroup.ErrorsChanged += RaiseCanExecuteChanged;
                UnDoReDo.Register(SelectedDocumentsGroup);
                foreach (bldMaterialCertificate   document in SelectedDocumentsGroup)
                {
                    UnDoReDo.Register(document);
                    foreach(bldMaterialCertificate attach_document in document.AttachedDocuments)
                          UnDoReDo.Register(attach_document);
                }
                Title = $"{SelectedDocumentsGroup.Code} {SelectedDocumentsGroup.Name}";
                SortedCommonPointersCollection = new ObservableCollection<bldMaterialCertificate>(SelectedDocumentsGroup.Where(el =>el!=null).ToList());
                FilteredCommonPointersCollection = new ObservableCollection<bldMaterialCertificate>(SortedCommonPointersCollection);
                
            }
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_documents"];
            if (((bldMaterialCertificatesGroup)navigane_message.Object).Id != SelectedDocumentsGroup.Id)
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
