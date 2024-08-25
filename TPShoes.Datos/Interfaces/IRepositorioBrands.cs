using TPShoes.Entidades.Clases;

namespace TPShoes.Datos.Interfaces
{
	public interface IRepositorioBrands:IRepositorioGenerico<Brand>
	{     
        void Editar(Brand brand);
        bool EstaRelacionado(Brand brand);
        bool Existe(Brand brand);   
        Brand GetBrandPorNombre(string brandNombre);
        int GetCantidad();
        void SaveChanges();
    }

}
