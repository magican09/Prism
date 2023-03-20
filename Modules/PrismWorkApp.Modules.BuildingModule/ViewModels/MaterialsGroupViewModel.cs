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
using System.Linq;
using System.Windows.Controls;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class MaterialsGroupViewModel : BaseViewModel<bldMaterialsGroup>, INotifyPropertyChanged, INavigationAware, IActiveAware
    {
        private string _title = "Списко материалов";
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
        
        private bldMaterial _selectedMaterial;
        public bldMaterial SelectedMaterial
        {
            get { return _selectedMaterial; }
            set { SetProperty(ref _selectedMaterial, value); }
        }
        private bldMaterialsGroup _selectedMaterialsGroup;
        public bldMaterialsGroup SelectedMaterialsGroup
        {
            get { return _selectedMaterialsGroup; }
            set { SetProperty(ref _selectedMaterialsGroup, value); }
        }
        private ObservableCollection<bldMaterial> _selectedMaterials = new ObservableCollection<bldMaterial>();
        public ObservableCollection<bldMaterial> SelectedMaterials
        {
            get { return _selectedMaterials; }
            set { SetProperty(ref _selectedMaterials, value); }
        }
        private ObservableCollection<bldResourseCategory> _materialsCategories = new ObservableCollection<bldResourseCategory>();
        public ObservableCollection<bldResourseCategory> MaterialsCategories
        {
            get { return _materialsCategories; }
            set { SetProperty(ref _materialsCategories, value); }
        }
        private bldResourseCategory _selectedMaterialCategory;
        public bldResourseCategory SelectedMaterialCategory
        {
            get { return _selectedMaterialCategory; }
            set { SetProperty(ref _selectedMaterialCategory, value); }
        }
        private bool _filterEnable = false;
        public bool FilterEnable
        {
            get { return _filterEnable; }
            set { SetProperty(ref _filterEnable, value); }
        }
        private ObservableCollection<bldMaterial> _filteredCommonPointersCollection = new ObservableCollection<bldMaterial>();
        public ObservableCollection<bldMaterial> FilteredCommonPointersCollection
        {
            get { return _filteredCommonPointersCollection; }
            set { SetProperty(ref _filteredCommonPointersCollection, value); }
        }
        private ObservableCollection<bldMaterial> _sortedCommonPointersCollection = new ObservableCollection<bldMaterial>();
        public ObservableCollection<bldMaterial> SortedCommonPointersCollection
        {
            get { return _sortedCommonPointersCollection; }
            set { SetProperty(ref _sortedCommonPointersCollection, value); }
        }

        public NotifyCommand<object> DataGridLostFocusCommand { get; private set; }
        public NotifyCommand<object> DataGridSelectionChangedCommand { get; private set; }
        public NotifyCommand<object> FindMaterialCommand { get; private set; }
        public NotifyCommand<object> FilteredElementCommand { get; private set; }
        public NotifyCommand<object> SortedElementCommand { get; private set; }
        public NotifyCommand UnDoCommand { get; protected set; }
        public NotifyCommand ReDoCommand { get; protected set; }
        public NotifyCommand SaveCommand { get; protected set; }
        public NotifyCommand<object> CloseCommand { get; protected set; }

        public ObservableCollection<INotifyCommand> UnitsOfMeasurementContextMenuCommands { get; set; } = new ObservableCollection<INotifyCommand>();
        public NotifyCommand<object> SelectUnitOfMeasurementCommand { get; private set; }
        public NotifyCommand<object> RemoveUnitOfMeasurementCommand { get; private set; }

        public IBuildingUnitsRepository _buildingUnitsRepository { get; }
        public IbldMaterialsUnitsRepository _bldMaterialsUnitsRepository { get; }
        public MaterialsGroupViewModel(IDialogService dialogService,
           IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository,IbldMaterialsUnitsRepository materialsUnitsRepository, IApplicationCommands applicationCommands)
        {
           
            UnDoReDo = new UnDoReDoSystem();
            ApplicationCommands = applicationCommands;
            DataGridSelectionChangedCommand = new NotifyCommand<object>(OnDataGridSelectionChanged);
            DataGridLostFocusCommand = new NotifyCommand<object>(OnDataGridLostFocus);

            FindMaterialCommand = new NotifyCommand<object>(OnFindMaterial);
            FilteredElementCommand = new NotifyCommand<object>(OnFilteredElement);
            SortedElementCommand = new NotifyCommand<object>(OnSortingElement);
          
            _bldMaterialsUnitsRepository = materialsUnitsRepository;
            MaterialsCategories = new ObservableCollection<bldResourseCategory>(
                                    _bldMaterialsUnitsRepository.ResourseCategories.GetAll());
            SaveCommand = new NotifyCommand(OnSave, CanSave)
                .ObservesProperty(() => SelectedMaterial);
            CloseCommand = new NotifyCommand<object>(OnClose);
            UnDoCommand = new NotifyCommand(() => { UnDoReDo.UnDo(1); },
                                     () => { return UnDoReDo.CanUnDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            ReDoCommand = new NotifyCommand(() => UnDoReDo.ReDo(1),
               () => { return UnDoReDo.CanReDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);

            SelectUnitOfMeasurementCommand = new NotifyCommand<object>(OnSelectUnitOfMeasurement);
            SelectUnitOfMeasurementCommand.Name = "Установить";
            RemoveUnitOfMeasurementCommand = new NotifyCommand<object>(OnUnitOfMeasurement);
            RemoveUnitOfMeasurementCommand.Name = "Удалить";
            UnitsOfMeasurementContextMenuCommands.Add(SelectUnitOfMeasurementCommand);
            UnitsOfMeasurementContextMenuCommands.Add(RemoveUnitOfMeasurementCommand);

            ApplicationCommands.SaveAllCommand.RegisterCommand(SaveCommand);
            ApplicationCommands.ReDoCommand.RegisterCommand(ReDoCommand);
            ApplicationCommands.UnDoCommand.RegisterCommand(UnDoCommand);
        }

        private void OnFindMaterial(object obj)
        {
            string find_string = ((Tuple<object, object>) obj).Item2 as string;
            DataGrid data_grid = ((Tuple<object, object>)obj).Item1 as DataGrid;


            var r = SelectedGridColumn.HeaderTemplate;
             var find_materials = CoreFunctions.FindElementInCollection<bldMaterial, ObservableCollection<bldMaterial>>(
                SelectedMaterialsGroup, SelectedGridColumn.SortMemberPath, find_string);
     
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
                foreach (bldMaterial material in SelectedMaterialsGroup)
                {
                    SortedCommonPointersCollection.Add(material);
                    FilteredCommonPointersCollection.Add(material);
                }
            
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
                 new ObservableCollection<bldUnitOfMeasurement>(_bldMaterialsUnitsRepository.UnitOfMeasurementRepository.GetAllUnits());
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
            SelectedMaterials.Clear();

        }
        private void OnDataGridSelectionChanged(object materials)
        {
            SelectedMaterials.Clear();
            foreach (bldMaterial  material in (IList)materials)
                SelectedMaterials.Add(material);
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
            TextBox textBox = obj as TextBox;
            string text_box_text = textBox.Text;
            //FilteredCommonCollection.Clear();
            //foreach (T elemtnt in SortedCommonCollection.Where(el => el.Name.Contains(text_box_text)))
            //    FilteredCommonCollection.Add(elemtnt);

            FilteredCommonPointersCollection.Clear();
            foreach (bldMaterial elemtnt in SortedCommonPointersCollection.Where(el => el.Name.Contains(text_box_text)))
                FilteredCommonPointersCollection.Add(elemtnt);


        }
        public void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }
        private bool CanSave()
        {
            if (SelectedMaterial != null)
                return !SelectedMaterial.HasErrors;// && SelectedWork.UnDoReDoSystem.Count > 0;
            else
                return false;
        }
        public override void OnWindowClose()
        {
            ApplicationCommands.SaveAllCommand.UnregisterCommand(SaveCommand);
            ApplicationCommands.ReDoCommand.UnregisterCommand(ReDoCommand);
            ApplicationCommands.UnDoCommand.UnregisterCommand(UnDoCommand);

        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            ConveyanceObject navigane_message_works = (ConveyanceObject)navigationContext.Parameters["bld_materials"];
            if (navigane_message_works != null)
            {
                EditMode = navigane_message_works.EditMode;
                bldMaterialsGroup materials = (bldMaterialsGroup)navigane_message_works.Object;
                if (SelectedMaterialsGroup != null) SelectedMaterialsGroup.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedMaterialsGroup = (bldMaterialsGroup)navigane_message_works.Object;
                SelectedMaterialsGroup.ErrorsChanged += RaiseCanExecuteChanged;
                bldResourseCategory load_mat_category = SelectedMaterialsGroup.Parent as bldResourseCategory;
                SelectedMaterialCategory = MaterialsCategories.Where(mc => mc.Name == load_mat_category.Name).FirstOrDefault();
                if (SelectedMaterialCategory==null)
                {
                    MaterialsCategories.Add(load_mat_category);
                    SelectedMaterialCategory = load_mat_category;
                }
                UnDoReDo.Register(SelectedMaterialsGroup);
                foreach (bldMaterial  material in SelectedMaterialsGroup)
                {
                    UnDoReDo.Register(material);
                    foreach(bldDocument document in material.Documents)
                          UnDoReDo.Register(document);
                }
                Title = $"{SelectedMaterialsGroup.Code} {SelectedMaterialsGroup.Name}";
                SortedCommonPointersCollection = new ObservableCollection<bldMaterial>(SelectedMaterialsGroup.Where(el =>el!=null).ToList());
                FilteredCommonPointersCollection = new ObservableCollection<bldMaterial>(SortedCommonPointersCollection);
                
            }
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_materials"];
            if (((bldMaterialsGroup)navigane_message.Object).Id != SelectedMaterialsGroup.Id)
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
