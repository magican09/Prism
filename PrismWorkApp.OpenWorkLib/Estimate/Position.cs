using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PrismWorkApp.OpenWorkLib.Estimate
{
    public class Position : INotifyPropertyChanged, ICloneable //Расссценка 
    {
        private string caption;
        private string code; // Код расценки
        private int namber;
        private string name;
        private string header;
        private string units;
        private double quantity;
        private string indexCode;
        private double laborPerUnit;
        private double labor;

        private ObservableCollection<Double> baseParams;
        private ObservableCollection<Double> indexedParams;
        private ObservableCollection<CorrectionIndexes> correctionIndexes;

        private ObservableCollection<Resource> resurсes;
        private ObservableCollection<Koefficient> koefficients;
        public int Id { get; set; }
        public int Namber
        {
            get { return namber; }
            set
            {
                namber = value;
                OnPropertyChanged("Namber");
            }
        } // Номер в смете 
        public double LaborPerUnit
        {
            get
            {
                laborPerUnit = BaseParams[8];
                return laborPerUnit;
            }
            set
            {
                laborPerUnit = value;
                OnPropertyChanged("LaborPerUnit");
            }
        } // Трудозатраты на единцу
        public double Labor
        {
            get
            {
                labor = indexedParams[8];
                return labor;
            }
            set
            {
                labor = value;
                OnPropertyChanged("Labor");
            }
        } // Трудозатраты 
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }//Название  
        public string Caption
        {
            get { return caption; }
            set
            {
                caption = value;
                Name = caption;
                OnPropertyChanged("Caption");
            }
        } //Подпись сметы
        public string Header
        {
            get { return header; }
            set
            {
                header = value;
                OnPropertyChanged("Header");
            }
        }  // Заголовок раздела перед расценкой
        public string Units
        {
            get { return units; }
            set
            {
                units = value;
                OnPropertyChanged("Units");
            }
        }  // Единицы измерения
        public string Code
        {
            get
            { return code; }
            set
            {
                code = value;
                if (value.Contains("ТЕР")) this.Type = PositionType.TER;
                else if (value.Contains("ТССЦ")) this.Type = PositionType.TSCC;
                else this.Type = PositionType.COMMERCIAL;
            }
        } //Код  

        public PositionType Type { get; set; } //Тип расценки  
        public int SysID { get; set; } // 
        public double Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }  // Итоговое количество расценки
        public string Options { get; set; } // Options="Inactive" Если не активно - то в смете нет
        public string IndexCode
        {
            get { return indexCode; }
            set
            {
                indexCode = value;
                OnPropertyChanged("IndexCode");
            }
        }  // IndexCode="ТССЦ-101-0256" либо IndexCode="ТЕР15-01-019-05"Код расценки
        public int KUnit { get; set; } // KUnit коэфициент един измерения
                                       //public double Result { get; set; } // Итоговое количество расценки
        public double PZ { get; set; } // цена расцеки общая
        public double OZ { get; set; } // цена ЗП
        public double EM { get; set; } // цена ЭМ
        public double ZM { get; set; } // цена  ЗПМ
        public double MT { get; set; } // цена  Материала
        public double K_OZ { get; set; } // коэффицент персчета в текущие цены общий
        public double K_EM { get; set; } // коэффицент персчета в текущие цены эксплуатации машин
        public double K_ZM { get; set; } // коэффицент персчета в текущие цены зарплаты машиниста
        public double K_MT { get; set; } // коэффицент персчета в текущие цены материалов
        public Index Index { get; set; } // индексы пересчете общие для данной расцеки

        public ObservableCollection<Double> BaseParams
        {
            get { return baseParams; }
            set
            {
                baseParams = value;
                OnPropertyChanged("BaseParams");
            }
        }
        public ObservableCollection<Double> IndexedParams
        {
            get { return indexedParams; }
            set
            {
                indexedParams = value;
                OnPropertyChanged("IndexedParams");
            }
        }
        public ObservableCollection<CorrectionIndexes> CorrectionIndexes
        {
            get { return correctionIndexes; }
            set
            {
                correctionIndexes = value;
                OnPropertyChanged("CorrectionIndexes");
            }
        }
        public ObservableCollection<Resource> Resurсes
        {
            get { return resurсes; }
            set
            {
                resurсes = value;
                OnPropertyChanged("Resurсes");
            }
        }
        public ObservableCollection<Koefficient> Koefficients
        {
            get { return koefficients; }
            set
            {
                koefficients = value;
                OnPropertyChanged("Koefficients");
            }
        }
        public Position(string Name)
        {
            this.Name = Name;
            this.Caption = Name;
            Resurсes = new ObservableCollection<Resource>();

            Koefficients = new ObservableCollection<Koefficient>();
        }
        public void CalcParams()
        {
            Quantity = IndexedParams[(int)IndexedParamsType.W_SCOPE];
        }
        public (double value, string justification) GetCorrectionIndex(int CorrectionIndexType, int Target) //Получение значения поправочного коэфициента
        {
            ObservableCollection<CorrectionIndexes> CorrectionIndexesList;
            CorrectionIndexesList = new ObservableCollection<CorrectionIndexes>();
            double dResult = 1;
            string sJustification = "";
            foreach (CorrectionIndexes coridex in CorrectionIndexes)
            {
                if (coridex.Indexes[0] == CorrectionIndexType && coridex.Indexes[1] == Target) //Если необходимый поправочный индекс найден 
                    CorrectionIndexesList.Add(coridex); //Удаляем найденный индекс из времнного поправочных списка идексов.
            }
            foreach (CorrectionIndexes coridex in CorrectionIndexesList)
            {
                dResult = coridex.Indexes[3] * dResult;
                sJustification = sJustification + coridex.Justification;
            }

            var result = (value: dResult, justification: sJustification);
            return result;
        }

        private CorrectionIndexes FindCorrectionIndex(CorrectionIndexes correctionIndex, ObservableCollection<CorrectionIndexes> correctionIndexesList)
        {
            bool bInFlag = false;
            foreach (CorrectionIndexes coridex in CorrectionIndexes)
            {
                if (correctionIndex.Indexes[0] == correctionIndex.Indexes[0] && correctionIndex.Indexes[1] == correctionIndex.Indexes[1]) //Если нашли такие поправочные индексы передать ее ..
                {
                    return coridex;
                }
            }

            return null;
        }
        public Position()
        {
            Resurсes = new ObservableCollection<Resource>();
            Koefficients = new ObservableCollection<Koefficient>();
            BaseParams = new ObservableCollection<double>();
            CorrectionIndexes = new ObservableCollection<CorrectionIndexes>();
            for (int ii = 0; ii <= 9; ii++)
                BaseParams.Add(new Double());
            IndexedParams = new ObservableCollection<double>();
            for (int ii = 0; ii <= 12; ii++)
                IndexedParams.Add(new Double());


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
