using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TPShoes.Entidades.ViewModels.Shoe
{
    public class ShoeEditVm
    {

        public int ShoeId { get; set; }
        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(50, ErrorMessage = "{0} debe contener entre {2} y {1} caracteres", MinimumLength = 3)]
        [DisplayName("Marca")]
        public string? Brand { get; set; }
        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(20, ErrorMessage = "{0} debe contener entre {2} y {1} caracteres", MinimumLength = 3)]
        [DisplayName("Deporte")]
        public string? Sport { get; set; }
        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(10, ErrorMessage = "{0} debe contener entre {2} y {1} caracteres", MinimumLength = 3)]
        [DisplayName("Género")]
        public string? Genre { get; set; }
        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(50, ErrorMessage = "{0} debe contener entre {2} y {1} caracteres", MinimumLength = 3)]
        [DisplayName("Color")]
        public string? Colour { get; set; }
        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(150, ErrorMessage = "{0} debe contener entre {2} y {1} caracteres", MinimumLength = 3)]
        [DisplayName("Modelo")]
        public string? Model { get; set; }
        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(255, ErrorMessage = "{0} debe contener entre {2} y {1} caracteres", MinimumLength = 3)]
        [DisplayName("Descripción")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "{0} es requerido")]
        [DisplayName("Precio")]
        public decimal Price { get; set; }
    }
}
