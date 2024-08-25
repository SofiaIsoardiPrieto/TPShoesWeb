using System.Linq.Expressions;
using TPShoes.Entidades.Clases;

namespace TPShoes.Servicios.Interfaces
{
    public interface IColoursServicio
    {
        void Borrar(Colour colour);
        bool EstaRelacionado(Colour colour);
        bool Existe(Colour colour);
        Colour? GetColourPorId(Expression<Func<Colour, bool>>? filter = null,
			string? propertiesNames = null,
			bool tracked = true);
        Colour GetColourPorNombre(string colourNombre);
		IEnumerable<Colour>? GetLista(Expression<Func<Colour, bool>>? filter = null,
			Func<IQueryable<Colour>, IOrderedQueryable<Colour>>? orderBy = null,
			string? propertiesNames = null);
        void Guardar(Colour colour);
        int GetCantidad();

    }
}
