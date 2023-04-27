using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Modules.BuildingModule.ViewModels;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PrismWorkApp.Modules.BuildingModule
{
    public class FileFormatsViewModel : BaseViewModel<bldMaterialCertificate>, INotifyPropertyChanged, INavigationAware
    {
        private NameableObservabelObjectsCollection _fileFormats;
        public NameableObservabelObjectsCollection FileFormats
        {
            get { return _fileFormats; }
            set { SetProperty(ref _fileFormats, value); }
        }

        AppObjectsModel AppObjectsModel { get; set; }
        public IBuildingUnitsRepository _buildingUnitsRepository { get; }

        public FileFormatsViewModel(IDialogService dialogService,
          IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository, IApplicationCommands applicationCommands, IAppObjectsModel appObjectsModel)
        {

            UnDoReDo = new UnDoReDoSystem(this);
            ApplicationCommands = applicationCommands;
            AppObjectsModel = appObjectsModel as AppObjectsModel;
            _dialogService = dialogService;
            _buildingUnitsRepository = buildingUnitsRepository;
            _regionManager = regionManager;
        }
         
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_agrregation_document"];
            if (navigationContext.Parameters["title"] != null) Title = navigationContext.Parameters["title"].ToString();
            if (navigane_message != null)
            {
                bldAggregationDocument arg_document = (bldAggregationDocument)navigane_message.Object;

                EditMode = navigane_message.EditMode;
                //if (AggregationDocuments.Where(ad => ad.StoredId == arg_document.StoredId).FirstOrDefault() == null)
                //{

                //    AggregationDocuments.Add(arg_document);
                //    //  if(UnDoReDo.ParentUnDoReDo==null)
                //    arg_document.UnDoReDoSystem.SetUnDoReDoSystemAsChildren(UnDoReDo);
                //    UnDoReDo.Register(arg_document);
                //    if (SelectedAggregationDocument == null) SelectedAggregationDocument = arg_document;
                //}
                //if (AggregationDocuments != null) AggregationDocuments.ErrorsChanged -= RaiseCanExecuteChanged;
                //AggregationDocuments.ErrorsChanged += RaiseCanExecuteChanged;

            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        
    }
}
