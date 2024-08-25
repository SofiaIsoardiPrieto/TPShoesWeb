using System.Linq.Expressions;
using TPShoes.Entidades.Clases;

namespace TPShoes.Servicios.Interfaces
{
	public interface IBrandsServicio
    {
        void Borrar(Brand brand);
        bool EstaRelacionado(Brand brand);
        bool Existe(Brand brand);
        Brand? GetBrandPorId(Expression<Func<Brand, bool>>? filter = null,
			string? propertiesNames = null,
			bool tracked = true);
        Brand GetBrandPorNombre(string brandNombre);
        int GetCantidad();
		IEnumerable<Brand>? GetLista(Expression<Func<Brand, bool>>? filter = null,
			Func<IQueryable<Brand>, IOrderedQueryable<Brand>>? orderBy = null,
			string? propertiesNames = null);
        void Guardar(Brand brand);
    }
}
