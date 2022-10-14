using PrismWorkApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PrismWorkApp.ProjectModel.Data.Models
{
    public class Participant :oldNode, INotifyPropertyChanged
    {
        /*public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }*/
        public int Id { get; set; }
        private string _code;
        public string Code { get { return _code; } set { _code = value; OnPropertyChanged("Code"); } }
        private ConstructionCompany _company;
        public ConstructionCompany Company { get { return _company; } set { _company = value; OnPropertyChanged("Company"); } }
        private ParticipantRole _role;
        public ParticipantRole Role { get { return _role; } set { _role = value; NodeName = Name; OnPropertyChanged("Role"); } }
        private ObservableCollection<ResponsibleEmployee> _responsibleEmployees;
        public ObservableCollection<ResponsibleEmployee> ResponsibleEmployees
        { get { return _responsibleEmployees; } set { _responsibleEmployees = value; OnPropertyChanged("_responsibleEmployees"); } }
        public Participant()
        {
            ResponsibleEmployees = new ObservableCollection<ResponsibleEmployee>();
        }
        private string _fullName;
        public string FullName {
            get { switch (Role)
                {
                    case ParticipantRole.DEVELOPER:
                        _fullName = "Застройщик(технический заказчик, эксплуатирующая организация или региональный оператор)";
                        break;
                    case ParticipantRole.GENERAL_CONTRACTOR:
                        _fullName = "Генеральный подрядчик(лицо, осуществляющее строительство)";
                        break;
                    case ParticipantRole.DISIGNER:
                        _fullName = "Проектировщик (Лицо, осуществляющее подготовку проектной документации";
                            break;
                    case ParticipantRole.BUILDER:
                        _fullName = "Подрядчик(лицо, выполнившеее работы)";
                        break;
                    default:
                        _fullName = "Не определено";
                        break;
                }

                return _fullName; }
            set { _fullName = value;
                OnPropertyChanged("FullName");
            } }///Временно!!

        private string _name ;
        public string  Name
        {
            get
            {
                switch (Role)
                {
                    case ParticipantRole.DEVELOPER:
                        _name = "Заказчик(Застройщик)";
                        NodeName = _name;
                        break;
                    case ParticipantRole.GENERAL_CONTRACTOR:
                        _name = "Генподрядчик";
                        NodeName = _name;
                        break;
                    case ParticipantRole.DISIGNER:
                        _name = "Проектировщик";
                        NodeName = _name;
                        break;
                    case ParticipantRole.BUILDER:
                        _name = "Подрядчик";
                        NodeName = _name;
                        break;
                    default:
                        _name = "Не определено";
                        NodeName = _name;
                        break;
                }
                NodeName = _name;
                OnPropertyChanged("NodeName");
                return _name;
            }
            set
            {
                /*  _name = value;
                  NodeName = _name;
                  OnPropertyChanged("Name");
                  OnPropertyChanged("NodeName");*/
                NodeName = _name;
            }
        }///Временно!!

    }
    public enum ParticipantRole
    {
        DEVELOPER = 0,
        GENERAL_CONTRACTOR,
        DISIGNER,
        BUILDER,
        NONE
    }
}
