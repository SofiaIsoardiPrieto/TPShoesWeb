using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TPShoes.Entidades.Clases;
using TPShoes.Entidades.ViewModels;
using TPShoes.Servicios.Interfaces;
using X.PagedList.Extensions;

namespace TPShoes.Web.Controllers
{
	public class BrandController : Controller
	{
		private readonly IBrandsServicio? _servicios;
		private readonly IMapper? _mapper;
		public BrandController(IBrandsServicio? servicios, IMapper mapper)
		{
			_servicios = servicios;
			_mapper = mapper;
		}

		public IActionResult Index(int? page)
		{
			int pageNumber = page ?? 1;
			int pageSize = 10;
			var brands = _servicios?
				.GetLista(orderBy: o => o.OrderBy(c => c.BrandName));
			var categoriesVm = _mapper?.Map<List<BrandEditVm>>(brands)
				.ToPagedList(pageNumber, pageSize);

			return View(categoriesVm);
		}
		public IActionResult UpSert(int? id)
		{
			if (_servicios == null || _mapper == null)
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
					Brand? brand = _servicios.GetBrandPorId(filter: c => c.BrandId == id);
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
		public IActionResult UpSert(BrandEditVm categoryVm)
		{
			if (!ModelState.IsValid)
			{
				return View(categoryVm);
			}

			if (_servicios == null || _mapper == null)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
			}

			try
			{
				Brand brand = _mapper.Map<Brand>(categoryVm);

				if (_servicios.Existe(brand))
				{
					ModelState.AddModelError(string.Empty, "Record already exist");
					return View(categoryVm);
				}

				_servicios.Guardar(brand);
				TempData["success"] = "Record successfully added/edited";
				return RedirectToAction("Index");
			}
			catch (Exception)
			{
				// Log the exception (ex) here as needed
				ModelState.AddModelError(string.Empty, "An error occurred while editing the record.");
				return View(categoryVm);
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
			Brand? brand = _servicios?.GetBrandPorId(filter: c => c.BrandId == id);
			if (brand is null)
			{
				return NotFound();
			}
			try
			{
				if (_servicios == null || _mapper == null)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
				}

				if (_servicios.EstaRelacionado(brand))
				{
					return Json(new { success = false, message = "Related Record... Delete Deny!!" }); ;
				}
				_servicios.Borrar(brand);
				return Json(new { success = true, message = "Record successfully deleted" });
			}
			catch (Exception)
			{

				return Json(new { success = false, message = "Couldn't delete record!!! " }); ;

			}
		}

	}

}

