using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PrismWorkApp.OpenWorkLib.Estimate
{
    public class Resource : INotifyPropertyChanged, ICloneable//Любой вид ресурса
    {
        private string caption;
        //  private string name;
        private string quantity;
        private string units;
        private string header;
        private string type;
        private string code;
        private string tip;
        private string id;
        private string ded;
        private string vid;
        private string sbor;
        private string tabl;
        private string poz;
        public double normConsumption;
        public double normPrice;
        public double mass;
        public double priceBase;
        public double price;
        public double priceLocal;
        public int Id { get; set; }
        public string Units
        {
            get { return units; }
            set
            {
                units = value;
                OnPropertyChanged("Units");
            }
        }  //  Единица измерения (текст).
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Header
        {
            get { return header; }
            set
            {
                header = value;
                Name = value;
                OnPropertyChanged("Header");
            }
        }//Заголовок ресурса (текст).
        public ResurceType Type { get; set; } //Тип Ресурса
        public double NormConsumption
        {
            get { return normConsumption; }
            set
            {
                normConsumption = value;
                OnPropertyChanged("NormConsumption");
            }
        }  //Норма расхода ресурса на единицу объема работ (число).
        public double NormPrice
        {
            get { return normPrice; }
            set
            {
                normPrice = value;
                OnPropertyChanged("NormPrice");
            }
        }  //Цена единицы ресурса нормативная (число, руб.).
        public double Price //
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged("Price");
            }
        } //Цена единицы ресурса фактическая (число, руб.).
        public double PriceLocal //
        {
            get { return priceLocal; }
            set
            {
                priceLocal = value;
                OnPropertyChanged("PriceLocal");
            }
        } //Цена единицы ресурса местная (число, руб.).
        public string TIP
        {
            get { return tip; }
            set
            {
                tip = value;
                OnPropertyChanged("TIP");
            }
        } //Тип узла
        public string VID
        {
            get { return vid; }
            set
            {
                vid = value;
                OnPropertyChanged("VID");
            }
        }  //Вид узла
        public string ID
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("ID");
            }
        }  //ID узла
        public string DED
        {
            get { return ded; }
            set
            {
                ded = value;
                OnPropertyChanged("DED");
            }
        }  //DED родительского узла
        public string SBOR
        {
            get { return sbor; }
            set
            {
                sbor = value;
                OnPropertyChanged("SBOR");
            }
        }  //ID Сборника родительского узла
        public string TABL
        {
            get { return tabl; }
            set
            {
                tabl = value;
                OnPropertyChanged("TABL");
            }
        }  //ID Таблицы
        public string POZ
        {
            get { return poz; }
            set
            {
                poz = value;
                OnPropertyChanged("POZ");
            }
        }  //номер позиции
        public string Caption
        {
            get { return caption; }
            set
            {
                caption = value;
                Name = caption;
                OnPropertyChanged("Caption");
            }
        } //Подпись раз
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        } //Код
        public double WorkClass { get; set; } //
        public string Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        } // Количество факт
        // public ObservableCollection<String> ResourсesTypeObservableCollection; //Список типов ресурсов внутри ресурса
        public double Mass
        {
            get { return mass; }
            set
            {
                mass = value;
                OnPropertyChanged("Mass");
            }
        }  // Масса
        public double PriceBase
        {
            get { return priceBase; }
            set
            {
                priceBase = value;
                OnPropertyChanged("PriceBase");
            }
        }  // Базовая цена

        public string Group { get; set; } // Группа в листе ресурсов
        public ObservableCollection<String> ResurсesTypeList { get; set; } //Список типов ресурсов внутри ресурса
        public Resource()
        {
            ResurсesTypeList = new ObservableCollection<string>();

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
