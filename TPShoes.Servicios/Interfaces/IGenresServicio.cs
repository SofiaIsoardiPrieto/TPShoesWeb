using System.Linq.Expressions;
using TPShoes.Entidades.Clases;

namespace TPShoes.Servicios.Interfaces
{
    public interface IGenresServicio
    {
        void Borrar(Genre genre);
        bool EstaRelacionado(Genre genre);
        bool Existe(Genre genre);
        int GetCantidad();
        Genre? GetGenrePorId(Expression<Func<Genre, bool>>? filter = null,
			string? propertiesNames = null,
			bool tracked = true);
        Genre GetGenrePorNombre(string genreNombre);
        IEnumerable<Genre>? GetLista(Expression<Func<Genre, bool>>? filter = null,
			Func<IQueryable<Genre>, IOrderedQueryable<Genre>>? orderBy = null,
			string? propertiesNames = null);
        void Guardar(Genre genre);
        
    }
}
