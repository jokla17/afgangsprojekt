using System.Xml.Serialization;

namespace API
{
    public class Compumat {
        [XmlElement(ElementName="Id")]
        public int? Id { get; set; }

        [XmlElement(ElementName ="StationNo")]
        public string? StationNo { get; set; }

        [XmlElement(ElementName = "Name")]
        public string? Name { get; set; }

        [XmlElement(ElementName = "Latitude")]
        public double Latitude { get; set; }

        [XmlElement(ElementName = "Longitude")]
        public double Longitude { get; set; }

        [XmlElement(ElementName = "Type")]
        public CompumatType Type { get; set; }

        [XmlElement(ElementName = "Status")]
        public string? Status { get; set; }

        public enum CompumatType {
            VENDINGMACHINE = 1,
            GATE = 2
        };
    }

    [XmlRoot("Compumats")]
    public class Compumats {

        public Compumats() {
            compumats = new List<Compumat>();
        }

        [XmlElement("Compumat")]
        public List<Compumat>? compumats { get; set; }
    }
}
