using System.Linq.Expressions;
using TPShoes.Entidades.Clases;
using TPShoes.Entidades.Dtos;
using TPShoes.Entidades.Enum;

namespace TPShoes.Datos.Interfaces
{
	public interface IRepositorioShoes : IRepositorioGenerico<Shoe>
    {
        bool Existe(Shoe shoe);
       int GetCantidad(Expression<Func<Shoe, bool>>? filtro = null);
        IEnumerable<IGrouping<int, Shoe>> GetShoesAgrupadosPorGenre();
        IEnumerable<IGrouping<int, Shoe>> GetShoesAgrupadosPorSport();
        void Editar(Shoe shoe);
        void EliminarRelaciones(Shoe shoe);
        bool EstaRelacionado(int shoeId);
    
        void AsignarSizeAShoe(SizeShoe nuevoSizeShoe);
        List<ShoeDto> GetListaDto();
        IEnumerable<ShoeDto> GetShoesFiltradosPorBrandYColour(int brandId, int colourId);
        IEnumerable<IGrouping<int, Shoe>> GetShoesPorMarcaEntreRangoPrecios(decimal rangoMin, decimal rangoMax);
    }
}
