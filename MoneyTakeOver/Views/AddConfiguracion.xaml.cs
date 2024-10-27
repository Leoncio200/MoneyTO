using MoneyTakeOver.DataAccess;
using MoneyTakeOver.ViewModels;

namespace MoneyTakeOver.Views;

public partial class AddConfiguracion : ContentPage
{
    private CasaViewModel _casaViewModel;

    public AddConfiguracion()
    {
        InitializeComponent();
        _casaViewModel = new CasaViewModel(new DivisasDbContext());
        BindingContext = _casaViewModel;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        // Asigna los valores desde las entradas al ViewModel
        _casaViewModel.NuevaCasaNombre = EntNomb.Text;
        _casaViewModel.NuevaCasaDireccion = EntDireccion.Text;
        _casaViewModel.NuevaCasaCiudad = EntCiudad.Text;
        _casaViewModel.NuevaCasaEstado = EntEstado.Text;

        // Obtiene los valores seleccionados en los TimePickers y los convierte a cadena
        _casaViewModel.NuevaCasaHInicio = TimePickerHInicio.Time.ToString(@"hh\:mm");
        _casaViewModel.NuevaCasaHCierre = TimePickerHCierre.Time.ToString(@"hh\:mm");

        // Agrega la nueva casa a la base de datos
        await _casaViewModel.AddCasa();

        // Mensaje de confirmación
        await DisplayAlert("Éxito", "Configuración guardada correctamente", "OK");

        // Regresa a la página anterior
        await Navigation.PopAsync();
    }
}
