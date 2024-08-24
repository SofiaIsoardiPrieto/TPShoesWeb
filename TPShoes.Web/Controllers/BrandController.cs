using Microsoft.AspNetCore.Mvc;
using TPShoes.Entidades.Clases;
using TPShoes.Entidades.ViewModels;
using TPShoes.Servicios.Interfaces;

namespace TPShoes.Web.Controllers
{
	public class BrandController : Controller
	{
		private readonly IBrandsServicio? _servicios;
		//private readonly IMapper? _mapper;
		public BrandController(IBrandsServicio? servicios/*,IMapper mapper*/)
		{
			_servicios = servicios;
			//_mapper = mapper;
		}

		public IActionResult Index(int? page = 1)
		{
			int currentPage = page ?? 1;
			int pageSize = 10;
			var listaTipos = _servicios?.GetLista()
				.ToPagedList(currentPage, pageSize);
			return View(listaTipos);
		}
		public IActionResult UpSert(int? id)
		{
			if (_servicios is null /*|| _mapper is null*/)
			{
				return StatusCode(StatusCodes
					.Status500InternalServerError,
					"Dependencias no están configuradas correctamente");
			}
			BrandEditVm brandEditVm;
			if (id is null || id.Value == 0)
			{
				brandEditVm = new BrandEditVm();
			}
			else
			{
				Brand? brand = _servicios.GetBrandPorId(id.Value);
				if (brand is null)
				{
					return NotFound();
				}
			//	brandEditVm = _mapper
			//		.Map<BrandEditVm>(brand);

			}
			return View(brandEditVm);


		}
		[HttpPost]
		public IActionResult UpSert(Brand brand)
		{
			if (!ModelState.IsValid)
			{
				return View(brand);

			}
			if (_servicios is null || _mapper is null)
			{
				return StatusCode(StatusCodes
					.Status500InternalServerError,
					"Dependencias no están configuradas correctamente");
			}
			TipoDePlanta tipo = _mapper.Map<TipoDePlanta>(brand);
			try
			{
				if (_servicios.Existe(tipo))
				{
					ModelState.AddModelError(string.Empty, "Registro duplicado!!!");
					return View(brand);
				}
				_servicios.Guardar(tipo);
				TempData["success"] = "Registro agregado/editado satisfactoriamente!!!";
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{

				ModelState.AddModelError(string.Empty, ex.Message);
				return View(brand);
			}
		}

		[HttpDelete]
		public IActionResult Delete(int? id)
		{
			if (id is null || id.Value == 0)
			{
				return NotFound();
			}
			if (_servicios is null || _mapper is null)
			{
				return StatusCode(StatusCodes
					.Status500InternalServerError,
					"Dependencias no están configuradas correctamente");
			}
			TipoDePlanta? tipo = _servicios.GetTipoDePlantaPorId(id.Value);
			if (tipo is null)
			{
				return NotFound();
			}
			try
			{
				if (_servicios.EstaRelacionado(tipo))
				{
					return Json(new { success = false, message = "Registro relacionado... Baja denegada" });
				}
				_servicios.Borrar(tipo);
				return Json(new { success = true, message = "Registro borrado satisfactoriamente" });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
		}

	}

}
}
