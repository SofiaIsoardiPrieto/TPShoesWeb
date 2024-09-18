using System.Linq.Expressions;
using System.Numerics;
using TPShoes.Entidades.Clases;
using TPShoes.Entidades.Dtos;
using TPShoes.Entidades.Enum;

namespace TPShoes.Servicios.Interfaces
{
	public interface IShoesServicio
    {

        void Guardar(Shoe shoe);
        void Borrar(Shoe shoe);
        bool Existe(Shoe shoe);
        int GetCantidad(Expression<Func<Shoe, bool>>? filtro = null);

        Shoe? GetShoePorId(Expression<Func<Shoe, bool>>? filter = null,
             string? propertiesNames = null,
             bool tracked = true);
        IEnumerable<Shoe>? GetLista(Expression<Func<Shoe, bool>>? filter = null,
            Func<IQueryable<Shoe>, IOrderedQueryable<Shoe>>? orderBy = null,
            string? propertiesNames = null);
      
        IEnumerable<IGrouping<int, Shoe>> GetShoesPorMarcaEntreRangoPrecios(decimal rangoMin, decimal rangoMax);
        IEnumerable<IGrouping<int, Shoe>> GetShoesAgrupadosPorGenre();
        IEnumerable<IGrouping<int, Shoe>> GetShoesAgrupadosPorSport();
        bool EstaRelacionado(int shoeId);
        void AsignarSizeAShoe(Shoe shoe, Size size);
        List<ShoeDto> GetListaDto();
        IEnumerable<ShoeDto> GetShoesFiltradosPorBrandYColour(int brandId, int colourId);
    }
}
