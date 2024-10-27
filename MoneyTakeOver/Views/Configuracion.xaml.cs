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
}
