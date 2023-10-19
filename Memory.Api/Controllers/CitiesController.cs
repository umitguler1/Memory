using Memory.Business.Abstract;
using Memory.Entities.Concrete.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Memory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        #region asd//List<string> isimler = new List<string>() {"Tunahan","Ümit","Ayşe","Hüseyin","Hakan" };
        //[HttpGet("{id}")]
        //public IActionResult Gets(int id)
        //{
        //    return Ok(isimler[id]);
        //}

        //[HttpGet]
        //public IActionResult Gets()
        //{
        //    return BadRequest(isimler);
        //}

        //[HttpDelete]
        //public IActionResult Remove(int index)
        //{
        //    bool response = isimler.Remove(isimler[index]);
        //    return response ? Ok("Başarılı oldu") : NotFound(index);
        //}

        //[HttpPut]
        //public IActionResult Update(string name, int index)
        //{
        //    isimler[index] = name;
        //    return Ok(isimler[index]);
        //}

        //[HttpPost]
        //public IActionResult Add(string name)
        //{
        //    isimler.Add(name);
        //    return Ok(isimler);
        //}
        #endregion
        private readonly ICityService _cityService;
        public CitiesController(ICityService cityService)
        {
            _cityService = cityService;
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> Gets(int id)
        //{
        //    CityDto cityDto = await _cityService.GetCityByIdAsync(id);
        //    return cityDto != null ? Ok(cityDto) : BadRequest();
        //}
       
        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Gets()
        {
            List<CityDto> cityDtos = await _cityService.GetAllCityAsync();
            return cityDtos.Count > 0 ? Ok(cityDtos) : NoContent();
        }
        [HttpPost]
        public async Task<IActionResult> Add(CityDto cityDto)
        {
            var response = await _cityService.AddCityAsync(cityDto);
            return response ? Ok(response) : BadRequest();
        }
        [HttpPut]
        public async Task<IActionResult> Update(CityDto cityDto)
        {
            bool response = await _cityService.UpdateCityAsync(cityDto);
            if (response)
            {
                return Ok(response);
            }
            return BadRequest();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(CityDto cityDto)
        {
            bool response = await _cityService.DeleteCityAsync(cityDto);
            return response ? Ok("Silme işlemi başarılı!") : BadRequest(cityDto);
        }



    }
}
