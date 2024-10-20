using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TPShoes.Entidades.Clases;
using TPShoes.Entidades.ViewModels.Brand;
using TPShoes.Entidades.ViewModels.Shoe;
using TPShoes.Entidades.ViewModels.Size;
using TPShoes.Servicios.Interfaces;
using X.PagedList;

namespace TPShoes.Web.Areas.Admin.Controllers
{
    public class SizeController : Controller
    {
        private readonly ISizesServicio? _serviciosSize;
        private readonly ISizeShoesServicio? _serviciosSizeShoe;
        private readonly IShoesServicio? _serviciosShoe;
        private readonly IMapper? _mapper;
        public SizeController(ISizesServicio? serviciosSize, ISizeShoesServicio? serviciosSizeShoe, IShoesServicio? serviciosShoe, IMapper mapper)
        {
            _serviciosSize = serviciosSize ?? throw new ApplicationException("Dependencies not set");
            _serviciosSizeShoe = serviciosSizeShoe ?? throw new ApplicationException("Dependencies not set");
            _serviciosShoe = serviciosShoe ?? throw new ApplicationException("Dependencies not set");
            _mapper = mapper ?? throw new ApplicationException("Dependencies not set");
        }

        public IActionResult Index(int? page, string? searchTerm = null, bool viewAll = false, int pageSize = 10)
        {
            int pageNumber = page ?? 1;
            ViewBag.currentPageSize = pageSize;
            IEnumerable<Size>? Sizes;
            if (!viewAll)
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    Sizes = _serviciosSize?
                        .GetLista(orderBy: o => o.OrderBy(c => c.SizeNumber),
                            filter: c => c.SizeNumber.ToString().Contains(searchTerm));
                    ViewBag.currentSearchTerm = searchTerm;
                }
                else
                {
                    Sizes = _serviciosSize?
                        .GetLista(orderBy: o => o.OrderBy(c => c.SizeNumber));
                }
            }
            else
            {
                Sizes = _serviciosSize?
                    .GetLista(orderBy: o => o.OrderBy(c => c.SizeNumber));
            }
            var SizeListVm = _mapper?.Map<List<SizeListVm>>(Sizes)
               .ToPagedList(pageNumber, pageSize);
            foreach (var item in SizeListVm)
            {
                item.CantShoes = _serviciosSizeShoe.GetListaShoeDtoPorSize(item.SizeId).Count;
            }
            return View(SizeListVm);
        }

        public IActionResult Details(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            try
            {
                if (_serviciosSize == null || _mapper == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
                }
                Size? size = _serviciosSize?.GetSizePorId(id.Value);

                if (size is null)
                {
                    return NotFound();
                }
                var shoeList = _serviciosSizeShoe.GetListaShoePorSize(id.Value);

                var shoeListVm = _mapper?.Map<IEnumerable<ShoeListVm>>(shoeList).ToList();


                return View(shoeListVm);
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Couldn't delete record!!! " }); ;

            }

        }


    }

}

