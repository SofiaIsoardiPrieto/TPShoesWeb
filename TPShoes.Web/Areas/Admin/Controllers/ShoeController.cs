using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TPShoes.Entidades.Clases;
using TPShoes.Entidades.ViewModels.Shoe;
using TPShoes.Entidades.ViewModels.SizeShoe;
using TPShoes.Servicios.Interfaces;
using X.PagedList;


namespace TPShoes.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShoeController : Controller
    {
        private readonly IShoesServicio? _shoeService;
        private readonly ISizeShoesServicio? _sizeShoeService;
        private readonly IBrandsServicio? _brandService;
        private readonly IColoursServicio? _colourService;
        private readonly IGenresServicio? _genreService;
        private readonly ISportsServicio? _sportService;
        private readonly ISizesServicio? _sizeService;
        private readonly IMapper? _mapper;

        private int pageSize = 10;

        public ShoeController(IShoesServicio? shoeService, ISizeShoesServicio? sizeShoeService,
            IBrandsServicio brandService,
            IColoursServicio colourService,
            IGenresServicio genreService,
            ISportsServicio sportService, ISizesServicio sizeService,
            IMapper? mapper)
        {
            _shoeService = shoeService ?? throw new ApplicationException("Dependencies not set");
            _sizeShoeService = sizeShoeService ?? throw new ApplicationException("Dependencies not set");
            _brandService = brandService ?? throw new ApplicationException("Dependencies not set");
            _colourService = colourService ?? throw new ApplicationException("Dependencies not set");
            _genreService = genreService ?? throw new ApplicationException("Dependencies not set");
            _sportService = sportService ?? throw new ApplicationException("Dependencies not set");
            _sizeService = sizeService ?? throw new ApplicationException("Dependencies not set");
            _mapper = mapper ?? throw new ApplicationException("Dependencies not set");
        }

        public IActionResult Index(int? page, string? searchTerm = null, int? FilterBrandId = null, int? FilterColorId = null, int? FilterGenreId = null, int? FilterSportId = null, int pageSize = 10, bool viewAll = false, string orderBY = "Brand")
        {
            int pagenumber = page ?? 1;
            ViewBag.currentPageSize = pageSize;
            ViewBag.currentsearchTerm = searchTerm; //Un viewBag es como una variable de la vista que se usa en la vista para no ensuciar más el modelo de mi vista
            ViewBag.currentFilterBrandId = FilterBrandId;
            ViewBag.currentFilterColorId = FilterColorId;
            ViewBag.currentFilterGenreId = FilterGenreId;
            ViewBag.currentFilterSportId = FilterSportId;
            ViewBag.currentOrderBy = orderBY;
            IEnumerable<Shoe> shoes;
            if (!viewAll)
            {
                if (!string.IsNullOrEmpty(searchTerm) || FilterBrandId != null || FilterColorId != null || FilterGenreId != null || FilterSportId != null)
                {
                    if (searchTerm != null && FilterBrandId == null && FilterColorId == null && FilterGenreId == null && FilterSportId == null)
                    {
                        FilterBrandId = null;
                        FilterColorId = null;
                        FilterGenreId = null;
                        FilterSportId = null;
                        ViewBag.currentFilterBrandId = FilterBrandId;
                        ViewBag.currentFilterColorId = FilterColorId;
                        ViewBag.currentFilterGenreId = FilterGenreId;
                        ViewBag.currentFilterSportId = FilterSportId;
                        //Hay que escribir las propiedades como estan en la entidad
                        shoes = _shoeService!.GetLista(propertiesNames: "Sport,Brand,Colour,Genre", orderBy: o => o.OrderBy(c => c.Price),
                                    filter: f => f.Brand!.BrandName.Contains(searchTerm!) || f.Colour!.ColourName.Contains(searchTerm) || f.Sport!.SportName.Contains(searchTerm) ||
                                    f.Genre!.GenreName.Contains(searchTerm))!;
                    }
                    else
                    {
                        searchTerm = string.Empty;
                        ViewBag.currentsearchTerm = searchTerm;
                        shoes = _shoeService!.GetLista(propertiesNames: "Sport,Brand,Colour,Genre", orderBy: o => o.OrderBy(order => order.Price),
                            filter: f => f.BrandId == FilterBrandId || f.ColourId == FilterColorId || f.GenreId == FilterGenreId || f.SportId == FilterSportId)!;
                    }

                }
                else
                {
                    shoes = _shoeService!.GetLista(orderBy: o => o.OrderBy(b => b.Price), propertiesNames: "Sport,Brand,Colour,Genre")!;
                }
            }
            else
            {
                searchTerm = string.Empty;
                FilterBrandId = null;
                FilterColorId = null;
                FilterGenreId = null;
                FilterSportId = null;
                ViewBag.currentsearchTerm = searchTerm;
                ViewBag.currentFilterBrandId = FilterBrandId;
                ViewBag.currentFilterColorId = FilterColorId;
                ViewBag.currentFilterGenreId = FilterGenreId;
                ViewBag.currentFilterSportId = FilterSportId;
                shoes = _shoeService!.GetLista(orderBy: o => o.OrderBy(b => b.Price), propertiesNames: "Sport,Brand,Colour,Genre")!;
            }

            var shoeListVm = _mapper?.Map<IEnumerable<ShoeListVm>>(shoes).ToList();
            if (orderBY == "Brand")
            {
                shoeListVm = shoeListVm!.OrderBy(o => o.Brand).ToList();
            }
            if (orderBY == "Colour")
            {
                shoeListVm = shoeListVm!.OrderBy(o => o.Colour).ToList();
            }
            if (orderBY == "Genre")
            {
                shoeListVm = shoeListVm!.OrderBy(o => o.Genre).ToList();
            }
            if (orderBY == "Sport")
            {
                shoeListVm = shoeListVm!.OrderBy(o => o.Sport).ToList();
            }
            var shoeFilterVM = new ShoeFilterVm()
            {
                Shoes = shoeListVm!.ToPagedList(pagenumber, pageSize),
                Brands = _brandService!.GetLista(orderBy: o => o.OrderBy(order => order.BrandName))!.Select(s => new SelectListItem { Text = s.BrandName, Value = s.BrandId.ToString() }).ToList(),
                Genres = _genreService!.GetLista(orderBy: o => o.OrderBy(order => order.GenreName))!.Select(s => new SelectListItem { Text = s.GenreName, Value = s.GenreId.ToString() }).ToList(),
                Colours = _colourService!.GetLista(orderBy: o => o.OrderBy(order => order.ColourName))!.Select(s => new SelectListItem { Text = s.ColourName, Value = s.ColourId.ToString() }).ToList(),
                Sports = _sportService!.GetLista(orderBy: o => o.OrderBy(order => order.SportName))!.Select(s => new SelectListItem { Text = s.SportName, Value = s.SportId.ToString() }).ToList(),

            };
            return View(shoeFilterVM);
        }
        //private List<SelectListItem> GetSizes()
        //{
        //    return _sizeService!.GetLista(
        //            orderBy: o => o.OrderBy(c => c.SizeNumber))!
        //        .Select(c => new SelectListItem
        //        {
        //            Text = c.SizeNumber.ToString(),
        //            Value = c.SizeId.ToString()
        //        }).ToList();
        //}
        public IActionResult UpSert(int? id)
        {
            ShoeEditVm shoeEditVm;
            if (id == null || id == 0)
            {
                shoeEditVm = new ShoeEditVm();
                CargarListasCombosShoe(shoeEditVm);

            }
            else
            {
                try
                {
                    Shoe? shoe = _shoeService!.GetShoePorId(filter: c => c.ShoeId == id);
                    if (shoe == null)
                    {
                        return NotFound();
                    }
                    shoeEditVm = _mapper!.Map<ShoeEditVm>(shoe);
                    CargarListasCombosShoe(shoeEditVm);


                    return View(shoeEditVm);
                }
                catch (Exception)
                {
                    // Log the exception (ex) here as needed
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the record.");
                }

            }
            return View(shoeEditVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSert(ShoeEditVm shoeEditVm)
        {
            if (!ModelState.IsValid)
            {
                CargarListasCombosShoe(shoeEditVm);
                return View(shoeEditVm);
            }
            try
            {
                Shoe shoe = _mapper!.Map<Shoe>(shoeEditVm);

                if (_shoeService!.Existe(shoe))
                {
                    ModelState.AddModelError(string.Empty, "Record already exist");
                    CargarListasCombosShoe(shoeEditVm);

                    return View(shoeEditVm);
                }

                _shoeService.Guardar(shoe);
                TempData["success"] = "Record successfully added/edited";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                // Log the exception (ex) here as needed
                ModelState.AddModelError(string.Empty, "An error occurred while editing the record.");
                CargarListasCombosShoe(shoeEditVm);

                return View(shoeEditVm);
            }
        }
        private void CargarListasCombosSizeShoe(SizeShoeEditVm sizeShoeEditVm)
        {

            sizeShoeEditVm.Shoes = _shoeService!.GetLista(
                                        orderBy: o => o.OrderBy(c => c.Model))
                                    .Select(c => new SelectListItem
                                    {
                                        Text = c.Model,
                                        Value = c.ShoeId.ToString()
                                    }).ToList();
            sizeShoeEditVm.Sizes = _sizeService!.GetLista(
                                        orderBy: o => o.OrderBy(c => c.SizeNumber))
                                    .Select(c => new SelectListItem
                                    {
                                        Text = c.SizeNumber.ToString(),
                                        Value = c.SizeId.ToString()
                                    }).ToList();
        }
        private void CargarListasCombosShoe(ShoeEditVm shoeEditVm)
        {
            shoeEditVm.Brands = _brandService!.GetLista(
                                        orderBy: o => o.OrderBy(c => c.BrandName))
                                    .Select(c => new SelectListItem
                                    {
                                        Text = c.BrandName,
                                        Value = c.BrandId.ToString()
                                    }).ToList();
            shoeEditVm.Genres = _genreService!.GetLista(
                                        orderBy: o => o.OrderBy(c => c.GenreName))
                                    .Select(c => new SelectListItem
                                    {
                                        Text = c.GenreName,
                                        Value = c.GenreId.ToString()
                                    }).ToList();
            shoeEditVm.Colours = _colourService!.GetLista(
                                        orderBy: o => o.OrderBy(c => c.ColourName))
                                    .Select(c => new SelectListItem
                                    {
                                        Text = c.ColourName,
                                        Value = c.ColourId.ToString()
                                    }).ToList();
            shoeEditVm.Sports = _sportService!.GetLista(
                                        orderBy: o => o.OrderBy(c => c.SportName))
                                    .Select(c => new SelectListItem
                                    {
                                        Text = c.SportName,
                                        Value = c.SportId.ToString()
                                    }).ToList();

        }
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            Shoe? Shoe = _shoeService?.GetShoePorId(filter: c => c.ShoeId == id);
            if (Shoe is null)
            {
                return NotFound();
            }
            try
            {
                if (_shoeService!.EstaRelacionado(Shoe.ShoeId))
                {
                    return Json(new { success = false, message = "Related Record... Delete Deny!!" }); ;
                }
                _shoeService.Borrar(Shoe);
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
            int idShoe = id.Value;
            try
            {
                if (_sizeShoeService == null || _mapper == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
                }

                var sizeShoeList = _sizeShoeService.GetSizeShoesPorId(idShoe);
                if (sizeShoeList is null || !sizeShoeList.Any())
                {

                    return NotFound();

                }
                var sizeShoeListVm = _mapper?.Map<IEnumerable<SizeShoeListVm>>(sizeShoeList).ToList();


                return View(sizeShoeList);
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Error!!! " }); ;

            }

        }
        public IActionResult EditSize(int ShoeId, int? SizeId)
        {
           
            SizeShoeEditVm sizeShoeEditVm;
            if (SizeId == null || SizeId == 0)
            {
                sizeShoeEditVm = new SizeShoeEditVm();
                CargarListasCombosSizeShoe(sizeShoeEditVm);
            }
            else
            {
                try
                {
                    int sizeId = SizeId.Value;
                    SizeShoe sizeShoe = _sizeShoeService!.GetSizeShoePorId(ShoeId, sizeId);
                    if (sizeShoe == null)
                    {
                        return NotFound();
                    }
                    sizeShoeEditVm = _mapper!.Map<SizeShoeEditVm>(sizeShoe);
                    CargarListasCombosSizeShoe(sizeShoeEditVm);
                    return View(sizeShoeEditVm);

                }
                catch (Exception)
                {
                    // Log the exception (ex) here as needed
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the record.");
                }
            }
            return View(sizeShoeEditVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSize(SizeShoeEditVm sizeShoeEditVm)
        {
            //ModelState.Remove("Sizes");
            //ModelState.Remove("Shoes");
            if (!ModelState.IsValid)
            {
                CargarListasCombosSizeShoe(sizeShoeEditVm);
                return View(sizeShoeEditVm);
            }
            if (_sizeShoeService == null || _mapper == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
            }
            try
            {
                SizeShoe sizeShoe = _mapper!.Map<SizeShoe>(sizeShoeEditVm);

                //if (_sizeShoeService.Existe(sizeShoe))
                //{
                   
                //    ModelState.AddModelError(string.Empty, "Record already exist");
                //    CargarListasCombosSizeShoe(sizeShoeEditVm);
                //    return View(sizeShoeEditVm);
               // }

                _sizeShoeService.Guardar(sizeShoe);
                TempData["success"] = "Record successfully added/edited";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                // Log the exception (ex) here as needed
                ModelState.AddModelError(string.Empty, "An error occurred while editing the record.");
                return View(sizeShoeEditVm);
            }
        }
    }
}