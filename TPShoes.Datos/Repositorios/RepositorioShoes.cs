using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TPShoes.Datos.Interfaces;
using TPShoes.Entidades.Clases;
using TPShoes.Entidades.Dtos;
using TPShoes.Entidades.Enum;

namespace TPShoes.Datos.Repositorios
{
	public class RepositorioShoes : RepositorioGenerico<Shoe>, IRepositorioShoes
    {
        private readonly DBContextShoes _db;

        public RepositorioShoes(DBContextShoes db) : base(db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public bool Existe(Shoe shoe)
        {
            try
            {
                if (shoe.ShoeId == 0)
                {
                    return _db.Shoes
                        .Any(s => s.Model == shoe.Model && s.BrandId == shoe.BrandId && s.ColourId == shoe.ColourId
                        && s.GenreId == shoe.GenreId && s.SportId == shoe.SportId);
                }
                return _db.Shoes
                    .Any(s => s.Model == shoe.Model && s.BrandId == shoe.BrandId && s.ColourId == shoe.ColourId
                        && s.GenreId == shoe.GenreId && s.SportId == shoe.SportId && s.ShoeId != shoe.ShoeId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetCantidad(Expression<Func<Shoe, bool>>? filtro = null)
        {
            int cantidad;

            if (filtro != null)
            {
                cantidad = _db.Shoes.Count(filtro);
            }
            else
            {
                cantidad = _db.Shoes.Count();

            }
            return cantidad;
        }
        public List<Shoe> GetLista()
        {
            var shoesQuery = _db.Shoes
                .Include(s => s.Brand)
                .Include(s => s.Colour)
                .Include(s => s.Genre)
                 .Include(s => s.Sport)
                .AsNoTracking();

            return shoesQuery.ToList(); ;
        }
        public List<ShoeDto> GetListaPaginadaOrdenadaFiltrada
          (int registrosPorPagina, int paginaActual,
          Orden? orden = null, Brand? BrandFiltro = null,
          Colour? ColourFiltro = null, Expression<Func<Shoe, bool>>? rangoPrecio = null)
        {
            IQueryable<Shoe> query = _db.Shoes
                .Include(s => s.Brand)
                .Include(s => s.Colour)
                .Include(s => s.Genre)
                .Include(s => s.Sport)
                .OrderBy(s => s.ShoeId)
                .AsNoTracking();

            // Aplicar filtro si se proporciona un brand
            if (BrandFiltro is not null)
            {
                query = query
                    .Where(s => s.BrandId == BrandFiltro.BrandId);
            }

            // Aplicar filtro si se proporciona un colour
            if (ColourFiltro is not null)
            {
                query = query
                    .Where(s => s.ColourId == ColourFiltro.ColourId);
            }
            if (rangoPrecio is not null)
            {
                query = query.Where(rangoPrecio);
            }
            // Aplicar orden si se proporciona
            if (orden != null)
            {
                switch (orden)
                {
                    case Orden.AZ:
                        query = query.OrderBy(p => p.Model);
                        break;
                    case Orden.ZA:
                        query = query.OrderByDescending(p => p.Model);
                        break;
                    case Orden.MenorPrecio:
                        query = query.OrderBy(p => p.Price);
                        break;
                    case Orden.MayorPrecio:
                        query = query.OrderByDescending(p => p.Price);
                        break;
                    default:
                        break;
                }
            }

            //// Cambio en el paginado, se quitó el -1 y take(paginaAtual)
            //List<Shoe> listaPaginada = query
            //.AsNoTracking()

            //    .Skip(registrosPorPagina * (paginaActual))
            //    .Take(paginaActual)
            //    .ToList();

            // Paginación correcta
            List<Shoe> listaPaginada = query
                .Skip(registrosPorPagina * (paginaActual - 1)) // Restar 1 para obtener el índice correcto
                .Take(registrosPorPagina) // Tomar la cantidad de registros por página
                .ToList();

            List<ShoeDto> listaDto = listaPaginada
                .Select(p => new ShoeDto
                {
                    ShoeId = p.ShoeId,
                    Brand = p.Brand?.BrandName ?? "N/A",
                    Genre = p.Genre?.GenreName ?? "N/A",
                    Colour = p.Colour?.ColourName ?? "N/A",
                    Sport = p.Sport?.SportName ?? "N/A",
                    Description = p.Description,
                    Price = p.Price,
                    Model = p.Model
                })
                .ToList();

            return listaDto;
        }


        public IEnumerable<IGrouping<int, Shoe>> GetShoesAgrupadosPorGenre()
        {
            return _db.Shoes.GroupBy(s => s.GenreId)
               .ToList();
        }
        public IEnumerable<IGrouping<int, Shoe>> GetShoesPorMarcaEntreRangoPrecios(decimal rangoMin, decimal rangoMax)
        {
            return _db.Shoes
                .Include(s => s.Brand)
                .Include(s => s.Genre)
                .Include(s => s.Colour)
                .Include(s => s.Sport)
                .Where(s => s.Price >= rangoMin && s.Price <= rangoMax)  // Filtrar por rango de precios
                .GroupBy(s => s.BrandId)
                .ToList();
        }

        public IEnumerable<IGrouping<int, Shoe>> GetShoesAgrupadosPorSport()
        {
            return _db.Shoes.GroupBy(s => s.SportId).ToList();
        }

        public void Agregar(Shoe shoe)
        {
            // Verificar si brand asociado a la shoe ya existe en la base de datos:

            var brandExistente = _db.Brands
            .FirstOrDefault(t => t.BrandId == shoe.BrandId);

            // Si brand ya existe, adjuntarlo al contexto en lugar de agregarlo nuevamente:
            if (brandExistente != null)
            {
                _db.Attach(brandExistente);
                shoe.Brand = brandExistente;
            }

            //Genre
            var genreExistente = _db.Genres
            .FirstOrDefault(t => t.GenreId == shoe.GenreId);
            if (genreExistente != null)
            {
                _db.Attach(genreExistente);
                shoe.Genre = genreExistente;
            }

            //Colour
            var colourExistente = _db.Colours
            .FirstOrDefault(t => t.ColourId == shoe.ColourId);
            if (colourExistente != null)
            {
                _db.Attach(colourExistente);
                shoe.Colour = colourExistente;
            }

            //Sport
            var sportExistente = _db.Sports
            .FirstOrDefault(t => t.SportId == shoe.SportId);
            if (sportExistente != null)
            {
                _db.Attach(sportExistente);
                shoe.Sport = sportExistente;
            }

            // Agregar la planta al contexto de la base de datos
            _db.Shoes.Add(shoe);
        }

        public void Editar(Shoe shoe)
        {
            var brandExistente = _db.Brands
               .FirstOrDefault(t => t.BrandId == shoe.BrandId);

            if (brandExistente != null)
            {
                _db.Attach(brandExistente);
                shoe.Brand = brandExistente;
            }

            var sportExistente = _db.Sports
                .FirstOrDefault(t => t.SportId == shoe.SportId);
            if (sportExistente != null)
            {
                _db.Attach(sportExistente);
                shoe.Sport = sportExistente;
            }

            // Verificar si el TipoDeGenero asociado
            // al shoe ya existe en la base de datos
            var GenreExistente = _db.Genres
              .FirstOrDefault(t => t.GenreId == shoe.GenreId);
            if (GenreExistente != null)
            {
                _db.Attach(GenreExistente);
                shoe.Genre = GenreExistente;
            }
            // Verificar si el TipoDeColor asociado
            // al shoe ya existe en la base de datos
            var colourExistente = _db.Colours
              .FirstOrDefault(t => t.ColourId == shoe.ColourId);
            if (colourExistente != null)
            {
                _db.Attach(colourExistente);
                shoe.Colour = colourExistente;
            }

            // Agregar la planta al contexto de la base de datos
            _db.Shoes.Update(shoe);
        }

        public Shoe? GetShoePorId(int shoeId)
        {
            //el problema viena de aca!!!!
            Shoe? shoe =
             _db?.Shoes
                .Include(p => p.Brand)   // Propiedad de navegación
                .Include(p => p.Genre)   // Propiedad de navegación
                .Include(p => p.Sport)   // Propiedad de navegación
                .Include(p => p.Colour)  // Propiedad de navegación
                .FirstOrDefault(p => p.ShoeId == shoeId);

            return shoe;
        }


        public void EliminarRelaciones(Shoe shoe)
        {
            var relacionesPasadas = _db.Shoes
              .Where(pp => pp.ShoeId == shoe.ShoeId)
              .ToList();

            _db.Shoes
                .RemoveRange(relacionesPasadas);
        }

        public void Borrar(Shoe shoe)
        {
            _db.Shoes.Remove(shoe);
        }

        public void AsignarSizeAShoe(SizeShoe nuevoSizeShoe)
        {
            _db.Set<SizeShoe>().Add(nuevoSizeShoe);
        }

        public List<ShoeDto> GetListaDto()
        {
            var query = _db.Shoes
               .Include(p => p.Brand)
               .Include(p => p.Colour)
               .Include(p => p.Genre)
               .Include(p => p.Sport)
               .AsNoTracking()
               .ToList();

            List<ShoeDto> listaDto = query
               .Select(p => new ShoeDto
               {
                   ShoeId = p.ShoeId,
                   Brand = p.Brand?.BrandName ?? "N/A",
                   Genre = p.Genre?.GenreName ?? "N/A",
                   Colour = p.Colour?.ColourName ?? "N/A",
                   Sport = p.Sport?.SportName ?? "N/A",
                   Description = p.Description,
                   Price = p.Price,
                   Model = p.Model
               })
               .ToList();

            return listaDto;
        }

        public IEnumerable<ShoeDto> GetShoesFiltradosPorBrandYColour(int brandId, int colourId)
        {

            try
            {
                return _db.Shoes
                    .Where(s => s.BrandId == brandId && s.ColourId == colourId)
                    .Select(s => new ShoeDto
                    {
                        ShoeId = s.ShoeId,
                        Brand = s.Brand.BrandName,
                        Colour = s.Colour.ColourName,
                        Sport = s.Sport.SportName,
                        Genre = s.Genre.GenreName,
                        Model = s.Model,
                        Price = s.Price,
                        Description = s.Description
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los Shoes filtrados.", ex);
            }
        }

        public bool EstaRelacionado(int shoeId)
        {
            return _db.SizeShoes.Any(p => p.ShoeId == shoeId);
        }
    }
}

