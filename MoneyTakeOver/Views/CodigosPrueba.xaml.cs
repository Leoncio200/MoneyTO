using MoneyTakeOver.Models;
using MoneyTakeOver.ViewModels;

namespace MoneyTakeOver.Views;

public partial class CodigosPrueba : ContentPage
{
    private OperacionesViewModel _operacionesViewModel;

    public CodigosPrueba()
	{
		InitializeComponent();
        _operacionesViewModel = new OperacionesViewModel();
        BindingContext = _operacionesViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        //await _operacionesViewModel.GetMonTipoCambiConfig();
    }

    private async void Operacion_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var moneda = button!.BindingContext as Monedas;

        if (moneda != null)
        {
            string tipoOperacion = button.Text;
            await NavigateToOperacionesPage(moneda.Id, tipoOperacion);
        }
    }

    private async Task NavigateToOperacionesPage(int monedaId, string tipoOperacion)
    {
        //await Navigation.PushAsync(new Operaciones(monedaId, tipoOperacion));
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        //_operacionesViewModel.TxtSearch = e.NewTextValue;
    }
}