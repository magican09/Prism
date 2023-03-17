using Prism;
using Prism.Mvvm;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using System;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels.RibbonViewModels
{
    public class ToolBarRibbonTabViewModel : LocalBindableBase, IActiveAware
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
        private void OnActiveChanged(object sender, EventArgs e)
        {
            if (IsActive)
            {
      
            }
            else
            {
        
            }
        }
    }
}
