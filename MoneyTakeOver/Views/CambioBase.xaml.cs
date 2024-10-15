namespace MoneyTakeOver.Views;

public partial class CambioBase : ContentPage
{
	public CambioBase()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CodigosPrueba());
    }
}