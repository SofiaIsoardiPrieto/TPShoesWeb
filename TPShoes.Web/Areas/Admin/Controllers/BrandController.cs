﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TPShoes.Entidades.Clases;
using TPShoes.Entidades.ViewModels.Brand;
using TPShoes.Entidades.ViewModels.Shoe;
using TPShoes.Servicios.Interfaces;
using X.PagedList;

namespace TPShoes.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly IBrandsServicio? _serviciosBrand;
        private readonly IShoesServicio? _serviciosShoe;
        private readonly IMapper? _mapper;
        private readonly IWebHostEnvironment? _webHostEnvironment;

        public BrandController(IBrandsServicio? serviciosBrand, IShoesServicio? serviciosShoe, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _serviciosBrand = serviciosBrand ?? throw new ApplicationException("Dependencies not set");
            _serviciosShoe = serviciosShoe ?? throw new ApplicationException("Dependencies not set");
            _mapper = mapper ?? throw new ApplicationException("Dependencies not set");
            _webHostEnvironment = webHostEnvironment ?? throw new ApplicationException("Dependencies not set");
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
            var brandListVm = _mapper?.Map<List<BrandListVm>>(brands)
               .ToPagedList(pageNumber, pageSize);
            foreach (var item in brandListVm)
            {
                item.CantShoes = _serviciosShoe.GetCantidad(b => b.BrandId == item.BrandId);
            }
            return View(brandListVm);
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
                    string? wwwWebRoot = _webHostEnvironment!.WebRootPath;
                    Brand? brand = _serviciosBrand.GetBrandPorId(filter: c => c.BrandId == id);
                    if (brand == null)
                    {
                        return NotFound();
                    }
                    if (brand.ImageUrl != null)
                    {
                        var filePath = Path.Combine(wwwWebRoot, brand.ImageUrl!.TrimStart('/'));
                        ViewData["ImageExist"] = System.IO.File.Exists(filePath);
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
                string? wwwWebRoot = _webHostEnvironment!.WebRootPath;
                Brand brand = _mapper.Map<Brand>(BrandVm);

                if (_serviciosBrand.Existe(brand))
                {
                    ModelState.AddModelError(string.Empty, "Record already exist");
                    return View(BrandVm);
                }

                if (BrandVm.ImageFile != null)
                {
                    var permittedExtensions = new string[] { ".png", ".jpg", ".jpeg", ".gif" };
                    var fileExtension = Path.GetExtension(BrandVm.ImageFile.FileName);

                    if (!permittedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError(string.Empty, "File not allowed");
                        return View(BrandVm);

                    }

                    if (brand.ImageUrl != null)
                    {
                        string oldFilePath = Path.Combine(wwwWebRoot, brand.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(BrandVm.ImageFile.FileName)}";
                    string pathName = Path.Combine(wwwWebRoot, "images", fileName);

                    using (var fileStream = new FileStream(pathName, FileMode.Create))
                    {
                        BrandVm.ImageFile.CopyTo(fileStream);
                    }
                    brand.ImageUrl = $"/images/{fileName}";


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


        public IActionResult Details(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            try
            {
                if (_serviciosBrand == null || _mapper == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
                }
                Brand? brand = _serviciosBrand?.GetBrandPorId(filter: c => c.BrandId == id.Value);

                if (brand is null )
                {
                    return NotFound();
                }
                var shoeList = _serviciosShoe.GetLista(filter: b => b.BrandId == brand.BrandId, propertiesNames: "Brand,Genre,Colour,Sport");
                if (shoeList is null || !shoeList.Any())
                {
                    
                    return NotFound();
                   
                }
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

