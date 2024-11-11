using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TPShoes.Entidades.Clases;
using TPShoes.Entidades.ViewModels.Shoe;
using TPShoes.Entidades.ViewModels.Size;
using TPShoes.Servicios.Interfaces;
using TPShoes.Web.Models;
using X.PagedList;

namespace TPShoes.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IShoesServicio? _serviciosShoe;
        private readonly ISizeShoesServicio? _serviciosSizeShoe;
        private readonly IMapper? _mapper;
        private readonly int pageSize = 8;

        public HomeController(IShoesServicio serviciosShoe, ISizeShoesServicio? serviciosSizeShoe, IMapper? mapper)
        {
            _serviciosShoe = serviciosShoe ?? throw new ApplicationException("Dependencies not set");
            _serviciosSizeShoe = serviciosSizeShoe ?? throw new ApplicationException("Dependencies not set");
            _mapper = mapper ?? throw new ApplicationException("Dependencies not set");
        }
        public IActionResult Hero()
        {
            return View();
        }
        public IActionResult Index(int? page)
        {
            var currentPage = page ?? 1;
            var shoeList = _serviciosShoe!.GetLista(
                orderBy: o => o.OrderBy(p => p.Model),
                propertiesNames: "Sport,Brand,Colour,Genre");
            var shoesHomeIndexVm = _mapper!.Map<List<ShoeHomeIndexVm>>(shoeList);
            return View(shoesHomeIndexVm.ToPagedList(currentPage, pageSize));
        }

        public IActionResult Details(int? id)
        {
            if (id == null || id.Value == 0) { return NotFound(); }
            int idShoe = id.Value;
            List<Size> TallesList = _serviciosSizeShoe!.GetSizesPorId(idShoe, true);
            if (TallesList is null || !TallesList.Any())
            {

                return NotFound();

            }
            var sizeListVm = _mapper!.Map<List<SizeListVm>>(TallesList);
            return View(sizeListVm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
