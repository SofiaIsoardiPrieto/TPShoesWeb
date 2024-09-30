using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace TPShoes.Entidades.ViewModels.Shoe
{
    public class ShoeFilterVm
    {
        public IPagedList<ShoeListVm>? Shoes { get; set; }
        public List<SelectListItem>? Sizes { get; set; }
		public List<SelectListItem>? Brands { get; set; }
		public List<SelectListItem>? Colours { get; set; }
		public List<SelectListItem>? Genres { get; set; }
		public List<SelectListItem>? Sports { get; set; }
	}
}
