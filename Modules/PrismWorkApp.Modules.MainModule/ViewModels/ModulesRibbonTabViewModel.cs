//using OfficeOpenXml;
using Prism.Mvvm;
using PrismWorkApp.Core.Console;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PrismWorkApp.Modules.MainModule.ViewModels
{
    public class ModulesRibbonTabViewModel : BindableBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private string _title = "Диспетчер";
        private IModulesContext _modulesContext;
        public IModulesContext ModulesContext
        {
            get { return _modulesContext; }
            set { SetProperty(ref _modulesContext, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public ModulesRibbonTabViewModel(IModulesContext modulesContext)
        {
            _modulesContext = modulesContext;
        }

    }

}