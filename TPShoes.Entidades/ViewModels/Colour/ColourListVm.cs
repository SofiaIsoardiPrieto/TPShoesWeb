using System.ComponentModel;

namespace TPShoes.Entidades.ViewModels.Colour
{
    public class ColourListVm
    {
        public int ColourId { get; set; }
        [DisplayName("Colour")]
        public string ColourName { get; set; } = null!;
        public int CantShoes { get; set; }
    }
}
