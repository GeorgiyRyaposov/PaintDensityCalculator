using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using PaintDensityCalculator.Utils;

namespace PaintDensityCalculator.Models
{
    [Serializable, XmlRoot("Densities")]
    public class Densities
    {
        [XmlArrayItem("Density", typeof(Density))]
        public List<Density> DensitiesList { get; set; }
    }


    [Serializable, XmlRoot("Density")]
    public class Density : NotifyPropertyChangedImplementation
    {
        [XmlElement(ElementName = "id", DataType = "int")]
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        [XmlElement(ElementName = "DensType", DataType = "int")]
        public int DensType
        {
            get { return _densType; }
            set
            {
                _densType = value;
                OnPropertyChanged("DensType");
            }
        }

        [XmlElement(ElementName = "Name", DataType = "string")]
        public string Name {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        [XmlElement(ElementName = "DensityValue", DataType = "float")]
        public float DensityValue {
            get { return _densityValue; }
            set
            {
                _densityValue = value;
                OnPropertyChanged("DensityValue");
            }
        }

        [XmlElement(ElementName = "AccordingThinner", DataType = "int")]
        public int AccordingThinner {
            get { return _accordingThinner; }
            set
            {
                _accordingThinner = value;
                OnPropertyChanged("AccordingThinner");
            }
        }

        [XmlElement(ElementName = "AccordingHardener", DataType = "int")]
        public int AccordingHardener {
            get { return _accordingHardener; }
            set
            {
                _accordingHardener = value;
                OnPropertyChanged("AccordingHardener");
            }
        }

        [XmlElement(ElementName = "ProportionThinner", DataType = "float")]
        public float ProportionThinner {
            get { return _proportionThinner; }
            set
            {
                _proportionThinner = value;
                OnPropertyChanged("ProportionThinner");
            }
        }

        [XmlElement(ElementName = "ProportionHardener", DataType = "float")]
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
