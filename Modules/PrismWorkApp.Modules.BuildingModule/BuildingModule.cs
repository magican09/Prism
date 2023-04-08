using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Core.Console;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.Core.Events;
using PrismWorkApp.Modules.BuildingModule.Dialogs;
using PrismWorkApp.Modules.BuildingModule.ViewModels;
using PrismWorkApp.Modules.BuildingModule.Views;
using PrismWorkApp.Services.Repositories;
using System.Text;

namespace PrismWorkApp.Modules.BuildingModule
{
    public class BuildingModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private int moduleId;
        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }
        private string moduleName = "Строительный_модуль";
        public string ModuleName
        {
            get { return moduleName; }
            set { moduleName = value; }
        }
        private IApplicationCommands _applicationCommands;
        private IBuildingUnitsRepository _buildingUnitsRepository;
        public IbldMaterialsUnitsRepository _bldMaterialsUnitsRepository;
        private IDialogService _dialogService;
        private IAppSettingsSystem _appSettings;
        private IAppObjectsModel _appObjectsModel;
        public BuildingModule(IRegionManager regionManager, IEventAggregator eventAggregator, IBuildingUnitsRepository buildingUnitsRepository/*, IbldMaterialsUnitsRepository bldMaterialsUnitsRepository*/,
            IDialogService dialogService, IApplicationCommands applicationCommands, IAppSettingsSystem appSettings, IAppObjectsModel appObjectsModel)
        {

            ModuleId = 2;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _applicationCommands = applicationCommands;
            _dialogService = dialogService;
            _buildingUnitsRepository = buildingUnitsRepository;
            _appSettings = appSettings;
            _appObjectsModel = appObjectsModel;
            //   _bldMaterialsUnitsRepository = bldMaterialsUnitsRepository;

        }


        public void OnInitialized(IContainerProvider containerProvider)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //    _regionManager.RequestNavigate(RegionNames.ContentRegion, "ProjectExplorerView");
            //  _regionManager.RequestNavigate(RegionNames.SolutionExplorerRegion, "ConvertersView");
            _regionManager.Regions[RegionNames.SolutionExplorerRegion].Add(new DocumentationExplorerView());
            _regionManager.Regions[RegionNames.SolutionExplorerRegion].Add(new ProjectExplorerView());

            var projectManagerRibbonTab = new ProjectManagerRibbonTabView();
            var dataImportRibbonGroup = new DataImportRibbonGroupView();
            var currentProjectRibbonGroup = new CurentProjectRibbonGroupView();
            projectManagerRibbonTab.DataContext = new ProjectManagerRibbonTabViewModel(_regionManager, _eventAggregator, _buildingUnitsRepository, _dialogService, _applicationCommands);
            projectManagerRibbonTab.Items.Add(dataImportRibbonGroup);//
            projectManagerRibbonTab.Items.Add(currentProjectRibbonGroup);//Созадем группу панели инструметов с конвекторами
            _regionManager.Regions[RegionNames.RibbonRegion].Add(projectManagerRibbonTab);

            var toolBarRibbonTab = new ToolBarRibbonTabView();
            var toolBarRibbonTabDataContext = new ToolBarRibbonTabViewModel(_dialogService, _applicationCommands, _appSettings);
            toolBarRibbonTab.DataContext = toolBarRibbonTabDataContext;
            var toolBarRibbonGroup = new EditToollsRibbonGroupView();
            toolBarRibbonTab.Items.Add(toolBarRibbonGroup);//
            _regionManager.Regions[RegionNames.RibbonRegion].Add(toolBarRibbonTab);

            var documentsRibbonTab = new DocumentsRibbonTabView();
            documentsRibbonTab.DataContext = new DocumentsRibbonTabViewModel(_regionManager, _eventAggregator, _buildingUnitsRepository, _dialogService, _applicationCommands, _appObjectsModel);
            //  var materialsRibbonGroup = new MaterialsRibbonGroupView();
            // documentsRibbonTab.Items.Add(materialsRibbonGroup);
            var documentsRibbonGroup = new DocumentsRibbonGroupView();
            documentsRibbonTab.Items.Add(documentsRibbonGroup);
            var materialCertificatesRibbonGroup = new MaterialCertificatesRibbonGroup();
            documentsRibbonTab.Items.Add(materialCertificatesRibbonGroup);
            _regionManager.Regions[RegionNames.RibbonRegion].Add(documentsRibbonTab);

            var quickAccessTollBar = new QuickAccessToolBarView();
            quickAccessTollBar.Items.Add(new QuickAccessToolBar());
            quickAccessTollBar.DataContext = toolBarRibbonTabDataContext;
            _regionManager.Regions[RegionNames.RibbonQuickAccessToolBarRegion].Add(quickAccessTollBar);//Добавяем кнопку сохраниять все на панель панель быстрого вызова

            // _regionManager.Regions[RegionNames.RibbonRegion].Add(ribbinTab);


            _eventAggregator.GetEvent<MessageConveyEvent>().Subscribe(OnGetModuleMessage, //Подписывается на соощения которые адресованы этому модулю
                ThreadOption.PublisherThread, false,
                message => message.To == ConsoleParameters.ModuleIdParameter.ALL_MODULE_ID || message.To == ModuleId);


        }

        private void OnGetModuleMessage(EventMessage eventMessage)
        {
            EventMessage message = new EventMessage();
            message.From = ModuleId;
            switch (eventMessage.ParameterName)
            {
                case "Command":
                    {
                        string[] command_str = eventMessage.Value.ToString().Split(" ");
                        string command_name = command_str[0];
                        switch (command_name)
                        {
                            case "who": //Отвечаем главному модулю, что мы здесь.
                                {
                                    message.To = eventMessage.From;
                                    message.ParameterName = "Command";
                                    message.Value = $"register_module {ModuleId.ToString()} {ModuleName}";
                                    _eventAggregator.GetEvent<MessageConveyEvent>().Publish(message); //Регистрием модуль в гланом модуле

                                    break;
                                }
                                /*  case "get_ribbon_tab": //Отправляем в гланый модуль вкладку в панель инструменьтов.
                                    {
                                        var ribbinTab = new RibbonTabView();
                                        // ribbinTab.Header = $"Name = {command_str[2]} ID={command_str[1]}";
                                        EventMessage requestEventMessage = new EventMessage();
                                        requestEventMessage.From = ModuleId;
                                        requestEventMessage.To = eventMessage.From;
                                        requestEventMessage.ParameterName = "RibbonTabEntity";
                                        var ribbonGroup = new ConvertersView();
                                        requestEventMessage.Value = ribbinTab;
                                        ribbinTab.Items.Add(new ConvertersView());//Созадем группу панели инструметов с конвекторами
                                        _eventAggregator.GetEvent<MessageConveyEvent>().Publish(requestEventMessage);

                                        break;
                                    }
                                    case "quick_access_tool_bar": //Отправляем в гланый модуль вкладку в панель инструменьтов.
                                           {
                                               var quickAccessTollBar = new QuickAccessToolBarView();
                                               // ribbinTab.Header = $"Name = {command_str[2]} ID={command_str[1]}";
                                               EventMessage requestEventMessage = new EventMessage();
                                               requestEventMessage.From = ModuleId;
                                               requestEventMessage.To = eventMessage.From;
                                               requestEventMessage.ParameterName = "QuickAccessToolBarEntity";
                                               requestEventMessage.Value = quickAccessTollBar;
                                         //      quickAccessTollBar.Items.Add(new QuickAccessToolBar());//
                                               _eventAggregator.GetEvent<MessageConveyEvent>().Publish(requestEventMessage);

                                               break;
                                           }*/
                        }
                    }
                    break;
                case "Data":
                    {

                        break;
                    }
            }
            //    var con_view = new RibbonTabView();
            //     _eventAggregator.GetEvent<RibbonTabViewSentEvent>().Publish(con_view);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ProjectExplorerView>();
            containerRegistry.RegisterForNavigation<DocumentationExplorerView>();
            containerRegistry.RegisterForNavigation<WorksTableView>();
            containerRegistry.RegisterForNavigation<AOSRDocumentsTableView>();
            containerRegistry.RegisterForNavigation<MaterialView>();
            containerRegistry.RegisterForNavigation<WorkView>();
            containerRegistry.RegisterForNavigation<ProjectView>();
            containerRegistry.RegisterForNavigation<ParticipantView>();
            containerRegistry.RegisterForNavigation<ObjectView>();
            containerRegistry.RegisterForNavigation<ResponsibleEmployeeView>();
            containerRegistry.RegisterForNavigation<ConstructionView>();
            containerRegistry.RegisterForNavigation<AOSRDocumentView>();
            containerRegistry.RegisterForNavigation<WorksGroupView>();
            containerRegistry.RegisterForNavigation<MaterialsGroupView>();
            containerRegistry.RegisterForNavigation<ParticipantsGroupView>();
            containerRegistry.RegisterForNavigation<DocumentsGroupView>();
            containerRegistry.RegisterForNavigation<MaterialCertificatesGroupView>();
            containerRegistry.RegisterForNavigation<AggregationDocumentsView>();

            containerRegistry.RegisterDialog<UserParametersDialogView, UserParametersDialogViewModel>();

            containerRegistry.RegisterDialog<AddbldObjectToCollectionDialogView, AddbldObjectToCollectionViewModel>();
            containerRegistry.RegisterDialog<ObjectDialogView, ObjectDialogViewModel>();

            containerRegistry.RegisterDialog<AddbldConstructionToCollectionDialogView, AddbldConstructionToCollectionViewModel>();
            containerRegistry.RegisterDialog<AddbldConstructionToCollectionFromListDialogView, AddbldConstructioneToCollectionFromListDialogViewModel>();

            containerRegistry.RegisterDialog<ConstructionDialogView, ConstructionDialogViewModel>();

            containerRegistry.RegisterDialog<AddbldWorkToCollectionDialogView, AddbldWorkToCollectionViewModel>();
            containerRegistry.RegisterDialog<AddWorksToCollectionFromListDialogView, AddWorksToCollectionFromListDialogViewModel>();
            containerRegistry.RegisterDialog<WorkDialogView, WorkDialogViewModel>();

            containerRegistry.RegisterDialog<AddUnitOfMeasurementToCollectionFromListDialogView, AddUnitOfMeasurementToCollectionFromListDialogViewModel>();

            containerRegistry.RegisterDialog<AddMaterialToCollectionFromListDialogView, AddMaterialToCollectionFromListDialogViewModel>();
            containerRegistry.RegisterDialog<MaterialDialogView, MaterialDialogViewModel>();

            containerRegistry.RegisterDialog<AddProjectDocumentToCollectionFromListDialogView, AddProjectDocumentToCollectionFromListDialogViewModel>();
            containerRegistry.RegisterDialog<AddLaboratoryReportToCollectionFromListDialogView, AddLaboratoryReportToCollectionFromListDialogViewModel>();
            containerRegistry.RegisterDialog<AddExecutiveSchemeToCollectionFromListDialogView, AddExecutiveSchemeToCollectionFromListDialogViewModel>();
            containerRegistry.RegisterDialog<AddbldResponsibleEmployeeToCollectionDialogView, AddbldResponsibleEmployeeToCollectionDialogViewModel>();
            containerRegistry.RegisterDialog<SelectConstructionFromTreeViewDialog, SelectConstructionFromTreeViewDialogViewModel>();

            containerRegistry.RegisterDialog<AddbldParticipantToCollectionDialogView, AddbldParticipantToCollectionViewModel>();
            containerRegistry.RegisterDialog<ParticipantDialogView, ParticipantDialogViewModel>();

            containerRegistry.RegisterDialog<AddbldResponsibleEmployeeToCollectionDialogView, AddbldResponsibleEmployeeToCollectionDialogViewModel>();
            containerRegistry.RegisterDialog<ResponsibleEmployeeDialogView, ResponsibleEmployeeDialogViewModel>();

            containerRegistry.RegisterDialog<SelectProjectFromCollectionDialogView, SelectProjectFromCollectionDialogViewModel>();

            containerRegistry.RegisterDialog<SelectDocumentFromTreeViewDialog, SelectDocumentFromTreeViewDialogViewModel>();

            containerRegistry.RegisterDialog<AddConstructionCompaniesToCollectionFromListDialogView, AddConstructionCompaniesToCollectionFromListDialogViewModel>();
            containerRegistry.RegisterDialog<SelectAggregationDocumentFromCollectionDialogView, SelectAggregationDocumentFromCollectionDialogViewModel>();

            containerRegistry.RegisterDialog<GetObjectFromCollectionDialogVeiw, GetObjectFromCollectionDialogVeiwModel>();

            //   containerRegistry.RegisterSingleton<IAppObjectsModel,AppObjectsModel>();
            //  containerRegistry.RegisterForNavigation<ToolBarRibbonTabView, ToolBarRibbonTabViewModel>();
            //   containerRegistry.RegisterDialog<ConfirmCreateDialogViewModel, ConfirmCreateDialogViewModel>();

            // containerRegistry.RegisterForNavigation<ProjectExplorerView>();
            // containerRegistry.RegisterForNavigation<ConvertersView>();

        }
    }
}