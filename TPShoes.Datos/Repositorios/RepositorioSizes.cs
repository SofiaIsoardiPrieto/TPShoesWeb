using Microsoft.EntityFrameworkCore;
using TPShoes.Datos.Interfaces;
using TPShoes.Entidades.Clases;
using TPShoes.Entidades.Dtos;
using TPShoes.Entidades.Enum;

namespace TPShoes.Datos.Repositorios
{
    public class RepositorioSizes : RepositorioGenerico<Size>, IRepositorioSizes
    {
        private readonly DBContextShoes _db;

        public RepositorioSizes(DBContextShoes db) : base(db)
		{
            _db = db ?? throw new ArgumentNullException(nameof(db));
		}
        public void Agregar(Entidades.Clases.Size sizes)
        {

            _db.Add(sizes);
        }

        public void AgregarSizeShoe(SizeShoe nuevaRelacion)
        {
            _db.Set<SizeShoe>().Add(nuevaRelacion);
        }


        public void Borrar(Entidades.Clases.Size sizes)
        {
            _db.Remove(sizes);
        }

        public void Borrar(SizeShoe sizeShoe)
        {
            throw new NotImplementedException();
        }

        public void Editar(Entidades.Clases.Size sizes)
        {
            _db.Update(sizes);
        }

        public void Editar(SizeShoe sizeShoe)
        {
            throw new NotImplementedException();
        }

        public bool EstaRelacionado(Entidades.Clases.Size sizes)
        {
            return _db.Sizes.Any(ss => ss.SizeId == sizes.SizeId);
        }

        public bool EstaRelacionado(SizeShoe size)
        {
            throw new NotImplementedException();
        }

        public bool Existe(Entidades.Clases.Size sizes)
        {
            return _db.Sizes.
                 Any(s => s.SizeId == sizes.SizeNumber
                 && s.SizeId != sizes.SizeId);
        }

        public bool Existe(SizeShoe sizeShoe)
        {
            throw new NotImplementedException();
        }

        public int GetCantidad()
        {
            return _db.Sizes.Count();
        }

        public List<Size> GetLista()
        {
            try
            {
                // Utilizando Entity Framework para obtener todos los registros de la tabla Sizes
                return _db.Sizes.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                throw new Exception("Error al obtener la lista de Sizes.", ex);
            }
        }

        public Size? GetSizePorId(int sizeId)
        {
            try
            {
                // Utilizando Entity Framework para buscar el Size por su ID
                var size = _db.Sizes
                    .FirstOrDefault(s => s.SizeId == sizeId);

                return size;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                throw new Exception("Error al obtener el Size por ID.", ex);
            }
        }

        public List<SizeShoeDto>? GetSizeShoeDtoPorId(int shoeId)
        {
            throw new NotImplementedException();
        }

        public SizeShoe? GetSizeShoePorId(int sizeShoeId)
        {
            throw new NotImplementedException();
        }

        public List<Size>? GetSizesNoAsociadosPorShoeId(int shoeId)
        {
            var sizeIdsAsociados = _db.SizeShoes
            .Where(ss => ss.ShoeId == shoeId)
            .Select(ss => ss.SizeId)
            .ToList();

            // Obtenemos todos los Size que no están en la lista de asociados
            var sizesNoAsociados = _db.Sizes
                .Where(s => !sizeIdsAsociados.Contains(s.SizeId))
                .ToList();

            return sizesNoAsociados;
        }

        public List<Entidades.Clases.Size> GetSizesPorId(int shoeId, bool incluyeShoe = false)
        {
            IQueryable<Entidades.Clases.Size> query = _db.Sizes;

            if (incluyeShoe)
            {
                return query
                    .Include(s => s.SizeShoe)
                    .ThenInclude(ss => ss.Shoe)
                    .Where(s => s.SizeShoe.Any(ss => ss.ShoeId == shoeId))
                    .ToList();
            }

            return query
                .Where(s => s.SizeShoe.Any(ss => ss.ShoeId == shoeId))
                .ToList();
        }

    }
}
