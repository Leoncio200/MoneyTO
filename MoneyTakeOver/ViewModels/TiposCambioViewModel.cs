using Microsoft.EntityFrameworkCore;
using MoneyTakeOver.DataAccess;
using MoneyTakeOver.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MoneyTakeOver.ViewModels
{
    public class TiposCambioViewModel : INotifyPropertyChanged
    {
        private readonly DivisasDbContext _dbContext;
        private ObservableCollection<TiposCambio> _tiposcambios;
        private decimal _nuevoTipoCambioPrecio;
        private decimal _nuevoTipoCambioVenta;
        private string _nuevoTipoCambioMoneda;
        private bool _isTiposCambiosEmpty;
        private bool _isInitialLoad = true;
        private bool _isLoading = false;

        public ObservableCollection<TiposCambio> TiposCambios
        {
            get { return _tiposcambios; }
            set { SetProperty(ref _tiposcambios, value); }
        }

        public decimal NuevoTipoCambioVenta
        {
            get { return _nuevoTipoCambioVenta; }
            set { SetProperty(ref _nuevoTipoCambioVenta, value); }
        }

        public decimal NuevoTipoCambioPrecio
        {
            get { return _nuevoTipoCambioPrecio; }
            set { SetProperty(ref _nuevoTipoCambioPrecio, value); }
        }

        public string NuevoTipoCambioMoneda
        {
            get { return _nuevoTipoCambioMoneda; }
            set { SetProperty(ref _nuevoTipoCambioMoneda, value); }
        }

        public bool IsTiposCambiosEmpty
        {
            get { return _isTiposCambiosEmpty; }
            set { SetProperty(ref _isTiposCambiosEmpty, value); }
        }

        public async Task GetTiposCambios()
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

                var query = _dbContext.TiposCambio.AsQueryable();

                var allTiposCambios = await query.ToListAsync();
                TiposCambios.Clear();
                foreach (var tipocambio in allTiposCambios)
                {
                    TiposCambios.Add(tipocambio);
                }

                IsTiposCambiosEmpty = TiposCambios.Count == 0;
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

                if (string.IsNullOrWhiteSpace(NuevoTipoCambioMoneda) || NuevoTipoCambioPrecio < 1 || NuevoTipoCambioVenta < 1)
                    return;

                var moneda = await _dbContext.Monedas.FirstOrDefaultAsync(m => m.Nombre == NuevoTipoCambioMoneda);
                if (moneda != null) {
                    TiposCambio nuevoTipoCambio = new TiposCambio
                    {
                        MonedaId = moneda.Id,
                        Moneda = moneda,
                        TipoCambioCompra = NuevoTipoCambioPrecio,
                        TipoCambioVenta = NuevoTipoCambioVenta
                    };
                    _dbContext.TiposCambio.Add(nuevoTipoCambio);
                    await _dbContext.SaveChangesAsync();
                }

                

                NuevoTipoCambioMoneda = "";
                NuevoTipoCambioPrecio = 0.0m;
                NuevoTipoCambioVenta = 0.0m;
                
                await GetTiposCambios();
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

        public async Task DeleteTipoCambio(TiposCambio tipocambio)
        {
            try
            {
                await Task.Delay(100);

                if (tipocambio != null)
                {
                    _dbContext.TiposCambio.Remove(tipocambio);
                    await _dbContext.SaveChangesAsync();

                    await GetTiposCambios();
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
            TiposCambios = new ObservableCollection<TiposCambio>();
        }

        public async Task GetDatosAsync()
        {
            await GetTiposCambios();
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
