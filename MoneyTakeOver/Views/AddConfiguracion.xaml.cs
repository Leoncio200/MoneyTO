using MoneyTakeOver.DataAccess;
using MoneyTakeOver.ViewModels;

namespace MoneyTakeOver.Views;

public partial class AddConfiguracion : ContentPage
{
	private DivisasDbContext _dbContext = new DivisasDbContext();
	private CasaViewModel _casaViewModel;
	public AddConfiguracion()
	{
		InitializeComponent();
		_casaViewModel = new CasaViewModel(_dbContext);
		BindingContext = _casaViewModel;
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        // Asigna los valores desde las entradas al ViewModel
        _casaViewModel.NuevaCasaNombre = EntNomb.Text;
        _casaViewModel.NuevaCasaDireccion = EntDireccion.Text;
        _casaViewModel.NuevaCasaCiudad = EntCiudad.Text;
        _casaViewModel.NuevaCasaEstado = EntEstado.Text;
        _casaViewModel.NuevaCasaHInicio = EntHInicio.Text;
        _casaViewModel.NuevaCasaHCierre = EntHCierre.Text;

        // Agrega la nueva casa a la base de datos
        await _casaViewModel.AddCasa();

        // Regresa a la página anterior
        await Navigation.PopAsync();
    }
}