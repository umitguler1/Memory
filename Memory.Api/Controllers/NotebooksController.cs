using Memory.Business.Abstract;
using Memory.Entities.Concrete.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Memory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotebooksController : ControllerBase
    {
        private readonly INotebookService _notebookService;
        public NotebooksController(INotebookService notebookService)
        {
            _notebookService = notebookService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
           NotebookDto notebookDto = await  _notebookService.GetNotebookAsync(id);
           return notebookDto != null ? Ok(notebookDto) : NotFound("Nesne yok!");
        }

        [HttpGet]
        [Route("Gets/Notlar")]
        public async Task<IActionResult> Gets()
        {
            List<NotebookDto> notebookDtos = await _notebookService.GetNotebookListAsync();
            return notebookDtos.Count > 0 ? Ok(notebookDtos) : NotFound("Nesne mevcut değil!");
        }

        [HttpPost]
        public async Task<IActionResult> Add(NotebookDto notebookDto)
        {
            int response = await _notebookService.AddNotebookAsync(notebookDto);
            return response > 0 ? Ok(notebookDto) : NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> Update(NotebookDto notebookDto)
        {
            int response = await _notebookService.UpdateNotebookAsync(notebookDto);
            if (response > 0)
            {
                return Ok(notebookDto);
            }
            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Abdurrezzak(NotebookDto notebookDto)
        {
           int response = await _notebookService.RemoveNotebookAsync(notebookDto);
            return response > 0 ? Ok("Silme işlemi başarılı.") : NotFound("Silme işlemi başarısız.");
        }

    }
}
