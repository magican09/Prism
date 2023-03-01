using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PrismWorkApp.OpenWorkLib.Estimate
{
    public class Chapter : INotifyPropertyChanged, ICloneable //Раздел сметы 
    {
        private string caption;
        private int level; //Уровень в иерархии
        public int Id { get; set; }
        public int Namber { get; set; } // Номер раздела 
        public double CommonPrice { get; set; } // Итого затрат 
        public int Level
        {
            get { return level; }
            set
            {
                level = value;
                OnPropertyChanged("Level");
            }
        } // Уровень в иерархии
        public string Name { get; set; } //Название раздела
        public string Caption
        {
            get { return caption; }
            set
            {
                caption = value;
                Name = caption;
                OnPropertyChanged("Caption");
            }
        } //Подпись раздела
        public int SysID { get; set; } // 
        private ObservableCollection<Position> positions;
        public ObservableCollection<Position> Positions  //Расценки внутри раздела
        {
            get { return positions; }
            set
            {
                positions = value;
                OnPropertyChanged("Positions");
            }
        }
        public Chapter()
        {
            Positions = new ObservableCollection<Position>();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
