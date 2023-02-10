using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Estimate
{
    public class Index : INotifyPropertyChanged, ICloneable //Индексы.. 
    {
        private string caption;
        private string code; // Код расценки
        private int namber;
        private string name;
        public int Id { get; set; }
        public string Caption
        {
            get { return caption; }
            set
            {
                caption = value;
                OnPropertyChanged("Caption");
            }
        } //Подпись сметы
        public string Code
        {
            get
            { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        } //Код  
        public string Options { get; set; } // Options="Inactive" Если не активно - то в смете нет
        public double K_OZ { get; set; } // коэффицент персчета в текущие цены общий
        public double K_EM { get; set; } // коэффицент персчета в текущие цены эксплуатации машин
        public double K_ZM { get; set; } // коэффицент персчета в текущие цены зарплаты машиниста
        public double K_MT { get; set; } // коэффицент персчета в текущие цены материалов
        public Index()
        {

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
