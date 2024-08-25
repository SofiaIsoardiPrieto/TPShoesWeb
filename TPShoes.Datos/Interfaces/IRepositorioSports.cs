using TPShoes.Entidades.Clases;

namespace TPShoes.Datos.Interfaces
{
    public interface IRepositorioSports : IRepositorioGenerico<Sport>
	{
      
        void Editar(Sport sport);
        bool EstaRelacionado(Sport sport);
        bool Existe(Sport sport);
        Sport GetSportPorNombre(string sportNombre);
        void SaveChanges();
        int GetCantidad();
    }

}
