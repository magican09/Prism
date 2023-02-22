using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace PrismWorkApp.OpenWorkLib.Estimate
{
    public class Estimate : INotifyPropertyChanged, ICloneable   //Смета
    {
        private string caption;
        private double dataOutIndicate;
        private double dataOutIndicate_1;
        private double dataOutIndicate_2;
        public int Id { get; set; }
        public int Namber { get; set; } // Номер раздела 
        public string Name { get; set; } //Название раздела
        public double UnseenCostCoefficient { get; set; } //Кофициент непридвиденных расходов
        public double NDS { get; set; } //НДС
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
        public double DataOutIndicate
        {
            get { return dataOutIndicate; }
            set
            {
                dataOutIndicate = value;
                OnPropertyChanged("DataOutIndicate");

            }
        } //Показатель индикации выгрузки данных уровень 1...
        public double DataOutIndicate_1
        {
            get { return dataOutIndicate_1; }
            set
            {
                dataOutIndicate_1 = value;
                OnPropertyChanged("DataOutIndicate_1");

            }
        } //Показатель индикации выгрузки данных уровень 1...
        public double DataOutIndicate_2
        {
            get { return dataOutIndicate_2; }
            set
            {
                dataOutIndicate_2 = value;
                OnPropertyChanged("DataOutIndicate_2");

            }
        } //Показатель индикации выгрузки данных уровень 2.....
        public int SysID { get; set; } // 
        public ObservableCollection<Chapter> Chapters { get; set; } //Расценки внутри раздела
        public ObservableCollection<Resource> Resurсes { get; set; } //Расценки внутри раздела
        public ObservableCollection<ImplemAct> ImplemActs { get; set; } //Наборы индексов
        public ObservableCollection<Index> Indexes { get; set; } //Наборы индексов
        public Estimate()
        {
            Chapters = new ObservableCollection<Chapter>();
            ImplemActs = new ObservableCollection<ImplemAct>();
            Indexes = new ObservableCollection<Index>();
        }

        public void LoadXMLData(XmlDocument xmlDoc) // Загрузка сметы из файла XmL
        {
            var root = xmlDoc.DocumentElement; // загружаем корневой узел xml документа

            foreach (XmlNode childnode_1 in root)
            {

                if (childnode_1.Name == "Chapters")
                    foreach (XmlNode childnode_2 in childnode_1)
                    {
                        if (childnode_2.Name == "Chapter")//Ищем раздел сметы
                        {
                            Chapters.Add(new Chapter()); //Создаем раздел сметы
                            if (childnode_2.Attributes.GetNamedItem("Caption") != null)
                            {
                                Chapters[Chapters.Count - 1].Name = childnode_2.Attributes.GetNamedItem("Caption").Value;
                                Chapters[Chapters.Count - 1].Caption = childnode_2.Attributes.GetNamedItem("Caption").Value;
                            }
                            else
                                Chapters[Chapters.Count - 1].Caption = "Нет названия";
                            if (childnode_2.Attributes.GetNamedItem("SysID") != null)
                                Chapters[Chapters.Count - 1].SysID = Convert.ToInt32(childnode_2.Attributes.GetNamedItem("SysID").Value);
                            foreach (XmlNode childnode_3 in childnode_2)
                            {
                                string sHeader = "";
                                if (childnode_3.Name == "Header") sHeader = childnode_3.Attributes.GetNamedItem("Caption").Value;
                                if (childnode_3.Name == "Position") //Проходим по расценкам сметы
                                {
                                    int iChapterPointer, iPositionPointer;
                                    iChapterPointer = Chapters.Count - 1;
                                    if (childnode_3.Attributes.GetNamedItem("Caption") != null)
                                    {
                                        Chapters[iChapterPointer].Positions.Add(new Position(childnode_3.Attributes.GetNamedItem("Caption").Value.Replace(";", ",")));//Добравляем расценку 
                                        iPositionPointer = Chapters[iChapterPointer].Positions.Count - 1;

                                        Chapters[iChapterPointer].Positions[iPositionPointer].Namber = Convert.ToInt32(childnode_3.Attributes.GetNamedItem("Number").Value);
                                        Chapters[iChapterPointer].Positions[iPositionPointer].Code = childnode_3.Attributes.GetNamedItem("Code").Value;
                                        Chapters[iChapterPointer].Positions[iPositionPointer].Units = childnode_3.Attributes.GetNamedItem("Units").Value;
                                        Chapters[iChapterPointer].Positions[iPositionPointer].SysID = Convert.ToInt32(childnode_3.Attributes.GetNamedItem("SysID").Value);

                                        foreach (XmlNode childnode_4 in childnode_3)//По разделам расценки
                                        {
                                            if (childnode_4.Name == "PriceBase")
                                            {
                                                if (childnode_4.Attributes.GetNamedItem("PZ") != null)
                                                    Chapters[iChapterPointer].Positions[iPositionPointer].PZ = Convert.ToDouble(childnode_4.Attributes.GetNamedItem("PZ").Value.Replace(",", "."));
                                                if (childnode_4.Attributes.GetNamedItem("OZ") != null)
                                                    Chapters[iChapterPointer].Positions[iPositionPointer].OZ = Convert.ToDouble(childnode_4.Attributes.GetNamedItem("OZ").Value.Replace(",", "."));
                                                if (childnode_4.Attributes.GetNamedItem("EM") != null)
                                                    Chapters[iChapterPointer].Positions[iPositionPointer].EM = Convert.ToDouble(childnode_4.Attributes.GetNamedItem("EM").Value.Replace(",", "."));
                                                if (childnode_4.Attributes.GetNamedItem("ZM") != null)
                                                    Chapters[iChapterPointer].Positions[iPositionPointer].ZM = Convert.ToDouble(childnode_4.Attributes.GetNamedItem("ZM").Value.Replace(",", "."));
                                                if (childnode_4.Attributes.GetNamedItem("MT") != null)
                                                    Chapters[iChapterPointer].Positions[iPositionPointer].MT = Convert.ToDouble(childnode_4.Attributes.GetNamedItem("MT").Value.Replace(",", "."));


                                            }
                                            if (childnode_4.Name == "Quantity")
                                            {
                                                if (childnode_4.Attributes.GetNamedItem("KUnit") != null)
                                                    Chapters[iChapterPointer].Positions[iPositionPointer].KUnit = Convert.ToInt32(childnode_4.Attributes.GetNamedItem("KUnit").Value);
                                                double dbuffer = 0;
                                                string sbuf;
                                                if (childnode_4.Attributes.GetNamedItem("Result") != null)
                                                {
                                                    sbuf = childnode_4.Attributes.GetNamedItem("Result").Value;

                                                    Chapters[iChapterPointer].Positions[iPositionPointer].Quantity = Convert.ToDouble(childnode_4.Attributes.GetNamedItem("Result").Value.Replace(',', '.'));

                                                    // Chapters[iChapterPointer].Positions[iPositionPointer].Quantity = dbuffer;
                                                }
                                            }
                                            if (childnode_4.Name == "Resources")
                                                foreach (XmlNode childnode_5 in childnode_4)//По разделам ресурсов расценки 
                                                {
                                                    if (childnode_5.Name == "Tzr" || childnode_5.Name == "Mch" || childnode_5.Name == "Mat")
                                                    {
                                                        int iResursePointer;
                                                        Chapters[iChapterPointer].Positions[iPositionPointer].Resurсes.Add(new Resource());
                                                        iResursePointer = Chapters[iChapterPointer].Positions[iPositionPointer].Resurсes.Count - 1;
                                                        Chapters[iChapterPointer].Positions[iPositionPointer].Resurсes[iResursePointer].Caption = childnode_5.Attributes.GetNamedItem("Caption").Value.Replace(";", ",");
                                                        Chapters[iChapterPointer].Positions[iPositionPointer].Resurсes[iResursePointer].Code = childnode_5.Attributes.GetNamedItem("Code").Value;
                                                        Chapters[iChapterPointer].Positions[iPositionPointer].Resurсes[iResursePointer].Units = childnode_5.Attributes.GetNamedItem("Units").Value;
                                                        if (childnode_5.Name != "Tzm" && childnode_5.Name != "Mat")
                                                            Chapters[iChapterPointer].Positions[iPositionPointer].Resurсes[iResursePointer].PriceBase = Convert.ToDouble(childnode_5.ChildNodes[0].Attributes["Value"].Value.Replace(",", "."));
                                                        if (childnode_5.Name == "Tzr")
                                                        {
                                                            Chapters[iChapterPointer].Positions[iPositionPointer].Resurсes[iResursePointer].Type = ResurceType.WORKER;
                                                            Chapters[iChapterPointer].Positions[iPositionPointer].Resurсes[iResursePointer].Group = "Трудовые";
                                                            //  Chapters[iChapterPointer].Positions[iPositionPointer].Resourсes[iResursePointer].WorkClass = childnode_5.Attributes.GetNamedItem("WorkClass").Value;
                                                            Chapters[iChapterPointer].Positions[iPositionPointer].Resurсes[iResursePointer].WorkClass = Convert.ToDouble(childnode_5.Attributes.GetNamedItem("WorkClass").Value.Replace(',', '.'));

                                                        }
                                                        if (childnode_5.Name == "Mch")
                                                        {
                                                            Chapters[iChapterPointer].Positions[iPositionPointer].Resurсes[iResursePointer].Type = ResurceType.MEC_WORKER;
                                                            Chapters[iChapterPointer].Positions[iPositionPointer].Resurсes[iResursePointer].Group = "Машины и механизмы";
                                                        }
                                                        if (childnode_5.Name == "Mat")
                                                        {
                                                            Chapters[iChapterPointer].Positions[iPositionPointer].Resurсes[iResursePointer].Type = ResurceType.MATERIAL;
                                                            Chapters[iChapterPointer].Positions[iPositionPointer].Resurсes[iResursePointer].Group = "Материалы";
                                                            //if(childnode_5.ChildNodes.Count>0)
                                                            //Chapters[iChapterPointer].Positions[iPositionPointer].Resourсes[iResursePointer].PriceBase = Convert.ToDouble(childnode_5.ChildNodes[0].Attributes.GetNamedItem("Value").Value.Replace(",", "."));

                                                        }

                                                        /*if (childnode_5.Attributes.GetNamedItem("Quantity") != null)
                                                            Chapters[iChapterPointer].Positions[iPositionPointer].Resourсes[iResursePointer].NormConsumption =// Устанавливаемнорму расхода ресурса на единицу
                                                            Convert.ToDouble(childnode_5.Attributes.GetNamedItem("Quantity").Value.Replace(',', '.'));
                                                      *///  Chapters[iChapterPointer].Positions[iPositionPointer].Resourсes[iResursePointer].Quantity = // Устанавливаемрасход ресурса
                                                        /* Chapters[iChapterPointer].Positions[iPositionPointer].Resourсes[iResursePointer].NormConsumption *
                                                         Convert.ToDouble(Chapters[iChapterPointer].Positions[iPositionPointer].Quantity); *///Высляеми фактический расход

                                                    }

                                                }
                                            if (childnode_4.Name == "WorksList")
                                            {
                                                foreach (XmlNode childnode_5 in childnode_4)//По разделам ресурсов расценки 
                                                {
                                                    if (childnode_5.Name == "Work")
                                                    {
                                                        foreach (Resource res in Chapters[iChapterPointer].Positions[iPositionPointer].Resurсes)
                                                        {
                                                            if (res.Type == ResurceType.WORKER) res.ResurсesTypeList.Add(childnode_5.Attributes.GetNamedItem("Caption").Value);
                                                            // res.ResourсesTypeObservableCollection.Add("dsd");
                                                        }
                                                    }


                                                }

                                            }//По списку работ...

                                            if (childnode_4.Name == "Koefficients")
                                            {
                                                foreach (XmlNode childnode_5 in childnode_4)//По разделам ресурсов расценки 
                                                {
                                                    if (childnode_5.Name == "K")
                                                    {
                                                        /* Chapters[iChapterPointer].Positions[iPositionPointer].Koefficients.Add(new Koefficient());
                                                      Chapters[iChapterPointer].Positions[iPositionPointer].Koefficients[Chapters[iChapterPointer].Positions[iPositionPointer].Koefficients.Count -1].Value_OZ =
                                                      Convert.ToDouble(childnode_5.Attributes.GetNamedItem("Value_OZ").Value.Replace(",", "."));
                                                      Chapters[iChapterPointer].Positions[iPositionPointer].Koefficients[Chapters[iChapterPointer].Positions[iPositionPointer].Koefficients.Count - 1].Value_EM =
                                                      Convert.ToDouble(childnode_5.Attributes.GetNamedItem("Value_EM").Value.Replace(",", "."));
                                                      Chapters[iChapterPointer].Positions[iPositionPointer].Koefficients[Chapters[iChapterPointer].Positions[iPositionPointer].Koefficients.Count - 1].Level =
                                                      Convert.ToInt32(childnode_5.Attributes.GetNamedItem("Level").Value.Replace(",", "."));
                                                      */
                                                        if (childnode_5.Attributes.GetNamedItem("Value_OZ") != null)
                                                            Chapters[iChapterPointer].Positions[iPositionPointer].K_OZ = Convert.ToDouble(childnode_5.Attributes.GetNamedItem("Value_OZ").Value.Replace(",", "."));
                                                        else Chapters[iChapterPointer].Positions[iPositionPointer].K_OZ = 1;
                                                        if (childnode_5.Attributes.GetNamedItem("Value_EM") != null)
                                                        {
                                                            Chapters[iChapterPointer].Positions[iPositionPointer].K_EM = Convert.ToDouble(childnode_5.Attributes.GetNamedItem("Value_EM").Value.Replace(",", "."));
                                                            Chapters[iChapterPointer].Positions[iPositionPointer].K_ZM = Chapters[iChapterPointer].Positions[iPositionPointer].K_EM;
                                                        }
                                                        else Chapters[iChapterPointer].Positions[iPositionPointer].K_EM = 1;
                                                        if (childnode_5.Attributes.GetNamedItem("Value_MT") != null)
                                                            Chapters[iChapterPointer].Positions[iPositionPointer].K_MT = Convert.ToDouble(childnode_5.Attributes.GetNamedItem("Value_MT").Value.Replace(",", "."));
                                                        else Chapters[iChapterPointer].Positions[iPositionPointer].K_MT = 1;
                                                        //  if (childnode_5.Attributes.GetNamedItem("Value_ZM") != null)
                                                        //    Chapters[iChapterPointer].Positions[iPositionPointer].K_ZM = Convert.ToDouble(childnode_5.Attributes.GetNamedItem("Value_ZM").Value.Replace(",", "."));
                                                        //  else
                                                        //    Chapters[iChapterPointer].Positions[iPositionPointer].K_ZM = 1;

                                                    }
                                                }

                                            }//По списку коэффицентов...

                                        }
                                    }
                                }
                            }
                        }
                    }
            }
            var IndexesXml = root.GetElementsByTagName("Indexes"); //Загружаем наборы текущих индесов  общий
            int IndexesCount = 0;
            foreach (XmlNode childnode_1 in IndexesXml[0])
            {

                foreach (XmlNode childnode_2 in childnode_1) //Пробегаем по внутренней структуре  ImplemActs
                {
                    if (childnode_2.Name == "Index")
                        Indexes.Add(new Index());
                    IndexesCount = Indexes.Count - 1;
                    foreach (XmlAttribute ind_atr in childnode_2.Attributes)
                    {

                        switch (ind_atr.Name)
                        {

                            case "Caption":
                                Indexes[IndexesCount].Caption = (childnode_2.Attributes.GetNamedItem("Caption").Value);
                                break;
                            case "Code":
                                Indexes[IndexesCount].Code = (childnode_2.Attributes.GetNamedItem("Code").Value);
                                break;
                            case "OZ":
                                Indexes[IndexesCount].K_OZ = Convert.ToDouble(childnode_2.Attributes.GetNamedItem("OZ").Value.Replace(",", "."));
                                break;
                            case "EM":
                                Indexes[IndexesCount].K_EM = Convert.ToDouble(childnode_2.Attributes.GetNamedItem("EM").Value.Replace(",", "."));
                                break;
                            case "ZM":
                                Indexes[IndexesCount].K_ZM = Convert.ToDouble(childnode_2.Attributes.GetNamedItem("ZM").Value.Replace(",", "."));
                                break;
                            case "MT":
                                Indexes[IndexesCount].K_MT = Convert.ToDouble(childnode_2.Attributes.GetNamedItem("MT").Value.Replace(",", "."));
                                break;
                        }

                    }
                }


            }

            var ImplemActsXml = root.GetElementsByTagName("ImplemActs"); //Загружаем наборы текущих индесов 
            int iImplemActsCount = 0;
           if(ImplemActsXml.Count>0)
            foreach (XmlNode childnode_1 in ImplemActsXml[0])
            {
                ImplemActs.Add(new ImplemAct());
                iImplemActsCount = ImplemActs.Count - 1;
                foreach (XmlAttribute atr in childnode_1.Attributes)
                {
                    switch (atr.Name)
                    {
                        case "Caption":
                            ImplemActs[iImplemActsCount].Caption = childnode_1.Attributes.GetNamedItem("Caption").Value;
                            break;
                        case "Number":
                            ImplemActs[iImplemActsCount].Namber = Convert.ToInt32(childnode_1.Attributes.GetNamedItem("Number").Value);
                            break;
                        case "MakingDate":
                            ImplemActs[iImplemActsCount].MakingDate = Convert.ToDateTime(childnode_1.Attributes.GetNamedItem("MakingDate").Value);
                            break;
                        case "Year":
                            ImplemActs[iImplemActsCount].Year = childnode_1.Attributes.GetNamedItem("Year").Value;
                            break;
                        case "Month":
                            ImplemActs[iImplemActsCount].Month = childnode_1.Attributes.GetNamedItem("Month").Value;
                            break;
                        case "DayStart":
                            ImplemActs[iImplemActsCount].DayStart = Convert.ToInt32(childnode_1.Attributes.GetNamedItem("DayStart").Value);
                            break;
                        case "DayFinish":
                            ImplemActs[iImplemActsCount].DayFinish = Convert.ToInt32(childnode_1.Attributes.GetNamedItem("DayFinish").Value);
                            break;
                        case "ActIndex":
                            ImplemActs[iImplemActsCount].ActIndex = Convert.ToInt32(childnode_1.Attributes.GetNamedItem("ActIndex").Value);
                            break;

                    }

                    foreach (XmlNode childnode_2 in childnode_1) //Пробегаем по внутренней структуре  ImplemActs
                    {
                        if (childnode_2.Name == "Indexes")
                            foreach (XmlNode childnode_3 in childnode_2)
                            {
                                if (childnode_3.Name == "IndexesPos")
                                {
                                    foreach (XmlNode childnode_4 in childnode_3)
                                    {
                                        ImplemActs[iImplemActsCount].Indexs.Add(new Index());
                                        foreach (XmlAttribute ind_atr in childnode_4.Attributes)
                                        {

                                            int iIndexesCount = ImplemActs[iImplemActsCount].Indexs.Count - 1;
                                            switch (ind_atr.Name)
                                            {

                                                case "Caption":
                                                    ImplemActs[iImplemActsCount].Indexs[iIndexesCount].Caption = (childnode_4.Attributes.GetNamedItem("Caption").Value);
                                                    break;
                                                case "Code":
                                                    ImplemActs[iImplemActsCount].Indexs[iIndexesCount].Code = (childnode_4.Attributes.GetNamedItem("Code").Value);
                                                    break;
                                                case "OZ":
                                                    ImplemActs[iImplemActsCount].Indexs[iIndexesCount].K_OZ = Convert.ToDouble(childnode_4.Attributes.GetNamedItem("OZ").Value.Replace(",", "."));
                                                    break;
                                                case "EM":
                                                    ImplemActs[iImplemActsCount].Indexs[iIndexesCount].K_EM = Convert.ToDouble(childnode_4.Attributes.GetNamedItem("EM").Value.Replace(",", "."));
                                                    break;
                                                case "ZM":
                                                    ImplemActs[iImplemActsCount].Indexs[iIndexesCount].K_ZM = Convert.ToDouble(childnode_4.Attributes.GetNamedItem("ZM").Value.Replace(",", "."));
                                                    break;
                                                case "MT":
                                                    ImplemActs[iImplemActsCount].Indexs[iIndexesCount].K_MT = Convert.ToDouble(childnode_4.Attributes.GetNamedItem("MT").Value.Replace(",", "."));
                                                    break;
                                            }
                                            if (ImplemActs[iImplemActsCount].Indexs[iIndexesCount].K_OZ == 0) ImplemActs[iImplemActsCount].Indexs[iIndexesCount].K_OZ = 1;
                                            if (ImplemActs[iImplemActsCount].Indexs[iIndexesCount].K_EM == 0) ImplemActs[iImplemActsCount].Indexs[iIndexesCount].K_EM = 1;
                                            if (ImplemActs[iImplemActsCount].Indexs[iIndexesCount].K_ZM == 0) ImplemActs[iImplemActsCount].Indexs[iIndexesCount].K_ZM = 1;
                                            if (ImplemActs[iImplemActsCount].Indexs[iIndexesCount].K_MT == 0) ImplemActs[iImplemActsCount].Indexs[iIndexesCount].K_MT = 1;
                                        }
                                    }
                                }
                            }



                    }
                }
            }

            foreach (Chapter chapter in Chapters)
            {
                foreach (Position position in chapter.Positions)
                {
                    foreach (Index index in Indexes)
                    {
                        if (position.Code == index.Code)
                            position.Index = index;
                    }
                }
            }

        }
       
        public void LoadARPData(string fileName) // Загрузка сметы из файла АРП
        {
            int positionCounter = 0;
            int chapterCounter = 0;
            string parsingString;
            using (StreamReader sr = new StreamReader(fileName, Encoding.GetEncoding(866)))
            {
                while (sr.EndOfStream != true)
                {
                    parsingString = sr.ReadLine();
                    if (parsingString.Split(new char[] { '#' })[0] == "3")//Идентификация документа
                        Code03Parser(parsingString);


                    if (parsingString.Split(new char[] { '#' })[0] == "10")//Добавляем раздел в смету
                        Chapters.Add(Code10Parser(parsingString));
                    if (parsingString.Split(new char[] { '#' })[0] == "20")
                    {
                        Chapters[Chapters.Count - 1].Positions.Add(Code20Parser(parsingString));//Добавляем позицию в смету
                        Chapters[Chapters.Count - 1].Positions[Chapters[Chapters.Count - 1].Positions.Count - 1].CalcParams();
                    }
                    if (parsingString.Split(new char[] { '#' })[0] == "25")//
                        Code25Parser(parsingString, Chapters[Chapters.Count - 1].Positions[Chapters[Chapters.Count - 1].Positions.Count - 1]);
                    if (parsingString.Split(new char[] { '#' })[0] == "30")
                        Code30Parser(parsingString, Chapters[Chapters.Count - 1].Positions[Chapters[Chapters.Count - 1].Positions.Count - 1]);
                    if (parsingString.Split(new char[] { '#' })[0] == "50")
                        Code50Parser(parsingString);
                }
            }
        }
        public void Code03Parser(string str)  // Парсер кода #03  
        {
            Chapter chapter = new Chapter();
            string[] parms = str.Split(new char[] { '#' });
            Caption = parms[2];

        }
        public Chapter Code10Parser(string str)  // Парсер кода #10  Заголовок раздела документа
        {
            Chapter chapter = new Chapter();
            string[] parms = str.Split(new char[] { '#' });
            chapter.Level = Convert.ToInt32(parms[1]);
            if (parms[2] != "")
                chapter.Namber = Convert.ToInt32(parms[2]);
            chapter.Caption = parms[3];
            return chapter;
        }
        public Position Code20Parser(string str)  // Парсер кода #20 -позиция документа
        {
            Position position = new Position();
            string[] parms = str.Split(new char[] { '#' });

            position.Namber = Convert.ToInt32(parms[1]);
            position.Code = parms[2];
            position.Units = parms[3];
            position.Caption = parms[4];
            for (int ii = 0; ii <= 9; ii++)
                position.BaseParams[ii] = Convert.ToDouble(parms[5 + ii].Replace(",", "."));
            for (int ii = 0; ii <= 12; ii++)
                position.IndexedParams[ii] = Convert.ToDouble(parms[15 + ii].Replace(",", "."));


            if (position.Code.Contains("ТССЦ"))
                position.Type = PositionType.TSCC;
            else if (position.Code.Contains("ТЕР"))
            {
                position.Type = PositionType.TER;


            }
            else
                position.Type = PositionType.COMMERCIAL;

            /*position.Resourсes = new ObservableCollection<Resourсe>();
            position.Resourсes.Add(new Resourсe());
            position.Resourсes[position.Resourсes.Count - 1].Type = ResurceType.WORKER;
            position.Resourсes[position.Resourсes.Count - 1]. = Convert.ToDouble(parms[8].Replace(",", ".")); // Заработная плата машинистов (число, руб.) (входит в общую стоимость эксплуатации машин и механизмов).
            */


            return position;
        }
        public void Code25Parser(string str, Position position)  // Парсер кода #25 -позиция сметы
        {
            string[] parms = str.Split(new char[] { '#' });
            double dParam3 = 0; ;
            position.CorrectionIndexes.Add(new CorrectionIndexes());
            for (int ii = 0; ii <= 3; ii++)
                position.CorrectionIndexes[position.CorrectionIndexes.Count - 1].Indexes[ii] = Convert.ToDouble(parms[1 + ii].Replace(",", "."));
            position.CorrectionIndexes[position.CorrectionIndexes.Count - 1].Name = parms[4];
            position.CorrectionIndexes[position.CorrectionIndexes.Count - 1].Justification = parms[5];
            ///////////////////////Проверка суммы процентов по описанию
            Regex regex = new Regex(@"(\d\W\d)%");
            if (parms[5] != "" && parms[5].Contains('%'))
            {
                MatchCollection matches = regex.Matches(parms[5]); // 
                foreach (Match mh in matches)
                    dParam3 += Convert.ToDouble(mh.Value.Replace(',', '.').Replace('%', ' '));
                dParam3 = dParam3 / 100 + 1;
                position.CorrectionIndexes[position.CorrectionIndexes.Count - 1].Indexes[3] = dParam3;
                // if()
            }


        }
        public void Code30Parser(string str, Position position)  // Парсер кода #30 -позиция сметы
        {
            string[] parms = str.Split(new char[] { '#' });
            position.Resurсes.Add(new Resource());
            position.Resurсes[position.Resurсes.Count - 1].Code = parms[1];
            position.Resurсes[position.Resurсes.Count - 1].Units = parms[2];
            position.Resurсes[position.Resurсes.Count - 1].Name = parms[3];
            position.Resurсes[position.Resurсes.Count - 1].Caption = parms[3];
            position.Resurсes[position.Resurсes.Count - 1].Type = (ResurceType)Convert.ToInt32(parms[4]);
            position.Resurсes[position.Resurсes.Count - 1].NormConsumption = Convert.ToDouble(parms[5].Replace(",", "."));
            position.Resurсes[position.Resurсes.Count - 1].NormPrice = Convert.ToDouble(parms[6].Replace(",", "."));
            position.Resurсes[position.Resurсes.Count - 1].Price = Convert.ToDouble(parms[7].Replace(",", "."));
            position.Resurсes[position.Resurсes.Count - 1].PriceLocal = Convert.ToDouble(parms[8].Replace(",", "."));
            if (position.Resurсes[position.Resurсes.Count - 1].Type == ResurceType.WORKER &&
                    position.Resurсes[position.Resurсes.Count - 1].Name.Contains("Затраты труда рабочих")) // Если ресурс  работники
            {
                position.Resurсes[position.Resurсes.Count - 1].WorkClass = Convert.ToDouble(position.Resurсes[position.Resurсes.Count - 1].Code.Remove(0, 2).Replace('-', '.'));
                position.Resurсes[position.Resurсes.Count - 1].Group = "Трудозатраты";
            }




        }
        public void Code50Parser(string str)  // Парсер кода #50 -позиция сметы
        {
            string[] parms = str.Split(new char[] { '#' });


            if (parms[1].Contains("Непредвиденные"))
                UnseenCostCoefficient = Convert.ToDouble(parms[2].Replace(',', '.'));
            if (parms[1].Contains("НДС"))
                NDS = Convert.ToDouble(parms[2].Replace(',', '.'));


        }
        public void ParseResources()
        {
            int kk = 1;
            foreach (Chapter part in Chapters) ///Удаление отрицательных материалных расценок и соотвествующих ресурсов из расценок
            {
                for (int ii = 0; ii < part.Positions.Count - 1; ii++)
                {
                    while ((ii + kk) < part.Positions.Count && (part.Positions[ii + kk].Type == PositionType.TSCC ||
                     part.Positions[ii + kk].Type == PositionType.COMMERCIAL))
                    {

                        for (int jj = 0; jj < part.Positions[ii].Resurсes.Count; jj++) //Проходим по ресурсам расценки и ищем там материал
                        {
                            //if (part.Positions[ii].Resourсes[jj].Code == part.Positions[ii + 1].Code.Replace("ТССЦ-", "").Replace(" ","")) //Если нашли материал, то копируем следующую  
                            if (TanimotoK(part.Positions[ii].Resurсes[jj].Code, part.Positions[ii + kk].Code.Replace("ТССЦ-", "")) ||
                             TanimotoK(part.Positions[ii].Resurсes[jj].Name, part.Positions[ii + kk].Name)) //Если нашли материал, то копируем следующую  
                            {
                                if (part.Positions[ii + kk].Quantity < 0)
                                {
                                    part.Positions[ii].Resurсes.Remove(part.Positions[ii].Resurсes[jj]);//Удаяем ресурс из текущей расценки

                                    part.Positions.Remove(part.Positions[ii + kk]); //Удаляем расценку с ресурсом
                                    jj--;
                                    ii--;
                                }
                            }


                        }
                        kk++;
                    }
                    kk = 1;
                }

            }

            foreach (Chapter part in Chapters) // Перенос материалов из расценов в расценки выше
            {
                for (int ii = 0; ii < part.Positions.Count - 1; ii++)
                {
                    if (part.Positions[ii + 1].Type == PositionType.TSCC || part.Positions[ii + 1].Type == PositionType.COMMERCIAL) //Если слудующая расценка материал, то проверяем текущую расценку на это материал
                    {
                        for (int jj = 0; jj < part.Positions[ii].Resurсes.Count; jj++) //Проходим по ресурсам расценки и ищем там материал
                        {
                            if (part.Positions[ii + 1].Type == PositionType.TSCC || part.Positions[ii + 1].Type == PositionType.COMMERCIAL) //Если слудющая расценка материал , добавляем в текущую расценку
                            {
                                part.Positions[ii].Resurсes.Add(new Resource());
                                int iResursePointer;
                                iResursePointer = part.Positions[ii].Resurсes.Count - 1;
                                part.Positions[ii].Resurсes[iResursePointer].Name = part.Positions[ii + 1].Name;
                                // part.Positions[ii].Resurсes[iResursePointer].Quantity = part.Positions[ii + 1].Quantity;
                                part.Positions[ii].Resurсes[iResursePointer].Units = part.Positions[ii + 1].Units;
                                part.Positions[ii].Resurсes[iResursePointer].Type = ResurceType.MATERIAL;
                                part.Positions[ii].Resurсes[iResursePointer].Code = part.Positions[ii + 1].Code.Replace("ТССЦ-", "");
                                part.Positions.Remove(part.Positions[ii + 1]);
                                if (ii > 0) ii--;

                            }

                        }
                    }
                }

            }

            ResusrceFilter(); //Нормируем ресурсы в кг,м, м2, м3 и зануляем слишком маленькие значения 
            foreach (Chapter part in Chapters) //Удаляем нулевые  
                foreach (Position price in part.Positions)
                    for (int ii = 0; ii < price.Resurсes.Count; ii++)
                    {
                        /*            if (price.Resurсes[ii].Quantity < 0)
                                    {
                                        price.Resurсes.Remove(price.Resurсes[ii]);
                                        ii--;
                                    }*/
                    }

            Resurсes = new ObservableCollection<Resource>();
            bool bFlag = false;
            foreach (Chapter part in Chapters) //Создаем список ресурсов
                foreach (Position price in part.Positions)
                    foreach (Resource res_1 in price.Resurсes)
                    {
                        foreach (Resource res_2 in Resurсes)
                        {
                            if (res_1.Code == res_2.Code && res_1.Name == res_2.Name)
                            {
                                bFlag = true; //Если в общес списке имеется ресурс
                                res_2.Quantity += res_1.Quantity; //Накапливаем общую потребность в ресурсах
                            }
                        }
                        if (bFlag == false)//Если ресурса в общем списке нет - создаем его там 
                        {
                            Resurсes.Add((Resource)res_1.Clone());

                        }
                        bFlag = false;
                    }
        }
        public int GetResourceID(string name, string code)
        {
            foreach (Resource res in Resurсes)
            {
                if (TanimotoK(res.Name, name) && res.Code == code) return (res.Id);
            }
            return 0;
        }
        public void ResusrceFilter()
        {
            foreach (Chapter chapter in Chapters) //Фильтруем значения ресурсов
                foreach (Position price in chapter.Positions)
                    foreach (Resource res_1 in price.Resurсes)
                    {
                        if (res_1.Units == "т")
                        {
                            res_1.Units = "кг";
                            //  res_1.Quantity *= 1000;
                            res_1.NormConsumption *= 1000;

                        }
                        if (res_1.Units == "м3" && res_1.Name.Contains("Вода"))
                        {
                            res_1.Units = "л";
                            //   res_1.Quantity *= 1000;
                            res_1.NormConsumption *= 1000;

                        }
                        if (res_1.Units == "100 м2")
                        {
                            res_1.Units = "м2";
                            // res_1.Quantity *= 100;
                            res_1.NormConsumption *= 100;

                        }
                        if (res_1.Units == "100 м")
                        {
                            res_1.Units = "м.";
                            // res_1.Quantity *= 100;
                            res_1.NormConsumption *= 100;
                        }
                        if (res_1.Units == "10 м")
                        {
                            res_1.Units = "м.";
                            //res_1.Quantity *= 10;
                            res_1.NormConsumption *= 10;
                        }

                        /*if (res_1.Units == "л" && res_1.Quantity < Convert.ToDouble(textBoxLitr.Text.Replace(",", "."))) res_1.Quantity = 0;
                        if (res_1.Units == "маш.час" && res_1.Quantity < Convert.ToDouble(textBoxMashChas.Text.Replace(",", "."))) res_1.Quantity = 0;
                        if (res_1.Units == "м3" && res_1.Quantity < Convert.ToDouble(textBoxMetr3.Text.Replace(",", "."))) res_1.Quantity = 0;
                        if (res_1.Units == "м2" && res_1.Quantity < Convert.ToDouble(textBoxMetr2.Text.Replace(",", "."))) res_1.Quantity = 0;
                        if (res_1.Units == "м") res_1.Units = "м.";
                        if (res_1.Units == "м." && res_1.Quantity < Convert.ToDouble(textBoxMetr.Text.Replace(",", "."))) res_1.Quantity = 0; ;
                        if (res_1.Units == "кг" && res_1.Quantity < Convert.ToDouble(textBoxKg.Text.Replace(",", "."))) res_1.Quantity = 0;
                        */
                        res_1.Name.Replace(";", ", ");
                    }
        }
        public bool TanimotoK(string s1, string s2) //Функция определяет степень совпадения строк
        {
            int a = s1.Length;
            int b = s2.Length;
            int c = 0;
            const double NAMES_PROB = 0.9;

            Regex regex = new Regex(@"\W");
            s1 = regex.Replace(s1, "");
            s2 = regex.Replace(s2, "");


            for (int ii = 0; ii < s1.Length && ii < s2.Length; ii++)
            {
                if (s1[ii] == s2[ii]) c++;
            }
            double d = (double)c / s1.Length;
            // double d = (double)c / (a + b - c);
            if (d > NAMES_PROB) return true;
            else return false;
        }
        //public void WorkDataOutExell()
        //{
        //    Exel._Application exlApp = new Exel.Application();
        //    exlApp.Workbooks.Add();
        //    Exel.Workbook wkb = exlApp.Workbooks[1];
        //    Exel.Worksheet wsh = wkb.Worksheets[1];
        //    exlApp.Visible = true;
        //    int ii = 3;
        //    wsh.Columns[1].ColumnWidth = 100;
        //    wsh.Columns[2].ColumnWidth = 20;
        //    wsh.Columns[3].ColumnWidth = 10;
        //    wsh.Columns[4].ColumnWidth = 5;
        //    wsh.Columns[5].ColumnWidth = 6;
        //    wsh.Columns[6].ColumnWidth = 6;
        //    wsh.Columns[7].ColumnWidth = 6;
        //    wsh.Columns[8].ColumnWidth = 6;

        //    wsh.Cells[1, 1] = "Наименование работ";
        //    wsh.Cells[1, 2] = "Ед.изм.";
        //    wsh.Cells[1, 3] = "Кол-во.";
        //    wsh.Cells[1, 4] = "Цена за ед.изм.";
        //    wsh.Cells[1, 5] = "Итого:";
        //    wsh.Cells[1, 6] = "ФОТ+СП+НР Руб. за ед. по смете";
        //    wsh.Cells[1, 7] = "Руб. итого по смете";
        //    wsh.Cells[1, 8] = "Труд.затр на ед.изм. чел.час.";
        //    wsh.Cells[1, 9] = "Средн. разряд раб.";
        //    wsh.Cells[1, 10] = "ФОТ по смете";
        //    int iStarlLine;
        //    //  SelectedEstimate.GetTreeView(ref treeView1, Convert.ToDouble(textBox1LevelPay.Text), Convert.ToDouble(textBox6LevelPay.Text));
        //    foreach (Chapter chapter in Chapters) //Фильтруем значения ресурсов
        //    {
        //        wsh.Cells[ii++, 1] = chapter.Caption;
        //        foreach (Position position in chapter.Positions)
        //        {
        //            if (position.Type == PositionType.TER)
        //            {

        //                wsh.Cells[ii, 1].Font.Bold = true;
        //                wsh.Cells[ii, 2].Font.Bold = true;
        //                wsh.Cells[ii, 3].Font.Bold = true;
        //                wsh.Cells[ii, 4].Font.Bold = true;
        //                wsh.Cells[ii, 5].Font.Bold = true;
        //                wsh.Cells[ii, 6].Font.Bold = true;
        //                wsh.Cells[ii, 7].Font.Bold = true;

        //                double w_scope = 0; double em = 0; double k_nr = 0; double nr = 0; double k_zpm = 0;
        //                double pz = 0; double zpm = 0; ; double k_sp = 0; double sp = 0; double k_mr = 0;
        //                double zp = 0; double mr = 0; double k_zp = 0; double k_em = 0;

        //                double ki_nr = 0; double ki_zpm = 0;
        //                double ki_sp = 0; double ki_mr = 0;
        //                double ki_zp = 0; double ki_em = 0;
        //                w_scope = position.IndexedParams[(int)IndexedParamsType.W_SCOPE];//Объем работ (число).
        //                pz = position.BaseParams[(int)BaseParamsType.PZ];// Прямые затраты (всего) (число, руб.) база.
        //                zp = position.BaseParams[(int)BaseParamsType.ZP];
        //                em = position.BaseParams[(int)BaseParamsType.EM];// Стоимость эксплуатации машин и механизмов (число, руб.)  база.
        //                zpm = position.BaseParams[(int)BaseParamsType.ZPM];// Заработная плата машинистов (число, руб.) (входит в общую стоимость эксплуатации машин и механизмов) база.
        //                mr = position.BaseParams[(int)BaseParamsType.MR];// Стоимость материалов (число, руб.) база.
        //                k_nr = position.GetCorrectionIndex(2, 0).value;// Норматив накладных расходов 2 – норматив накладных расходов,0 – заработная плата.
        //                k_sp = position.GetCorrectionIndex(3, 0).value;// Норматив сметной прибыли.3 – норматив сметной прибыли,0 – заработная плата.
        //                k_zp = position.GetCorrectionIndex(4, 0).value;// (4 – коэффициент учета условий работ,0 – заработная плата)*.
        //                k_em = position.GetCorrectionIndex(4, 1).value;// 4 – коэффициент учета условий работ,1 – эксплуатация машин и механизмов.
        //                k_zpm = position.GetCorrectionIndex(4, 3).value;// 4 – коэффициент учета условий работ,3 – заработная плата механизаторов.
        //                k_mr = position.GetCorrectionIndex(4, 2).value;// 4 – коэффициент учета условий работ,2 – стоимость материалов
        //                zp = zp * k_zp * w_scope;// ЗП в базисном уровне цен 
        //                em = em * k_em * w_scope;// ЭМ в базисном уровне цен
        //                zpm = zpm * k_zpm * w_scope;// ЗПМ в базисном уровне цен 
        //                mr = mr * k_mr * w_scope; // // МР в базисном уровне цен
        //                nr = (zp + zpm) * k_nr;// НР в базисном уровне цен 
        //                sp = (zp + zpm) * k_sp;// СП в базисном уровне цен 
        //                ki_zp = position.GetCorrectionIndex(0, 0).value;// 0 – коэффициент учета инфляции (коэффициент пересчета сметных цен),0 – заработная плата.
        //                ki_em = position.GetCorrectionIndex(0, 1).value;// 0 – коэффициент учета инфляции (коэффициент пересчета сметных цен),1 – эксплуатация машин и механизмов.
        //                ki_zpm = position.GetCorrectionIndex(0, 3).value;// 0 – коэффициент учета инфляции (коэффициент пересчета сметных цен),3 – заработная плата механизаторов.
        //                ki_mr = position.GetCorrectionIndex(0, 2).value;// 0 – коэффициент учета инфляции (коэффициент пересчета сметных цен),2 – стоимость материалов.
        //                zp = zp * ki_zp;
        //                em = em * ki_em;
        //                zpm = zpm * ki_zpm;
        //                mr = mr * ki_mr;
        //                nr = nr * ki_zp;
        //                sp = sp * ki_zp;


        //                /* wsh.Cells[ii + 2, 10] = zp;
        //                 wsh.Cells[ii + 3, 10] = em;
        //                 wsh.Cells[ii + 4, 10] = zpm;
        //                 wsh.Cells[ii + 5, 10] = mr;
        //                 wsh.Cells[ii + 6, 10] = nr;
        //                 wsh.Cells[ii + 7, 10] = sp;
        //                 wsh.Cells[ii + 8, 10] = zp + em + mr + nr + sp;
        //                 */
        //                if (position.KUnit == 0)
        //                    position.KUnit = 1;
        //                wsh.Cells[ii, 1] = position.Caption;
        //                wsh.Cells[ii, 2] = position.Units;
        //                wsh.Cells[ii, 3] = position.Quantity;
        //                wsh.Cells[ii, 4] = zp + em + nr + sp;
        //                /*                        string sUnints;
        //                                        sUnints = position.Units;
        //                                        sUnints = sUnints.Replace("100", "");
        //                                        sUnints = sUnints.Replace("10", "");
        //                                        sUnints = sUnints.Replace("1", "");
        //                */



        //                iStarlLine = ii + 1;
        //                foreach (Resurсe res in position.Resurсes)
        //                {
        //                    if (res.Type == ResurceType.WORKER)
        //                    {
        //                        wsh.Cells[++ii, 1] = res.Caption;
        //                        wsh.Cells[ii, 8] = res.NormConsumption; //"Труд.затр на ед.изм. чел.час."
        //                        /*   if (position.Index != null)
        //                               wsh.Cells[ii, 6] = position.Quantity * (position.OZ * position.K_OZ * position.Index.K_OZ + position.EM * position.K_EM * position.Index.K_EM
        //                                             + position.ZM * position.K_ZM * position.Index.K_ZM);  //"ФОТ+СП+НР Руб. за ед. по смете"
        //                           else
        //                               wsh.Cells[ii, 5] = position.Quantity * (position.OZ * position.K_OZ + position.EM * position.K_EM
        //                                        + position.ZM * position.K_ZM);  //"ФОТ+СП+НР Руб. за ед. по смете"*/

        //                        wsh.Cells[ii++, 9] = res.WorkClass; //"ФОТ по смете";

        //                    }

        //                    foreach (string str in res.ResurсesTypeList)
        //                        wsh.Cells[ii++, 1] = "" + str;

        //                }
        //                if (iStarlLine < ii) wsh.Range[wsh.Rows[iStarlLine], wsh.Rows[ii - 1]].Group();
        //            }
        //        }

        //    }
        //    wsh.Cells[2, 6].FormulaLocal = "=СУММ(F3:F3540)";
        //}
        //public async void EstimateOutExell()
        //{
        //    Exel._Application exlApp = new Exel.Application();
        //    string sTamplatePath = Directory.GetCurrentDirectory() + "\\Шаблон сметы 1.xlsx";

        //    //    exlApp.Workbooks.Open(sTamplatePath);
        //    Exel.Workbook wkb = exlApp.Workbooks.Open(sTamplatePath);
        //    //    exlApp.Visible = true;
        //    exlApp.DisplayAlerts = false;
        //    wkb.SaveAs(Directory.GetCurrentDirectory() + "\\Смета out.xlsx");
        //    Exel.Worksheet wsh = wkb.Worksheets["Смета"];
        //    Exel.Worksheet wsh_tmpl = wkb.Worksheets["Шаблоны"];
        //    int ii = 19;
        //    int iStarlLine;
        //    int iChaptersCounter = 0;
        //    int iPositionCounter = 0;

        //    bool bNewChapter = false;
        //    double dZP_Summ = 0;//ФОТ по смете
        //    double dEM_Summ = 0;//Стоимость материриалов по смете
        //    double dZPM_Summ = 0;//Стоимость материриалов по смете
        //    double dMR_Summ = 0;//Стоимость материриалов по смете
        //    double dNR_Summ = 0; //Накладные расходы по смете
        //    double dSP_Summ = 0;//Сметная прибыль по смете
        //    double dBildWorks_Summ = 0;// Итого строительные работы
        //    double dMountigWorks_Summ = 0; //Монтажные работы
        //    double dOtherWorks_Summ = 0;//Прочие работы


        //    Exel.Range fromRange;
        //    Exel.Range toRange;


        //    await Task.Run(() =>
        //    {
        //        foreach (Chapter chapter in Chapters) //Фильтруем значения ресурсов
        //        {
        //            fromRange = wsh_tmpl.Range[wsh_tmpl.Rows[19], wsh_tmpl.Rows[19]];
        //            toRange = wsh.Range[wsh.Cells[ii, 1], wsh.Cells[ii, 1]];
        //            fromRange.Copy(toRange);
        //            wsh.Cells[ii, 1] = chapter.Caption;
        //            bNewChapter = true;
        //            if (ii == 19) bNewChapter = false; else bNewChapter = true;
        //            foreach (Position position in chapter.Positions)
        //            {
        //                fromRange = wsh_tmpl.Range[wsh_tmpl.Rows[20], wsh_tmpl.Rows[27]];
        //                toRange = wsh.Range[wsh.Rows[ii + 1], wsh.Rows[ii + 1 + 8]];
        //                fromRange.Copy(toRange);
        //                wsh.Cells[ii + 1, 1] = position.Namber; //омер строки в документе (положительное число).
        //                wsh.Cells[ii + 1, 2] = position.Code; //Код позиции (обычно в формате АВС, например Е44-М101) (текст).
        //                wsh.Cells[ii + 1, 3] = position.Caption;//Наименование позиции (текст).
        //                wsh.Cells[ii + 6, 3] = "НР от ФОТ " + position.GetCorrectionIndex(2, 0).value * 100;// Норматив накладных расходов 2 – норматив накладных расходов,0 – заработная плата.;//Наименование позиции (текст).
        //                wsh.Cells[ii + 7, 3] = "СП от ФОТ " + position.GetCorrectionIndex(3, 0).value * 100;// Норматив сметной прибыли.3 – норматив сметной прибыли,0 – заработная плата.
        //                wsh.Cells[ii + 1, 4] = position.Units;//Наименование позиции (текст).
        //                double w_scope = 0; double em = 0; double k_nr = 0; double nr = 0; double k_zpm = 0;
        //                double pz = 0; double zpm = 0; ; double k_sp = 0; double sp = 0; double k_mr = 0;
        //                double zp = 0; double mr = 0; double k_zp = 0; double k_em = 0;

        //                double ki_nr = 0; double ki_zpm = 0;
        //                double ki_sp = 0; double ki_mr = 0;
        //                double ki_zp = 0; double ki_em = 0;
        //                w_scope = position.IndexedParams[(int)IndexedParamsType.W_SCOPE];//Объем работ (число).
        //                pz = position.BaseParams[(int)BaseParamsType.PZ];// Прямые затраты (всего) (число, руб.) база.
        //                zp = position.BaseParams[(int)BaseParamsType.ZP];
        //                em = position.BaseParams[(int)BaseParamsType.EM];// Стоимость эксплуатации машин и механизмов (число, руб.)  база.
        //                zpm = position.BaseParams[(int)BaseParamsType.ZPM];// Заработная плата машинистов (число, руб.) (входит в общую стоимость эксплуатации машин и механизмов) база.
        //                mr = position.BaseParams[(int)BaseParamsType.MR];// Стоимость материалов (число, руб.) база.
        //                k_nr = position.GetCorrectionIndex(2, 0).value;// Норматив накладных расходов 2 – норматив накладных расходов,0 – заработная плата.
        //                k_sp = position.GetCorrectionIndex(3, 0).value;// Норматив сметной прибыли.3 – норматив сметной прибыли,0 – заработная плата.
        //                k_zp = position.GetCorrectionIndex(4, 0).value;// (4 – коэффициент учета условий работ,0 – заработная плата)*.
        //                k_em = position.GetCorrectionIndex(4, 1).value;// 4 – коэффициент учета условий работ,1 – эксплуатация машин и механизмов.
        //                k_zpm = position.GetCorrectionIndex(4, 3).value;// 4 – коэффициент учета условий работ,3 – заработная плата механизаторов.
        //                k_mr = position.GetCorrectionIndex(4, 2).value;// 4 – коэффициент учета условий работ,2 – стоимость материалов
        //                if (position.Namber == 89) ;
        //                wsh.Cells[ii + 1, 5] = w_scope;//Объем работ (число).
        //                wsh.Cells[ii + 1, 6] = pz;// Прямые затраты (всего) (число, руб.) база.
        //                wsh.Cells[ii + 2, 6] = zp;// Основная заработная плата (число, руб.)  база.
        //                wsh.Cells[ii + 3, 6] = em;// Стоимость эксплуатации машин и механизмов (число, руб.)  база.
        //                wsh.Cells[ii + 4, 6] = zpm;// Заработная плата машинистов (число, руб.) (входит в общую стоимость эксплуатации машин и механизмов) база.
        //                wsh.Cells[ii + 5, 6] = mr;// Стоимость материалов (число, руб.) база.
        //                wsh.Cells[ii + 6, 5] = k_nr * 100;// Норматив накладных расходов 2 – норматив накладных расходов,0 – заработная плата.
        //                wsh.Cells[ii + 7, 5] = k_sp * 100;// Норматив сметной прибыли.3 – норматив сметной прибыли,0 – заработная плата.
        //                wsh.Cells[ii + 2, 7] = k_zp;// (4 – коэффициент учета условий работ,0 – заработная плата)*.
        //                wsh.Cells[ii + 3, 7] = k_em;// 4 – коэффициент учета условий работ,1 – эксплуатация машин и механизмов.
        //                wsh.Cells[ii + 4, 7] = k_zpm;// 4 – коэффициент учета условий работ,3 – заработная плата механизаторов.
        //                wsh.Cells[ii + 5, 7] = k_mr;// 4 – коэффициент учета условий работ,2 – стоимость материалов.
        //                wsh.Cells[ii + 6, 7] = k_nr * 100;// Норматив накладных расходов 2 – норматив накладных расходов,0 – заработная плата.
        //                wsh.Cells[ii + 7, 7] = k_sp * 100;// Норматив сметной прибыли.3 – норматив сметной прибыли,0 – заработная плата.
        //                zp = zp * k_zp * w_scope;// ЗП в базисном уровне цен 
        //                em = em * k_em * w_scope;// ЭМ в базисном уровне цен
        //                zpm = zpm * k_zpm * w_scope;// ЗПМ в базисном уровне цен 
        //                mr = mr * k_mr * w_scope; // // МР в базисном уровне цен
        //                nr = (zp + zpm) * k_nr;// НР в базисном уровне цен 
        //                sp = (zp + zpm) * k_sp;// СП в базисном уровне цен 
        //                wsh.Cells[ii + 2, 8] = zp;// ЗП в базисном уровне цен 
        //                wsh.Cells[ii + 3, 8] = em;// ЭМ в базисном уровне цен 
        //                wsh.Cells[ii + 4, 8] = zpm; // ЗПМ в базисном уровне цен 
        //                wsh.Cells[ii + 5, 8] = mr;// МР в базисном уровне цен 
        //                wsh.Cells[ii + 6, 8] = nr;// НР в базисном уровне цен 
        //                wsh.Cells[ii + 7, 8] = sp;// СП в базисном уровне цен 
        //                wsh.Cells[ii + 8, 8] = zp + em + zpm + mr + nr + sp;

        //                ki_zp = position.GetCorrectionIndex(0, 0).value;// 0 – коэффициент учета инфляции (коэффициент пересчета сметных цен),0 – заработная плата.
        //                ki_em = position.GetCorrectionIndex(0, 1).value;// 0 – коэффициент учета инфляции (коэффициент пересчета сметных цен),1 – эксплуатация машин и механизмов.
        //                ki_zpm = position.GetCorrectionIndex(0, 3).value;// 0 – коэффициент учета инфляции (коэффициент пересчета сметных цен),3 – заработная плата механизаторов.
        //                ki_mr = position.GetCorrectionIndex(0, 2).value;// 0 – коэффициент учета инфляции (коэффициент пересчета сметных цен),2 – стоимость материалов.

        //                wsh.Cells[ii + 2, 9] = ki_zp;// 0 – коэффициент учета инфляции (коэффициент пересчета сметных цен),0 – заработная плата.
        //                wsh.Cells[ii + 3, 9] = ki_em;// 0 – коэффициент учета инфляции (коэффициент пересчета сметных цен),1 – эксплуатация машин и механизмов.
        //                wsh.Cells[ii + 4, 9] = ki_zpm;// 0 – коэффициент учета инфляции (коэффициент пересчета сметных цен),3 – заработная плата механизаторов.
        //                wsh.Cells[ii + 5, 9] = ki_mr;// 0 – коэффициент учета инфляции (коэффициент пересчета сметных цен),2 – стоимость материалов.
        //                wsh.Cells[ii + 6, 9] = k_nr * 100;// Норматив накладных расходов 2 – норматив накладных расходов,0 – заработная плата.
        //                wsh.Cells[ii + 7, 9] = k_sp * 100;// Норматив сметной прибыли.3 – норматив сметной прибыли,0 – заработная плата.

        //                zp = zp * ki_zp;
        //                em = em * ki_em;
        //                zpm = zpm * ki_zpm;
        //                mr = mr * ki_mr;
        //                nr = nr * ki_zp;
        //                sp = sp * ki_zp;


        //                wsh.Cells[ii + 2, 10] = zp;
        //                wsh.Cells[ii + 3, 10] = em;
        //                wsh.Cells[ii + 4, 10] = zpm;
        //                wsh.Cells[ii + 5, 10] = mr;
        //                wsh.Cells[ii + 6, 10] = nr;
        //                wsh.Cells[ii + 7, 10] = sp;
        //                wsh.Cells[ii + 8, 10] = zp + em + mr + nr + sp;
        //                dZP_Summ += zp;
        //                dEM_Summ += em;
        //                dZPM_Summ += zpm;
        //                dMR_Summ += mr;
        //                dSP_Summ += sp;
        //                dNR_Summ += nr;
        //                ii += 8;
        //                chapter.CommonPrice += zp + em + zpm + nr + sp;
        //                if (position.Code.Contains("ТЕРм"))
        //                    dMountigWorks_Summ += zp + em + zpm + nr + sp;
        //                else
        //                    if (position.Code.Contains("ТЕРр"))
        //                    dOtherWorks_Summ += zp + em + zpm + nr + sp;
        //                else
        //                      if (position.Code.Contains("ТЕР"))
        //                    dBildWorks_Summ += zp + em + zpm + nr + sp;

        //                DataOutIndicate_2 = ((double)(iPositionCounter + 1) / (chapter.Positions.Count)) * 100;
        //                iPositionCounter++;
        //                //  Exel.Range fromRange = wsh.Range[wsh.Cells[ii, 1], wsh.Cells[ii + 6, 10]];a
        //                //  Exel.Range toRange = wsh.Range[wsh.Cells[ii, 1], wsh.Cells[ii + 6, 10]];

        //                /*      if (position.KUnit == 0)
        //                          position.KUnit = 1;
        //                      wsh.Cells[ii, 1] = position.Caption;
        //                      string sUnints;
        //                      sUnints = position.Units;
        //                      sUnints = sUnints.Replace("100", "");
        //                      sUnints = sUnints.Replace("10", "");
        //                      sUnints = sUnints.Replace("1", "");
        //                      // wsh.Cells[ii, 2] = sUnints;
        //                      //wsh.Cells[ii, 3] = position.Quantity * position.KUnit;
        //                      wsh.Cells[ii, 2] = position.Units;
        //                      wsh.Cells[ii, 3] = position.Quantity;
        //                      wsh.Cells[ii, 1].Font.Bold = true;
        //                      wsh.Cells[ii, 2].Font.Bold = true;
        //                      wsh.Cells[ii, 3].Font.Bold = true;
        //                      wsh.Cells[ii, 4].Font.Bold = true;
        //                      wsh.Cells[ii, 5].Font.Bold = true;
        //                      wsh.Cells[ii, 6].Font.Bold = true;
        //                      wsh.Cells[ii, 7].Font.Bold = true;

        //                      iStarlLine = ii + 1;

        //                      if (iStarlLine < ii) wsh.Range[wsh.Rows[iStarlLine], wsh.Rows[ii - 1]].Group();
        //                      */

        //            }
        //            ii++;
        //            DataOutIndicate_1 = ((double)(iChaptersCounter + 1) / (Chapters.Count)) * 100;
        //            iChaptersCounter++;
        //            iPositionCounter = 0;
        //        }
        //    });
        //    fromRange = wsh_tmpl.Range[wsh_tmpl.Rows[28], wsh_tmpl.Rows[62]];
        //    toRange = wsh.Range[wsh.Rows[ii + 1], wsh.Rows[ii + 1]];
        //    fromRange.Copy(toRange);
        //    //  exlApp.Visible = true;
        //    wsh.Cells[ii + 1, 10] = dZP_Summ + dMR_Summ + dEM_Summ;//ИТОГО прямые затраты в текущих ценах
        //    wsh.Cells[ii + 2, 10] = dNR_Summ;//ИТОГО НР в текущих ценах
        //    wsh.Cells[ii + 3, 10] = dSP_Summ;//ИТОГО СП в текущих ценах
        //    /* wsh.Cells[ii + 5, 10] = dBildWorks_Summ ;//ИТОГО строительные работы
        //     wsh.Cells[ii + 6, 10] =dMountigWorks_Summ ;//ИТОГО монтажные работы
        //     wsh.Cells[ii + 7, 10] = dOtherWorks_Summ;//ИТОГО Прочие 
        //  */
        //    double dSumCost = dMR_Summ + dEM_Summ + dZP_Summ + dNR_Summ + dSP_Summ;//Итого
        //    wsh.Cells[ii + 8, 10] = dSumCost;//Итого
        //    // В том числе
        //    wsh.Cells[ii + 10, 10] = dMR_Summ;//  МР в текущих ценах
        //    wsh.Cells[ii + 11, 10] = dEM_Summ;//  МР в текущих ценах
        //    wsh.Cells[ii + 12, 10] = dZP_Summ + dZPM_Summ;// ФОТ в текущих ценах
        //    wsh.Cells[ii + 13, 10] = dNR_Summ;//  НР в текущих ценах
        //    wsh.Cells[ii + 14, 10] = dSP_Summ;//  СП в текущих ценах
        //    double UnseenCost = (UnseenCostCoefficient - 1) * dSumCost;//  Непридвиденные
        //    wsh.Cells[ii + 15, 10] = UnseenCost;//  Непридвиденные
        //    dSumCost = UnseenCost + dSumCost;//  Итого с непридвиденными..
        //    wsh.Cells[ii + 16, 10] = dSumCost;//  Итого с непридвиденными..

        //    wsh.Cells[ii + 17, 10] = dSumCost * (NDS - 1);//  НДС....
        //    dSumCost *= NDS; ///Всего по смете
        //    wsh.Cells[ii + 18, 10] = dSumCost;///Всего по смете

        //    // wsh.Cells[2, 6].FormulaLocal = "=СУММ(F3:F3540)";
        //    wkb.Save();
        //    exlApp.Quit();
        //}
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
