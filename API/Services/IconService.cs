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

        //get svg file as text
        public string readSvgFile()
        {
            string text = File.ReadAllText(pathToSvg);
            return text;
        } 

        //get list of icons
        public List<Icon> readSvgFiles(){
            return _iconRepository.GetAllIcons();

        }

        //get icon by id
        public Icon getIcon(int id)
        {
            return _iconRepository.GetIcon(id);
        }

        //create icon
        public void createIcon(Icon icon)
        {
            _iconRepository.CreateIcon(icon);
        }

        //update icon
        public void updateIcon(Icon icon)
        {
            _iconRepository.UpdateIcon(icon);
        }

        //delete icon
        public void deleteIcon(string id)
        {
            _iconRepository.DeleteIcon(id);
        }


    }
}
