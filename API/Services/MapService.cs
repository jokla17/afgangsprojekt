using API.Repositories;

namespace API.Services
{
    public class MapService
    {

        private readonly MapRepository _mapRepository;

        public MapService(MapRepository mapRepository)
        {
            _mapRepository = mapRepository;
        }

        //read one from table
        public async Task<Map> GetMap(int id)
        {
            return await _mapRepository.GetMap(id);
        }

        //read all from table
        public async Task<List<Map>> GetAllMaps()
        {
            return await _mapRepository.GetAllMaps();
        }

        //create map
        public async Task<Map> CreateMap(Map map)
        {
            return await _mapRepository.CreateMap(map);
        }

        //update map
        public async Task<Map> UpdateMap(Map map)
        {
            var result = await _mapRepository.UpdateMap(map);
            if (result == null)
            {
                return null;
            }
            return result;
        }

        //delete map
        public async Task<string> DeleteMap(string id)
        {
            return await _mapRepository.DeleteMap(id);
        }

    }
}
