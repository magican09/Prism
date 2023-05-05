using Microsoft.EntityFrameworkCore;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Modules.BuildingModule.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Windows.Controls;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class FindDocumentViewModel : BaseViewModel<IEntityObject>, INavigationAware
    {
        private AggregationDocumentsViewModel _documentsViewModel;
        public AggregationDocumentsViewModel DocumentsViewModel
        {
            get { return _documentsViewModel; }
            set { SetProperty(ref _documentsViewModel, value); }
        }
        private ObservableCollection<bldDocument> _selectedDocuments = new ObservableCollection<bldDocument>();
        public ObservableCollection<bldDocument> SelectedDocuments
        {
            get { return _selectedDocuments; }
            set { SetProperty(ref _selectedDocuments, value); }
        }
        private ObservableCollection<bldDocument> _findedDocuments = new ObservableCollection<bldDocument>();
        public ObservableCollection<bldDocument> FindedDocuments
        {
            get { return _findedDocuments; }
            set { SetProperty(ref _findedDocuments, value); }
        }
        private string _searchString ="";
        public string SearchString
        {
            get { return _searchString; }
            set { SetProperty(ref _searchString, value); }
        }
        private bool _isDatePanelEnable = false;
        public bool IsDatePanelEnable
        {
            get { return _isDatePanelEnable; }
            set { SetProperty(ref _isDatePanelEnable, value); }
        }
        private DateTime _docStarTime = DateTime.Now;
        public DateTime DocStarTime
        {
            get { return _docStarTime; }
            set { SetProperty(ref _docStarTime, value); }
        }
        private DateTime _endStarTime = DateTime.Now;
        public DateTime EndStarTime
        {
            get { return _endStarTime; }
            set { SetProperty(ref _endStarTime, value); }
        }
        private KeyValuePair<string, Type> _selectedDocTypeName;
        public KeyValuePair<string, Type> SelectedDocTypeName
        {
            get { return _selectedDocTypeName; }
            set
            {
                SetProperty(ref _selectedDocTypeName, value);

                for (int ii = AggragationDocumentsForSearch.Count - 1; ii > 1; ii--)
                {
                    AggragationDocumentsForSearch.Remove(AggragationDocumentsForSearch[ii]);
                }
                // var coll_3 = _buildingUnitsRepository.DocumentsRepository.Select().Where(d => d.ParentDocuments.Where(pd=>pd is bldAggregationDocument).Any());
                //  var coll = _buildingUnitsRepository.DocumentsRepository.Select().Where(d => d is bldAggregationDocument).Include(ad => ad.AttachedDocuments).ToList();
                //var agr_docs = coll.Where(dd => dd.AttachedDocuments.ContainsObjectWithType(_selectedDocTypeName.Value)).ToList();
                var agr_docs = _buildingUnitsRepository.DocumentsRepository.Select().Where(d => d is bldAggregationDocument);
                foreach (bldAggregationDocument aggregationDocument in agr_docs)
                    AggragationDocumentsForSearch.Add(aggregationDocument);
                ColumnNames.Clear();
                switch (_selectedDocTypeName.Value.Name)
                {
                    case (nameof(bldMaterialCertificate)):
                        {
                            ColumnNames.Add("Наименование материала", "MaterialName");
                            ColumnNames.Add("Геом. параменты", "GeometryParameters");
                            ColumnNames.Add("Контр. парам.", "ControlingParament");
                            ColumnNames.Add("ГОСТ,ТУ...", "RegulationDocumentsName");
                            break;
                        }
                    case (nameof(bldAggregationDocument)):
                    case (nameof(bldLaboratoryReport)):
                    case (nameof(bldExecutiveScheme)):
                        {
                            ColumnNames.Add("Наименование", "Name");
                            break;
                        }
                }


            }
        }
        private Dictionary<string, Type> _docTypeNames = new Dictionary<string, Type>();
        public Dictionary<string, Type> DocTypeNames
        {
            get { return _docTypeNames; }
            set { SetProperty(ref _docTypeNames, value); }
        }

        private bldAggregationDocument _selectedAggragationDocumentForSearch;
        public bldAggregationDocument SelectedAggragationDocumentForSearch
        {
            get { return _selectedAggragationDocumentForSearch; }
            set { SetProperty(ref _selectedAggragationDocumentForSearch, value); }
        }
        private ObservableCollection<bldAggregationDocument> _aggragationDocumentsForSearch = new ObservableCollection<bldAggregationDocument>();
        public ObservableCollection<bldAggregationDocument> AggragationDocumentsForSearch
        {
            get { return _aggragationDocumentsForSearch; }
            set { SetProperty(ref _aggragationDocumentsForSearch, value); }
        }

        private KeyValuePair<string, string> _selectedColumnName;
        public KeyValuePair<string, string> SelectedColumnName
        {
            get { return _selectedColumnName; }
            set { SetProperty(ref _selectedColumnName, value); }
        }
        private Dictionary<string, string> _columnNames = new Dictionary<string, string>();
        public Dictionary<string, string> ColumnNames
        {
            get { return _columnNames; }
            set { SetProperty(ref _columnNames, value); }
        }
    
        public NotifyCommand<object> DataGridSelectionChangedCommand { get; private set; }
        public NotifyCommand<object> ContextMenuOpenedCommand { get; private set; }
        public NotifyCommand FindDocumentsCommand { get; private set; }
        public NotifyCommand<object>  AddDocumentToAppModelCommand { get; set; }
    
        public NotifyMenuCommands AggregationDocumentMenuCommands { get; set; }

        AppObjectsModel _appObjectsModel;
        private IBuildingUnitsRepository _buildingUnitsRepository;

        public FindDocumentViewModel(IDialogService dialogService,
         IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository, IApplicationCommands applicationCommands, IAppObjectsModel appObjectsModel)
        {
            ApplicationCommands = applicationCommands;
            _appObjectsModel = appObjectsModel as AppObjectsModel;
            _dialogService = dialogService;
            _buildingUnitsRepository = buildingUnitsRepository;
            _regionManager = regionManager;
            _appObjectsModel = appObjectsModel as AppObjectsModel;
            DocumentsViewModel = new AggregationDocumentsViewModel(dialogService, regionManager, buildingUnitsRepository, applicationCommands, appObjectsModel);
            bldAggregationDocument aggregationDocument = new bldAggregationDocument();
            DocumentsViewModel.SelectedAggregationDocument = aggregationDocument;
           
            DataGridSelectionChangedCommand = new NotifyCommand<object>(OnDataGridSelectionChanged);
            ContextMenuOpenedCommand = DocumentsViewModel.ContextMenuOpenedCommand;
            FindedDocuments = DocumentsViewModel.SelectedAggregationDocument.AttachedDocuments;
            FindDocumentsCommand = new NotifyCommand(OnFindDocuments);//, () => SearchString != "").ObservesProperty(()=>SearchString);

            AddDocumentToAppModelCommand = new NotifyCommand<object>(OnAddDocumentToAppModel,(ob)=> SelectedDocuments.Count>0)
                .ObservesPropertyChangedEvent(SelectedDocuments);
            AddDocumentToAppModelCommand.Name = "Добавить в модель";
            AggregationDocumentMenuCommands = DocumentsViewModel.AggregationDocumentMenuCommands;
            AggregationDocumentMenuCommands.Clear();
            AggregationDocumentMenuCommands.Add(AddDocumentToAppModelCommand);

            DocTypeNames.Add("Перечень", typeof(bldAggregationDocument));
            DocTypeNames.Add("Сертификат/Паспорт", typeof(bldMaterialCertificate));
            DocTypeNames.Add("Лабораторные испытания", typeof(bldLaboratoryReport));

            bldAggregationDocument temp_ADoc = new bldAggregationDocument("Искать везде");
            AggragationDocumentsForSearch.Add(temp_ADoc);
            SelectedAggragationDocumentForSearch = temp_ADoc;
            Title = "Поиск документа";
        }

        private void OnAddDocumentToAppModel(object obj)
        {
            _dialogService.ShowDialog(nameof(SelectObjectInAppModelDialogView),(result)=>
            {
                if(result.Result== ButtonResult.Yes)
                {
                    IEntityObject selected_element = result.Parameters.GetValue<IEntityObject>("element");
                    if (selected_element is INameableObservableCollection coll_element)
                    {
                        foreach (bldDocument document in SelectedDocuments)
                          if (!selected_element.ContainsDownWard(document) && !selected_element.ContainsUpWard(document))
                                    coll_element.Add(document);
                    }
                    if (selected_element is bldDocument selected_document)
                    {
                        foreach (bldDocument document in SelectedDocuments)
                            if (!selected_element.ContainsDownWard(document) && !selected_element.ContainsUpWard(document))
                                selected_document.AttachedDocuments.Add(document);
                    }
                }
            }
            );
        }

        private void OnDataGridSelectionChanged(object obj)
        {
            bldDocument selected_certificate = ((IList)obj)[0] as bldDocument;
            bldDocument selected_aggr_document = ((IList)obj)[1] as bldDocument;
            SelectedDocuments.Clear();
            foreach (object elm in ((IList)obj)[2] as IList)
                SelectedDocuments.Add(elm as bldDocument);
        }
        private void OnFindDocuments()
        {

            ObservableCollection<bldDocument> docs;

            //  docs = new ObservableCollection<bldDocument>(_buildingUnitsRepository.DocumentsRepository.Select().Where(ad =>ad is bldMaterialCertificate 
            //  &&  EF.Functions.Like(ad.Name , $"%{SearchString}%")).ToList());
            if (SearchString == null) SearchString = "";
            StringComparison comparisonType = StringComparison.InvariantCultureIgnoreCase;
            docs = new ObservableCollection<bldDocument>(_buildingUnitsRepository.DocumentsRepository.Select(SelectedDocTypeName.Value)
            .Where($"doc =>doc.{SelectedColumnName.Value}.ToLower().Contains(@0)", SearchString.ToLower())
            .Include(d=>d.AttachedDocuments)
            .Include(d => d.ImageFile).ThenInclude(d => d.FileType).ToList());


            FindedDocuments.Clear();
            foreach (bldDocument document in docs)
                FindedDocuments.Add(document);
        }


        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }
    }
}
