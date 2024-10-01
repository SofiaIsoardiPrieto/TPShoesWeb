using System.Linq.Expressions;
using TPShoes.Datos;
using TPShoes.Datos.Interfaces;
using TPShoes.Entidades.Clases;
using TPShoes.Entidades.Dtos;
using TPShoes.Servicios.Interfaces;

namespace TPShoes.Servicios.Servicios
{
	public class SizesServicio : ISizesServicio
	{

		private readonly IRepositorioSizes _repository;
		private readonly IUnitOfWork _unitOfWork;

		public SizesServicio(IRepositorioSizes repository,
			IUnitOfWork unitOfWork)
		{
			_repository = repository ?? throw new ArgumentException("Error en la dependencia");
			_unitOfWork = unitOfWork ?? throw new ArgumentException("Error en la dependencia");
		}

		public void Borrar(SizeShoe sizeShoe)
		{
			try
			{
				_unitOfWork.BeginTransaction();
				_repository.Borrar(sizeShoe);
				_unitOfWork.Commit();
			}
			catch (Exception)
			{
				_unitOfWork.Rollback();
				throw;
			}
		}

		public void Borrar(Size size)
		{
			try
			{
				_unitOfWork.BeginTransaction();
				_repository.Delete(size);
				_unitOfWork.Commit();
			}
			catch (Exception)
			{
				_unitOfWork.Rollback();
				throw;
			}
		}

		public bool EstaRelacionado(SizeShoe sizeShoe)
		{
			try
			{
				return _repository.EstaRelacionado(sizeShoe);
			}
			catch (Exception)
			{

				throw;
			}
		}

		public bool EstaRelacionado(Size size)
		{
			throw new NotImplementedException();
		}

		public bool Existe(Size size)
		{
			try
			{
				return _repository.Existe(size);
			}
			catch (Exception)
			{

				throw;
			}
		}

		public bool Existe(SizeShoe sizeShoeEditado)
		{
			throw new NotImplementedException();
		}



		public int GetCantidad()
		{
			try
			{
				return _repository.GetCantidad();
			}
			catch (Exception)
			{

				throw;
			}
		}

		public IEnumerable<Size>? GetLista(Expression<Func<Size, bool>>? filter = null,
		   Func<IQueryable<Size>, IOrderedQueryable<Size>>? orderBy = null,
		   string? propertiesNames = null)
		{
			return _repository.GetAll(filter, orderBy, propertiesNames);
		}

		public Size? GetSizePorId(Expression<Func<Size, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
		{
			return _repository.Get(filter, propertiesNames, tracked);
		}

		public Size GetSizePorId(int sizeId)
		{
			return _repository.GetSizePorId(sizeId);

        }

		public List<SizeShoeDto>? GetSizeShoeDtoPorId(int shoeId)
		{
			try
			{
				return _repository.GetSizeShoeDtoPorId(shoeId);
			}
			catch (Exception)
			{

				throw;
			}
		}

		public SizeShoe? GetSizeShoePorId(int sizeShoeId)
		{
			try
			{
				return _repository.GetSizeShoePorId(sizeShoeId);
			}
			catch (Exception)
			{

				throw;
			}
		}

		public List<Size>? GetSizesNoAsociadosPorShoeId(int shoeId)
		{
			try
			{
				return _repository.GetSizesNoAsociadosPorShoeId(shoeId);
			}
			catch (Exception)
			{

				throw;
			}
		}

		public List<Size> GetSizesPorId(int shoeId, bool incluyeShoe = false)
		{
			try
			{
				return _repository.GetSizesPorId(shoeId, incluyeShoe);
			}
			catch (Exception)
			{

				throw;
			}
		}

		public List<Size> GetSizesPorId(Expression<Func<Size, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
		{
			throw new NotImplementedException();
		}

		public void Guardar(Size size)
		{
			try
			{
				_unitOfWork.BeginTransaction();
				if (size.SizeId == 0)
				{
					if (!_repository.Existe(size))
					{
						_repository.Add(size);
					}
					else
					{
						throw new Exception("El Size ya existe.");
					}
				}
				else
				{
					_repository.Editar(size);
				}
				_unitOfWork.Commit();
			}
			catch (Exception)
			{
				_unitOfWork.Rollback();
				throw;
			}
		}

		public void Guardar(SizeShoe sizeShoe)
		{
			try
			{
				_unitOfWork.BeginTransaction();
				if (sizeShoe.SizeId == 0)
				{
					if (!_repository.Existe(sizeShoe))
					{
						_repository.AgregarSizeShoe(sizeShoe);
					}
					else
					{
						throw new Exception("El sizeShoe ya existe.");
					}
				}
				else
				{
					_repository.Editar(sizeShoe);
				}
				_unitOfWork.Commit();
			}
			catch (Exception)
			{
				_unitOfWork.Rollback();
				throw;
			}
		}



	}
}
