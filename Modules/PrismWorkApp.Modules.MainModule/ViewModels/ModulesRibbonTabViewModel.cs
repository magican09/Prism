using Microsoft.Win32;
//using OfficeOpenXml;
using Prism.Commands;
using Prism.Mvvm;
using PrismWorkApp.Core.Console;
using PrismWorkApp.ProjectModel.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

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
        private  IModulesContext _modulesContext;
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
          public ModulesRibbonTabViewModel(IModulesContext modulesContext )
        {
            _modulesContext = modulesContext;
        }
    
    }

}