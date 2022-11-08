using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Console;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.Core.Events;
using PrismWorkApp.Modules.BuildingModule.Dialogs;
using PrismWorkApp.Modules.BuildingModule.Views;
using System;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;

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
        public BuildingModule(IRegionManager regionManager, IEventAggregator eventAggregator)
        {

            ModuleId = 2;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            //    _regionManager.RequestNavigate(RegionNames.ContentRegion, "ProjectExplorerView");
            //  _regionManager.RequestNavigate(RegionNames.SolutionExplorerRegion, "ConvertersView");
            _regionManager.Regions[RegionNames.SolutionExplorerRegion].Add(new ProjectExplorerView());
            var ribbinTab = new RibbonTabView();
            var ribbonGroup = new ConvertersView();
            ribbinTab.Items.Add(ribbonGroup);//Созадем группу панели инструметов с конвекторами
            _regionManager.Regions[RegionNames.RibbonRegion].Add(ribbinTab);


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
            containerRegistry.RegisterForNavigation<WorksTableView>();
            containerRegistry.RegisterForNavigation<ProjectExplorerView>();
            containerRegistry.RegisterForNavigation<AOSRDocumentsTableView>();
            containerRegistry.RegisterForNavigation<WorkView>();
            containerRegistry.RegisterForNavigation<ProjectView>();
            containerRegistry.RegisterForNavigation<ParticipantView>();
            containerRegistry.RegisterForNavigation<ObjectView>();
            containerRegistry.RegisterForNavigation<ResponsibleEmployeeView>();
            containerRegistry.RegisterForNavigation<ConstructionView>();
            containerRegistry.RegisterForNavigation<AOSRDocumentView>();

            containerRegistry.RegisterDialog<AddbldObjectToCollectionDialogView, AddbldObjectToCollectionViewModel>();
            containerRegistry.RegisterDialog<ObjectDialogView, ObjectDialogViewModel>();

            containerRegistry.RegisterDialog<AddbldConstructionToCollectionDialogView, AddbldConstructionToCollectionViewModel>();
            containerRegistry.RegisterDialog<ConstructionDialogView, ConstructionDialogViewModel>();

            containerRegistry.RegisterDialog<AddbldWorkToCollectionDialogView, AddbldWorkToCollectionViewModel>();
            containerRegistry.RegisterDialog<WorkDialogView, WorkDialogViewModel>();

            containerRegistry.RegisterDialog<AddbldParticipantToCollectionDialogView, AddbldParticipantToCollectionViewModel>();
            containerRegistry.RegisterDialog<ParticipantDialogView, ParticipantDialogViewModel>();

            containerRegistry.RegisterDialog<AddbldResponsibleEmployeeToCollectionDialogView, AddbldResponsibleEmployeeToCollectionDialogViewModel>();
            containerRegistry.RegisterDialog<ResponsibleEmployeeDialogView, ResponsibleEmployeeDialogViewModel>();

            containerRegistry.RegisterDialog<SelectProjectFromCollectionDialogView, SelectProjectFromCollectionDialogViewModel>();

            //   containerRegistry.RegisterDialog<ConfirmCreateDialogViewModel, ConfirmCreateDialogViewModel>();

            // containerRegistry.RegisterForNavigation<ProjectExplorerView>();
            // containerRegistry.RegisterForNavigation<ConvertersView>();

        }
    }
}