using System.ComponentModel;

namespace TPShoes.Entidades.ViewModels.Genre
{
    public class GenreListVm
    {
        public int GenreId { get; set; }
        [DisplayName("Genre")]
        public string GenreName { get; set; } = null!;
        public int CantShoes { get; set; }
    }
}
