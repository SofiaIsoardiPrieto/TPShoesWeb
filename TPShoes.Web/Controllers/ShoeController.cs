using Microsoft.AspNetCore.Mvc;
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
            var shoeLista = _servicio.GetLista();
            return View(shoeLista);
        }
    }
}
