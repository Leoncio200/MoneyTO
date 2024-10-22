namespace MoneyTakeOver.Views;

public partial class CambioBase : ContentPage
{
	public CambioBase()
	{
		InitializeComponent();
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