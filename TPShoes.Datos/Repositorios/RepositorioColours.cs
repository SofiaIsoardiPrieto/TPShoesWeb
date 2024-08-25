using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TPShoes.Datos.Interfaces;
using TPShoes.Entidades;
using TPShoes.Entidades.Clases;

namespace TPShoes.Datos.Repositorios
{
    public class RepositorioColours : RepositorioGenerico<Colour>, IRepositorioColours
    {
        private readonly DBContextShoes _db;

        public RepositorioColours(DBContextShoes db) : base(db)
		{
            _db = db ?? throw new ArgumentNullException(nameof(db));

		}
        public void Editar(Colour colour)
        {
            _db.Update(colour);
        }
        public bool EstaRelacionado(Colour colour)
        {
            return _db
            .Shoes
                .Any(c => c.ColourId == colour.ColourId);
        }
        public int GetCantidad()
        {
            return _db.Colours.Count();
        }
        public bool Existe(Colour colour)
        {
            if (colour.ColourId == 0)
            {
                return _db.Colours
                    .Any(co => co.ColourName == colour.ColourName);
            }
            return _db.Colours
                .Any(co => co.ColourName == colour.ColourName &&
                co.ColourId != colour.ColourId);
        }
        public Colour? GetColourPorNombre(string colourNombre)
        {
            return _db.Colours
                .FirstOrDefault(co => co.ColourName == colourNombre);
        }
        public void SaveChanges()
        {
            _db.SaveChanges();
        }
	}

}
