using AutoMapper;
using TPShoes.Entidades.Clases;
using TPShoes.Entidades.ViewModels;

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
		}

		private void LoadBrandMapping()
		{
			CreateMap<Brand, BrandEditVm>();
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
			//CreateMap<Country, CountryListVm>();
			//CreateMap<Country, CountryEditVm>().ReverseMap();
		}

		private void LoadColourMapping()
		{
			//CreateMap<Category, CategoryListVm>();
			//CreateMap<Category, CategoryEditVm>().ReverseMap();
		}

		private void LoadSportMapping()
		{
			//CreateMap<State, StateListVm>()
			//	.ForMember(dest => dest.Country,
			//	opt => opt.MapFrom(src => src.Country.CountryName));
			//CreateMap<State, StateEditVm>().ReverseMap();
		}

	}
}
