using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Estimate
{
    /*5) Прямые затраты(всего) (число, руб.).
   6) Основная заработная плата(число, руб.).
   7) Стоимость эксплуатации машин и механизмов(число, руб.).
   8) Заработная плата машинистов(число, руб.) (входит в общую стоимость эксплуатации машин и механизмов).
   9) Стоимость материалов(число, руб.).
   10) Возврат материалов(число, руб.).
   11) Транспорт материалов(число, руб.).
   12) Шефмонтаж(число, руб.).
   13) Трудозатраты основных рабочих(число, чел.-час.).
   14) Трудозатраты машинистов(число, чел.-час.).*/

    public class CorrectionIndexes : INotifyPropertyChanged, ICloneable // Попавочные коэфициенты
    {
        private string name; // Наименование
        private string justification; //Обоснование
        private ObservableCollection<Double> indexes;
        private Position position; // Ссылка на позицию к которой применены поправочные коэфициенты
        public string Justification
        {
            get { return justification; }
            set
            {
                justification = value;
                OnPropertyChanged("Justification");
            }
        }//Обоснование  
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }//Название  
        public Position Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }// Ссылка на позицию к которой применены поправочные коэфициенты  
        public ObservableCollection<Double> Indexes
        {
            get { return indexes; }
            set
            {
                indexes = value;
                OnPropertyChanged("Indexes");
            }
        } // Поправочные коэфициенты
        public CorrectionIndexes()
        {
            Indexes = new ObservableCollection<double>();
            for (int ii = 0; ii <= 5; ii++)
                Indexes.Add(new double());
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
