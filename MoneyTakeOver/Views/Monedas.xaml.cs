using MoneyTakeOver.DataAccess;
using MoneyTakeOver.Models;
using MoneyTakeOver.ViewModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MoneyTakeOver.Views
{
    public partial class Monedas : ContentPage
    {
        private readonly TiposCambioViewModel _tiposCambioViewModel;

        public Monedas()
        {
            InitializeComponent();
            _tiposCambioViewModel = new TiposCambioViewModel(new DivisasDbContext());
            BindingContext = _tiposCambioViewModel;

            // Carga de monedas
            _ = CargarMonedas();

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _tiposCambioViewModel.GetDatosAsync();  // Recargar los datos al volver a la página.
        }

        private async Task CargarMonedas()
        {
            try
            {
                await _tiposCambioViewModel.GetDatosAsync();
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

        private async void OnEditButtonClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var tipoCambio = (TiposCambio)button.BindingContext;
            await Navigation.PushAsync(new ModificarMoneda(tipoCambio));
        }
    }
}