using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Console;
using PrismWorkApp.Core.Events;
using PrismWorkApp.Modules.MainModule.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls.Ribbon;

namespace PrismWorkApp.Modules.MainModule
{
    public class MainModule : IModule, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
       
        private readonly IContainerProvider _containerProvider;
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private int moduleId;
        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }
        private string moduleName = "Модуль_ядра";
        public string ModuleName
        {
            get { return moduleName; }
            set { moduleName = value; }
        }

        public string ConsoleParametrs { get; private set; }

        private IModulesContext _modulesContext;
        public IModulesContext ModulesContext
        {
            get { return _modulesContext; }
            set { _modulesContext = value; OnPropertyChanged("ModulesContext"); }
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            //После инициализация шлем широковещательное сообщение с запросом всех згруженный модулей
            EventMessage msg = new EventMessage();
            msg.From = ModuleId;
            msg.ParameterName = "Command";
            msg.To = ConsoleParameters.ModuleIdParameter.ALL_MODULE_ID;
            msg.Sender = "MainMudule";
            msg.Value = "who";
            msg.Recipient = "all";
            _eventAggregator.GetEvent<MessageConveyEvent>().Publish(msg); ///Оповещаем все модули, что гланвй модуль загружет
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
                            case "register_module": //регистрием дополнительные модули
                                {
                                    ModuleInfoData moduleInfoData = new ModuleInfoData();
                                    moduleInfoData.Id = Convert.ToInt32(command_str[1]);
                                    moduleInfoData.Name = (command_str[2]);
                                    moduleInfoData.IsEnable = true;
                                    ModulesContext.ModulesInfoData.Add(moduleInfoData);
                                 /*   EventMessage requestEventMessage = new EventMessage();
                                    requestEventMessage.From = ModuleId;
                                    requestEventMessage.To = eventMessage.From;
                                    requestEventMessage.ParameterName = "Command";
                                    requestEventMessage.Value = $"get_ribbon_tab {moduleInfoData.Id.ToString()} {moduleInfoData.Name}"; //Запрашиваем RibbonTab
                                    _eventAggregator.GetEvent<MessageConveyEvent>().Publish(requestEventMessage);
                                    requestEventMessage.Value = $"quick_access_tool_bar {moduleInfoData.Id.ToString()} {moduleInfoData.Name}"; //Запрашиваем RibbonTab
                                    _eventAggregator.GetEvent<MessageConveyEvent>().Publish(requestEventMessage);*/
                                    break;
                                }

                        }
                        break;
                    }
                case "RibbonTabEntity":
                    {
                        _regionManager.Regions[RegionNames.RibbonRegion].Add(eventMessage.Value);
                        break;
                    }
                case "QuickAccessToolBarEntity":
                    {
                        _regionManager.Regions[RegionNames.RibbonQuickAccessToolBarRegion].Add(eventMessage.Value);
                        break;
                    }

            }
            


        }

        public MainModule(IContainerProvider containerProvider, IRegionManager regionManager, IEventAggregator eventAggregator, IModulesContext modulesContext)
        {
            ModuleId = ConsoleParameters.ModuleIdParameter.MAIN_MODULE_ID; //Регистрируем модуль 
            ModuleInfoData mainModuleInfo = new ModuleInfoData();
            mainModuleInfo.Id = ModuleId;
            mainModuleInfo.Name = ModuleName;
            mainModuleInfo.IsEnable = true;
            _modulesContext = modulesContext;
            ModulesContext.ModulesInfoData.Add(mainModuleInfo);
         
            _containerProvider = containerProvider;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<MessageConveyEvent>().Subscribe(OnGetModuleMessage, //Подключение к общей шине сообщений 
                ThreadOption.PublisherThread, false,                                       //с фильтрацией сообщений
                message => message.To == ConsoleParameters.ModuleIdParameter.ALL_MODULE_ID || message.To == ModuleId);

        }
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //   containerRegistry.RegisterForNavigation<RibbonView>();
            // _regionManager.RequestNavigate(RegionNames.RibbonRegion, "RibbonView");
            //var ribbonAppMenu = new RibbonApplicationMenuView();

            //   _regionManager.Regions[RegionNames.RibbonRegion].Add(ribbonAppMenu);
            // var ribbonAppMenuItem = new RibbonApplicationMenuItem();
            // ribbonAppMenuItem.Header = "Ghbdtn dfd";
            //     _regionManager.Regions[RegionNames.RibbonRegion].Add(ribbonAppMenuItem);
         //   _regionManager.Regions[RegionNames.RibbonRegion].Add(new ModulesRibbonTabView());
          //  _regionManager.AddToRegion(RegionNames.RibbonQuickAccessToolBarRegion,new QuickAccessToolBarView());

        }
    }
}
