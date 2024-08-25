using System.Linq.Expressions;
using TPShoes.Datos;
using TPShoes.Datos.Interfaces;
using TPShoes.Entidades.Clases;
using TPShoes.Servicios.Interfaces;

namespace TPShoes.Servicios.Servicios
{
    public class ColoursServicio : IColoursServicio
    {

        private readonly IRepositorioColours _repository;
        private readonly IUnitOfWork _unitOfWork;
        public ColoursServicio(IRepositorioColours repository, IUnitOfWork unitOfWork)
        {
            _repository = repository ?? throw new ArgumentException("Error en la dependencia");
			_unitOfWork = unitOfWork ?? throw new ArgumentException("Error en la dependencia");
		}
        public void Borrar(Colour colour)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _repository!.Delete(colour);
                _unitOfWork.SaveChanges();
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public bool EstaRelacionado(Colour colour)
        {
            return _repository!.EstaRelacionado(colour);
        }

        public bool Existe(Colour colour)
        {
            return _repository!.Existe(colour);
        }
        public int GetCantidad()
        {
            return _repository!.GetCantidad();
        }
        public Colour? GetColourPorId(Expression<Func<Colour, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
        {
            return _repository!.Get(filter, propertiesNames, tracked);
        }

        public Colour GetColourPorNombre(string colourNombre)
        {
            return _repository!.GetColourPorNombre(colourNombre);
        }

		public IEnumerable<Colour>? GetLista(Expression<Func<Colour, bool>>? filter = null,
		   Func<IQueryable<Colour>, IOrderedQueryable<Colour>>? orderBy = null,
		   string? propertiesNames = null)
		{
			return _repository!.GetAll(filter, orderBy, propertiesNames);
		}

		public void Guardar(Colour colour)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                if (colour.ColourId == 0)
                {
                    _repository!.Add(colour);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    _repository!.Editar(colour);
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
