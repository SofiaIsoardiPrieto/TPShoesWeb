using TPShoes.Entidades.Clases;

namespace TPShoes.Datos.Interfaces
{
    public interface IRepositorioGenres : IRepositorioGenerico<Genre>
	{
      
        void Editar(Genre genre);
        bool EstaRelacionado(Genre genre);
        bool Existe(Genre genre);    
        Genre GetGenrePorNombre(string genreNombre);
        void SaveChanges();
        int GetCantidad();
    }

}
