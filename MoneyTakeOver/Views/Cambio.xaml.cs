using MoneyTakeOver.DataAccess;
using MoneyTakeOver.ViewModels;

namespace MoneyTakeOver.Views;

public partial class Cambio : ContentPage
{
    private readonly MonedasViewModel _monedasViewModel;

    public Cambio(string selectedCurrency, string selectedCurrencyId)
    {
        InitializeComponent();
        _monedasViewModel = new MonedasViewModel(new DivisasDbContext());
        BindingContext = _monedasViewModel;
        _ = CargarMonedas();

        // Muestra la moneda base seleccionada
        baseCurrencyLabel.Text = selectedCurrency;
        baseCurrencyIdLabel.Text = selectedCurrencyId;
    }

    private async Task CargarMonedas()
    {
        await _monedasViewModel.GetDatosAsync();
    }

    private async Task ConvertirMoneda()
    {
        try
        {
            var monedaBase = baseCurrencyIdLabel.Text;
            if (string.IsNullOrEmpty(monedaBase))
            {
                await DisplayAlert("Error", "No se seleccionó una moneda base.", "OK");
                return;
            }

            var monedaConvertir = PckOrigenConvertir.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(monedaConvertir))
            {
                await DisplayAlert("Error", "No se seleccionó una moneda a convertir.", "OK");
                return;
            }

            monedaConvertir = monedaConvertir.Split('-')[1].Trim();
            int monedaConvertirId = Convert.ToInt32(monedaConvertir);

            // Obtener el tipo de cambio basado en el tipo seleccionado en PckTipoCambio
            var tipoCambioSeleccionado = PckTipoCambio.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(tipoCambioSeleccionado))
            {
                await DisplayAlert("Error", "No se seleccionó un tipo de cambio (Compra o Venta).", "OK");
                return;
            }

            // Obtener los valores de tipo de cambio
            var tipoCambio = await _monedasViewModel.GetTipoCambioById(monedaConvertirId);
            if (tipoCambio == null)
            {
                await DisplayAlert("Error", "No se pudo obtener el tipo de cambio para la moneda seleccionada.", "OK");
                return;
            }

            // Seleccionar entre Compra o Venta
            decimal tipoCambioUsado = tipoCambioSeleccionado == "Compra"
                ? tipoCambio.TipoCambioCompra
                : tipoCambio.TipoCambioVenta;

            // Multiplicar el tipo de cambio por la cantidad a convertir
            var cantidad = Convert.ToDecimal(cantidadInput.Text);
            var resultado = tipoCambioUsado * cantidad;
        resultadoLabel.Text = resultado.ToString("F3");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en ConvertirMoneda: {ex.Message}");
            await DisplayAlert("Error", "Error al convertir la moneda.", "OK");
        }
    }

    private void OnAceptarClicked(object sender, EventArgs e)
    {
        // Lógica a ejecutar cuando el usuario presiona "Aceptar"
        DisplayAlert("Confirmación", "Conversión aceptada.", "OK");
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await ConvertirMoneda();
    }
}
