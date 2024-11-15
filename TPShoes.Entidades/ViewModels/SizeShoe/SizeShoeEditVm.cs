using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace TPShoes.Entidades.ViewModels.SizeShoe
{
    public class SizeShoeEditVm
    {
        public int SizeShoeId { get; set; }

        [DisplayName("Shoe")]
        public int ShoeId { get; set; }

        [DisplayName("Size")]
        public int SizeId { get; set; }
        public int Stock { get; set; }
        [ ValidateNever ]
        public List<SelectListItem> Shoes { get; set; } = new List<SelectListItem>();
        [ValidateNever]
        public List<SelectListItem> Sizes { get; set; } = new List<SelectListItem>();

    }
}
