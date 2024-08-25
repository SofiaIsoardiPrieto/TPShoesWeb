using System.Linq.Expressions;
using TPShoes.Entidades.Clases;
using TPShoes.Entidades.Dtos;
using TPShoes.Entidades.Enum;

namespace TPShoes.Servicios.Interfaces
{
    public interface ISizesServicio
    {
        void Guardar(Size size);
        void Guardar(SizeShoe sizeShoe);
        void Borrar(Size size);
        bool Existe(Size size);
        List<Size> GetSizesPorId(Expression<Func<Size, bool>>? filter = null,
			string? propertiesNames = null,
			bool tracked = true);
       // int GetCantidad();
        bool EstaRelacionado(Size size);
        List<SizeShoeDto>? GetSizeShoeDtoPorId(int shoeId);
        SizeShoe? GetSizeShoePorId(int sizeShoeId);
        bool Existe(SizeShoe sizeShoeEditado);
		IEnumerable<Size>? GetLista(Expression<Func<Size, bool>>? filter = null,
			Func<IQueryable<Size>, IOrderedQueryable<Size>>? orderBy = null,
			string? propertiesNames = null);
        Size GetSizePorId(int sizeId);
        List<Size>? GetSizesNoAsociadosPorShoeId(int shoeId);
    }
}
