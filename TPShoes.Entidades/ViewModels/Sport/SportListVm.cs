using System.ComponentModel;

namespace TPShoes.Entidades.ViewModels.Sport
{
    public class SportListVm
    {
        public int SportId { get; set; }
        [DisplayName("Sport")]
        public string SportName { get; set; } = null!;

        public int CantShoes { get; set; }


    }
}
