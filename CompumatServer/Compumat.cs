
namespace CompumatServer { 
    public class Compumat {

        public int? Id { get; set; }

        public string? Name { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public CompumatType Type { get; set; }

        public string? Status { get; set; }

        public enum CompumatType {
            VENDINGMACHINE = 1,
            GATE = 2
        };
    }
}

