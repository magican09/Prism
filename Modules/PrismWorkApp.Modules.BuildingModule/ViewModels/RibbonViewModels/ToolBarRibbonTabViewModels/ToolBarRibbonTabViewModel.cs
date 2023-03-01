using Prism.Mvvm;
using PrismWorkApp.Core.Commands;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels.RibbonViewModels
{
    public class ToolBarRibbonTabViewModel : BindableBase
    {
        //private string _title = "Производство";
        //public string Title
        //{
        //    get { return _title; }
        //    set { SetProperty(ref _title, value); }
        //}
        private IApplicationCommands _applicationCommands;
        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }
        public ToolBarRibbonTabViewModel(IApplicationCommands applicationCommands)
        {
            ApplicationCommands = applicationCommands;
        }

    }
}
