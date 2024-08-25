using System.Linq.Expressions;
using TPShoes.Datos;
using TPShoes.Datos.Interfaces;
using TPShoes.Entidades.Clases;
using TPShoes.Servicios.Interfaces;

namespace TPShoes.Servicios.Servicios
{
    public class GenresServicio : IGenresServicio
    {
        private readonly IRepositorioGenres _repository;
        private readonly IUnitOfWork _unitOfWork;
        public GenresServicio(IRepositorioGenres repository, IUnitOfWork unitOfWork)
        {
            _repository = repository ?? throw new ArgumentException("Error en la dependencia");
			_unitOfWork = unitOfWork ?? throw new ArgumentException("Error en la dependencia");
		}
        public void Borrar(Genre genre)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _repository!.Delete(genre);
                _unitOfWork.SaveChanges();
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();

                throw;
            }
        }

        public bool EstaRelacionado(Genre genre)
        {
            return _repository!.EstaRelacionado(genre);
        }

        public bool Existe(Genre genre)
        {
            return _repository!.Existe(genre);
        }
        public int GetCantidad()
        {
            return _repository!.GetCantidad();
        }
        public Genre? GetGenrePorId(Expression<Func<Genre, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
		{
			return _repository!.Get(filter, propertiesNames, tracked);
		}

		public Genre GetGenrePorNombre(string genreNombre)
        {
            return _repository!.GetGenrePorNombre(genreNombre);
        }

		public IEnumerable<Genre>? GetLista(Expression<Func<Genre, bool>>? filter = null,
		   Func<IQueryable<Genre>, IOrderedQueryable<Genre>>? orderBy = null,
		   string? propertiesNames = null)
		{
			return _repository!.GetAll(filter, orderBy, propertiesNames);
		}

		public void Guardar(Genre genre)
        {

            try
            {
                _unitOfWork.BeginTransaction();
                if (genre.GenreId == 0)
                {
                    _repository!.Add(genre);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    _repository!.Editar(genre);
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
