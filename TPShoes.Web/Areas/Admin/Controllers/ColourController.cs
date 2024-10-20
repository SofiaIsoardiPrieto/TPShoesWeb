using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TPShoes.Entidades.Clases;
using TPShoes.Entidades.ViewModels.Colour;
using TPShoes.Entidades.ViewModels.Shoe;
using TPShoes.Servicios.Interfaces;
using X.PagedList;

namespace TPShoes.Web.Areas.Admin.Controllers
{
    public class ColourController : Controller
    {
        private readonly IColoursServicio? _serviciosColour;
        private readonly IShoesServicio? _serviciosShoe;
        private readonly IMapper? _mapper;
        public ColourController(IColoursServicio? servicios, IShoesServicio? serviciosShoe, IMapper mapper)
        {
            _serviciosColour = servicios ?? throw new ApplicationException("Dependencies not set");
            _serviciosShoe = serviciosShoe ?? throw new ApplicationException("Dependencies not set");
            _mapper = mapper ?? throw new ApplicationException("Dependencies not set");
        }

        public IActionResult Index(int? page, string? searchTerm = null, bool viewAll = false, int pageSize = 10)
        {
            int pageNumber = page ?? 1;
            ViewBag.currentPageSize = pageSize;
            IEnumerable<Colour>? colours;
            if (!viewAll)
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    colours = _serviciosColour?
                        .GetLista(orderBy: o => o.OrderBy(c => c.ColourName),
                            filter: c => c.ColourName.Contains(searchTerm));
                    ViewBag.currentSearchTerm = searchTerm;
                }
                else
                {
                    colours = _serviciosColour?
                        .GetLista(orderBy: o => o.OrderBy(c => c.ColourName));
                }
            }
            else
            {
                colours = _serviciosColour?
                    .GetLista(orderBy: o => o.OrderBy(c => c.ColourName));
            }
            var colourListVm = _mapper?.Map<List<ColourListVm>>(colours)
               .ToPagedList(pageNumber, pageSize);
            foreach (var item in colourListVm)
            {
                item.CantShoes = _serviciosShoe.GetCantidad(b => b.ColourId == item.ColourId);
            }
            return View(colourListVm);
        }
        public IActionResult UpSert(int? id)
        {
            if (_serviciosColour == null || _mapper == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
            }
            ColourEditVm colourVm;
            if (id == null || id == 0)
            {
                colourVm = new ColourEditVm();
            }
            else
            {
                try
                {
                    Colour? colour = _serviciosColour.GetColourPorId(filter: c => c.ColourId == id);
                    if (colour == null)
                    {
                        return NotFound();
                    }
                    colourVm = _mapper.Map<ColourEditVm>(colour);
                    return View(colourVm);
                }
                catch (Exception)
                {
                    // Log the exception (ex) here as needed
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the record.");
                }
            }
            return View(colourVm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSert(ColourEditVm ColourVm)
        {
            if (!ModelState.IsValid)
            {
                return View(ColourVm);
            }

            if (_serviciosColour == null || _mapper == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
            }

            try
            {
                Colour colour = _mapper.Map<Colour>(ColourVm);

                if (_serviciosColour.Existe(colour))
                {
                    ModelState.AddModelError(string.Empty, "Record already exist");
                    return View(ColourVm);
                }

                _serviciosColour.Guardar(colour);
                TempData["success"] = "Record successfully added/edited";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                // Log the exception (ex) here as needed
                ModelState.AddModelError(string.Empty, "An error occurred while editing the record.");
                return View(ColourVm);
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
            Colour? colour = _serviciosColour?.GetColourPorId(filter: c => c.ColourId == id);
            if (colour is null)
            {
                return NotFound();
            }
            try
            {
                if (_serviciosColour == null || _mapper == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
                }

                if (_serviciosColour.EstaRelacionado(colour))
                {
                    return Json(new { success = false, message = "Related Record... Delete Deny!!" }); ;
                }
                _serviciosColour.Borrar(colour);
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
                if (_serviciosColour == null || _mapper == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
                }
                Colour? colour = _serviciosColour?.GetColourPorId(filter: c => c.ColourId == id.Value);

                if (colour is null)
                {
                    return NotFound();
                }
                var shoeList = _serviciosShoe.GetLista(filter: b => b.ColourId == colour.ColourId, propertiesNames: "Brand,Genre,Colour,Sport");
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

