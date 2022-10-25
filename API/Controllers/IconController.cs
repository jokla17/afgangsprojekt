using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using API.Services;

namespace API.Controllers
{
    [EnableCors("myPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class IconController: ControllerBase
    {
        private readonly ILogger<IconController> _logger;
        private readonly IconService _iconService;

        public IconController(ILogger<IconController> logger, IconService iconService)
        {
            _logger = logger;
            _iconService = iconService;
        }


        [HttpGet("SvgFile")]
        public string Get()
        {
            return _iconService.readSvgFile();
        }

        [HttpGet("SvgFiles")]
        public List<Icon> GetFiles()
        {
            return _iconService.readSvgFiles();
        }

        
    }
}
