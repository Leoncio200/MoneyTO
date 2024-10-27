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
        CreateDB();
        _monedasViewModel = new MonedasViewModel(_dbContext);
        BindingContext = _monedasViewModel;
        CargarMonedas();
	}

    private async void CreateDB()
    {
        var con = await _dbContext.Database.CanConnectAsync();
        if(!con)
        {
            await _dbContext.Database.EnsureCreatedAsync();
        }
    }

    private void CargarMonedas()
    {
        _ = _monedasViewModel.GetDatosAsync();
    }

    private async void Button_Clicked(object sender, EventArgs e)

    {   
        await Navigation.PushAsync(new Cambio());
    {
        try
        {
           //mandar la moneda a la vista cambio
           var monedaSeleccionada = PckOrigen.SelectedItem.ToString();
              await Navigation.PushAsync(new Cambio(monedaSeleccionada));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }



    private void ObtenerMonedaSeleccionada()
    {
        try
        {
            var monedaSeleccionada = PckOrigen.SelectedItem.ToString();
            Console.WriteLine(monedaSeleccionada);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Configuracion());
    }
    
    }


}
