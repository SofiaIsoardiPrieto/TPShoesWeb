﻿using TPShoes.Entidades.Clases;
using TPShoes.Entidades.Dtos;
using TPShoes.Entidades.Enum;
using Size = TPShoes.Entidades.Clases.Size;

namespace TPShoes.Datos.Interfaces
{
    public interface IRepositorioSizeShoes
    {
        bool ExisteRelacion(Shoe shoe, Size size);
        void Agregar(Size size);
        void AgregarSizeShoe(SizeShoe sizeShoe);
        void Borrar(Size size);
        void Editar(Size size);
        bool Existe(SizeShoe sizeShoe);
        void Borrar(SizeShoe sizeShoe);
        void Editar(SizeShoe sizeShoe);
        bool Existe(Size size);
        //  List<Size> GetLista();
        List<Size> GetSizesPorId(int shoeId, bool incluyeShoe = false);
        List<SizeShoe>? GetSizeShoesPorId(int shoeId);
      //  int GetCantidad();
      //  List<Size> GetSizesPaginadosOrdenados(int page, int pageSize, Orden? orden = null);
        bool EstaRelacionado(Size size);
        bool EstaRelacionado(SizeShoe size);
        int GetCantidad();
        List<SizeShoeDto>? GetSizeShoeDtoPorId(int shoeId);
        SizeShoe? GetSizeShoePorId(int sizeShoeId);
        void EditarSizeShoe(SizeShoe sizeShoe);
        SizeShoe GetSizeShoePorId(int shoeId, int sizeId);
        List<ShoeDto> GetListaShoeDtoPorSize(int sizeIdSeleccionado);
        List<Shoe> GetListaShoePorSize(int value);
    }
}
