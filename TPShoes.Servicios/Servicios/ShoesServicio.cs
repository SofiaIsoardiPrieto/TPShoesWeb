using System.Diagnostics;
using System.Linq.Expressions;
using System.Web.Mvc;
using TPShoes.Datos;
using TPShoes.Datos.Interfaces;
using TPShoes.Entidades.Clases;
using TPShoes.Entidades.Dtos;
using TPShoes.Entidades.Enum;
using TPShoes.Servicios.Interfaces;

namespace TPShoes.Servicios.Servicios
{
	public class ShoesServicio : IShoesServicio
    {
        private readonly IRepositorioShoes _repository;
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IProveedoresRepository _proveedorRepository;

        public ShoesServicio(IRepositorioShoes repository, IUnitOfWork unitOfWork)
        {
            _repository = repository ?? throw new ArgumentException("Error en la dependencia");
			_unitOfWork = unitOfWork ?? throw new ArgumentException("Error en la dependencia");
			//_proveedorRepository = proveedorRepository;
		}

      
        public void Borrar(Shoe shoe)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _repository!.Delete(shoe);
                _unitOfWork.SaveChanges();
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();

                throw;
            }
        }

        public bool Existe(Shoe shoe)
        {
            return _repository.Existe(shoe);
        }

        public int GetCantidad(Expression<Func<Shoe, bool>>? filtro = null)
        {
            try
            {
                return _repository.GetCantidad(filtro);
            }
            catch (Exception ex)
            {

                throw new Exception("Habilita el servidor!.", ex);
            }
        }
       

        public IEnumerable<IGrouping<int, Shoe>> GetShoesAgrupadosPorGenre()
        {
            return _repository.GetShoesAgrupadosPorGenre();
        }

        public IEnumerable<IGrouping<int, Shoe>> GetShoesAgrupadosPorSport()
        {
            return _repository.GetShoesAgrupadosPorSport();
        }

        public void Guardar(Shoe shoe)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                if (shoe.GenreId == 0)
                {
                    _repository!.Add(shoe);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    _repository!.Editar(shoe);
                    _unitOfWork.SaveChanges();
                }
                _unitOfWork.SaveChanges();
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }

        }

        public void Editar(Shoe shoe, int? sizeId = null)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                _repository.Editar(shoe);

                //if (sizeId.HasValue)
                //{
                //    // Buscar el proveedor
                //    var proveedor = _proveedorRepository
                //        .GetProveedorPorId(sizeId.Value);
                //    if (proveedor != null)
                //    {
                //        // Crear la nueva relación si no existe
                //        if (!shoe.ProveedoresPlantas
                //            .Any(pp => pp.ProveedorId == sizeId))
                //        {
                //            var nuevaRelacion = new ProveedorPlanta
                //            {
                //                PlantaId = shoe.PlantaId,
                //                ProveedorId = proveedor.ProveedorId
                //            };
                //            _proveedorRepository.AgregarProveedorPlanta(nuevaRelacion);

                //        }
                //    }
                //    else
                //    {
                //        throw new Exception("Proveedor no encontrado.");
                //    }
                //}

                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

      

        public void AsignarSizeAShoe(Shoe shoe, Size size)
        {
            try
            {
                _unitOfWork.BeginTransaction();


                // Crear una nueva relación entre la shoe y el size
                SizeShoe nuevoSizeShoe = new SizeShoe
                {
                    
                    Shoe = shoe,
                    Size = size
                };
                _repository.AsignarSizeAShoe(nuevoSizeShoe);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }

        }

        public List<ShoeDto> GetListaDto()
        {
            return _repository.GetListaDto();
        }

        public IEnumerable<ShoeDto> GetShoesFiltradosPorBrandYColour(int brandId, int colourId)
        {
            try
            {
                return _repository.GetShoesFiltradosPorBrandYColour(brandId, colourId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<IGrouping<int, Shoe>> GetShoesPorMarcaEntreRangoPrecios(decimal rangoMin, decimal rangoMax)
        {

            try
            {
                return _repository.GetShoesPorMarcaEntreRangoPrecios(rangoMin, rangoMax);
            }
            catch (Exception)
            {

                throw;
            }
        }

        IEnumerable<Shoe>? IShoesServicio.GetLista(Expression<Func<Shoe, bool>>? filter, Func<IQueryable<Shoe>, IOrderedQueryable<Shoe>>? orderBy, string? propertiesNames)
        {
            return _repository!.GetAll(filter, orderBy, propertiesNames);
        }
       
        public Shoe? GetShoePorId(Expression<Func<Shoe, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
        {
            return _repository!.Get(filter, propertiesNames, tracked);
        }
      

        public bool EstaRelacionado(int shoeId)
        {
            return _repository!.EstaRelacionado(shoeId);
        }
    }

}
