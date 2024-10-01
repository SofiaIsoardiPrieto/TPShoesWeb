using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPShoes.Entidades.ViewModels.Shoe
{
    public class ShoeListVm
    {
        public int ShoeId { get; set; }
        public string Brand { get; set; } = null!;
        public string Genre { get; set; } = null!;
        public string Colour { get; set; } = null!;
        public string Sport { get; set; } = null!;
        public string Size { get; set; } = null!;


        [DisplayName("Modelo")]
        public string Model { get; set; } = null!;


        [DisplayName("Descripción")]
        public string? Description { get; set; }


        [DisplayName("Precio")]
        public string Price { get; set; } = null!;
    }
}
