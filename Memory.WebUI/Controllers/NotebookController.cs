using Memory.Business.Abstract;
using Memory.Entities.Concrete.Dtos;
using Memory.WebUI.BasketTransaction;
using Memory.WebUI.BasketTransaction.BasketModels;
using Microsoft.AspNetCore.Mvc;

namespace Memory.WebUI.Controllers
{
    public class NotebookController : Controller
    {
        private readonly INotebookService _notebookService;
        private readonly ICityService _cityService;
        private readonly IBasketTransaction _basketTransaction;

        public NotebookController(INotebookService notebookService, ICityService cityService, IBasketTransaction basketTransaction)
        {
            _notebookService = notebookService;
            _cityService = cityService;
            _basketTransaction = basketTransaction;
        }
        public async Task<IActionResult> Index()
        {
            List<NotebookDto> notebooks = await _notebookService.GetNotebookListAsync();
            return View(notebooks);
        }
        public async Task<IActionResult> Detail(int id)
        {
            NotebookDto notebookDto = await _notebookService.GetNotebookAsync(id);
            ViewBag.Sehir = (await _cityService.GetCityByIdAsync(notebookDto.CityId)).Name;
            return View(notebookDto);
        }
        public async Task<IActionResult> AddBasketItem(int id)
        {
            NotebookDto notebookDto = await _notebookService.GetNotebookAsync(id);
            if (notebookDto != null)
            {
                BasketItemDto basketItemDto = new BasketItemDto()
                {
                    Content = notebookDto.Content,
                    NotebookId = notebookDto.Id,
                    Price = notebookDto.Price,
                    Title = notebookDto.Title,
                    Quantity = 1
                };
                _basketTransaction.SaveUpdateBasketItem(basketItemDto);
            }
            return RedirectToAction("Index");
        }


    }
}
