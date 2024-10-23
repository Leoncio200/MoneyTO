namespace MoneyTakeOver.Views;

public partial class Cambio : ContentPage
{
	public Cambio()
	{
		InitializeComponent();

        
	}

    private void OnDestinoChanged(object sender, EventArgs e)
    {
        // Lógica para cambiar el valor mostrado en ResultadoLabel basado en la selección
        // Esta lógica debería incluir la conversión según la divisa seleccionada.
        string selectedCurrency = (string)((Picker)sender).SelectedItem;
        //double amount = double.TryParse(CantidadEntry.Text, out amount) ? amount : 0;

        // Aquí puedes agregar la lógica para calcular el cambio.
        double conversionRate = selectedCurrency switch
        {
            "Dólar(USD)" => 19, // Ejemplo: tasa de conversión
            "Peso(MXN)" => 1,
            "Euro(EUR)" => 0.05, // Ejemplo: tasa de conversión
            "Yen(JP)" => 6.67, // Ejemplo: tasa de conversión
            "Renminbi(CNY)" => 0.14, // Ejemplo: tasa de conversión
            _ => 1
        };

        //double result = amount / conversionRate;
        //ResultadoLabel.Text = $"$ {result:F2}"; // Muestra el resultado formateado a 2 decimales
    }

    private void OnAceptarClicked(object sender, EventArgs e)
    {
        // Lógica a ejecutar cuando el usuario presiona "Aceptar"
        DisplayAlert("Confirmación", "Conversión aceptada.", "OK");
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MensajeCambio());
    }
}