using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Estimate
{
    public class ImplemAct : INotifyPropertyChanged, ICloneable // Набор индексов к акту  
    {
        private string caption; // Название акта
        private int namber; // Номер акта
        private DateTime makingDate; //Дата создания
        private string year; //год
        private string month; //Месяц
        private int dayStart; //Первый день 
        private int dayFinish; //Первый последний 
        private int actIndex; //Номер набора индесов 
        private string indexesMode; //Режим индексации 
        private ObservableCollection<Index> indexs;
        public int Id { get; set; }

        public string Caption
        {
            get { return caption; }
            set
            {
                caption = value;
                OnPropertyChanged("Caption");
            }
        } //Заголовок..
        public int Namber
        {
            get { return namber; }
            set
            {
                namber = value;
                OnPropertyChanged("Namber");
            }
        } // Номер  
        public DateTime MakingDate
        {
            get { return makingDate; }
            set
            {
                makingDate = value;
                OnPropertyChanged("MakingDate");
            }
        }//Дата создания набора индексов  
        public string Year { get; set; }// 
        public string Month { get; set; }// 
        public int DayStart { get; set; }// 
        public int DayFinish { get; set; }// 
        public string IndexesMode { get; set; }//Режим индексации 
        public int ActIndex
        {
            get { return actIndex; }
            set
            {
                actIndex = value;
                OnPropertyChanged("ActIndex");
            }
        }  //Номер набора индексов

        public ObservableCollection<Index> Indexs
        {
            get { return indexs; }
            set
            {
                indexs = value;
                OnPropertyChanged("Indexs");
            }
        }

        public ImplemAct()
        {
            Indexs = new ObservableCollection<Index>();
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
