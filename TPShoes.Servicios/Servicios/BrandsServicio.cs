using System.Linq.Expressions;
using TPShoes.Datos;
using TPShoes.Datos.Interfaces;
using TPShoes.Entidades.Clases;
using TPShoes.Servicios.Interfaces;

namespace TPShoes.Servicios.Servicios
{
	public class BrandsServicio : IBrandsServicio
    {
        private readonly IRepositorioBrands _repository;
        private readonly IUnitOfWork _unitOfWork;
        public BrandsServicio(IRepositorioBrands repository, IUnitOfWork unitOfWork)
        {
            _repository = repository ?? throw new ArgumentException("Error en la dependencia");
			_unitOfWork = unitOfWork ?? throw new ArgumentException("Error en la dependencia");
		}
        public void Borrar(Brand brand)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _repository!.Delete(brand);
                _unitOfWork.SaveChanges();
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }


        }
        public bool EstaRelacionado(Brand brand)
        {
            return _repository!.EstaRelacionado(brand);
        }
        public bool Existe(Brand brand)
        {
            return _repository!.Existe(brand);
        }
        public Brand? GetBrandPorId(Expression<Func<Brand, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
        {
            return _repository!.Get(filter, propertiesNames, tracked);
        }
		public Brand GetBrandPorNombre(string brandNombre)
        {
            return _repository!.GetBrandPorNombre(brandNombre);
        }
        public int GetCantidad()
        {
            return _repository!.GetCantidad();
        }
        public IEnumerable<Brand>? GetLista(Expression<Func<Brand, bool>>? filter = null,
			Func<IQueryable<Brand>, IOrderedQueryable<Brand>>? orderBy = null,
			string? propertiesNames = null)
        {
            return _repository!.GetAll(filter, orderBy, propertiesNames);
        }	
		public void Guardar(Brand brand)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                if (brand.BrandId == 0)
                {
                    _repository!.Add(brand);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    _repository!.Editar(brand);
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
    }
}
