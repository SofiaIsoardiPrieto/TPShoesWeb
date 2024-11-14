using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPShoes.Entidades.ViewModels.SizeShoe
{
    public class SizeShoeEditVm
    {
        public int SizeShoeId { get; set; }
        public int ShoeId { get; set; }
        public int SizeId { get; set; }
        public int Stock { get; set; }

        public List<SelectListItem> Shoes { get; set; } = null!;
        public List<SelectListItem> Sizes { get; set; } = null!;

    }
}
