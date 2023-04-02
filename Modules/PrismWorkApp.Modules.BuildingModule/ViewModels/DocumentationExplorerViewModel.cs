using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Core.Events;
using PrismWorkApp.Modules.BuildingModule.Core;
using PrismWorkApp.Modules.BuildingModule.Views;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Globalization;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class DocumentationExplorerViewModel : LocalBindableBase, INotifyPropertyChanged, INavigationAware//, IConfirmNavigationRequest
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
        public bldDocumentsGroup Documentation{ get; set; }

        public NotifyMenuCommands DocumentationCommands { get; set; }
        public NotifyCommand<object> TreeViewItemSelectedCommand { get; private set; }
        public NotifyCommand<object> TreeViewItemExpandedCommand { get; private set; }
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        private IDialogService _dialogService;
        private AppObjectsModel _appObjectsModel;
        public   DocumentationExplorerViewModel(IEventAggregator eventAggregator, 
                            IRegionManager regionManager, IDialogService dialogService,IAppObjectsModel appObjectsModel)
        {
            _appObjectsModel = appObjectsModel as AppObjectsModel;
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            _dialogService = dialogService; Documentation = _appObjectsModel.Documentation;
            DocumentationCommands = _appObjectsModel.DocumentationCommands;

            Items = new DataItemCollection(null);
            DataItem root = new DataItem();
            root.DataItemInit += _appObjectsModel.OnDataItemInit; ;
            root.AttachedObject = Documentation;
            Items.Add(root);
            
            TreeViewItemSelectedCommand = new NotifyCommand<object>(OnTreeViewItemSelected);
            TreeViewItemExpandedCommand = new NotifyCommand<object>(onTreeViewItemExpanded);
         
            _eventAggregator.GetEvent<MessageConveyEvent>().Subscribe(OnGetMessage,
             ThreadOption.PublisherThread, false,
             message => message.Recipient == "DocumentationExplorer");

    }

  
        static private GetImageFrombldProjectObjectConvecter ObjecobjectTo_Url_Convectert = new GetImageFrombldProjectObjectConvecter();

      private void OnGetMessage(EventMessage event_message)
        {
            //bldAggregationDocument bld_document = (bldAggregationDocument)event_message.Value;
            ////  if (bld_project != null && bld_Projects.Where(pr => pr.Id == bld_project.Id).FirstOrDefault() == null && bld_project != null)
            //bldAggregationDocument doc = AggregationDocuments.Where(doc => doc.Id == bld_document.Id).FirstOrDefault();
            //if (bld_document != null && doc == null)
            //{
            //    AggregationDocuments.Add(bld_document);
            //}
        }

        private void OnTreeViewItemSelected(object clc_node)
        {
            System.Type node_value_type;
            object clicked_node;
            if (clc_node.GetType() == typeof(Node))
            {
                node_value_type = ((Node)clc_node).Value?.GetType();
                clicked_node = ((Node)clc_node).Value;
            }
            else
            {
                node_value_type = clc_node?.GetType();
                clicked_node = clc_node;
            }
            NavigationParameters navParam = new NavigationParameters();
            switch (node_value_type?.Name)
            {
                case (nameof(bldMaterialCertificate)):
                    {
                        //navParam.Add("bld_material_certificate", (new ConveyanceObject((bldMaterialCertificate)clicked_node, ConveyanceObjectModes.EditMode.FOR_EDIT)));
                        //_regionManager.RequestNavigate(RegionNames.ContentRegion, typeof(MaterialView).Name, navParam);
                        break;
                    }
                case (nameof(bldMaterialCertificatesGroup)):
                    {
                        bldMaterialCertificatesGroup materialCertificates = clc_node as bldMaterialCertificatesGroup;
                        navParam.Add("bld_material_certificates", (new ConveyanceObject(materialCertificates, ConveyanceObjectModes.EditMode.FOR_EDIT)));
                        _regionManager.RequestNavigate(RegionNames.ContentRegion, typeof(MaterialCertificatesGroupView).Name, navParam);
                        break;
                    }
                case (nameof(bldAggregationDocument)):
                    {
                        bldAggregationDocument aggregationDocument = clc_node as bldAggregationDocument;
                        if (aggregationDocument.AttachedDocuments.Count > 0 && aggregationDocument.AttachedDocuments[0] is bldMaterialCertificate)
                        {
                            bldDocumentsGroup materialCertificates = aggregationDocument.AttachedDocuments ;
                         //   navParam.Add("bld_material_certificates", (new ConveyanceObject(materialCertificates, ConveyanceObjectModes.EditMode.FOR_EDIT)));
                            navParam.Add("bld_material_certificates_agrregation", (new ConveyanceObject(aggregationDocument, ConveyanceObjectModes.EditMode.FOR_EDIT)));
                            _regionManager.RequestNavigate(RegionNames.ContentRegion, typeof(AggregationDocumentsView).Name, navParam);
                        
                        }
                        break;
                    }
                case (nameof(bldDocumentsGroup)):
                    {
                        bldDocumentsGroup documents = clc_node as bldDocumentsGroup;
                        if (documents.Count > 0 && documents[0] is bldMaterialCertificate)
                        {
                            navParam.Add("bld_material_certificates", (new ConveyanceObject(documents, ConveyanceObjectModes.EditMode.FOR_EDIT)));
                            _regionManager.RequestNavigate(RegionNames.ContentRegion, typeof(MaterialCertificatesGroupView).Name, navParam);
                        
                        }
                        break;
                    }

                    break;
                     
            }

            }

        private void onTreeViewItemExpanded(object obj)
        {

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
              var doc= Documentation.Where(doc => doc.Id == bld_aggregationDoc.Id).FirstOrDefault();
                if (doc == null)
                {
                    Documentation.Add(bld_aggregationDoc);
                }
            }
           
        }
    }
}
