using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TPShoes.Entidades.ViewModels.Brand
{
    public class BrandEditVm
    {
        public int BrandId { get; set; }
        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(50, ErrorMessage = "{0} debe contener entre {2} y {1} caracteres", MinimumLength = 3)]
        [DisplayName("Nombre Brand")]
        public string BrandName { get; set; } = null!;


    }
}
