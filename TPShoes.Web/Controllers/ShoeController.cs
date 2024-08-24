using Microsoft.AspNetCore.Mvc;
using TPShoes.Entidades.Dtos;
using TPShoes.Entidades.ViewModels;
using TPShoes.Servicios.Interfaces;

namespace TPShoes.Web.Controllers
{
    public class ShoeController : Controller
    {
        private readonly IShoesServicio _servicio;

        public ShoeController(IShoesServicio servicio)
        {
            _servicio = servicio;
        }

        public IActionResult Index()
        {
            List<ShoeDto> shoeLista = _servicio.GetListaDto();
            return View(shoeLista);
        }

        public IActionResult Create(ShoeEditVm shoeEditVm)
        {
            if (!ModelState.IsValid)
            {
                return View(shoeEditVm);
            }
            return RedirectToAction("Index");
        }

    }
}
