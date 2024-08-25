using Microsoft.EntityFrameworkCore;
using TPShoes.Datos.Interfaces;
using TPShoes.Entidades.Clases;

namespace TPShoes.Datos.Repositorios
{
    public class RepositorioGenres : RepositorioGenerico<Genre>, IRepositorioGenres
    {
        private readonly DBContextShoes _db;

        public RepositorioGenres(DBContextShoes db) : base(db)
		{
            _db = db ?? throw new ArgumentNullException(nameof(db));
		}
   

        public void Editar(Genre genre)
        {
            _db.Update(genre);
        }

        public bool EstaRelacionado(Genre genre)
        {
            return _db
            .Shoes
                .Any(c => c.GenreId == genre.GenreId);
        }
        public int GetCantidad()
        {
            return _db.Genres.Count();
        }

        public bool Existe(Genre genre)
        {
            if (genre.GenreId == 0)
            {
                return _db.Genres
                    .Any(co => co.GenreName == genre.GenreName);
            }
            return _db.Genres
                .Any(co => co.GenreName == genre.GenreName &&
                co.GenreId != genre.GenreId);
        }
        public Genre? GetGenrePorNombre(string genreNombre)
        {
            return _db.Genres
                .FirstOrDefault(co => co.GenreName == genreNombre);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }

}
