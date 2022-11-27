using Prism.Events;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Console;
using PrismWorkApp.Core.Events;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class RibbonTabViewModel : LocalBindableBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private const int CURRENT_MODULE_ID = 2;
        private readonly IEventAggregator _eventAggregator;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private string _title = "Менеджер проектов";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private IModulesContext _modulesContext;
        public IModulesContext ModulesContext
        {
            get { return _modulesContext; }
            set { SetProperty(ref _modulesContext, value); }
        }
        private IModuleInfoData _moduleInfoData;
        public IModuleInfoData ModuleInfo
        {
            get { return _moduleInfoData; }
            set { SetProperty(ref _moduleInfoData, value); }
        }
        public RibbonTabViewModel(IModulesContext modulesContext, IEventAggregator eventAggregator)
        {
            ModulesContext = modulesContext;
            _eventAggregator = eventAggregator;
            ModuleInfo = ModulesContext.ModulesInfoData.Where(mi => mi.Id == CURRENT_MODULE_ID).FirstOrDefault();
            //IsModuleEnable =  ModuleInfo.IsEnable;
            //  _eventAggregator.GetEvent<MessageConveyEvent>().Subscribe(OnGetMessage,
            //       ThreadOption.PublisherThread, false,
            //      message => message.Recipient == "RibbonTab");
            EventMessage message = new EventMessage();
            message.From = CURRENT_MODULE_ID;
            message.To = CURRENT_MODULE_ID; ;
            message.Sender = "RibbinTab";
            message.Recipient = "RibonGroup";
            message.ParameterName = "Command";
            message.Value = "GetRibbobGroupEntity";
            //    _eventAggregator.GetEvent<MessageConveyEvent>().Publish(message);
        }
        private void OnGetMessage(EventMessage message)
        {
            switch (message.ParameterName)
            {
                case "Project":

                    break;
            }
        }





    }

}