using Prism.Mvvm;
using PrismWorkApp.Core.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels.RibbonViewModels
{
    public class ToolBarRibbonTabViewModel :BindableBase
    {

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
