using API.Repositories;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace API.Services
{
    public class IconService
    {

        private const string pathToSvg = "C:\\Users\\themi\\OneDrive\\Skrivebord\\Github Afgangsprojekt\\afgangsprojekt\\API\\Icons\\";
        private readonly IconRepository _iconRepository;
        public IconService(IconRepository iconRepository)
        {
            _iconRepository = iconRepository;
        }

        public string readSvgFile()
        {
            string text = File.ReadAllText(pathToSvg);
            return text;
        } 

        public List<Icon> readSvgFiles(){
            return _iconRepository.GetAll();

        }

        
    }
}
