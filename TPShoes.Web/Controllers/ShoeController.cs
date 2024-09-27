using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using TPShoes.Entidades.Clases;
using TPShoes.Entidades.ViewModels.Shoe;
using TPShoes.Servicios.Interfaces;
using X.PagedList.Extensions;

namespace TPShoes.Web.Controllers
{
    public class ShoeController : Controller
    {
        private readonly IShoesServicio? _shoeService;
        private readonly IBrandsServicio? _brandsService;
        private readonly IColoursServicio? _colourService;
        private readonly IGenresServicio? _genreService;
        private readonly ISportsServicio? _sportService;
        private readonly IMapper? _mapper;

        private int pageSize = 10;

        public ShoeController(IShoesServicio? shoeService,
            IBrandsServicio brandsService,
            IColoursServicio colourService,
            IGenresServicio genreService,
            ISportsServicio sportService,
            IMapper? mapper)
        {
            _shoeService = shoeService ?? throw new ApplicationException("Dependencies not set");
            _brandsService = brandsService ?? throw new ApplicationException("Dependencies not set");
            _colourService = colourService ?? throw new ApplicationException("Dependencies not set");
            _genreService = genreService ?? throw new ApplicationException("Dependencies not set");
            _sportService = sportService ?? throw new ApplicationException("Dependencies not set");
            _mapper = mapper ?? throw new ApplicationException("Dependencies not set");
        }

        public IActionResult Index(int? page)
        {
            var currentPage = page ?? 1;
            var ShoeList = _shoeService?
                    .GetLista(
                        orderBy: o => o.OrderBy(s => s.Model),
                        propertiesNames: "Brand,Genre,Colour,Sport");//????
            var shoeListVm = _mapper?.Map<List<ShoeListVm>>(ShoeList);
            return View(shoeListVm?.ToPagedList(currentPage, pageSize));
        }

        public IActionResult UpSert(int? id)
        {
            ShoeEditVm shoeEditVm;
            if (id == null || id == 0)
            {
                shoeEditVm = new ShoeEditVm();
                CargarListasCombos(shoeEditVm);

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
                    CargarListasCombos(shoeEditVm);


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
                CargarListasCombos(shoeEditVm);

                return View(shoeEditVm);
            }


            try
            {
                Shoe shoe = _mapper!.Map<Shoe>(shoeEditVm);

                if (_shoeService!.Existe(shoe))
                {
                    ModelState.AddModelError(string.Empty, "Record already exist");
                    CargarListasCombos(shoeEditVm);

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
                CargarListasCombos(shoeEditVm);

                return View(shoeEditVm);
            }
        }

        private void CargarListasCombos(ShoeEditVm shoeEditVm)
        {
            shoeEditVm.Brands = _brandsService!.GetLista(
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



       

    }
}