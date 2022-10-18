using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace API.Controllers
{
    [EnableCors("myPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class CompumatController: ControllerBase
    {
        private readonly ILogger<MapController> _logger;

        private Compumat[] compumats =  new Compumat[]
        { 
            new Compumat{
            Id = "01",
            Name = "Automat ved vandland",
            Latitude = 55.11404492554651,
            Longitude = 9.787099052273176,
            Type = Compumat.CompumatType.VENDINGMACHINE
            },
            new Compumat{
            Id = "02",
            Name = "Gate ved hovedbygning",
            Latitude = 55.11404492554651,
            Longitude = 9.787099052273176,
            Type = Compumat.CompumatType.GATE
            },
            new Compumat{
            Id = "03",
            Name = "Gate ved vandland",
            Latitude = 55.11404492554651,
            Longitude = 9.787099052273176,
            Type = Compumat.CompumatType.GATE
            },
            new Compumat{
            Id = "04",
            Name = "Automat ved hovedbygning",
            Latitude = 55.11404492554651,
            Longitude = 9.787099052273176,
            Type = Compumat.CompumatType.VENDINGMACHINE
            }
        };

        public CompumatController(ILogger<MapController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}/Compumat")]
        public Compumat Get(string id)
        {
            return this.compumats.First<Compumat>(obj => (String.Compare(obj.Id, id) == 0));
        }

        [HttpGet("/Compumats")]
        public Compumat[] GetAll()
        {
            return compumats;
        }
    }
}
