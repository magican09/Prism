using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PrismWorkApp.Core.Console
{
    public interface IModuleInfoData
    { 
         int Id { get; set; }
         string Name { get; set; }
         bool IsEnable { get; set; }
        ObservableCollection<IModuleInfoData> ChildModules{ get; set; }
    }
    public class ModuleInfoData : IModuleInfoData, INotifyPropertyChanged
    {
      
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private int _moduleId;
        private string _moduleName;
        private bool _isModuleEneable;
        private ObservableCollection<IModuleInfoData> _childModules;
        public int Id { get => _moduleId; set  {_moduleId = value; OnPropertyChanged("Id"); } }
        public string Name { get => _moduleName; set{ _moduleName = value; OnPropertyChanged("Name"); } }
        public bool IsEnable { get => _isModuleEneable; set { _isModuleEneable = value; OnPropertyChanged("IsEnable"); } }
        public ObservableCollection<IModuleInfoData> ChildModules { get => _childModules; set { _childModules = value; OnPropertyChanged("ChildModules"); } }
        public ModuleInfoData()
        {
            ChildModules = new ObservableCollection<IModuleInfoData>();
        }

    }
}
