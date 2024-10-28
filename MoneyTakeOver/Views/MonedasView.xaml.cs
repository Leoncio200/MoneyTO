using MoneyTakeOver.DataAccess;
using MoneyTakeOver.Models;
using MoneyTakeOver.ViewModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;


namespace MoneyTakeOver.Views;

public partial class MonedasView : ContentPage
{
    private readonly MonedasViewModel _monedasViewModel;

    public MonedasView()
    {
        InitializeComponent();
        _monedasViewModel = new MonedasViewModel(new DivisasDbContext());
        BindingContext = _monedasViewModel;

        // Carga de monedas
        _ = CargarMonedas();
    }
    protected override async void OnAppearing()
{
    base.OnAppearing();

    // Vuelve a cargar la lista de monedas
    await _monedasViewModel.GetDatosAsyncTipo();
    MonedasListView.ItemsSource = _monedasViewModel.TiposCambioList;
}

    private async Task CargarMonedas()
    {
        try
        {
            await _monedasViewModel.GetDatosAsyncTipo();
            Console.WriteLine("Cargando monedas");
            

            // Cargar las monedas en la lista
            MonedasListView.ItemsSource = _monedasViewModel.TiposCambioList;
            
            // if (_monedasViewModel.Divisas.Count == 0)
            // {
            //     await DisplayAlert("Error", "No se han podido cargar las monedas", "OK");
            // }
            // else
            // {
            //     await DisplayAlert("Exito", "Monedas cargadas", "OK");
            // }

        
          
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void OnMonedaSelected(object sender, SelectedItemChangedEventArgs e)
    {
        // CONSOLEAR EL NOMBRE DE LA MONEDA
    try
    {
        var moneda = (TiposCambio)e.SelectedItem;
        Console.WriteLine(moneda);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    }

   private async void OnEditButtonClicked(object sender, EventArgs e)
{
    var button = (Button)sender;
    var tipoCambio = (TiposCambio)button.CommandParameter; // Obtén el objeto TiposCambio completo

    if (tipoCambio != null)
    {
        // Navegar a la página de edición pasando el objeto completo
        await Navigation.PushAsync(new EditarMoneda(tipoCambio));
    }
    else
    {
        await DisplayAlert("Error", "No se pudo obtener la moneda para editar", "OK");
    }
}
}
