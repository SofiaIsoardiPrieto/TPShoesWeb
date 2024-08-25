using System.Linq.Expressions;
using TPShoes.Entidades.Clases;

namespace TPShoes.Servicios.Interfaces
{
    public interface ISportsServicio
    {
        void Borrar(Sport sport);
        bool EstaRelacionado(Sport sport);
        bool Existe(Sport sport);
        IEnumerable<Sport>? GetLista(Expression<Func<Sport, bool>>? filter = null,
			Func<IQueryable<Sport>, IOrderedQueryable<Sport>>? orderBy = null,
			string? propertiesNames = null);
        Sport? GetSportPorId(Expression<Func<Sport, bool>>? filter = null,
			string? propertiesNames = null,
			bool tracked = true);
        Sport GetSportPorNombre(string sportNombre);
        void Guardar(Sport sport);
        int GetCantidad();
    }
}
