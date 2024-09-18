using System;
using System.Collections.Generic;
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

        public string Model { get; set; } = null!;
        public string? Description { get; set; }
        public string Price { get; set; } = null!;
    }
}
