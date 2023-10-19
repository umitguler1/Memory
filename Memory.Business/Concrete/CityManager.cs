using AutoMapper;
using FluentValidation.Results;
using Memory.Business.Abstract;
using Memory.Business.ValidationRules.FluentValidation;
using Memory.DataAccess.Abstract;
using Memory.Entities.Concrete;
using Memory.Entities.Concrete.Dtos;

namespace Memory.Business.Concrete
{
    public class CityManager : ICityService
    {
        private readonly ICityDal _cityDal;
        private readonly IMapper _mapper;
        CityValidator rules = new CityValidator();
        public CityManager(ICityDal cityDal, IMapper mapper)
        {
            _cityDal = cityDal;
            _mapper = mapper;
        }
        private ValidationResult Validate(City city)
        {
            return rules.Validate(city);
        }
        public async Task<bool> AddCityAsync(CityDto cityDto)
        {
            City city = CityDtoConvert(cityDto);
            ValidationResult result = Validate(city);
            if (result.IsValid)
            {
                int response = await _cityDal.AddAsync(city);
                return response > 0;
            }
            return false;
        }
        public async Task<bool> DeleteCityAsync(CityDto cityDto)
        {
            City city = CityDtoConvert(cityDto);
            int response = await _cityDal.DeleteAsync(city);
            return response > 0;
        }
        public City CityDtoConvert(CityDto cityDto)
        {
            return _mapper.Map<City>(cityDto);
        }
        public async Task<List<CityDto>> GetAllCityAsync()
        {
            List<City> cities = await _cityDal.GetAllAsync();
            List<CityDto> cityDtos = new List<CityDto>();
            foreach (City city in cities)
            {
                CityDto cityDto = _mapper.Map<CityDto>(city);
                cityDtos.Add(cityDto);
            }
            return cityDtos;
        }
        public async Task<List<CityDto>> GetAllCityByRuleAsync(string cityName)
        {
            List<City> cities = await _cityDal.GetAllAsync(x=>x.Name.Contains(cityName));
            List<CityDto> cityDtos = new List<CityDto>();
            foreach (var item in cities)
            {
                CityDto cityDto = _mapper.Map<CityDto>(item);
                cityDtos.Add(cityDto);
            }
            return cityDtos;
        }
        public async Task<CityDto> GetCityByIdAsync(int id)
        {
            City city = (await _cityDal.GetAsync(x=>x.Id == id));
            return _mapper.Map<CityDto>(city);
        }
        public async Task<bool> UpdateCityAsync(CityDto cityDto)
        {
            City city = CityDtoConvert(cityDto);
            ValidationResult result = Validate(city);
            if (result.IsValid)
            {
                int response = await _cityDal.UpdateAsync(city);
                return response == 1;
            }
            return false;
        }
    }
}
