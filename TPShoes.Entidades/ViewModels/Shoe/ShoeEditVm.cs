using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace TPShoes.Entidades.ViewModels.Shoe
{
    public class ShoeEditVm
    {

        public int ShoeId { get; set; }

        [Required(ErrorMessage = "Brand es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccinar un Brand")]
        [DisplayName("Brand")]
        public int BrandId { get; set; }
        [ValidateNever]
        public List<SelectListItem> Brands { get; set; } = null!;


        [Required(ErrorMessage = "Sport es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccinar un Sport")]
        [DisplayName("Sport")]
        public int SportId { get; set; }
        [ValidateNever]
        public List<SelectListItem> Sports { get; set; } = null!;



        [Required(ErrorMessage = "Genre es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccinar un Genre")]
        [DisplayName("Genre")]
        public int GenreId { get; set; }
        [ValidateNever]
        public List<SelectListItem> Genres { get; set; } = null!;



        [Required(ErrorMessage = "Colour es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccinar un colour")]
        [DisplayName("Colour")]
        public int ColourId { get; set; }
        [ValidateNever]
        public List<SelectListItem> Colours { get; set; } = null!;



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
