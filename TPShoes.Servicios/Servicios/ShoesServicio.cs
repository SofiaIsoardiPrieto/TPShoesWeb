﻿using System.Linq.Expressions;
using TPShoes.Datos;
using TPShoes.Datos.Interfaces;
using TPShoes.Entidades;
using TPShoes.Entidades.Clases;
using TPShoes.Entidades.Dtos;
using TPShoes.Entidades.Enum;
using TPShoes.Servicios.Interfaces;

namespace TPShoes.Servicios.Servicios
{
    public class ShoesServicio : IShoesServicio
    {
        private readonly IRepositorioShoes _repository;
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IProveedoresRepository _proveedorRepository;

        public ShoesServicio(IRepositorioShoes repository, IUnitOfWork unitOfWork)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _unitOfWork = unitOfWork;
            //_proveedorRepository = proveedorRepository;
        }

        public void Borrar(int shoeId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var shoe = _repository.GetShoePorId(shoeId) ?? throw new Exception("El Shoe especificado no existe.");

                _repository.EliminarRelaciones(shoe);
               // _unitOfWork.SaveChanges();


                _repository.Borrar(shoe);
                //error aqui
               // _unitOfWork.SaveChanges();

                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public bool Existe(Shoe shoe)
        {
            return _repository.Existe(shoe);
        }

        public int GetCantidad(Expression<Func<Shoe, bool>>? filtro = null)
        {
            try
            {
                return _repository.GetCantidad(filtro);
            }
            catch (Exception ex)
            {

                throw new Exception("Habilita el servidor!.", ex);
            }
        }
        public List<Shoe> GetLista()
        {
            return _repository.GetLista();
        }

        public List<ShoeDto> GetListaPaginadaOrdenadaFiltrada(int registrosPorPagina,
            int paginaActual, Orden? orden = null, Brand? BrandFiltro = null,
            Colour? ColourFiltro = null, Expression<Func<Shoe, bool>>? rangoPrecio = null)
        {
            return _repository.GetListaPaginadaOrdenadaFiltrada( registrosPorPagina,paginaActual,
                orden, BrandFiltro, ColourFiltro,rangoPrecio);
        }

        public Shoe GetShoePorId(int shoeId)
        {
            return _repository.GetShoePorId(shoeId);
        }

        public IEnumerable<IGrouping<int, Shoe>> GetShoesAgrupadosPorGenre()
        {
            return _repository.GetShoesAgrupadosPorGenre();
        }

        public IEnumerable<IGrouping<int, Shoe>> GetShoesAgrupadosPorSport()
        {
            return _repository.GetShoesAgrupadosPorSport();
        }

        public void Guardar(Shoe shoe)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                if (shoe.ShoeId == 0)
                {
                    _repository.Agregar(shoe);
                    _unitOfWork.SaveChanges(); // Guardar cambios para obtener el id de la planta agregada

                    
                }
                else
                {
                    _repository.Editar(shoe);
                    _unitOfWork.SaveChanges(); // Guardar cambios de la planta antes de manejar relaciones

                }
                _unitOfWork.SaveChanges(); // Guardar todos los cambios al final
                _unitOfWork.Commit(); // Confirmar los cambios
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public void Editar(Shoe shoe, int? sizeId = null)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                _repository.Editar(shoe);

                //if (sizeId.HasValue)
                //{
                //    // Buscar el proveedor
                //    var proveedor = _proveedorRepository
                //        .GetProveedorPorId(sizeId.Value);
                //    if (proveedor != null)
                //    {
                //        // Crear la nueva relación si no existe
                //        if (!shoe.ProveedoresPlantas
                //            .Any(pp => pp.ProveedorId == sizeId))
                //        {
                //            var nuevaRelacion = new ProveedorPlanta
                //            {
                //                PlantaId = shoe.PlantaId,
                //                ProveedorId = proveedor.ProveedorId
                //            };
                //            _proveedorRepository.AgregarProveedorPlanta(nuevaRelacion);

                //        }
                //    }
                //    else
                //    {
                //        throw new Exception("Proveedor no encontrado.");
                //    }
                //}

                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public bool ExisteRelacion(Shoe shoe, Size size)
        {
            return _repository.ExisteRelacion(shoe, size);
        }

        public void AsignarSizeAShoe(Shoe shoe, Size size)
        {
            try
            {
                _unitOfWork.BeginTransaction();


                // Crear una nueva relación entre la shoe y el size
                SizeShoe nuevoSizeShoe = new SizeShoe
                {
                    
                    Shoe = shoe,
                    Size = size
                };
                _repository.AsignarSizeAShoe(nuevoSizeShoe);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }

        }

        public List<ShoeDto> GetListaDto()
        {
            return _repository.GetListaDto();
        }

        public IEnumerable<ShoeDto> GetShoesFiltradosPorBrandYColour(int brandId, int colourId)
        {
            try
            {
                return _repository.GetShoesFiltradosPorBrandYColour(brandId, colourId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<IGrouping<int, Shoe>> GetShoesPorMarcaEntreRangoPrecios(decimal rangoMin, decimal rangoMax)
        {

            try
            {
                return _repository.GetShoesPorMarcaEntreRangoPrecios(rangoMin, rangoMax);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

}
