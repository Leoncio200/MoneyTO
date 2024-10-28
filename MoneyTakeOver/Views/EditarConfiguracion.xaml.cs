using MoneyTakeOver.Models;
using MoneyTakeOver.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.Windows.Input;


namespace MoneyTakeOver.Views
{
    public  partial class EditarConfiguracion : ContentPage
    {
        private Casa _casa;
        private readonly CasaViewModel _casaViewModel;
        public EditarConfiguracion(Casa casa, CasaViewModel viewModel)
        {
            InitializeComponent();
            _casa = casa;
            _casaViewModel = viewModel;

            // Cargar los datos de la casa en los campos
            CargarDatos();
        }

        private void CargarDatos()
        {
            NombreEntry.Text = _casa.Nombre;
            DireccionEntry.Text = _casa.Direccion;
            CiudadEntry.Text = _casa.Ciudad;
            EstadoEntry.Text = _casa.Estado;
            HorarioInicioEntry.Text = _casa.HInicio;
            HorarioCierreEntry.Text = _casa.HCierre;
        }

        private async void OnGuardarClicked(object sender, EventArgs e)
        {
            _casa.Nombre = NombreEntry.Text;
            _casa.Direccion = DireccionEntry.Text;
            _casa.Ciudad = CiudadEntry.Text;
            _casa.Estado = EstadoEntry.Text;
            _casa.HInicio = HorarioInicioEntry.Text;
            _casa.HCierre = HorarioCierreEntry.Text;

            await _casaViewModel.UpdateCasa(_casa);
            await DisplayAlert("Éxito", "La configuración ha sido guardada.", "OK");
            await Navigation.PopAsync(); // Volver a la pantalla anterior
        }
    }
}