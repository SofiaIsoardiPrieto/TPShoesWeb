using AutoMapper;
using TPShoes.Entidades.Clases;
using TPShoes.Entidades.ViewModels.Brand;
using TPShoes.Entidades.ViewModels.Colour;
using TPShoes.Entidades.ViewModels.Genre;
using TPShoes.Entidades.ViewModels.Shoe;
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
            LoadShoeMapping();
        }

        private void LoadBrandMapping()
        {
            CreateMap<Brand, BrandListVm>();
            CreateMap<Brand, BrandEditVm>().ReverseMap();

            //CreateMap<City, CityListVm>().
            //	ForMember(dest => dest.CountryName,
            //	opt => opt.MapFrom(c => c.Country.CountryName))
            //	.ForMember(dest => dest.StateName,
            //	opt => opt.MapFrom(s => s.State.StateName));
            //CreateMap<City, CityEditVm>();
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

            //CreateMap<State, StateListVm>()
            //	.ForMember(dest => dest.Country,
            //	opt => opt.MapFrom(src => src.Country.CountryName));
            //CreateMap<State, StateEditVm>().ReverseMap();
        }
        private void LoadShoeMapping()
        {
            CreateMap<Shoe, ShoeListVm>()
                 .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.BrandName))
                 .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.GenreName))
                 .ForMember(dest => dest.Colour, opt => opt.MapFrom(src => src.Colour.ColourName))
             .ForMember(dest => dest.Sport, opt => opt.MapFrom(src => src.Sport.SportName));
            CreateMap<Shoe, ShoeEditVm>().ReverseMap();

        }
    }
}
