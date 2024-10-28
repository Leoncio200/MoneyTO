using MoneyTakeOver.DataAccess;
using MoneyTakeOver.Models;
using MoneyTakeOver.ViewModels;
using System;

namespace MoneyTakeOver.Views;

public partial class Cambio : ContentPage
{
    private readonly MonedasViewModel _monedasViewModel;

    public Cambio(string selectedCurrency, string selectedCurrencyId)
    {
        InitializeComponent();
        _monedasViewModel = new MonedasViewModel(new DivisasDbContext());
        BindingContext = _monedasViewModel;

        // Mostrar la moneda base seleccionada
        baseCurrencyLabel.Text = selectedCurrency;
        baseCurrencyIdLabel.Text = selectedCurrencyId;

        _ = CargarMonedas();
    }

    private async Task CargarMonedas()
    {
        await _monedasViewModel.GetDatosAsync();
    }

    private async Task ConvertirMoneda()
    {
        try
        {
            var monedaBase = baseCurrencyLabel.Text;
            var monedaSeleccionada = PckOrigenConvertir.SelectedItem as Models.Monedas;

            if (monedaSeleccionada == null)
            {
                await DisplayAlert("Error", "No se seleccionó una moneda a convertir.", "OK");
                return;
            }

            // Buscar y agregar el tipo de cambio desde la API si es necesario
            await _monedasViewModel.BuscarYAgregarTipoCambio(monedaBase, monedaSeleccionada.Nombre);

            // Obtener el tipo de cambio de la moneda seleccionada
            var tipoCambioVenta = await _monedasViewModel.GetTipoCambioById(monedaSeleccionada.Id);
            if (tipoCambioVenta == null)
            {
                await DisplayAlert("Error", "No se encontró el tipo de cambio.", "OK");
                return;
            }

            // Verificar que la cantidad ingresada sea válida
            if (!decimal.TryParse(cantidadInput.Text, out decimal cantidad) || cantidad <= 0)
            {
                await DisplayAlert("Error", "Ingrese una cantidad válida.", "OK");
                return;
            }

            // Realizar la conversión y mostrar el resultado
            var resultado = tipoCambioVenta.Value * cantidad;
            resultadoLabel.Text = $"{resultado:C}";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en la conversión: {ex.Message}");
            await DisplayAlert("Error", "Error al realizar la conversión.", "OK");
        }
    }



    private void OnAceptarClicked(object sender, EventArgs e)
    {
        DisplayAlert("Confirmación", "Conversión aceptada.", "OK");
    }

    private async void OnBuscarYAgregarTipoCambioClicked(object sender, EventArgs e)
    {
        try
        {
            string monedaBase = baseCurrencyLabel.Text;
            var monedaDestino = PckOrigenConvertir.SelectedItem as Models.Monedas;

            if (monedaDestino == null)
            {
                await DisplayAlert("Error", "Selecciona una moneda para convertir.", "OK");
                return;
            }

            await _monedasViewModel.BuscarYAgregarTipoCambio(monedaBase, monedaDestino.Nombre);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Hubo un problema: {ex.Message}", "OK");
        }
    }


    private async void Button_Clicked(object sender, EventArgs e)
    {
        await ConvertirMoneda();
    }

}
