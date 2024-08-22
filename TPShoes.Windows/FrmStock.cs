using TPShoes.Entidades.Clases;

namespace TPShoes.Windows
{
    public partial class FrmStock : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private int stock;
        private SizeShoe sizeShoe;
        public FrmStock(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        private void Aceptarbutton_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (sizeShoe is not null)
                {
                    sizeShoe.Stok = int.Parse(StocktextBox.Text);
                }
                DialogResult = DialogResult.OK;
            }
        }
        private bool ValidarDatos()
        {
            bool valido = true;
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(StocktextBox.Text) || string.IsNullOrWhiteSpace(StocktextBox.Text))
            {
                valido = false;
                errorProvider1.SetError(StocktextBox, "Stock requerido");
            }

            // Validar que el stock sea un entero v�lido y mayor o igual que cero
            if (!int.TryParse(StocktextBox.Text, out int stock) || stock < 0)
            {
                valido = false;
                errorProvider1.SetError(StocktextBox, "Stock no v�lido o mal ingresado. Debe ser un n�mero entero positivo.");
            }

            // Validar que el stock est� dentro del rango permitido (opcional, si tienes un l�mite superior)
            if (stock > 99999) // Cambia 99999 por el valor m�ximo permitido si es diferente
            {
                valido = false;
                errorProvider1.SetError(StocktextBox, "El stock debe estar entre 0 y 99999.");
            }
            return valido;
        }
        private void Cancelarbutton_Click(object sender, EventArgs e)
        {
            Close();
        }

        internal void SetSizShoe(SizeShoe sizeShoeOriginal)
        {
            sizeShoe = sizeShoeOriginal;
        }

        internal SizeShoe GetSizeShoe()
        {
            return sizeShoe;
        }
    }
}
