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
            _ = CargarMonedas();
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
            await _monedasViewModel.GetDatosAsync();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Cambio());
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Configuracion());
        }
    }
}
