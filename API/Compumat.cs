namespace API
{
    public class Compumat
    {

        public int? Id { get; set; }

        public string? Name { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public CompumatType Type { get; set; }

        public enum CompumatType {
            VENDINGMACHINE = 1,
            GATE = 2
        };

    }
}
