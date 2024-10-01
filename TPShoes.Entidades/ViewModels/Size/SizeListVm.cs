using System.ComponentModel;

namespace TPShoes.Entidades.ViewModels.Size
{
    public class SizeListVm
    {
        public int SizeId { get; set; }
        [DisplayName("Size")]
        public decimal SizeNumber { get; set; }
        public int CantShoes { get; set; }
    }
}
