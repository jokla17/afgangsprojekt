using API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    
    public class CompumatService
    {

        private const string pathToSvg = "C:\\Users\\themi\\OneDrive\\Skrivebord\\Github Afgangsprojekt\\afgangsprojekt\\API\\Icons\\";
        private readonly CompumatRepository _compumatRepository;

        public CompumatService(CompumatRepository compumatRepository)
        {
            _compumatRepository = compumatRepository;
        }
        
        //read one from table
        public async Task<Compumat> GetCompumat(int id)
        {
            return await _compumatRepository.GetCompumat(id);
        }

        //read all from table
        public async Task<List<Compumat>> GetAllCompumats()
        {
            return await _compumatRepository.GetAllCompumats();
        }


        //create compumat
        public async Task<Compumat> CreateCompumat(Compumat compumat)
        {
            return await _compumatRepository.CreateCompumat(compumat);
        }

        //update compumat
        public async Task<Compumat> UpdateCompumat(Compumat compumat)
        {
            var result = await _compumatRepository.UpdateCompumat(compumat);
            if (result == null)
            {
                return null;
            }
            return result;
        }

        //delete compumat
        public async Task<string> DeleteCompumat(string id)
        {
            return await _compumatRepository.DeleteCompumat(id);
        }
    }
}
