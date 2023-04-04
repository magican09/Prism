using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Modules.BuildingModule.Core;
using PrismWorkApp.Modules.BuildingModule.Dialogs;
using PrismWorkApp.Modules.BuildingModule.Views;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
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
    public class AppObjectsModel : BindableBase, IAppObjectsModel
    {
        private bldDocumentsGroup _documentation = new bldDocumentsGroup();
        public bldDocumentsGroup Documentation
        {
            get { return _documentation; }
            set { SetProperty(ref _documentation, value); }
        }

        //private bldDocument _selectedDocument;
        //public bldDocument SelectedDocument
        //{
        //    get { return _selectedDocument; }
        //    set { SetProperty(ref _selectedDocument, value); }
        //}
        //private bldDocumentsGroup _selectedDocumentsGroup = new bldDocumentsGroup();
        //public bldDocumentsGroup SelectedDocumentsGroup
        //{
        //    get { return _selectedDocumentsGroup; }
        //    set { SetProperty(ref _selectedDocumentsGroup, value); }
        //}

        #region Commands 
        //        public NotifyMenuCommands DocumentsGroupCommands { get; set; } = new NotifyMenuCommands();



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


            SaveDocumentationToDBCommand = new NotifyCommand(OnSaveDocumentationToDB);
            LoadAggregationDocumentFromDBCommand = new NotifyCommand<object>(OnLoadAggregationDocumentFromDB);
            LoadAggregationDocumentFromDBCommand.Name = "Загрузить ведомость документов из БД";
            CreateNewAggregationDocumentCommand = new NotifyCommand<object>(OnCreateNewAggregationDocument);
            CreateNewAggregationDocumentCommand.Name = "Добавить новую ведомость документов";
            RemoveAggregationDocumentCommand = new NotifyCommand<object>(OnRemoveAggregationDocument);
            RemoveAggregationDocumentCommand.Name = "Удалить ведомость документов";

            _applicationCommands.SaveAllToDBCommand.RegisterCommand(SaveDocumentationToDBCommand);

        }
        #region bldDocumentaation  services
      
        public NotifyCommand SaveDocumentationToDBCommand { get; private set; }
        
        public NotifyCommand<object> LoadAggregationDocumentFromDBCommand { get; set; }
        public NotifyCommand<object> CreateNewAggregationDocumentCommand { get; set; }
        public NotifyCommand<object> RemoveAggregationDocumentCommand { get; private set; }
        public NotifyCommand<object> LoadDocumentCommand { get; set; }

        private void OnRemoveAggregationDocument(object selected_object)
        {

            CoreFunctions.RemoveFromParentObject(((IHierarchical)selected_object).Parent, selected_object,
                "ведомость", ((INameable)selected_object).Name, _dialogService);

        }
        private void OnCreateNewAggregationDocument(object selected_object)
        {
            if (selected_object is IbldDocumentsGroup)
            {
                bldAggregationDocument new_agr_doc = new bldAggregationDocument();
                new_agr_doc.Name = "Новый каталог";
                new_agr_doc.Id = Guid.NewGuid();
                (selected_object as IbldDocumentsGroup).Add(new_agr_doc as bldDocument);
            }
        }
        private void OnLoadAggregationDocumentFromDB(object selected_object)
        {
            switch (selected_object?.GetType().Name)
            {
                case (nameof(bldDocumentsGroup)):
                case (nameof(bldDocument)):
                case (nameof(bldAggregationDocumentsGroup)):
                    {
                        bldAggregationDocumentsGroup All_AggregationDocuments = new bldAggregationDocumentsGroup(_buildingUnitsRepository.DocumentsRepository.AggregationDocuments.GetAllAsync().ToList());

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
                                          if (doc_collection != null && doc_collection.Where(d => d.Id == selected_aggregation_doc.Id).FirstOrDefault() == null)
                                              doc_collection.Add(selected_aggregation_doc);
                                      }
                                  }, typeof(SelectAggregationDocumentFromCollectionDialogView).Name,
                                  "Выберете каталог для сохранения",
                                     "Форма для загрузки ведомости документации из базы данных."
                                    , "Перечень каталогов");
                        break;
                    }
            }
        }
        #endregion



        #endregion
        #region Model data 
        #region Documentation
        /// <summary>
        /// Коллекция для хранения документации
        /// </summary>
        #endregion

        #endregion

     
        #region Save 
        public void  OnSaveDocumentationToDB()
        {
            foreach(bldDocument document in Documentation)
            {
                if (_buildingUnitsRepository.DocumentsRepository.Get(document.Id) == null)
                    _buildingUnitsRepository.DocumentsRepository.Add(document);
            }
            _buildingUnitsRepository.Complete();
        }

        #endregion
    }
}
