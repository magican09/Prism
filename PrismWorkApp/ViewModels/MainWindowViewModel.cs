using Prism.Mvvm;
using PrismWorkApp.Core.Commands;

namespace PrismWorkApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {


        IApplicationCommands _applicationCommands;
        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }
        private string _title = "Executive Documentation";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
