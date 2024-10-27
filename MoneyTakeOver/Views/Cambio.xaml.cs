namespace MoneyTakeOver.Views;
public partial class Cambio : ContentPage
{
<<<<<<< HEAD
	public Cambio()
	{
		InitializeComponent();
	}
=======
    private string _monedaSeleccionada;
    private double _cantidad;

    public Cambio(string monedaSeleccionada)
    {
        InitializeComponent();
        _monedaSeleccionada = monedaSeleccionada;
        LblCantidad.Text = monedaSeleccionada;
    }
>>>>>>> b5adad7 (push)

    private void OnDestinoChanged(object sender, EventArgs e)
    {
        string monedaDestino = (string)((Picker)sender).SelectedItem;

        double tasaConversion = monedaDestino switch
        {
            "Dólar(USD)" => 19,
            "Peso(MXN)" => 1,
            "Euro(EUR)" => 0.05,
            "Yen(JP)" => 6.67,
            "Renminbi(CNY)" => 0.14,
            _ => 1
        };

        // Realiza el cálculo de conversión
        double resultado = _cantidad / tasaConversion;
        LblCantidad.Text = $"$ {resultado:F2}"; // Muestra el resultado formateado a 2 decimales
    }

    private async void OnAceptarClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Confirmación", "Conversión aceptada.", "OK");
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MensajeCambio());
    }
}