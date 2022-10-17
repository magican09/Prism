using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.Modules.BuildingModule.Dialogs;
using PrismWorkApp.Modules.BuildingModule.Views;
using PrismWorkApp.OpenWorkLib.Data;
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
    public class ObjectViewModel : BaseViewModel<bldObject>, INotifyPropertyChanged, INavigationAware
    {

        private string _title = "";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
      
        private bldObject _selectedObject;
        public bldObject SelectedBuildingObject
        {
            get { return _selectedObject; }
            set { SetProperty(ref _selectedObject, value); }
        }
        private bldObject _resivedObject;
        public bldObject ResivedObject
        {
            get { return _resivedObject; }
            set { SetProperty(ref _resivedObject, value); }
        }
        private bldObject _ChildSelectedBuildingObject;
        public bldObject SelectedChildBuildingObject
        {
            get { return _ChildSelectedBuildingObject; }
            set { SetProperty(ref _ChildSelectedBuildingObject, value); }
        }
       
        private bldObjectsGroup _objects;
        public bldObjectsGroup Objects
        {
            get { return _objects; }
            set { SetProperty(ref _objects, value); }
        }
        private bldObjectsGroup _allObjects;
        public bldObjectsGroup AllObjects
        {
            get { return _allObjects; }
            set { SetProperty(ref _allObjects, value); }
        }
        private bldObjectsGroup _buildingObjects;
        public bldObjectsGroup BuildingObjects
        {
            get { return _buildingObjects; }
            set { SetProperty(ref _buildingObjects, value); }
        }

      
        private bldConstruction _selectedConstruction;
        public bldConstruction SelectedConstruction
        {
            get { return _selectedConstruction; }
            set { SetProperty(ref _selectedConstruction, value); }
        }
        public DelegateCommand<object> DataGridLostFocusCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand<object> CloseCommand { get; private set; }
        public DelegateCommand AddBuildingObjectsCommand { get; private set; }
        public DelegateCommand AddConstructionCommand { get; private set; }
        public DelegateCommand EditBuildingObjectCommand { get; private set; }
        public DelegateCommand EditConstructionCommand { get; private set; }
        public DelegateCommand RemoveBuildingObjectCommand { get; private set; }
        public DelegateCommand RemoveConstructionCommand { get; private set; }

     
        private readonly IBuildingUnitsRepository _buildingUnitsRepository;
        public ObjectViewModel(IDialogService dialogService, IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository)
        {

            DataGridLostFocusCommand = new DelegateCommand<object>(OnDataGridLostSocus);
            SaveCommand = new DelegateCommand(OnSave, CanSave);
            CloseCommand = new DelegateCommand<object>(OnClose);
            RemoveBuildingObjectCommand = new DelegateCommand(OnRemoveBuildingObject,
                                      () => SelectedChildBuildingObject != null)
                  .ObservesProperty(() => SelectedChildBuildingObject);
            RemoveConstructionCommand = new DelegateCommand(OnRemoveConstruction,
                                        () => SelectedConstruction != null)
                .ObservesProperty(() => SelectedConstruction);
            AddBuildingObjectsCommand = new DelegateCommand(OnAddBuildingObject);
           
            AddConstructionCommand = new DelegateCommand(OnAddConstruction);


            EditBuildingObjectCommand = new DelegateCommand(OnEditBuildingObject,
                                         () => SelectedChildBuildingObject != null)
                     .ObservesProperty(() => SelectedChildBuildingObject);

            EditConstructionCommand = new DelegateCommand(OnEditConstruction,
                                         () => SelectedConstruction != null)
                     .ObservesProperty(() => SelectedConstruction);
             
            _dialogService = dialogService;
            _regionManager = regionManager;
            _buildingUnitsRepository = buildingUnitsRepository;
        }
        private void OnDataGridLostSocus(object obj)
        {

            if (obj == SelectedChildBuildingObject)
            {
                    SelectedConstruction = null;
                  return;
            }
            if (obj == SelectedConstruction)
            {
                SelectedChildBuildingObject = null;
                  return;
            }
            
        }
       
        private void OnEditBuildingObject()
        {
              CoreFunctions.EditElementDialog<bldObject>(SelectedBuildingObject, "Строительный объект",
               (result) => { SaveCommand.RaiseCanExecuteChanged(); }, _dialogService, typeof(ObjectDialogView).Name, "Редактировать", Id);
        }
        private void OnAddBuildingObject()
        {
            BuildingObjects = new bldObjectsGroup(_buildingUnitsRepository.Objects.GetldObjectsAsync());//.GetBldObjects(SelectedProject.Id));
            if (SelectedBuildingObject.BuildingObjects == null) SelectedBuildingObject.BuildingObjects = new bldObjectsGroup();
            CoreFunctions.AddElementToCollectionWhithDialog<bldObjectsGroup, bldObject>
                  (SelectedBuildingObject.BuildingObjects, BuildingObjects, _dialogService,
                    (result) =>
                    {

                        if (result.Result == ButtonResult.Yes)
                        {
                            SaveCommand.RaiseCanExecuteChanged();
                        }
                        if (result.Result == ButtonResult.No)
                        {
                            SelectedBuildingObject.BuildingObjects.UnDoAll(Id);
                        }
                    },
                   typeof(AddbldObjectToCollectionDialogView).Name,
                    typeof(ObjectDialogView).Name, Id,
                   "Редактирование списка объектов",
                   "Форма для редактирования состава объектов проекта.",
                  "Объекты текущего проекта", "Все объекты");

        }
        private void OnRemoveBuildingObject()
        {

            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldObjectsGroup, bldObject>
               (SelectedBuildingObject.BuildingObjects, SelectedChildBuildingObject, "Строительный объект"
               , () =>
               {
                   SelectedChildBuildingObject = null;
                   SaveCommand.RaiseCanExecuteChanged();
               }, _dialogService);
        }

        private void OnEditConstruction()
        {
            CoreFunctions.EditElementDialog<bldConstruction>(SelectedConstruction, "Учасник строительства",
                  (result) => { SaveCommand.RaiseCanExecuteChanged(); }, _dialogService, typeof(ConstructionDialogView).Name, "Редактировать", Id);
       
        }
        private void OnAddConstruction()
        {
            bldConstructionsGroup Constructions =  
            new bldConstructionsGroup(_buildingUnitsRepository.Constructions.GetbldConstructionsAsync());
            NameablePredicate<bldConstructionsGroup, bldConstruction> predicate_1 = new NameablePredicate<bldConstructionsGroup, bldConstruction>();
            predicate_1.Name = "Показать только из текущего проекта.";
            predicate_1.Predicate = cl => cl.Where(el => el.bldObject.bldProject.Id == SelectedBuildingObject.bldProject.Id).ToList();
            NameablePredicate<bldConstructionsGroup, bldConstruction> predicate_2 = new NameablePredicate<bldConstructionsGroup, bldConstruction>();
            predicate_2.Name = "Показать все кроме текущего проекта";
            predicate_2.Predicate = cl => cl.Where(el => el.bldObject.bldProject.Id != SelectedBuildingObject.bldProject.Id).ToList();
            NameablePredicate<bldConstructionsGroup, bldConstruction> predicate_3 = new NameablePredicate<bldConstructionsGroup, bldConstruction>();
            predicate_3.Name = "Показать все";
            predicate_3.Predicate = cl => cl;

            NameablePredicateObservableCollection<bldConstructionsGroup, bldConstruction> nameablePredicatesCollection = new NameablePredicateObservableCollection<bldConstructionsGroup, bldConstruction>();
            nameablePredicatesCollection.Add(predicate_1);
            nameablePredicatesCollection.Add(predicate_2);
            nameablePredicatesCollection.Add(predicate_3);
            CoreFunctions.AddElementToCollectionWhithDialog_Test<bldConstructionsGroup, bldConstruction>
                (SelectedBuildingObject.Constructions, Constructions, 
                nameablePredicatesCollection,
                _dialogService,
                 (result) =>
                 {
                     if (result.Result == ButtonResult.Yes)
                     {
                         SaveCommand.RaiseCanExecuteChanged();
                         foreach (bldConstruction construction in SelectedBuildingObject.Constructions)
                             construction.bldObject = SelectedBuildingObject;
                     }
                     if (result.Result == ButtonResult.No)
                     {
                         SelectedBuildingObject.Constructions.UnDoAll(Id);
                     }
                 },
                typeof(AddbldConstructionToCollectionDialogView).Name,
                typeof(ConstructionDialogView).Name, Id,
                "Редактирование списка конструкций",
                "Форма для редактирования состава коснструций объекта.",
                "Конструкции текущего объекта", "Все конструкции");
        }
        private void OnRemoveConstruction()
        {

            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldConstructionsGroup, bldConstruction>
                 (SelectedBuildingObject.Constructions, SelectedConstruction, "Строительная конструкция",
                 () => { SelectedConstruction = null; SaveCommand.RaiseCanExecuteChanged(); }, _dialogService);
        }

        public  void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        private bool CanSave()
        {
            return true;
        }
        public virtual void OnSave()
        {
            this.OnSave<bldObject>(SelectedBuildingObject);
        }
        public virtual void OnClose(object obj)
        {
            this.OnClose<bldObject>(obj, SelectedBuildingObject);
        }

       
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_object"];
           
                 
                if (((bldObject)navigane_message.Object).Id != SelectedBuildingObject.Id)
                    return false;
                else
                    return true;
             
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
           
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_object"];
            if (navigane_message != null)
            {
                ResivedObject = (bldObject)navigane_message.Object;
                EditMode = navigane_message.EditMode;
                SelectedBuildingObject = ResivedObject;
                if (SelectedBuildingObject != null) SelectedBuildingObject.ErrorsChanged -= RaiseCanExecuteChanged;
              //  SelectedBuildingObject = new SimpleEditableBldObject();
                SelectedBuildingObject.ErrorsChanged += RaiseCanExecuteChanged;
               
                Title = ResivedObject.Name;
            }
        }
    }
}
