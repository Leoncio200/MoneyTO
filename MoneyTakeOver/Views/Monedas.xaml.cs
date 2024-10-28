using MoneyTakeOver.DataAccess;
using MoneyTakeOver.Models;
using MoneyTakeOver.ViewModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MoneyTakeOver.Views
{
    public partial class Monedas : ContentPage
    {
        private readonly MonedasViewModel _monedasViewModel;

        public Monedas()
        {
            InitializeComponent();
            _monedasViewModel = new MonedasViewModel(new DivisasDbContext());
            BindingContext = _monedasViewModel;

            // Carga de monedas
            _ = CargarMonedas();

        }

        private async Task CargarMonedas()
        {
            try
            {
                await _monedasViewModel.GetDatosAsync();
                //Console.WriteLine(_monedasViewModel.Divisas);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void OnMonedaSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //CONSOLEAR EL NOMBRE DE LA MONEDA
            var moneda = (TiposCambio)e.SelectedItem;
            Console.WriteLine(moneda.Id);
        }

        private void OnEditButtonClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var moneda = (TiposCambio)button.CommandParameter;
            Console.WriteLine(moneda.Id);

            // Navegar a la página de edición
           // await Navigation.PushAsync(new EditarMoneda(moneda));
        }
    }
}