using TPShoes.Entidades.Clases;

namespace TPShoes.Datos.Interfaces
{
    public interface IRepositorioColours : IRepositorioGenerico<Colour>
	{
        
        void Editar(Colour colour);
        bool EstaRelacionado(Colour colour);
        bool Existe(Colour colour);      
        Colour GetColourPorNombre(string colourNombre);
        void SaveChanges();
        int GetCantidad();
    }

}
