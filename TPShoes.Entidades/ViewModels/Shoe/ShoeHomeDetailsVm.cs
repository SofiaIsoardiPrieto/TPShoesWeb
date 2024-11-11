using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace TPShoes.Entidades.ViewModels.Shoe
{
    public class ShoeHomeDetailsVm
    {
        public int ShoeId { get; set; }
        public string Brand { get; set; } = null!;
        public string Genre { get; set; } = null!;
        public string Colour { get; set; } = null!;
        public string Sport { get; set; } = null!;
        public List<SelectListItem> Sizes { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public bool Active { get; set; }

        [DisplayName("Modelo")]
        public string Model { get; set; } = null!;


        [DisplayName("Descripción")]
        public string? Description { get; set; }


        [DisplayName("Precio Venta")]
        public decimal FinalPrice { get; set; }

        [DisplayName("Precio")]
        public decimal Price { get; set; }
    }
}
