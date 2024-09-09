using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TPShoes.Entidades.Clases;
using TPShoes.Entidades.ViewModels.Size;
using TPShoes.Servicios.Interfaces;
using X.PagedList.Extensions;

namespace TPShoes.Web.Controllers
{
    public class SizeController : Controller
    {
        private readonly ISizesServicio? _serviciosSize;
        private readonly IMapper? _mapper;
        public SizeController(ISizesServicio? servicios, IMapper mapper)
        {
            _serviciosSize = servicios ?? throw new ApplicationException("Dependencies not set");
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
            var SizeVm = _mapper?.Map<List<SizeListVm>>(Sizes)
               .ToPagedList(pageNumber, pageSize);

            return View(SizeVm);
        }
    }

}

