﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TPShoes.Entidades.Clases;
using TPShoes.Entidades.ViewModels.Brand;
using TPShoes.Entidades.ViewModels.Genre;
using TPShoes.Entidades.ViewModels.Shoe;
using TPShoes.Servicios.Interfaces;
using X.PagedList;

namespace TPShoes.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GenreController : Controller
    {
        private readonly IGenresServicio? _serviciosGenre;
        private readonly IShoesServicio? _serviciosShoe;
        private readonly IMapper? _mapper;
        public GenreController(IGenresServicio? servicios, IShoesServicio? serviciosShoe, IMapper mapper)
        {
            _serviciosGenre = servicios ?? throw new ApplicationException("Dependencies not set");
            _serviciosShoe = serviciosShoe ?? throw new ApplicationException("Dependencies not set");
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
            var genreListVm = _mapper?.Map<List<GenreListVm>>(genres)
               .ToPagedList(pageNumber, pageSize);
            foreach (var item in genreListVm)
            {
                item.CantShoes = _serviciosShoe.GetCantidad(b => b.GenreId == item.GenreId);
            }
            return View(genreListVm);
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

        public IActionResult Details(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            try
            {
                if (_serviciosGenre == null || _mapper == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
                }
                Genre? genre = _serviciosGenre?.GetGenrePorId(filter: c => c.GenreId == id.Value);

                if (genre is null)
                {
                    return NotFound();
                }
                var shoeList = _serviciosShoe.GetLista(filter: b => b.GenreId == genre.GenreId, propertiesNames: "Brand,Genre,Colour,Sport");
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

