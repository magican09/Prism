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
using DialogParameters = Prism.Services.Dialogs.DialogParameters;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class DocumentationExplorerViewModel : BaseViewModel<object>, INotifyPropertyChanged, INavigationAware//, IConfirmNavigationRequest
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

        private string _title = "Документация";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
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
        public NotifyCommand<object> CloseCommand { get; protected set; }

        public NotifyMenuCommands DocumentationCommands { get; set; }

        public NotifyCommand<object> ContextMenuOpenedCommand { get; private set; }
        public NotifyCommand<object> MouseDoubleClickCommand { get; private set; }


        public NotifyCommand<object> EditInTreeViewItemCommand { get; private set; }
        public NotifyCommand<object> LoadAggregationDocumentFromDBCommand { get; set; }
        public NotifyCommand SaveDocumentationToDBCommand { get; private set; }

        public NotifyCommand<object> CreateNewAggregationDocumentCommand { get; set; }
        public NotifyCommand<object> RemoveAggregationDocumentCommand { get; private set; }

        private readonly IEventAggregator _eventAggregator;
        private AppObjectsModel _appObjectsModel;
        public AppObjectsModel AppObjectsModel
        {
            get { return _appObjectsModel; }
            set { SetProperty(ref _appObjectsModel, value); }
        }


        private IApplicationCommands _applicationCommands;
        IBuildingUnitsRepository _buildingUnitsRepository;
        public DocumentationExplorerViewModel(IEventAggregator eventAggregator,
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
            Documentation = AppObjectsModel.Documentation.AttachedDocuments;
           
            UnDoCommand = new NotifyCommand(() => { UnDoReDo.UnDo(1); },
                                   () => { return UnDoReDo.CanUnDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            ReDoCommand = new NotifyCommand(() => UnDoReDo.ReDo(1),
              () => { return UnDoReDo.CanReDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            UnDoCommand.MonitorCommandActivity = false;
            ReDoCommand.MonitorCommandActivity = false;

            SaveDocumentationToDBCommand = new NotifyCommand(OnSaveDocumentationToDB);
            SaveDocumentationToDBCommand.MonitorCommandActivity = false;

            SaveCommand = new NotifyCommand(OnSave);

            ContextMenuOpenedCommand = new NotifyCommand<object>(OnContextMenuOpened);
            MouseDoubleClickCommand = new NotifyCommand<object>(OnMouseDoubleClick);
            EditInTreeViewItemCommand = new NotifyCommand<object>(OnEditInTreeViewItem);
            EditInTreeViewItemCommand.Name = "Редактировать";

            LoadAggregationDocumentFromDBCommand = new NotifyCommand<object>(OnLoadAggregationDocumentFromDB);
            LoadAggregationDocumentFromDBCommand.Name = "Загрузить ведомость документов из БД";
            CreateNewAggregationDocumentCommand = new NotifyCommand<object>(OnCreateNewAggregationDocument);
            CreateNewAggregationDocumentCommand.Name = "Добавить новую ведомость документов";
            RemoveAggregationDocumentCommand = new NotifyCommand<object>(OnRemoveAggregationDocument);
            RemoveAggregationDocumentCommand.Name = "Удалить ведомость документов";


            Items = new DataItemCollection(null);
            DataItem root = new DataItem();
            root.DataItemInit += OnDataItemInit;
            Items.Add(root);
            root.AttachedObject = Documentation;

            UnDoReDo.Register(Documentation, true);
            _applicationCommands.SaveAllToDBCommand.RegisterCommand(SaveDocumentationToDBCommand);
            _applicationCommands.ReDoCommand.RegisterCommand(ReDoCommand);
            _applicationCommands.UnDoCommand.RegisterCommand(UnDoCommand);
            _applicationCommands.SaveAllCommand.RegisterCommand(SaveCommand);
        }

        private void OnRemoveAggregationDocument(object obj)
        {
            object selected_object = null;
            if (obj is IList list) selected_object = list[0]; else selected_object = obj;
            if (selected_object is bldDocument document)
            {
                int ch_namber = UnDoReDo.GetChangesNamber(document); //Отнимает 1, так как в изменениях 
                if (ch_namber != 0)
                {
                    CoreFunctions.ConfirmChangesDialog(_dialogService, "документе",
                        (result) =>
                        {
                            if(result.Result==ButtonResult.Yes|| result.Result == ButtonResult.No)
                            {
                                foreach (object parent in new List<object>(document.Parents))
                                {
                                    if (parent is bldDocument parent_doc)
                                        parent_doc.RemoveDocument(document);
                                    if (parent is IList list_parent)
                                        list_parent.Remove(document);
                                }
                                if (result.Result == ButtonResult.Yes)
                                {
                                    UnDoReDo.Save(document);
                                    UnDoReDo.UnRegister(document);
                                    var dialog_par = new DialogParameters();
                                    dialog_par.Add("message", $"{ch_namber.ToString()} изменения(й) сохранено!");
                                    _dialogService.ShowDialog(nameof(MessageDialog), dialog_par, (result) => { });
                                }
                                else
                                {
                                   // UnDoReDo.UnDoAll(document);
                                    UnDoReDo.UnRegister(document);
                                    UnDoReDo.UnRegister(document);
                                    var dialog_par = new DialogParameters();
                                    dialog_par.Add("message", $"{ch_namber.ToString()} изменения(й) сохранено!");
                                    _dialogService.ShowDialog(nameof(MessageDialog), dialog_par, (result) => { });
                                }
                              
                            }
                            if (result.Result == ButtonResult.Cancel)
                            { 

                            }
                        });

                }
                    
               
               
             
              
            }
        }

        private void OnCreateNewAggregationDocument(object obj)
        {
            object selected_object = null;
            if (obj is IList list) selected_object = list[0]; else selected_object = obj;
            if (selected_object is bldDocument document)
            {
                document.AddNewDocument<bldAggregationDocument>();
             //   UnDoReDo.Register(document.AddNewDocument<bldAggregationDocument>());
            }
            else if (selected_object is bldDocumentsGroup documents_coll)
                documents_coll.Add(new bldAggregationDocument("Новый перечень документации"));

        }

        private void OnLoadAggregationDocumentFromDB(object obj)
        {
            object selected_object = null;
            if (obj is IList list) selected_object = list[0]; else selected_object = obj;
            var command = AppObjectsModel.LoadAggregationDocumentFromDBCommand;
            bldAggregationDocumentsGroup All_AggregationDocuments = new bldAggregationDocumentsGroup(
                _buildingUnitsRepository.DocumentsRepository.AggregationDocuments.GetAllAsync().ToList());
            //   All_AggregationDocuments.SaveChanges();
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
                                  && !document.AttachedDocuments.Where(d=>d.Id==document.Id).Any()) 
                                      document.AddDocument(document);
                                  if (selected_object is bldDocumentsGroup doc_coll 
                                  && !doc_coll.Where(el=>el.Id==loaded_doc.Id).Any())
                                      doc_coll.Add(loaded_doc as bldDocument);
                                  UnDoReDo.Save(loaded_doc); //Сохраняемся после довбалвения в коллецию
                              }
                          }
                      }, typeof(SelectAggregationDocumentFromCollectionDialogView).Name,
                      "Выберете каталог для сохранения",
                         "Форма для загрузки ведомости документации из базы данных.",
                         "Перечень каталогов");

        }
        public void OnSaveDocumentationToDB()
        {
            CoreFunctions.ConfirmActionDialog("Сохранить все изменения в документации БД?", "Документация",
                "Сохранить", "Отмена", (result) =>
                {
                    if (result.Result == ButtonResult.Yes)
                    {
                        foreach (bldDocument document in Documentation)
                        {
                            if (_buildingUnitsRepository.DocumentsRepository.Get(document.Id) == null)
                                _buildingUnitsRepository.DocumentsRepository.Add(document);
                        }
                        _buildingUnitsRepository.Complete();
                    }
                }, _dialogService);

        }

        private void OnEditInTreeViewItem(object obj)
        {
            object selected_object = null;
            if (obj is IList list)
            {
                selected_object = list[1];
                // RadTreeView treeView  = selected_object as RadTreeView;
                ContextMenu contextMenu = selected_object as ContextMenu;
                var parent_ = LogicalTreeHelper.GetParent((DependencyObject)contextMenu);
                var parent = CoreFunctions.FindParent<RadTreeViewItem>((DependencyObject)contextMenu);
                RadTreeViewItem selected_treeViewItem = contextMenu.PlacementTarget as RadTreeViewItem;
                selected_treeViewItem.IsInEditMode = true;
            }
            else return;

        }

        #region DataItems init
        static private GetImageFrombldProjectObjectConvecter ObjecobjectTo_Url_Convectert = new GetImageFrombldProjectObjectConvecter();
        /// <summary>
        /// Шаблон построения дерева DataItems для TreeView в форме метода, который вызываеся каждый при инициализации или 
        /// обновления DataItem или вызове IPropertyChanged, ICollectionChanged прикрепленных к DataItem объектов.
        /// DataItem.DataItemInit+=OnDataItemInit;
        /// </summary>
        /// <param name="dataItem">Передается объект DataItem который необходимо инициализировать содержанием</param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnDataItemInit(DataItem dataItem, object sender, PropertyChangedEventArgs e)
        {
           // UnDoReDo.Register(dataItem.AttachedObject as IJornalable);
            switch (dataItem.AttachedObject.GetType().Name)
            {
                case (nameof(bldMaterialCertificate)):
                    {
                        bldMaterialCertificate document = ((bldMaterialCertificate)dataItem.AttachedObject);
                        Binding binding = new Binding("MaterialName");
                        binding.Source = document;
                        binding.Path = new PropertyPath("MaterialName");
                        binding.Mode = BindingMode.OneWay;
                        BindingOperations.SetBinding(dataItem, DataItem.TextProperty, binding);

                        dataItem.ImageUrl = (Uri)ObjecobjectTo_Url_Convectert.Convert(dataItem.AttachedObject, null, null, CultureInfo.CurrentCulture);
                        DataItem attachedDocs = new DataItem();
                        dataItem.Items.Add(attachedDocs);
                        attachedDocs.AttachedObject = document.AttachedDocuments;
                        break;
                    }
                case (nameof(bldAggregationDocument)):
                    {
                        bldAggregationDocument document = ((bldAggregationDocument)dataItem.AttachedObject);
                        Binding binding = new Binding("Name");
                        binding.Source = document;
                        binding.Path = new PropertyPath("Name");
                        binding.Mode = BindingMode.TwoWay;
                        BindingOperations.SetBinding(dataItem, DataItem.TextProperty, binding);

                        dataItem.ImageUrl = (Uri)ObjecobjectTo_Url_Convectert.Convert(dataItem.AttachedObject, null, null, CultureInfo.CurrentCulture);
                        DataItem attachedDocs = new DataItem();
                        dataItem.Items.Add(attachedDocs);
                        attachedDocs.AttachedObject = document.AttachedDocuments;
                        break;
                    }
                case (nameof(bldDocumentsGroup)):
                case (nameof(bldAggregationDocumentsGroup)):
                case (nameof(bldMaterialCertificatesGroup)):
                    {
                        bldDocumentsGroup documents = ((bldDocumentsGroup)dataItem.AttachedObject);
                        Binding binding = new Binding("Name");
                        binding.Source = documents;
                        binding.Path = new PropertyPath("Name");
                        binding.Mode = BindingMode.TwoWay;
                        // binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                        BindingOperations.SetBinding(dataItem, DataItem.TextProperty, binding);
                        dataItem.ImageUrl = (Uri)ObjecobjectTo_Url_Convectert.Convert(dataItem.AttachedObject, null, null, CultureInfo.CurrentCulture);
                        foreach (bldDocument doc in documents)
                        {
                            DataItem atch_doc_item = new DataItem();
                            dataItem.Items.Add(atch_doc_item);
                            atch_doc_item.AttachedObject = doc;

                        }
                        break;
                    }
                 
            }
        }
        #endregion
        #region Mouse methods
        private void OnMouseDoubleClick(object d_clicked_object)
        {
            switch (d_clicked_object?.GetType().Name)
            {
                case (nameof(bldAggregationDocument)):
                    {
                        bldAggregationDocument aggregationDocument = d_clicked_object as bldAggregationDocument;
                        if (aggregationDocument.AttachedDocuments.Count > 0 && aggregationDocument.AttachedDocuments[0] is bldMaterialCertificate)
                        {
                            NavigationParameters navParam = new NavigationParameters();
                            navParam.Add("bld_agrregation_document", (new ConveyanceObject(aggregationDocument, ConveyanceObjectModes.EditMode.FOR_EDIT)));
                            navParam.Add("parant_undoredo_system", (new ConveyanceObject(UnDoReDo, ConveyanceObjectModes.EditMode.FOR_EDIT)));
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
                             LoadAggregationDocumentFromDBCommand};
                        break;
                    }
                case (nameof(bldDocument)):
                case (nameof(bldAggregationDocument)):

                    {
                        context_menu_item_commands = new NotifyMenuCommands()
                            {
                               RemoveAggregationDocumentCommand
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
            object bld_document = (bldDocument)navigationContext.Parameters["bld_document"];
            //  if (bld_project != null && bld_Projects.Where(pr => pr.Id == bld_project.Id).FirstOrDefault() == null && bld_project != null)
            if (bld_document is bldAggregationDocument bld_aggregationDoc)
            {
                var doc = Documentation.Where(doc => doc.Id == bld_aggregationDoc.Id).FirstOrDefault();
                if (doc == null)
                {
                    Documentation.Add(bld_aggregationDoc);
                }
            }

        }
    }
}
