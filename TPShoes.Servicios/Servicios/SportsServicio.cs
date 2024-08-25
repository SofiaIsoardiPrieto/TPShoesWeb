using System.Linq.Expressions;
using TPShoes.Datos;
using TPShoes.Datos.Interfaces;
using TPShoes.Entidades.Clases;
using TPShoes.Servicios.Interfaces;

namespace TPShoes.Servicios.Servicios
{
    public class SportsServicio : ISportsServicio
    {
        private readonly IRepositorioSports _repository;
        private readonly IUnitOfWork _unitOfWork;
        public SportsServicio(IRepositorioSports repository, IUnitOfWork unitOfWork)
        {
            _repository = repository ?? throw new ArgumentException("Error en la dependencia");
			_unitOfWork = unitOfWork ?? throw new ArgumentException("Error en la dependencia");
		}
        public void Borrar(Sport sport)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _repository!.Delete(sport);
                _unitOfWork.SaveChanges();
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public bool EstaRelacionado(Sport sport)
        {
            return _repository!.EstaRelacionado(sport);
        }

        public bool Existe(Sport sport)
        {
            return _repository!.Existe(sport);
        }

        public Sport GetSportPorNombre(string sportNombre)
        {
            return _repository!.GetSportPorNombre(sportNombre);
        }
        public int GetCantidad()
        {
            return _repository!.GetCantidad();
        }
		public IEnumerable<Sport>? GetLista(Expression<Func<Sport, bool>>? filter = null,
			Func<IQueryable<Sport>, IOrderedQueryable<Sport>>? orderBy = null,
			string? propertiesNames = null)
		{
			return _repository!.GetAll(filter, orderBy, propertiesNames);
		}

		public void Guardar(Sport sport)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                if (sport.SportId == 0)
                {
                    _repository!.Add(sport);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    _repository!.Editar(sport);
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

        public Sport? GetSportPorId(Expression<Func<Sport, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
		{
			return _repository!.Get(filter, propertiesNames, tracked);
		}

	}

}
