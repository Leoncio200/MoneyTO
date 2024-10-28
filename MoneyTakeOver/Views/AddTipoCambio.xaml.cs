using MoneyTakeOver.DataAccess;
using MoneyTakeOver.Models;
using MoneyTakeOver.ViewModels;

namespace MoneyTakeOver.Views;

public partial class AddTipoCambio : ContentPage
{
    private readonly TiposCambioViewModel _tiposCambioViewModel;
    public AddTipoCambio(TiposCambioViewModel viewModel)
	{
		InitializeComponent();
        _tiposCambioViewModel = viewModel;
        BindingContext = _tiposCambioViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarMonedasEnPickers();
    }

    private async Task CargarMonedasEnPickers()
    {
        var monedas = await _tiposCambioViewModel.ObtenerMonedas();

        if (monedas != null && monedas.Count > 0)
        {
            MonedaBasePicker.ItemsSource = monedas;
            MonedaDestinoPicker.ItemsSource = monedas;
        }
        else
        {
            await DisplayAlert("Error", "No se encontraron monedas para cargar.", "OK");
        }
    }


    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        try
        {
            var monedaBase = MonedaBasePicker.SelectedItem as Monedas;
            var monedaDestino = MonedaDestinoPicker.SelectedItem as Monedas;

            if (monedaBase == null || monedaDestino == null)
            {
                await DisplayAlert("Error", "Selecciona ambas monedas.", "OK");
                return;
            }

            if (!decimal.TryParse(CompraEntry.Text, out var compra) ||
                !decimal.TryParse(VentaEntry.Text, out var venta))
            {
                await DisplayAlert("Error", "Introduce valores válidos para compra y venta.", "OK");
                return;
            }

            _tiposCambioViewModel.NuevoTipoCambioMoneda = monedaDestino.Nombre;
            _tiposCambioViewModel.NuevoTipoCambioPrecio = compra;
            _tiposCambioViewModel.NuevoTipoCambioVenta = venta;

            await _tiposCambioViewModel.AddTipoCambio();
            await DisplayAlert("Éxito", "Tipo de cambio añadido correctamente.", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Hubo un problema: {ex.Message}", "OK");
        }
    }
}