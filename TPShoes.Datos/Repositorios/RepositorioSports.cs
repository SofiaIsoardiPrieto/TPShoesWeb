using TPShoes.Datos.Interfaces;
using TPShoes.Entidades.Clases;

namespace TPShoes.Datos.Repositorios
{
	public class RepositorioSports : RepositorioGenerico<Sport>, IRepositorioSports
	{
		private readonly DBContextShoes _db;

		public RepositorioSports(DBContextShoes db) : base(db)
		{
			_db = db ?? throw new ArgumentNullException(nameof(db));
		}
		public void Editar(Sport sport)
		{
			_db.Update(sport);
		}
		public int GetCantidad()
		{
			return _db.Sports.Count();
		}
		public bool EstaRelacionado(Sport sport)
		{
			return _db
			.Shoes
				.Any(c => c.SportId == sport.SportId);
		}
		public bool Existe(Sport sport)
		{
			if (sport.SportId == 0)
			{
				return _db.Sports
					.Any(co => co.SportName == sport.SportName);
			}
			return _db.Sports
				.Any(co => co.SportName == sport.SportName &&
				co.SportId != sport.SportId);
		}
		public Sport? GetSportPorNombre(string sportNombre)
		{
			return _db.Sports
				.FirstOrDefault(co => co.SportName == sportNombre);
		}
		public void SaveChanges()
		{
			_db.SaveChanges();
		}
	}

}
