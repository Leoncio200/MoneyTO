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

        // Buscar el tipo de cambio de la moneda seleccionada
       var tipoCambio = await _monedasViewModel.GetTipoCambioById(monedaConvertirId);
        // Aquí agregas la lógica para multiplicar el tipo de cambio y mostrar el resultado
        var cantidad = Convert.ToDecimal(cantidadInput.Text);
        var resultado = tipoCambio * cantidad;
        resultadoLabel.Text = $"{resultado}";
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

    private async void OnBuscarYAgregarTipoCambioClicked(object sender, EventArgs e)
    {
        string monedaBase = "USD"; // Puedes obtener esto del usuario o configuración
        string monedaDestino = PckOrigenConvertir.SelectedItem.ToString(); // Selección del Picker

        await _monedasViewModel.BuscarYAgregarTipoCambio(monedaBase, monedaDestino);
    }


    private async void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            var monedaBase = baseCurrencyLabel.Text;
            var monedaDestino = PckOrigenConvertir.SelectedItem as Models.Monedas;

            if (monedaDestino == null)
            {
                await DisplayAlert("Error", "Selecciona una moneda para convertir.", "OK");
                return;
            }

            // Buscar y agregar el tipo de cambio
            await _monedasViewModel.BuscarYAgregarTipoCambio(monedaBase, monedaDestino.Nombre);

            // Ahora, realiza la conversión
            var tipoCambio = await _monedasViewModel.GetTipoCambioById(monedaDestino.Id);
            if (tipoCambio == null)
            {
                await DisplayAlert("Error", "No se encontró el tipo de cambio.", "OK");
                return;
            }

            var cantidad = Convert.ToDecimal(cantidadInput.Text);
            var resultado = tipoCambio.Value * cantidad;
            resultadoLabel.Text = $"{resultado:C}";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en la conversión: {ex.Message}");
            await DisplayAlert("Error", "Error al realizar la conversión.", "OK");
        }
    }

}