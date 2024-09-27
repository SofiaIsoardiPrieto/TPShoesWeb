using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TPShoes.Entidades.Clases;
using TPShoes.Entidades.ViewModels.Sport;
using TPShoes.Servicios.Interfaces;
using X.PagedList;

namespace TPShoes.Web.Controllers
{
    public class SportController : Controller
    {
        private readonly ISportsServicio? _serviciosSport;
        private readonly IMapper? _mapper;
        public SportController(ISportsServicio? servicios, IMapper mapper)
        {
            _serviciosSport = servicios ?? throw new ApplicationException("Dependencies not set");
            _mapper = mapper ?? throw new ApplicationException("Dependencies not set");
        }

        public IActionResult Index(int? page, string? searchTerm = null, bool viewAll = false, int pageSize = 10)
        {
            int pageNumber = page ?? 1;
            ViewBag.currentPageSize = pageSize;
            IEnumerable<Sport>? sports;
            if (!viewAll)
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    sports = _serviciosSport?
                        .GetLista(orderBy: o => o.OrderBy(c => c.SportName),
                            filter: c => c.SportName.Contains(searchTerm));
                    ViewBag.currentSearchTerm = searchTerm;
                }
                else
                {
                    sports = _serviciosSport?
                        .GetLista(orderBy: o => o.OrderBy(c => c.SportName));
                }
            }
            else
            {
                sports = _serviciosSport?
                    .GetLista(orderBy: o => o.OrderBy(c => c.SportName));
            }
            var sportVm = _mapper?.Map<List<SportListVm>>(sports)
               .ToPagedList(pageNumber, pageSize);

            return View(sportVm);
        }
        public IActionResult UpSert(int? id)
        {
            if (_serviciosSport == null || _mapper == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
            }
            SportEditVm sportVm;
            if (id == null || id == 0)
            {
                sportVm = new SportEditVm();
            }
            else
            {
                try
                {
                    Sport? sport = _serviciosSport.GetSportPorId(filter: c => c.SportId == id);
                    if (sport == null)
                    {
                        return NotFound();
                    }
                    sportVm = _mapper.Map<SportEditVm>(sport);
                    return View(sportVm);
                }
                catch (Exception)
                {
                    // Log the exception (ex) here as needed
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the record.");
                }
            }
            return View(sportVm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSert(SportEditVm SportVm)
        {
            if (!ModelState.IsValid)
            {
                return View(SportVm);
            }

            if (_serviciosSport == null || _mapper == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
            }

            try
            {
                Sport sport = _mapper.Map<Sport>(SportVm);

                if (_serviciosSport.Existe(sport))
                {
                    ModelState.AddModelError(string.Empty, "Record already exist");
                    return View(SportVm);
                }

                _serviciosSport.Guardar(sport);
                TempData["success"] = "Record successfully added/edited";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                // Log the exception (ex) here as needed
                ModelState.AddModelError(string.Empty, "An error occurred while editing the record.");
                return View(SportVm);
            }
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            Sport? sport = _serviciosSport?.GetSportPorId(filter: c => c.SportId == id);
            if (sport is null)
            {
                return NotFound();
            }
            try
            {
                if (_serviciosSport == null || _mapper == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
                }

                if (_serviciosSport.EstaRelacionado(sport))
                {
                    return Json(new { success = false, message = "Related Record... Delete Deny!!" }); ;
                }
                _serviciosSport.Borrar(sport);
                return Json(new { success = true, message = "Record successfully deleted" });
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Couldn't delete record!!! " }); ;

            }
        }

    }

}

