using Microsoft.EntityFrameworkCore;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class FindDocumentViewModel : BaseViewModel<IEntityObject>, INavigationAware
    {
        private ObservableCollection<bldDocument> _findedDocuments = new ObservableCollection<bldDocument>();
        public ObservableCollection<bldDocument> FindedDocuments
        {
            get { return _findedDocuments; }
            set { SetProperty(ref _findedDocuments, value); }
        }
        private string _searchString;
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

                for (int ii = AggragationDocumentsForSearch.Count; ii > 1; ii--)
                {
                    AggragationDocumentsForSearch.Remove(AggragationDocumentsForSearch[ii]);
                }
                 // var coll_3 = _buildingUnitsRepository.DocumentsRepository.Select().Where(d => d.ParentDocuments.Where(pd=>pd is bldAggregationDocument).Any());
                 //  var coll = _buildingUnitsRepository.DocumentsRepository.Select().Where(d => d is bldAggregationDocument).Include(ad => ad.AttachedDocuments).ToList();
                 //var agr_docs = coll.Where(dd => dd.AttachedDocuments.ContainsObjectWithType(_selectedDocTypeName.Value)).ToList();
                var agr_docs = _buildingUnitsRepository.DocumentsRepository.Select().Where(d => d is bldAggregationDocument);
                foreach (bldAggregationDocument aggregationDocument in agr_docs)
                    AggragationDocumentsForSearch.Add(aggregationDocument);
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

        public NotifyCommand FindDocumentsCommand { get; private set; } 

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

            FindDocumentsCommand = new NotifyCommand(OnFindDocuments,()=>SearchString!="");

            DocTypeNames.Add("Перечень", typeof(bldAggregationDocument));
            DocTypeNames.Add("Сертификат/Паспорт", typeof(bldMaterialCertificate));

            bldAggregationDocument temp_ADoc = new bldAggregationDocument("Искать везде");
            AggragationDocumentsForSearch.Add(temp_ADoc);
            SelectedAggragationDocumentForSearch = temp_ADoc;
            Title = "Поиск документа";
        }

        private void OnFindDocuments()
        {

            ObservableCollection<bldDocument> docs;

            //  docs = new ObservableCollection<bldDocument>(_buildingUnitsRepository.DocumentsRepository.Select().Where(ad =>ad is bldMaterialCertificate 
            //  &&  EF.Functions.Like(ad.Name , $"%{SearchString}%")).ToList());
            docs = new ObservableCollection<bldDocument>(_buildingUnitsRepository.DocumentsRepository.MaterialCertificates.Select()
                .Where(d => d is bldMaterialCertificate)
                // .Where($"{SelectedColumnName.Value}.Contains(@0)",SearchString ).ToList());
                .Where(string.Format("{0}.Contains(@0, \"{1}\")",
                        SelectedColumnName.Value, StringComparison.InvariantCultureIgnoreCase.ToString()), SearchString).ToList());


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
