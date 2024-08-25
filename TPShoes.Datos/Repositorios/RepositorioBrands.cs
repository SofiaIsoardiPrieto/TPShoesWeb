using Microsoft.EntityFrameworkCore;
using TPShoes.Datos.Interfaces;
using TPShoes.Entidades.Clases;

namespace TPShoes.Datos.Repositorios
{
	public class RepositorioBrands : RepositorioGenerico<Brand>, IRepositorioBrands
	{

        private readonly DBContextShoes _db;

        public RepositorioBrands(DBContextShoes db) : base(db)
		{
            _db = db ?? throw new ArgumentNullException(nameof(db)); 
        }
        public void Editar(Brand brand)
        {
            _db.Update(brand);
        }
     
        public bool EstaRelacionado(Brand brand)
        {
            return _db
                .Shoes
                .Any(p => p.BrandId == brand.BrandId);
        }
        public bool Existe(Brand brand)
        {
            if (brand.BrandId == 0)
            {
                return _db.Brands
                    .Any(br => br.BrandName == brand.BrandName);
            }
            return _db.Brands
                .Any(br => br.BrandName == brand.BrandName &&
                br.BrandId != brand.BrandId);
        }  
        public Brand? GetBrandPorNombre(string brandNombre)
        {
            return _db.Brands
                .FirstOrDefault(br => br.BrandName == brandNombre);
        }

        public int GetCantidad()
        {
            return _db.Brands.Count();
        }
        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }

}
