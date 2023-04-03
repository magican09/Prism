using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Modules.BuildingModule.Core;
using PrismWorkApp.Modules.BuildingModule.Dialogs;
using PrismWorkApp.Modules.BuildingModule.Views;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Telerik.Windows.Controls;

namespace PrismWorkApp.Modules.BuildingModule
{
    public class AppObjectsModel :BindableBase, IAppObjectsModel
    {
        public bldDocumentsGroup Documentation { get; set; } = new bldDocumentsGroup();
        private bldDocument _selectedDocument;
        public bldDocument SelectedDocument
        {
            get { return _selectedDocument; }
            set { SetProperty(ref _selectedDocument, value); }
        }
        private bldDocumentsGroup _selectedDocumentsGroup = new bldDocumentsGroup();
        public bldDocumentsGroup SelectedDocumentsGroup 
        {
            get { return _selectedDocumentsGroup; }
            set { SetProperty(ref _selectedDocumentsGroup, value); }
        }
        #region Commands 
        public NotifyMenuCommands DocumentsGroupCommands { get; set; } = new NotifyMenuCommands();
    
        public NotifyCommand<object> LoadAggregationDocumentFromDBCommand { get; set; }
        public NotifyCommand<object> CreateNewAggregationDocumentCommand { get; set; }
        public NotifyCommand<object> RemoveAggregationDocumentCommand { get; private set; }
        public NotifyCommand<object> LoadDocumentCommand { get; set; }

        #endregion
        #region Contructors
        private IApplicationCommands _applicationCommands;
        public IBuildingUnitsRepository _buildingUnitsRepository { get; }
        private readonly IRegionManager _regionManager;
        private IDialogService _dialogService;
        public AppObjectsModel(IRegionManager regionManager, IEventAggregator eventAggregator,
                                           IBuildingUnitsRepository buildingUnitsRepository, IDialogService dialogService, IApplicationCommands applicationCommands)
        {
            _regionManager = regionManager;
            _buildingUnitsRepository = buildingUnitsRepository;
            _dialogService = dialogService;
            _applicationCommands = applicationCommands;
           
            Documentation.Name = "Документация";
            LoadAggregationDocumentFromDBCommand = new NotifyCommand<object>(OnAggregationDocumentFromDB);
            LoadAggregationDocumentFromDBCommand.Name = "Загрузить перечень документов из БД";
            CreateNewAggregationDocumentCommand = new NotifyCommand<object>(OnCreateNewAggregationDocument);
            CreateNewAggregationDocumentCommand.Name = "Создать новый список документов";

            RemoveAggregationDocumentCommand = new NotifyCommand<object>(OnRemoveAggregationDocument);
            RemoveAggregationDocumentCommand.Name = "Удалить список документов";
            
           // DocumentsGroupCommands.Add(CreateNewDocumentsGroupCommand);
            //DocumentsGroupCommands.Add(RemoveDocumentsGroupCommand);
           // DocumentsGroupCommands.Add(LoadDocumentsGroupFromDBCommand);
          
        }

        private void OnRemoveAggregationDocument(object clicked_object)
        {
            switch(clicked_object?.GetType().Name)
            {
                case (nameof(bldDocumentsGroup)):
                case (nameof(bldMaterialCertificatesGroup)):
                case (nameof(bldAggregationDocumentsGroup)):
                    {
                        bldDocumentsGroup documents = clicked_object as bldDocumentsGroup;


                        break;
                    }


            }

        }

        private void OnCreateNewAggregationDocument(object clicked_object)
        {
          
        if (clicked_object is IbldDocumentsGroup)
            {
                bldAggregationDocument new_agr_doc  =  new bldAggregationDocument();
                new_agr_doc.Name = "Новый каталог";
                (clicked_object as IbldDocumentsGroup).Add(new_agr_doc as bldDocument);
            }
        }

        private void OnAggregationDocumentFromDB(object selected_object)
        {

            switch (selected_object?.GetType().Name)
            {
                case (nameof(bldDocumentsGroup)):
                case (nameof(bldDocument)):
                case (nameof(bldAggregationDocumentsGroup)):
                    {
                       
                        bldAggregationDocumentsGroup All_AggregationDocuments = new bldAggregationDocumentsGroup(_buildingUnitsRepository.AggregationDocumentsRepository.GetAllAsync().ToList());

                        CoreFunctions.SelectElementFromCollectionWhithDialog<bldAggregationDocumentsGroup, bldAggregationDocument>
                                  (All_AggregationDocuments, _dialogService, (result) =>
                                  {
                                      if (result.Result == ButtonResult.Yes)
                                      {
                                          bldAggregationDocument selected_aggregation_doc = result.Parameters.GetValue<bldAggregationDocument>("element");

                                          var navParam = new NavigationParameters();
                                          bldDocumentsGroup doc_collection = null;
                                          if (selected_object is bldDocument document) doc_collection = document.AttachedDocuments;
                                          if (selected_object is NameableObservableCollection<bldDocument> collection && collection.Parent?.Id != selected_aggregation_doc.Id)
                                              doc_collection = selected_object as bldDocumentsGroup;
                                         if(doc_collection!=null && doc_collection.Where(d=>d.Id== selected_aggregation_doc.Id).FirstOrDefault()==null)
                                           doc_collection.Add(selected_aggregation_doc);
                                          //navParam.Add("bld_document", selected_catalog);
                                          //_regionManager.RequestNavigate(RegionNames.SolutionExplorerRegion, typeof(DocumentationExplorerView).Name, navParam);
                                      }

                                  }, typeof(SelectAggregationDocumentFromCollectionDialogView).Name,
                                  "Выберете каталог для сохранения",
                                     "Форма для выбора каталога для загзузки из базы данных."
                                    , "Перечень каталогов");

                        break;
                    }


            }

          
        }


        #endregion
        #region Model data 
        #region Documentation
        /// <summary>
        /// Коллекция для хранения документации
        /// </summary>
        #endregion

        #endregion
        #region DataItems imlemantation
        static private GetImageFrombldProjectObjectConvecter ObjecobjectTo_Url_Convectert = new GetImageFrombldProjectObjectConvecter();
        /// <summary>
        /// Шаблон построения дерева DataItems для TreeView в форме метода, который вызываеся каждый при инициализации или 
        /// обновления DataItem или вызове IPropertyChanged, ICollectionChanged прикрепленных к DataItem объектов
        /// </summary>
        /// <param name="dataItem"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnDataItemInit(DataItem dataItem, object sender, PropertyChangedEventArgs e)
        {
            switch (dataItem.AttachedObject.GetType().Name)
            {

                case (nameof(bldAggregationDocument)):
                    {
                        bldAggregationDocument document = ((bldAggregationDocument)dataItem.AttachedObject);
                        dataItem.Text = document.Name;
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
                        dataItem.Text = documents.Name;
                        dataItem.ImageUrl = (Uri)ObjecobjectTo_Url_Convectert.Convert(dataItem.AttachedObject, null, null, CultureInfo.CurrentCulture);
                        foreach (bldDocument doc in documents)
                        {
                            DataItem atch_doc_item = new DataItem();
                            dataItem.Items.Add(atch_doc_item);
                            atch_doc_item.AttachedObject = doc;
                         
                        }
                        break;
                    }
                case (nameof(bldMaterialCertificate)):
                    {
                        bldMaterialCertificate document = ((bldMaterialCertificate)dataItem.AttachedObject);
                        dataItem.Text = document.MaterialName;
                        dataItem.ImageUrl = (Uri)ObjecobjectTo_Url_Convectert.Convert(dataItem.AttachedObject, null, null, CultureInfo.CurrentCulture);
                        break;
                    }
            }
        }
        #endregion

        #region ContextMenu Implamatation

        #endregion
    }
}
