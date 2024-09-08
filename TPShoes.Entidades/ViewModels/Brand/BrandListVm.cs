using System.ComponentModel;

namespace TPShoes.Entidades.ViewModels.Brand
{
    public class BrandListVm
    {
        public int BrandId { get; set; }
        [DisplayName("Brand")]
        public string BrandName { get; set; } = null!;
    }
}
