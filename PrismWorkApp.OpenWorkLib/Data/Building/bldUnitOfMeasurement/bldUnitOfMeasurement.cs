using System.ComponentModel.DataAnnotations.Schema;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldUnitOfMeasurement : BindableBase, IbldUnitOfMeasurement, IEntityObject
    {

        private string _shortName;
        public string ShortName
        {
            get { return _shortName; }
            set { SetProperty(ref _shortName, value); }
        }
        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set { SetProperty(ref _fullName, value); }
        }

        public bldUnitOfMeasurement(string name)
        {
            Name = name;
        }
        public bldUnitOfMeasurement()
        {

        }
        private double _conversionCoefficient;
        
        [NotMapped]
        public double ConversionCoefficient
        {
            get { return _conversionCoefficient; }
            set { _conversionCoefficient = value; }
        }



    }

    public enum MeasureType
    {
        MASS,
        TIME,
        LENGHT
    }
}
