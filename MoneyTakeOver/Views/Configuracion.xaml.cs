namespace MoneyTakeOver.Views;

public partial class Configuracion : ContentPage
{
	public Configuracion()
	{
		InitializeComponent();
	}

	private void OnGuardarClicked(object sender, EventArgs e)
	{
		// Lógica a ejecutar cuando el usuario presiona "Guardar"
		DisplayAlert("Confirmación", "Configuración guardada.", "OK");
	}

	private async void Button_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new EditarConfiguracion());
	}
}
