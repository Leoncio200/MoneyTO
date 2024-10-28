using Microsoft.EntityFrameworkCore;
using MoneyTakeOver.DataAccess;
using MoneyTakeOver.Models;
using MoneyTakeOver.ViewModels;

namespace MoneyTakeOver.Views
{
    public partial class CambioBase : ContentPage
    {
        private readonly MonedasViewModel _monedasViewModel;
        private readonly DivisasDbContext _dbContext;

        public CambioBase()
        {
            InitializeComponent();
            _dbContext = new DivisasDbContext();
            _monedasViewModel = new MonedasViewModel(_dbContext);
            BindingContext = _monedasViewModel;
            CreateDB();
            //eliminar las monedas de la bd
          //  _dbContext.Database.ExecuteSqlRaw("DELETE FROM Monedas");
            
            
            _ = CargarMonedas();
            //_ = CargarTiposCambio();
        }

        private async void CreateDB()
        {
            var con = await _dbContext.Database.CanConnectAsync();
            if (!con)
            {
                await _dbContext.Database.EnsureCreatedAsync();
            }
        }

        private async Task CargarMonedas()
        {
            await _monedasViewModel.InsertarDivisas();

        }

        private async Task CargarTiposCambio()
        {
            await _monedasViewModel.AgregarTiposCambio();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            // Verificar si se ha seleccionado una moneda
            var monedaSeleccionada = PckOrigen.SelectedItem as Models.Monedas;
            if (monedaSeleccionada == null)
            {
                await DisplayAlert("Error", "Por favor selecciona una moneda.", "OK");
                return;
            }

            // Obtener el nombre y el ID de la moneda seleccionada
            var selectedCurrency = monedaSeleccionada.Nombre;
            var selectedCurrencyId = monedaSeleccionada.Id.ToString();

            // Pasar los valores a la siguiente página
            await Navigation.PushAsync(new Cambio(selectedCurrency, selectedCurrencyId));
        }



        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Configuracion());
        }
    }
}
