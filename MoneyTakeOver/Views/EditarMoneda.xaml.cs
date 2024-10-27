using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MoneyTakeOver.DataAccess;
using MoneyTakeOver.Models;
using MoneyTakeOver.ViewModels;


namespace MoneyTakeOver.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public partial class EditarMoneda : ContentPage
    {

        private readonly MonedasViewModel _monedasViewModel;
        private readonly TiposCambio _tipoCambio;

        public EditarMoneda(TiposCambio tipoCambio)
        {
               _monedasViewModel = new MonedasViewModel(new DivisasDbContext());
           _tipoCambio = tipoCambio;
            BindingContext = tipoCambio;
            InitializeComponent();
        }


        //obtener los valores de nombre y tipo de cambio de la moneda seleccionada
        private async void OnGuardarCambiosClicked(object sender, EventArgs e)
        {
            try
            {
                // Obtén los valores actuales de TipoCambioCompra y TipoCambioVenta del BindingContext
                var nuevoTipoCompra = _tipoCambio.TipoCambioCompra;
                var nuevoTipoVenta = _tipoCambio.TipoCambioVenta;

                // Llama al método para actualizar los valores en la base de datos
                await _monedasViewModel.ActualizarValorTipoCambio(_tipoCambio.MonedaId, nuevoTipoCompra, nuevoTipoVenta);

                await DisplayAlert("Éxito", "Los valores han sido actualizados correctamente.", "OK");
                //Actualiza la lista de monedas

                await Navigation.PopAsync(); // Vuelve a la página anterior
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Hubo un problema al guardar los cambios: {ex.Message}", "OK");
            }
        }
    }
}