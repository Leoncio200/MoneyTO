using MoneyTakeOver.DataAccess;
using MoneyTakeOver.Models;
using MoneyTakeOver.ViewModels;

namespace MoneyTakeOver.Views;

public partial class ModificarMoneda : ContentPage
{
    private readonly TiposCambioViewModel _tipoCambioViewModel;
    private TiposCambio _tipoCambioActual;
    public ModificarMoneda(TiposCambio tipoCambio)
	{
		InitializeComponent();
        _tipoCambioViewModel = new TiposCambioViewModel(new DivisasDbContext());
        _tipoCambioActual = tipoCambio;

        LblMoneda.Text = tipoCambio.Moneda?.Nombre ?? "Sin Nombre";
        PCompra.Text = tipoCambio.TipoCambioCompra.ToString();
        PVenta.Text = tipoCambio.TipoCambioVenta.ToString();
    }

    private async void BtnEliminar_Clicked(object sender, EventArgs e)
    {
        var confirmacion = await DisplayAlert("Confirmación", "¿Estás seguro de eliminar esta moneda?", "Sí", "No");
        if (confirmacion)
        {
            await _tipoCambioViewModel.DeleteTipoCambio(_tipoCambioActual);
            await DisplayAlert("Eliminado", "El tipo de cambio ha sido eliminado.", "OK");
            await Navigation.PopAsync();  // Regresar a la pantalla anterior.
        }
    }

    private async void BtnActualizar_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(PVenta.Text) || string.IsNullOrEmpty(PCompra.Text))
        {
            await DisplayAlert("Error", "Debes ingresar valores de compra y venta.", "OK");
            return;
        }

        try
        {
            _tipoCambioActual.TipoCambioCompra = Convert.ToDecimal(PCompra.Text);
            _tipoCambioActual.TipoCambioVenta = Convert.ToDecimal(PVenta.Text);

            await _tipoCambioViewModel.UpdateTipoCambio(_tipoCambioActual);
            await DisplayAlert("Actualizado", "El tipo de cambio ha sido actualizado.", "OK");
            await Navigation.PopAsync();  // Regresar a la pantalla anterior.
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
        }
    }
}