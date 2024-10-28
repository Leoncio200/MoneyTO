using MoneyTakeOver.DataAccess;
using MoneyTakeOver.ViewModels;

namespace MoneyTakeOver.Views;

public partial class Configuracion : ContentPage
{
    private CasaViewModel _casasViewModel;
    private DivisasDbContext _dbContext = new DivisasDbContext();
    public Configuracion()
	{
		InitializeComponent();
		_casasViewModel = new CasaViewModel(_dbContext);
		BindingContext = _casasViewModel;
		_ = CargarCasas();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _casasViewModel.GetDatosAsync();  // Recargar los datos al volver a la página.
    }

    private async Task CargarCasas()
	{
        await _casasViewModel.GetDatosAsync();
	}

	private void OnGuardarClicked(object sender, EventArgs e)
	{
		// Lógica a ejecutar cuando el usuario presiona "Guardar"
		DisplayAlert("Confirmación", "Configuración guardada.", "OK");
	}

	private async void Button_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new AddConfiguracion());
	}

	//funcion de eliminar
	private async void OnDeleteButtonClicked(object sender, EventArgs e)
	{
		//llamar a la funcion de eliminar
		// var button = (Button)sender;
		// var casa = (Casa)button.CommandParameter;
	}
	private async void OnEditButtonClicked(object sender, EventArgs e)
{
    // var button = (Button)sender;
    // var casa = (Casa)button.CommandParameter;

    // if (casa != null)
    // {
    //     // Navegar a la página de edición pasando el objeto Casa
    //     await Navigation.PushAsync(new EditarConfiguracion(casa));
    // }
    // else
    // {
    //     await DisplayAlert("Error", "No se pudo obtener la información de la empresa para editar", "OK");
    // }
}



}
