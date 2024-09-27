using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TPShoes.Entidades.Clases;
using TPShoes.Entidades.ViewModels.Brand;
using TPShoes.Servicios.Interfaces;
using X.PagedList;

namespace TPShoes.Web.Controllers
{
    public class BrandController : Controller
    {
        private readonly IBrandsServicio? _serviciosBrand;
        private readonly IMapper? _mapper;
        public BrandController(IBrandsServicio? servicios, IMapper mapper)
        {
            _serviciosBrand = servicios ?? throw new ApplicationException("Dependencies not set");
            _mapper = mapper ?? throw new ApplicationException("Dependencies not set");
        }

        public IActionResult Index(int? page, string? searchTerm = null, bool viewAll = false, int pageSize = 10)
        {
            int pageNumber = page ?? 1;
            ViewBag.currentPageSize = pageSize;
            IEnumerable<Brand>? brands;
            if (!viewAll)
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    brands = _serviciosBrand?
                        .GetLista(orderBy: o => o.OrderBy(c => c.BrandName),
                            filter: c => c.BrandName.Contains(searchTerm));
                    ViewBag.currentSearchTerm = searchTerm;
                }
                else
                {
                    brands = _serviciosBrand?
                        .GetLista(orderBy: o => o.OrderBy(c => c.BrandName));
                }
            }
            else
            {
                brands = _serviciosBrand?
                    .GetLista(orderBy: o => o.OrderBy(c => c.BrandName));
            }
            var brandVm = _mapper?.Map<List<BrandListVm>>(brands)
               .ToPagedList(pageNumber, pageSize);

            return View(brandVm);
        }
        public IActionResult UpSert(int? id)
        {
            if (_serviciosBrand == null || _mapper == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
            }
            BrandEditVm brandVm;
            if (id == null || id == 0)
            {
                brandVm = new BrandEditVm();
            }
            else
            {
                try
                {
                    Brand? brand = _serviciosBrand.GetBrandPorId(filter: c => c.BrandId == id);
                    if (brand == null)
                    {
                        return NotFound();
                    }
                    brandVm = _mapper.Map<BrandEditVm>(brand);
                    return View(brandVm);
                }
                catch (Exception)
                {
                    // Log the exception (ex) here as needed
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the record.");
                }
            }
            return View(brandVm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSert(BrandEditVm BrandVm)
        {
            if (!ModelState.IsValid)
            {
                return View(BrandVm);
            }

            if (_serviciosBrand == null || _mapper == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
            }

            try
            {
                Brand brand = _mapper.Map<Brand>(BrandVm);

                if (_serviciosBrand.Existe(brand))
                {
                    ModelState.AddModelError(string.Empty, "Record already exist");
                    return View(BrandVm);
                }

                _serviciosBrand.Guardar(brand);
                TempData["success"] = "Record successfully added/edited";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                // Log the exception (ex) here as needed
                ModelState.AddModelError(string.Empty, "An error occurred while editing the record.");
                return View(BrandVm);
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
            Brand? brand = _serviciosBrand?.GetBrandPorId(filter: c => c.BrandId == id);
            if (brand is null)
            {
                return NotFound();
            }
            try
            {
                if (_serviciosBrand == null || _mapper == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
                }

                if (_serviciosBrand.EstaRelacionado(brand))
                {
                    return Json(new { success = false, message = "Related Record... Delete Deny!!" }); ;
                }
                _serviciosBrand.Borrar(brand);
                return Json(new { success = true, message = "Record successfully deleted" });
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Couldn't delete record!!! " }); ;

            }
        }

    }

}

