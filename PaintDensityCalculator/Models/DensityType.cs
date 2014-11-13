using System.Xml.Serialization;
using PaintDensityCalculator.Utils;

namespace PaintDensityCalculator.Models
{
    [XmlRoot("XmlDensityTypeRoot")]
    public class DensityType : NotifyPropertyChangedImplementation
    {
        [XmlAttribute("id")]
        public int Id { get { return _id; } set { _id = value; OnPropertyChanged("Id"); } }

        [XmlAttribute(DataType = "string")]
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged("Name"); } }

        private int _id;
        private string _name;
    }
}
