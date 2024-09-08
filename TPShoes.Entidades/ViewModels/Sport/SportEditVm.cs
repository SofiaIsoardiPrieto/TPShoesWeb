using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TPShoes.Entidades.ViewModels.Sport
{
    public class SportEditVm
    {
        public int SportId { get; set; }
        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(50, ErrorMessage = "{0} debe contener entre {2} y {1} caracteres", MinimumLength = 3)]
        [DisplayName("Nombre Sport")]
        public string SportName { get; set; } = null!;


    }
}
