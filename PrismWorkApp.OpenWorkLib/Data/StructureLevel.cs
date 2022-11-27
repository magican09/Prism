using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class StructureLevel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private int _level;
        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }
        private int _number;
        public int Number
        {
            get { return _number; }
            set { _number = value; }
        }
        private int _deptIndex;
        public int DeptIndex //Глубина в дереве объектов
        {
            get
            {
                return _deptIndex;
            }
            set { _deptIndex = value; OnPropertyChanged("DeptIndex"); }
        }
        public object Value { get; set; }
        // public bool IsDefined { get; set; } = false;
        public StructureLevelStatus Status { get; set; } = StructureLevelStatus.UN_DEFINED;

        public int MaxNumber { get; set; }

        public string _code = "";
        public string Code
        {
            get
            {
                return $"{DeptIndex.ToString()}_{Level.ToString()}:{Number.ToString()}";
            }
            set { _code = value; OnPropertyChanged("Code"); }
        }

        public ObservableCollection<StructureLevel> StructureLevels { get; set; } = new ObservableCollection<StructureLevel>();

        public StructureLevel ParentStructureLevel { get; set; }
        public StructureLevel()
        {
            StructureLevels.CollectionChanged += OnStructureLevelsChange;
            // Status = StructureLevelStatus.UN_DEFINED;
        }

        private void OnStructureLevelsChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {

            }
        }


        public StructureLevel(int root_level) : this()
        {
            Level = root_level;
        }


    }
    public enum StructureLevelStatus
    {
        UN_DEFINED,
        DEFINED,
        IN_PROCESS
    }
}
