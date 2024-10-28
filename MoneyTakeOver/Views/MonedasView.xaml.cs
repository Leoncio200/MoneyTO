using MoneyTakeOver.DataAccess;
using MoneyTakeOver.Models;
using MoneyTakeOver.ViewModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MoneyTakeOver.Views
{
    public partial class MonedasView : ContentPage
    {
        private readonly TiposCambioViewModel _tiposCambioViewModel;

        public MonedasView()
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
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnAddTipoCambioClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTipoCambio(_tiposCambioViewModel));
        }


        private async void OnEditButtonClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var tipoCambio = (TiposCambio)button.BindingContext;
            await Navigation.PushAsync(new ModificarMoneda(tipoCambio));
        }
    }
}