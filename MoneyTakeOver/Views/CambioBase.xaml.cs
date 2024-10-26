using Microsoft.EntityFrameworkCore;
using MoneyTakeOver.DataAccess;
using MoneyTakeOver.ViewModels;

namespace MoneyTakeOver.Views;

public partial class CambioBase : ContentPage
{
    private MonedasViewModel _monedasViewModel;
    private DivisasDbContext _dbContext = new DivisasDbContext();
	public CambioBase()
    {
		InitializeComponent();
        _dbContext.Database.EnsureCreatedAsync();
        _monedasViewModel = new MonedasViewModel(_dbContext);
        BindingContext = _monedasViewModel;
        CargarMonedas();
	}

    private void CargarMonedas()
    {
        // Llama la tarea para agregar estas monedas

        _ = _monedasViewModel.AgregarTodasLasMonedas();
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