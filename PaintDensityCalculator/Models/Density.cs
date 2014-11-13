using System.Xml.Serialization;
using PaintDensityCalculator.Utils;

namespace PaintDensityCalculator.Models
{
    [XmlRoot("XmlDensityRoot")]
    public class Density : NotifyPropertyChangedImplementation
    {
        [XmlAttribute("id")]
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        [XmlAttribute(DataType = "int")]
        public int DensType
        {
            get { return _densType; }
            set
            {
                _densType = value;
                OnPropertyChanged("DensType");
            }
        }

        [XmlAttribute(DataType = "string")]
        public string Name {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        [XmlAttribute(DataType = "double")]
        public float DensityValue {
            get { return _densityValue; }
            set
            {
                _densityValue = value;
                OnPropertyChanged("DensityValue");
            }
        }
        
        [XmlAttribute(DataType = "int")]
        public int AccordingThinner {
            get { return _accordingThinner; }
            set
            {
                _accordingThinner = value;
                OnPropertyChanged("AccordingThinner");
            }
        }

        [XmlAttribute(DataType = "int")]
        public int AccordingHardener {
            get { return _accordingHardener; }
            set
            {
                _accordingHardener = value;
                OnPropertyChanged("AccordingHardener");
            }
        }

        [XmlAttribute(DataType = "double")]
        public float ProportionThinner {
            get { return _proportionThinner; }
            set
            {
                _proportionThinner = value;
                OnPropertyChanged("ProportionThinner");
            }
        }

        [XmlAttribute(DataType = "double")]
        public float ProportionHardener {
            get { return _proportionHardener; }
            set
            {
                _proportionHardener = value;
                OnPropertyChanged("ProportionHardener");
            }
        }

        private int _id;
        private int _densType;
        private string _name;
        private float _densityValue;
        private int _accordingThinner;
        private int _accordingHardener;
        private float _proportionThinner;
        private float _proportionHardener;
    }
}
