using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace TPShoes.Entidades.ViewModels.Shoe
{
    public class ShoeEditVm
    {

        public int ShoeId { get; set; }

        [Required(ErrorMessage = "Brand es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccinar un Brand")]
        [DisplayName("Brand")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Sport es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccinar un Sport")]
        [DisplayName("Sport")]
        public int SportId { get; set; }


        [Required(ErrorMessage = "Genre es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccinar un Genre")]
        [DisplayName("Genre")]
        public int GenreId { get; set; }

        [Required(ErrorMessage = "Colour es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccinar un colour")]
        [DisplayName("Colour")]
        public int ColourId { get; set; }


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

        [ValidateNever]
        public IEnumerable<SelectListItem> Brands { get; set; } = null!;

        [ValidateNever]
        public IEnumerable<SelectListItem> Colours { get; set; } = null!;

        [ValidateNever]
        public IEnumerable<SelectListItem> Genres { get; set; } = null!;

        [ValidateNever]
        public IEnumerable<SelectListItem> Sports { get; set; } = null!;
    }
}
