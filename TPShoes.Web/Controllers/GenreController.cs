using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TPShoes.Entidades.Clases;
using TPShoes.Entidades.ViewModels.Genre;
using TPShoes.Servicios.Interfaces;
using X.PagedList;

namespace TPShoes.Web.Controllers
{
    public class GenreController : Controller
    {
        private readonly IGenresServicio? _serviciosGenre;
        private readonly IMapper? _mapper;
        public GenreController(IGenresServicio? servicios, IMapper mapper)
        {
            _serviciosGenre = servicios ?? throw new ApplicationException("Dependencies not set");
            _mapper = mapper ?? throw new ApplicationException("Dependencies not set");
        }

        public IActionResult Index(int? page, string? searchTerm = null, bool viewAll = false, int pageSize = 10)
        {
            int pageNumber = page ?? 1;
            ViewBag.currentPageSize = pageSize;
            IEnumerable<Genre>? genres;
            if (!viewAll)
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    genres = _serviciosGenre?
                        .GetLista(orderBy: o => o.OrderBy(c => c.GenreName),
                            filter: c => c.GenreName.Contains(searchTerm));
                    ViewBag.currentSearchTerm = searchTerm;
                }
                else
                {
                    genres = _serviciosGenre?
                        .GetLista(orderBy: o => o.OrderBy(c => c.GenreName));
                }
            }
            else
            {
                genres = _serviciosGenre?
                    .GetLista(orderBy: o => o.OrderBy(c => c.GenreName));
            }
            var genreVm = _mapper?.Map<List<GenreListVm>>(genres)
               .ToPagedList(pageNumber, pageSize);

            return View(genreVm);
        }
        public IActionResult UpSert(int? id)
        {
            if (_serviciosGenre == null || _mapper == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
            }
            GenreEditVm genreVm;
            if (id == null || id == 0)
            {
                genreVm = new GenreEditVm();
            }
            else
            {
                try
                {
                    Genre? genre = _serviciosGenre.GetGenrePorId(filter: c => c.GenreId == id);
                    if (genre == null)
                    {
                        return NotFound();
                    }
                    genreVm = _mapper.Map<GenreEditVm>(genre);
                    return View(genreVm);
                }
                catch (Exception)
                {
                    // Log the exception (ex) here as needed
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the record.");
                }
            }
            return View(genreVm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSert(GenreEditVm GenreVm)
        {
            if (!ModelState.IsValid)
            {
                return View(GenreVm);
            }

            if (_serviciosGenre == null || _mapper == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
            }

            try
            {
                Genre genre = _mapper.Map<Genre>(GenreVm);

                if (_serviciosGenre.Existe(genre))
                {
                    ModelState.AddModelError(string.Empty, "Record already exist");
                    return View(GenreVm);
                }

                _serviciosGenre.Guardar(genre);
                TempData["success"] = "Record successfully added/edited";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                // Log the exception (ex) here as needed
                ModelState.AddModelError(string.Empty, "An error occurred while editing the record.");
                return View(GenreVm);
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
            Genre? genre = _serviciosGenre?.GetGenrePorId(filter: c => c.GenreId == id);
            if (genre is null)
            {
                return NotFound();
            }
            try
            {
                if (_serviciosGenre == null || _mapper == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
                }

                if (_serviciosGenre.EstaRelacionado(genre))
                {
                    return Json(new { success = false, message = "Related Record... Delete Deny!!" }); ;
                }
                _serviciosGenre.Borrar(genre);
                return Json(new { success = true, message = "Record successfully deleted" });
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Couldn't delete record!!! " }); ;

            }
        }

    }

}

