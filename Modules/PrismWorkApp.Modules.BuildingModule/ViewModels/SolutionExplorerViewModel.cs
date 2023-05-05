﻿using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.Core.Events;
using PrismWorkApp.Modules.BuildingModule.Core;
using PrismWorkApp.Modules.BuildingModule.Dialogs;
using PrismWorkApp.Modules.BuildingModule.Views;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Telerik.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using DialogParameters = Prism.Services.Dialogs.DialogParameters;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class SolutionExplorerViewModel : BaseViewModel<object>, INotifyPropertyChanged, INavigationAware//, IConfirmNavigationRequest
    {


        public Dictionary<string, Node> _nodeDictionary;
        public Dictionary<string, Node> NodeDictionary
        {
            get { return _nodeDictionary; }
            set { SetProperty(ref _nodeDictionary, value); }
        }

        private DataItemCollection _items;
        public DataItemCollection Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }
        private DataItem _root;
        public DataItem Root
        {
            get { return _root; }
            set { SetProperty(ref _root, value); }
        }
        private string _title = "Менеджер проектов";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private NameableObservableCollection<IEntityObject> _allModels = new NameableObservableCollection<IEntityObject>();

        public NameableObservableCollection<IEntityObject> AllModels
        {
            get { return _allModels; }
            set { _allModels = value; }
        }

        private bldDocumentsGroup _documentation;
        public bldDocumentsGroup Documentation
        {
            get { return _documentation; }
            set { SetProperty(ref _documentation, value); }
        }
        private object _selectionObject;
        public object SelectionObject
        {
            get { return _selectionObject; }
            set { SetProperty(ref _selectionObject, value); }
        }
        public NotifyCommand UnDoCommand { get; protected set; }
        public NotifyCommand ReDoCommand { get; protected set; }
        public NotifyCommand SaveCommand { get; protected set; }
     //   public NotifyCommand<object> CloseCommand { get; protected set; }

        public NotifyMenuCommands DocumentationCommands { get; set; }

        public NotifyCommand<object> ContextMenuOpenedCommand { get; private set; }
        public NotifyCommand<object> MouseDoubleClickCommand { get; private set; }


        public NotifyCommand<CommandArgs> EditInTreeViewItemCommand { get; private set; }

        public NotifyCommand<CommandArgs> CreateBasedOnCommand { get; private set; }


        public NotifyCommand<CommandArgs> LoadAggregationDocumentFromDBCommand { get; set; }
        public NotifyCommand<CommandArgs> CreateNewAggregationDocumentCommand { get; set; }
        public NotifyCommand<CommandArgs> RemoveAggregationDocumentCommand { get; private set; }
        public NotifyCommand<CommandArgs> CloseAggregationDocumentCommand { get; private set; }
        public NotifyCommand<CommandArgs> OpenAsMaterialCertificateAggregationDocumentCommand { get; set; }


        //public NotifyCommand<object> LoadUnitOfMeasurementFromDBCommand { get; set; }
        public NotifyCommand<CommandArgs> CreateNewUnitOfMeasurementCommand { get; set; }
        public NotifyCommand<CommandArgs> RemoveUnitOfMeasurementCommand { get; private set; }
        public NotifyCommand<CommandArgs> CloseUnitOfMeasurementCommand { get; private set; }

        public NotifyCommand<CommandArgs> CreateNewTypeOfFileCommand { get; set; }
        public NotifyCommand<CommandArgs> CloseTypeOfFileCommand { get; private set; }
        public NotifyCommand<CommandArgs> RemoveTypeOfFileCommand { get; private set; }

        private readonly IEventAggregator _eventAggregator;
        private AppObjectsModel _appObjectsModel;
        public AppObjectsModel AppObjectsModel
        {
            get { return _appObjectsModel; }
            set { SetProperty(ref _appObjectsModel, value); }
        }


        private IApplicationCommands _applicationCommands;
        IBuildingUnitsRepository _buildingUnitsRepository;
        

        public SolutionExplorerViewModel(IEventAggregator eventAggregator,
                            IRegionManager regionManager, IDialogService dialogService,
                                           IBuildingUnitsRepository buildingUnitsRepository, IApplicationCommands applicationCommands, IAppObjectsModel appObjectsModel, IUnDoReDoSystem unDoReDoSystem)
        {
            AppObjectsModel = appObjectsModel as AppObjectsModel;
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            _dialogService = dialogService;
            _applicationCommands = applicationCommands;
            _buildingUnitsRepository = buildingUnitsRepository;
            UnDoReDo = unDoReDoSystem;
            AllModels = AppObjectsModel.AllModels;
            Documentation = AppObjectsModel.Documentation;
            UnDoCommand = new NotifyCommand(() => { UnDoReDo.UnDo(1); },
                                   () => { return UnDoReDo.CanUnDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            ReDoCommand = new NotifyCommand(() => UnDoReDo.ReDo(1),
              () => { return UnDoReDo.CanReDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            UnDoCommand.MonitorCommandActivity = false;
            ReDoCommand.MonitorCommandActivity = false;

            SaveCommand = new NotifyCommand(OnSave, () => { return UnDoReDo.HasAnyChangedObjectInAllSystems(); }).ObservesPropertyChangedEvent(UnDoReDo);
            SaveCommand.MonitorCommandActivity = false;

            ContextMenuOpenedCommand = new NotifyCommand<object>(OnContextMenuOpened);
            MouseDoubleClickCommand = new NotifyCommand<object>(OnMouseDoubleClick);
            EditInTreeViewItemCommand = new NotifyCommand<CommandArgs>(OnEditInTreeViewItem);
            EditInTreeViewItemCommand.Name = "Редактировать";
            CreateBasedOnCommand = _appObjectsModel.CreateBasedOnCommand;
            CreateBasedOnCommand.Name = "Создать на основании...";

            LoadAggregationDocumentFromDBCommand = new NotifyCommand<CommandArgs>(OnLoadAggregationDocumentFromDB);
            LoadAggregationDocumentFromDBCommand.Name = "Загрузить перечень документов из БД";
            CreateNewAggregationDocumentCommand = new NotifyCommand<CommandArgs>(OnCreateNewAggregationDocument);
            CreateNewAggregationDocumentCommand.Name = "Добавить новый перечень документов";
            RemoveAggregationDocumentCommand = new NotifyCommand<CommandArgs>((cm_args) => _appObjectsModel.RemoveObjCommand.Execute(cm_args));
            RemoveAggregationDocumentCommand.Name = "Удалить перечень документов";
            CloseAggregationDocumentCommand = new NotifyCommand<CommandArgs>((cm_args) => _appObjectsModel.CloseObjCommand.Execute(cm_args));
            CloseAggregationDocumentCommand.Name = "Закрыть перечень документов";


            CreateNewUnitOfMeasurementCommand = new NotifyCommand<CommandArgs>(OnCreateNewUnitOfMeasurement);
            CreateNewUnitOfMeasurementCommand.Name = "Добавить новую ед.изм.";
            RemoveUnitOfMeasurementCommand = new NotifyCommand<CommandArgs>((cm_args) => _appObjectsModel.RemoveObjCommand.Execute(cm_args));
            RemoveUnitOfMeasurementCommand.Name = "Удалить ед.изм. из БД";
            CloseUnitOfMeasurementCommand = new NotifyCommand<CommandArgs>((cm_args) => _appObjectsModel.CloseObjCommand.Execute(cm_args));
            CloseUnitOfMeasurementCommand.Name = "Закрыть ед.изм.";

            CreateNewTypeOfFileCommand = new NotifyCommand<CommandArgs>(OnCreateNewTypeOfFile);
            CreateNewTypeOfFileCommand.Name = "Добавить новый тип файла";
            RemoveTypeOfFileCommand = new NotifyCommand<CommandArgs>((cm_args) => _appObjectsModel.RemoveObjCommand.Execute(cm_args));
            RemoveTypeOfFileCommand.Name = "Удалить тип файла";
            CloseTypeOfFileCommand = new NotifyCommand<CommandArgs>((cm_args) => _appObjectsModel.CloseObjCommand.Execute(cm_args));
            CloseTypeOfFileCommand.Name = "Закрыть тип файла";

            OpenAsMaterialCertificateAggregationDocumentCommand = new NotifyCommand<CommandArgs>(OnOpenAsMaterialCertificateAggregationDocument);
            OpenAsMaterialCertificateAggregationDocumentCommand.Name = "Открыть как перечень сертификатов/паспартов";


            AllModels.Add(Documentation);
            DataItem Root = new DataItem();
            Root.AttachedObject = AllModels;
            Items = new DataItemCollection(null);
            Items.Add(Root);

            UnDoReDo.SaveAll();


        }
        private void OnOpenAsMaterialCertificateAggregationDocument(CommandArgs cm_args)
        {
          if (cm_args != null&& cm_args.Entity is bldAggregationDocument aggregationDocument)
            {
                NavigationParameters navParam = new NavigationParameters();
                navParam.Add("bld_agrregation_document", (new ConveyanceObject(aggregationDocument, ConveyanceObjectModes.EditMode.FOR_EDIT)));
                navParam.Add("title", "Перечень сертификатов/паспортов");
                _regionManager.RequestNavigate(RegionNames.ContentRegion, typeof(MaterialCertificateAggregationDocumentsView).Name, navParam);
            }
        }

        private void OnCreateNewAggregationDocument(CommandArgs cm_args)
        {
            cm_args.Type = typeof(bldAggregationDocument);
            _appObjectsModel.CreateNewCommand.Execute(cm_args);
        }
        private void OnCreateNewUnitOfMeasurement(CommandArgs cm_args)
        {
             cm_args.Type = typeof(bldUnitOfMeasurement);
            _appObjectsModel.CreateNewCommand.Execute(cm_args);
        }
        private void OnCreateNewTypeOfFile(CommandArgs cm_args)
        {
            cm_args.Type = typeof(TypeOfFile);
            _appObjectsModel.CreateNewCommand.Execute(cm_args);
        }

        private void OnLoadAggregationDocumentFromDB(CommandArgs cm_args)
        {
            object selected_object = null;
            if (cm_args == null)
                selected_object = Documentation;
            else
                selected_object = cm_args.Entity;
            bldAggregationDocumentsGroup All_AggregationDocuments = new bldAggregationDocumentsGroup(
                _buildingUnitsRepository.DocumentsRepository.AggregationDocuments.Select()
                .Include(ad => ad.AttachedDocuments)
                .ThenInclude(mc => mc.ImageFile)
                .ThenInclude(mc => mc.FileType).ToList()
                .Where(d => d.AttachedDocuments.Count > 0 && d.AttachedDocuments[0].GetType() == typeof(bldMaterialCertificate) &&
                !Documentation.Where(dc => dc.Id == d.Id).Any()).ToList()); ;
            CoreFunctions.SelectElementFromCollectionWhithDialog<bldAggregationDocumentsGroup, bldAggregationDocument>
                      (All_AggregationDocuments, _dialogService, (result) =>
                      {
                          if (result.Result == ButtonResult.Yes)
                          {
                              bldAggregationDocument selected_aggregation_doc = result.Parameters.GetValue<bldAggregationDocument>("element");
                              bldAggregationDocument loaded_doc = selected_aggregation_doc;
                              if (loaded_doc != null)
                              {

                                  if (selected_object is bldDocument document
                                  && !document.AttachedDocuments.Where(d => d.Id == loaded_doc.Id).Any())
                                  {
                                      loaded_doc.IsDbBranch = true;
                                      document.AddDocument(loaded_doc);
                                  }
                                  if (selected_object is bldDocumentsGroup doc_coll
                                  && !doc_coll.Where(el => el.Id == loaded_doc.Id).Any())
                                  {
                                      loaded_doc.IsDbBranch = true;
                                      doc_coll.Add(loaded_doc as bldDocument);
                                  }
                                  UnDoReDo.Save(loaded_doc); //Сохраняемся после довбалвения в коллецию
                                  loaded_doc.State = OpenWorkLib.Data.Service.EntityState.Unchanged;
                              }
                          }
                      }, typeof(SelectAggregationDocumentFromCollectionDialogView).Name,
                      "Выберете каталог для сохранения",
                         "Форма для загрузки ведомости документации из базы данных.",
                         "Перечень каталогов");

        }

        private void OnEditInTreeViewItem(CommandArgs cm_args)
        {
             if (cm_args.Buffet!=null)
            {
                ContextMenu contextMenu = cm_args.Buffet as ContextMenu;
                RadTreeViewItem selected_treeViewItem = contextMenu.PlacementTarget as RadTreeViewItem;
                selected_treeViewItem.IsInEditMode = true;
            }
            else return;

        }

        #region Mouse methods
        private void OnMouseDoubleClick(object d_clicked_object)
        {
            switch (d_clicked_object?.GetType().Name)
            {
                case (nameof(bldAggregationDocument)):
                    {
                        bldAggregationDocument aggregationDocument = d_clicked_object as bldAggregationDocument;
                        UnDoReDo.Save(aggregationDocument);
                        //  if (aggregationDocument.AttachedDocuments.Count > 0 && aggregationDocument.AttachedDocuments[0] is bldMaterialCertificate)
                        {
                            NavigationParameters navParam = new NavigationParameters();
                            navParam.Add("bld_agrregation_document", (new ConveyanceObject(aggregationDocument, ConveyanceObjectModes.EditMode.FOR_EDIT)));
                            navParam.Add("title", "Общий перечень документов");
                            //                   navParam.Add("parant_undoredo_system", (new ConveyanceObject(UnDoReDo, ConveyanceObjectModes.EditMode.FOR_EDIT)));
                            _regionManager.RequestNavigate(RegionNames.ContentRegion, typeof(AggregationDocumentsView).Name, navParam);
                        }
                        break;
                    }
            }
        }
        private void OnContextMenuOpened(object obj)
        {
            DataItem clicked_dataItem = ((IList)obj)[1] as DataItem;
            ContextMenu contextMenu = ((IList)obj)[0] as ContextMenu;
            NotifyMenuCommands context_menu_item_commands = null;
            switch (clicked_dataItem.AttachedObject.GetType().Name)
            {
                case (nameof(bldDocumentsGroup)):
                case (nameof(bldAggregationDocumentsGroup)):
                case (nameof(bldMaterialCertificatesGroup)):
                    {
                        context_menu_item_commands = new NotifyMenuCommands()
                            {

                             CreateNewAggregationDocumentCommand,
                             LoadAggregationDocumentFromDBCommand
                            };
                        break;
                    }
                case (nameof(bldDocument)):
                case (nameof(bldAggregationDocument)):

                    {
                        context_menu_item_commands = new NotifyMenuCommands()
                            {
                              CreateNewAggregationDocumentCommand,
                              CreateBasedOnCommand,
                              CloseAggregationDocumentCommand,
                              OpenAsMaterialCertificateAggregationDocumentCommand,
                              RemoveAggregationDocumentCommand
                            };
                        break;

                    }
                case (nameof(bldUnitOfMeasurement)):
                    {
                        context_menu_item_commands = new NotifyMenuCommands()
                            {
                               CreateBasedOnCommand,
                               CloseUnitOfMeasurementCommand,
                               RemoveUnitOfMeasurementCommand
                            };

                        break;
                    }
                case (nameof(bldUnitOfMeasurementsGroup)):
                    {
                        context_menu_item_commands = new NotifyMenuCommands()
                            {
                               CreateNewUnitOfMeasurementCommand
                            };

                        break;
                    }
                case (nameof(TypesOfFileGroup)):
                    {
                        context_menu_item_commands = new NotifyMenuCommands()
                            {
                              CreateNewTypeOfFileCommand,
                            };

                        break;
                    }
                case (nameof(TypeOfFile)):
                    {
                        context_menu_item_commands = new NotifyMenuCommands()
                            {
                              CreateBasedOnCommand,
                              CloseTypeOfFileCommand,
                              RemoveTypeOfFileCommand
                            };

                        break;
                    }

            }
            context_menu_item_commands?.Add(EditInTreeViewItemCommand);
            contextMenu.ItemsSource = context_menu_item_commands;

        }
        #endregion

        public virtual void OnSave()
        {
            base.OnSave("документации");
        }
        private void NavgationCoplete(NavigationResult obj)
        {
            //      throw new NotImplementedException();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            object bld_object = navigationContext.Parameters["bld_object"];
            //  if (bld_project != null && bld_Projects.Where(pr => pr.Id == bld_project.Id).FirstOrDefault() == null && bld_project != null)
            switch (bld_object.GetType().Name)
            {
                case (nameof(bldAggregationDocument)):
                    {
                        var bld_aggregationDoc = bld_object as bldAggregationDocument;
                        var doc = Documentation.Where(doc => doc.Id == bld_aggregationDoc.Id).FirstOrDefault();
                        if (doc == null)
                            Documentation.Add(bld_aggregationDoc);
                        break;
                    }
            }


        }
        public override void OnIsActiveChanged()
        {
            if (IsActive)
            {
                _applicationCommands.LoadAggregationDocumentsFromDBCommand.RegisterCommand(LoadAggregationDocumentFromDBCommand);

                _applicationCommands.ReDoCommand.RegisterCommand(ReDoCommand);
                _applicationCommands.UnDoCommand.RegisterCommand(UnDoCommand);
                _applicationCommands.SaveAllCommand.RegisterCommand(SaveCommand);
            }
            else
            {
                _applicationCommands.LoadAggregationDocumentsFromDBCommand.UnregisterCommand(LoadAggregationDocumentFromDBCommand);

                _applicationCommands.ReDoCommand.UnregisterCommand(ReDoCommand);
                _applicationCommands.UnDoCommand.UnregisterCommand(UnDoCommand);
                _applicationCommands.SaveAllCommand.UnregisterCommand(SaveCommand);
            }
            base.OnIsActiveChanged();

        }

        
    }
}
