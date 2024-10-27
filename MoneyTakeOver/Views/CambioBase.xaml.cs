using Microsoft.EntityFrameworkCore;
using MoneyTakeOver.DataAccess;
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
            _ = CargarTiposCambio();
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

            var picker = PckOrigen.SelectedItem.ToString();
            Console.WriteLine(picker);
            //pasa la moneda seleccionada a la siguiente pagina
            var selectedCurrency = picker.Split('-')[0].Trim();
            //tambien pasar el id de la moneda
            var selectedCurrencyId = picker.Split('-')[1].Trim();
            await Navigation.PushAsync(new Cambio(selectedCurrency, selectedCurrencyId));
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Configuracion());
        }
    }
}
