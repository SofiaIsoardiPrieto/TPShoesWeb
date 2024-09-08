using System.ComponentModel;

namespace TPShoes.Entidades.ViewModels.Size
{
    public class SizeListVm
    {
        public int SizeId { get; set; }
        [DisplayName("Size")]
        public string SizeName { get; set; } = null!;
    }
}
