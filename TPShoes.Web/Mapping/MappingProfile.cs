using AutoMapper;
using TPShoes.Entidades.Clases;
using TPShoes.Entidades.ViewModels.Brand;
using TPShoes.Entidades.ViewModels.Colour;
using TPShoes.Entidades.ViewModels.Genre;
using TPShoes.Entidades.ViewModels.Shoe;
using TPShoes.Entidades.ViewModels.Size;
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
            CreateMap<Shoe, ShoeListVm>()
                 .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.BrandName))
                 .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.GenreName))
                 .ForMember(dest => dest.Colour, opt => opt.MapFrom(src => src.Colour.ColourName))
                 .ForMember(dest => dest.Sport, opt => opt.MapFrom(src => src.Sport.SportName));
            CreateMap<Shoe, ShoeEditVm>().ReverseMap();
            CreateMap<Shoe, ShoeHomeIndexVm>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.BrandName))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.GenreName))
                .ForMember(dest => dest.Colour, opt => opt.MapFrom(src => src.Colour.ColourName))
                .ForMember(dest => dest.Sport, opt => opt.MapFrom(src => src.Sport.SportName))
                .ForMember(dest => dest.FinalPrice, opt => opt.MapFrom(src => src.Price * 1.21m));
          
        }
    }
}
