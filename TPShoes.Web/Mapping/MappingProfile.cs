using AutoMapper;
using TPShoes.Entidades.Clases;
using TPShoes.Entidades.Dtos;
using TPShoes.Entidades.ViewModels.Brand;
using TPShoes.Entidades.ViewModels.Colour;
using TPShoes.Entidades.ViewModels.Genre;
using TPShoes.Entidades.ViewModels.Shoe;
using TPShoes.Entidades.ViewModels.Size;
using TPShoes.Entidades.ViewModels.SizeShoe;
using TPShoes.Entidades.ViewModels.Sport;

namespace TPShoes.Web.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            LoadBrandMapping();
            LoadGenreMapping();
            LoadColourMapping();
            LoadSportMapping();
            LoadShoeMapping();
            LoadSizeShoeMapping();
            LoadSizeMapping();
        }

        private void LoadSizeShoeMapping()
        {
            CreateMap<SizeShoe, SizeShoeListVm>().ReverseMap();
            CreateMap<SizeShoe, SizeShoeEditVm>().ReverseMap();
        }

        private void LoadSizeMapping()
        {
            CreateMap<Size, SizeListVm>();

        }

        private void LoadBrandMapping()
        {
            CreateMap<Brand, BrandListVm>();
            CreateMap<Brand, BrandEditVm>().ReverseMap();
        }
        private void LoadGenreMapping()
        {
            CreateMap<Genre, GenreListVm>();
            CreateMap<Genre, GenreEditVm>().ReverseMap();
        }
        private void LoadColourMapping()
        {
            CreateMap<Colour, ColourListVm>();
            CreateMap<Colour, ColourEditVm>().ReverseMap();
        }

        private void LoadSportMapping()
        {
            CreateMap<Sport, SportListVm>();
            CreateMap<Sport, SportEditVm>().ReverseMap();
        }
        private void LoadShoeMapping()
        {
            // Mapeo para listado general
            CreateMap<Shoe, ShoeListVm>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.BrandName))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.GenreName))
                .ForMember(dest => dest.Colour, opt => opt.MapFrom(src => src.Colour.ColourName))
                .ForMember(dest => dest.Sport, opt => opt.MapFrom(src => src.Sport.SportName));

            // Mapeo para edición
            CreateMap<Shoe, ShoeEditVm>().ReverseMap();

            // Mapeo para el Home Index
            CreateMap<Shoe, ShoeHomeIndexVm>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.BrandName))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.GenreName))
                .ForMember(dest => dest.Colour, opt => opt.MapFrom(src => src.Colour.ColourName))
                .ForMember(dest => dest.Sport, opt => opt.MapFrom(src => src.Sport.SportName))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Brand.ImageUrl))
                .ForMember(dest => dest.FinalPrice, opt => opt.MapFrom(src => src.Price * 1.21m));

            // Mapeo de la tabla intermedia SizeShoe hacia el VM
            CreateMap<SizeShoe, SizeShoeListVm>()
                .ForMember(dest => dest.SizeShoeId, opt => opt.MapFrom(src => src.SizeShoeId))
                .ForMember(dest => dest.ShoeId, opt => opt.MapFrom(src => src.ShoeId))
                .ForMember(dest => dest.SizeId, opt => opt.MapFrom(src => src.SizeId))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
                .ForMember(dest => dest.SizeNumber, opt => opt.MapFrom(src => src.Size.SizeNumber));

            // Mapeo para el Home Details
            CreateMap<Shoe, ShoeHomeDetailsVm>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.BrandName))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.GenreName))
                .ForMember(dest => dest.Colour, opt => opt.MapFrom(src => src.Colour.ColourName))
                .ForMember(dest => dest.Sport, opt => opt.MapFrom(src => src.Sport.SportName))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Brand.ImageUrl))
                .ForMember(dest => dest.FinalPrice, opt => opt.MapFrom(src => src.Price * 1.21m))
                .ForMember(dest => dest.Sizes, opt => opt.MapFrom(src => src.SizeShoe)); // ahora sí mapea la colección
        }

    }
}
