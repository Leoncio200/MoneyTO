using MoneyTakeOver.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyTakeOver.Models;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace MoneyTakeOver.ViewModels
{
    public class CasaViewModel : INotifyPropertyChanged
    {
        private readonly DivisasDbContext _dbContext;
        private ObservableCollection<Casa> _casas;
        private string _nuevaCasaNombre;
        private string _nuevaCasaDireccion;
        private string _nuevaCasaCiudad;
        private string _nuevaCasaEstado;
        private string _nuevaCasaHInicio;
        private string _nuevaCasaHCierre;
        private bool _isCasasEmpty;
        private bool _isInitialLoad = true;
        private bool _isLoading = false;

        public ICommand EliminarCasaCommand { get; }


        public ObservableCollection<Casa> Casas
        {
            get { return _casas; }
            set { SetProperty(ref _casas, value); }
        }

        public string NuevaCasaNombre
        {
            get { return _nuevaCasaNombre; }
            set { SetProperty(ref _nuevaCasaNombre, value); }
        }

        public string NuevaCasaDireccion
        {
            get { return _nuevaCasaDireccion; }
            set { SetProperty(ref _nuevaCasaDireccion, value); }
        }

        public string NuevaCasaCiudad
        {
            get { return _nuevaCasaCiudad; }
            set { SetProperty(ref _nuevaCasaCiudad, value); }
        }

        public string NuevaCasaEstado
        {
            get { return _nuevaCasaEstado; }
            set { SetProperty(ref _nuevaCasaEstado, value); }
        }

        public string NuevaCasaHInicio
        {
            get { return _nuevaCasaHInicio; }
            set { SetProperty(ref _nuevaCasaHInicio, value); }
        }

        public string NuevaCasaHCierre
        {
            get { return _nuevaCasaHCierre; }
            set { SetProperty(ref _nuevaCasaHCierre, value); }
        }

        public bool IsCasasEmpty
        {
            get { return _isCasasEmpty; }
            set { SetProperty(ref _isCasasEmpty, value); }
        }

        public async Task GetCasas()
        {
            if (_isLoading)
                return;

            try
            {
                if (_isInitialLoad)
                {
                    _isInitialLoad = false;
                }

                _isLoading = true;
                await Task.Delay(100);

                var query = _dbContext.Casas.AsQueryable();

                var allCasas = await query.ToListAsync();
                Casas.Clear();
                foreach (var casa in allCasas)
                {
                    Casas.Add(casa);
                }

                IsCasasEmpty = Casas.Count == 0;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error al procesar la solicitud Message: {ex.Message}";
                var errorStackTrace = $"Error al procesar la solicitud StackTrace: {ex.StackTrace}";
                var errorMessageDialog = $"Fallo al procesar la solicitud: {ex.Message}";
                Console.WriteLine("=== ERROR DETECTADO ===");
                Console.WriteLine(errorMessage);
                Console.WriteLine(errorStackTrace);
                Console.WriteLine("=======================");
            }
            finally
            {
                _isLoading = false;
                if (!_isInitialLoad)
                {
                }
            }
        }

        public async Task AddCasa()
        {
            try
            {
                await Task.Delay(100);

                if (string.IsNullOrWhiteSpace(NuevaCasaNombre) || string.IsNullOrWhiteSpace(NuevaCasaCiudad)
                    || string.IsNullOrWhiteSpace(NuevaCasaEstado) || string.IsNullOrWhiteSpace(NuevaCasaDireccion)
                    || string.IsNullOrWhiteSpace(NuevaCasaHCierre) || string.IsNullOrWhiteSpace(NuevaCasaHInicio))
                    return;

                string primeraLetraMayuscula = char.ToUpper(NuevaCasaNombre[0]) + NuevaCasaNombre.Substring(1).ToLower();
                Casa nuevaCasa = new Casa { 
                    Nombre = primeraLetraMayuscula, 
                    Direccion = NuevaCasaDireccion,
                    Ciudad = NuevaCasaCiudad,
                    Estado = NuevaCasaEstado,
                    HInicio = NuevaCasaHInicio,
                    HCierre = NuevaCasaHCierre
                };

                _dbContext.Casas.Add(nuevaCasa);
                await _dbContext.SaveChangesAsync();

                NuevaCasaNombre = "";
                NuevaCasaDireccion = "";
                NuevaCasaCiudad = "";
                NuevaCasaEstado = "";
                NuevaCasaHInicio = "";
                NuevaCasaHCierre = "";
                await GetCasas();
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error al procesar la solicitud Message: {ex.Message}";
                var errorStackTrace = $"Error al procesar la solicitud StackTrace: {ex.StackTrace}";
                var errorMessageDialog = $"Fallo al procesar la solicitud: {ex.Message}";
                Console.WriteLine("=== ERROR DETECTADO ===");
                Console.WriteLine(errorMessage);
                Console.WriteLine(errorStackTrace);
                Console.WriteLine("=======================");
            }
            finally
            {
            }
        }

        public async Task DeleteCasa(Casa casa)
        {
            try
            {
                await Task.Delay(100);

                if (casa != null)
                {
                    _dbContext.Casas.Remove(casa);
                    await _dbContext.SaveChangesAsync();
                    Casas.Remove(casa);

                    await GetCasas();
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error al procesar la solicitud Message: {ex.Message}";
                var errorStackTrace = $"Error al procesar la solicitud StackTrace: {ex.StackTrace}";
                var errorMessageDialog = $"Fallo al procesar la solicitud: {ex.Message}";
                Console.WriteLine("=== ERROR DETECTADO ===");
                Console.WriteLine(errorMessage);
                Console.WriteLine(errorStackTrace);
                Console.WriteLine("=======================");
            }
            finally
            {
            }
        }

        public CasaViewModel(DivisasDbContext dbContext)
        {
            _dbContext = dbContext;
            Casas = new ObservableCollection<Casa>();
                EliminarCasaCommand = new Command<Casa>(async (casa) => await DeleteCasa(casa));

        }

        public async Task GetDatosAsync()
        {
            await GetCasas();
        }

        private string NormalizeString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var normalizedString = input.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            var cleanString = stringBuilder.ToString()
                .Replace(",", " ")
                .Replace(".", " ")
                .Replace("á", "a")
                .Replace("é", "e")
                .Replace("í", "i")
                .Replace("ó", "o")
                .Replace("ú", "u");

            cleanString = Regex.Replace(cleanString, @"\s+", " ").Trim();

            return cleanString.ToLower();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null!)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
